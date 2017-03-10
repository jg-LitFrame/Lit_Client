﻿/* SCRIPT INSPECTOR 3
 * version 3.0.9, November 2015
 * Copyright © 2012-2015, Flipbook Games
 * 
 * Unity's legendary editor for C#, UnityScript, Boo, Shaders, and text,
 * now transformed into an advanced C# IDE!!!
 * 
 * Follow me on http://twitter.com/FlipbookGames
 * Like Flipbook Games on Facebook http://facebook.com/FlipbookGames
 * Join discussion in Unity forums http://forum.unity3d.com/threads/138329
 * Contact info@flipbookgames.com for feedback, bug reports, or suggestions.
 * Visit http://flipbookgames.com/ for more info.
 */

namespace ScriptInspector
{

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class FindResultsWindow : EditorWindow
{
	private static GUIStyle evenItemStyle;
	private static GUIStyle oddItemStyle;
	private static GUIStyle pingStyle;
	private static GUIStyle toggleMixedStyle;
	private static GUIStyle boldToolbarButton;
	
	[System.Serializable]
	private class FoundResult
	{
		public string description;
		public string assetGuid;
		public string assetPath;
		public int line;
		public int characterIndex;
		public int length;
		public int trimOffset;
		public bool selected;
	}
	
	private string infoText;
	private string resultsCountText = "Found 0 results.";
	private System.Action<System.Action<string, string, TextPosition, int>, string, SearchOptions> searchFunction;
	private string[] assetGuids;
	private SearchOptions search;
	private bool foundNoResults;
	
	private int current;
	[System.NonSerialized]
	private bool repaintOnUpdate;
	private Vector2 scrollPosition;
	
	private int currentItem = 0;
	[System.NonSerialized]
	private bool scrollToCurrentItem;
	[System.NonSerialized]
	private float listViewHeight;
	
	private List<FoundResult> results = new List<FoundResult>();
	[System.NonSerialized]
	private List<FoundResult> flatResults = new List<FoundResult>();
	private int resultsCount;
	private int filesCount;
	private HashSet<string> collapsedPaths = new HashSet<string>();
	//[System.NonSerialized]
	//private ReaderWriterLockSlim resultsLock = new ReaderWriterLockSlim();
	
	private bool keepResults;
	public bool KeepResults {
		get { return keepResults; }
	}
	
	private bool groupByFile;
	private bool GroupByFile {
		get { return groupByFile; }
		set { SISettings.groupFindResultsByFile.Value = groupByFile = value; }
	}
	
	[System.NonSerialized]
	private string replaceText;
	[System.NonSerialized]
	private bool replaceAllAfterSearch;
	[System.NonSerialized]
	private EditorWindow focusAfterReplaceAll;
	
	internal static FindResultsWindow Create(
		string description,
		System.Action<System.Action<string, string, TextPosition, int>, string, SearchOptions> searchFunction,
		string[] assetGuids,
		SearchOptions searchOptions,
		bool inNewWindow)
	{
		var previousResults = (FindResultsWindow[]) Resources.FindObjectsOfTypeAll(typeof(FindResultsWindow));
		var reuseWnd = inNewWindow ? null : previousResults.FirstOrDefault(w => !w.KeepResults && w.replaceText == null);
		var wnd = reuseWnd ?? CreateInstance<FindResultsWindow>();
		
		wnd.infoText = description;
		wnd.searchFunction = searchFunction;
		wnd.assetGuids = assetGuids;
		wnd.search = searchOptions;
		wnd.groupByFile = SISettings.groupFindResultsByFile;
		
		if (!reuseWnd)
		{
			var docked = false;
			foreach (var prevWnd in previousResults)
			{
				if (prevWnd != wnd && prevWnd)
				{
					docked = FGCodeWindow.DockNextTo(wnd, prevWnd);
					if (docked)
						break;
				}
			}
			if (!docked)
			{
				var console = FGConsole.FindInstance() ?? (Resources.FindObjectsOfTypeAll(FGConsole.consoleWindowType).FirstOrDefault() as EditorWindow);
				if (console)
					docked = FGCodeWindow.DockNextTo(wnd, console);
			}
		}
		
		wnd.ClearResults();
		
		wnd.Show();
		wnd.Focus();
		
		return wnd;
	}
	
	public void SetReplaceText(string replaceText)
	{
		FGTextBuffer.onInsertedTextAll += OnBufferModified;
		FGTextBuffer.onRemovedTextAll += OnBufferModified;
		
		this.replaceText = replaceText;
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		title = "<b>Replace</b>";
#else
		titleContent.text = "<b>Replace</b>";
#endif
		Repaint();
	}
	
	public void ReplaceAllAfterSearchAndSetFocus(EditorWindow toWindow)
	{
		replaceAllAfterSearch = true;
		focusAfterReplaceAll = toWindow;
	}
		
	private void ClearResults()
	{
		resultsCountText = "Found 0 results.";
		foundNoResults = false;
		
		current = 0;
		scrollPosition = Vector2.zero;
		
		currentItem = 0;
		listViewHeight = 0f;
		
		results.Clear();
		flatResults.Clear();
		resultsCount = 0;
		filesCount = 0;
		collapsedPaths.Clear();
	}
	
	private void OnEnable()
	{
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		title = "Find Results";
#else
		titleContent.text = "Find Results";
#endif
		foreach (var result in results)
			if (result.description == "")
				result.description = null;
			else if (result.description != null)
				flatResults.Add(result);
		Repaint();
		
		FGTextBuffer.onInsertedLinesAll += OnInsertedLines;
		FGTextBuffer.onRemovedLinesAll += OnRemovedLines;
	}
	
	private void Unsubscribe()
	{
		searchFunction = null;
		
		FGTextBuffer.onInsertedLinesAll -= OnInsertedLines;
		FGTextBuffer.onRemovedLinesAll -= OnRemovedLines;
		
		FGTextBuffer.onInsertedTextAll -= OnBufferModified;
		FGTextBuffer.onRemovedTextAll -= OnBufferModified;
	}
	
	private void OnDisable()
	{
		Unsubscribe();
	}
	
	private void OnDestroy()
	{
		Unsubscribe();
	}
	
	private void OnBufferModified(string guid, FGTextBuffer.CaretPos from, FGTextBuffer.CaretPos to)
	{
		for (var i = 0; i < results.Count; i++)
		{
			var result = results[i];
			if (result.description != null && guid == result.assetGuid)
			{
				Unsubscribe();
				Close();
				break;
			}
		}
	}
	
	private void OnInsertedLines(string guid, int lineIndex, int numLines)
	{
		for (var i = 0; i < results.Count; i++)
		{
			var result = results[i];
			if (result.description != null && guid == result.assetGuid)
			{
				if (lineIndex <= result.line)
				{
					result.line += numLines;
					repaintOnUpdate = true;
				}
			}
		}
	}
	
	private void OnRemovedLines(string guid, int lineIndex, int numLines)
	{
		for (var i = 0; i < results.Count; i++)
		{
			var result = results[i];
			if (result.description != null && guid == result.assetGuid)
			{
				if (lineIndex <= result.line)
				{
					if (lineIndex + numLines <= result.line)
						result.line -= numLines;
					else
						result.line = lineIndex;
					repaintOnUpdate = true;
				}
			}
		}
	}
	
	private void Update()
	{
		if (repaintOnUpdate)
		{
			repaintOnUpdate = false;
			Repaint();
			return;
		}
		
		if (searchFunction != null && current < assetGuids.Length)
		{
			searchFunction(AddResult, assetGuids[current++], search);
		}
		else if (searchFunction != null)
		{
			searchFunction = null;
			infoText = "Find results for '" + search.text + "'";
			foundNoResults = resultsCount == 0;
			if (replaceAllAfterSearch)
			{
				replaceAllAfterSearch = false;
				ReplaceAll();
				if (focusAfterReplaceAll)
					focusAfterReplaceAll.Focus();
				focusAfterReplaceAll = null;
			}
			else
			{
				Repaint();
			}
		}
	}
	
	private void ReplaceAll()
	{
		Unsubscribe();
		
		FGTextBuffer buffer = null;
		string guid = null;
		bool canEditAll = true;
		var canEditBuffers = new HashSet<FGTextBuffer>();
		
		for (var i = flatResults.Count; i --> 0; )
		{
			var result = flatResults[i];
			if (!result.selected)
				continue;
			
			if (result.assetGuid != guid)
			{
				guid = result.assetGuid;
				buffer = FGTextBufferManager.GetBuffer(guid);
				if (buffer.IsLoading)
					buffer.LoadImmediately();
				
				if (FGCodeWindow.CodeWindows.All(x => x.TargetAssetGuid != guid))
				{
					var wnd = FGCodeWindow.OpenAssetInTab(guid);
					if (wnd)
						wnd.OnFirstUpdate();
				}
				if (!buffer.TryEdit())
					canEditAll = false;
				else
					canEditBuffers.Add(buffer);
			}
		}
		
		if (!canEditAll)
		{
			if (!EditorUtility.DisplayDialog("Replace", "Some assets are locked and cannot be edited!", "Continue Anyway", "Cancel"))
				Close();
		}
		
		buffer = null;
		guid = null;
		var updatedLines = new HashSet<int>();
		for (var i = flatResults.Count; i --> 0; )
		{
			var result = flatResults[i];
			if (!result.selected)
				continue;
			
			if (result.assetGuid != guid)
			{
				if (buffer != null)
				{
					foreach (var line in updatedLines)
						buffer.UpdateHighlighting(line, line);
					buffer.EndEdit();
					FGTextBufferManager.AddBufferToGlobalUndo(buffer);
					updatedLines.Clear();
				}
				
				guid = result.assetGuid;
				buffer = FGTextBufferManager.GetBuffer(guid);
				
				if (buffer != null)
				{
					if (canEditBuffers.Contains(buffer))
						buffer.BeginEdit("*Replace All");
					else
						buffer = null;
				}
			}
			
			if (buffer != null)
			{
				var from = new FGTextBuffer.CaretPos { line = result.line, characterIndex = result.characterIndex };
				var to = new FGTextBuffer.CaretPos { line = result.line, characterIndex = result.characterIndex + result.length };
				var insertAt = buffer.DeleteText(from, to);
				if (replaceText != "")
					buffer.InsertText(insertAt, replaceText);
				
				updatedLines.Add(result.line);
			}
		}
		if (buffer != null)
		{
			foreach (var line in updatedLines)
				buffer.UpdateHighlighting(line, line);
			buffer.EndEdit();
			FGTextBufferManager.AddBufferToGlobalUndo(buffer);
		}
		
		FGTextBufferManager.RecordGlobalUndo();
		Close();
	}
	
	private void AddResult(string text, string guid, TextPosition location, int length)
	{
		try
		{
			//resultsLock.EnterWriteLock();
			var trim = !char.IsWhiteSpace(text, location.index) && !char.IsWhiteSpace(text, location.index + length - 1);
			
			var lastAssetGuid = results.Count > 0 ? results[results.Count - 1].assetGuid : null;
			if (guid != lastAssetGuid)
			{
				++filesCount;
				
				results.Add(
					new FoundResult {
					assetGuid = guid,
					assetPath = AssetDatabase.GUIDToAssetPath(guid),
					selected = true,
				});
			}
			else if (lastAssetGuid == null)
			{
				++filesCount;
			}
			
			var trimmed = trim ? text.TrimStart() : text;
			var trimOffset = text.Length - trimmed.Length;
			trimmed = trim ? trimmed.TrimEnd() : text;
			var newFindResult =
				new FoundResult {
					description = trimmed,
					assetGuid = guid,
					assetPath = AssetDatabase.GUIDToAssetPath(guid),
					line = location.line,
					characterIndex = location.index,
					length = length,
					trimOffset = trimOffset,
					selected = true,
				};
			results.Add(newFindResult);
			flatResults.Add(newFindResult);
			++resultsCount;
			resultsCountText = "Found " + resultsCount + " results in " + filesCount + " files.";
		}
		finally
		{
			//resultsLock.ExitWriteLock();
		}
		
		repaintOnUpdate = true;
	}
	
	private void GoToResult(int index)
	{
		if (currentItem >= results.Count)
			return;
		
		var result = (GroupByFile ? results : flatResults)[currentItem];
		FGCodeWindow.OpenAssetInTab(result.assetGuid, result.line, result.characterIndex, result.length);
	}
	
	private void OnGUI()
	{
		if (Event.current.isKey && TabSwitcher.OnGUIGlobal())
			return;
		
		bool needsRepaint = false;
		
		if (Event.current.type == EventType.KeyDown)
		{
			var nextItem = currentItem;
			var results = GroupByFile ? this.results : flatResults;
			
			if (Event.current.keyCode == KeyCode.DownArrow)
			{
				++nextItem;
				if (GroupByFile)
				{
					while (nextItem < results.Count && results[nextItem].description != null &&
					  collapsedPaths.Contains(results[nextItem].assetPath))
						++nextItem;
				}
				if (nextItem == results.Count)
					nextItem = currentItem;
			}
			else if (Event.current.keyCode == KeyCode.RightArrow && currentItem < results.Count)
			{
				if (results[currentItem].description == null && collapsedPaths.Contains(results[currentItem].assetPath))
				{
					collapsedPaths.Remove(results[currentItem].assetPath);
					needsRepaint = true;
				}
				else
				{
					++nextItem;
				}
			}
			else if (Event.current.keyCode == KeyCode.UpArrow)
			{
				--nextItem;
				if (GroupByFile)
				{
					while (nextItem > 0 && results[nextItem].description != null &&
					  collapsedPaths.Contains(results[nextItem].assetPath))
						--nextItem;
				}
			}
			else if (Event.current.keyCode == KeyCode.LeftArrow && currentItem < results.Count)
			{
				if (results[currentItem].description == null)
				{
					collapsedPaths.Add(results[currentItem].assetPath);
					needsRepaint = true;
				}
				else if (GroupByFile)
				{
					while (results[nextItem].description != null)
						--nextItem;
				}
				else
				{
					--nextItem;
				}
			}
			else if (Event.current.keyCode == KeyCode.Home)
			{
				nextItem = 0;
			}
			else if (Event.current.keyCode == KeyCode.End)
			{
				nextItem = results.Count - 1;
				if (GroupByFile)
				{
					while (nextItem > 0 && results[nextItem].description != null &&
					  collapsedPaths.Contains(results[nextItem].assetPath))
						--nextItem;
				}
			}
			
			nextItem = Mathf.Max(0, Mathf.Min(nextItem, results.Count - 1));
			scrollToCurrentItem = scrollToCurrentItem || needsRepaint || nextItem != currentItem;
			needsRepaint = needsRepaint || nextItem != currentItem;
			currentItem = nextItem;
			
			if (Event.current.keyCode == KeyCode.Return ||
				Event.current.keyCode == KeyCode.KeypadEnter ||
				Event.current.keyCode == KeyCode.Space)
			{
				if (currentItem < results.Count)
				{
					Event.current.Use();
					
					if (results[currentItem].description != null)
					{
						if (replaceText == null || Event.current.keyCode != KeyCode.Space)
						{
							GoToResult(currentItem);
						}
						else
						{
							results[currentItem].selected = !results[currentItem].selected;
							needsRepaint = true;
						}
					}
					else
					{
						var path = results[currentItem].assetPath;
						if (collapsedPaths.Contains(path))
							collapsedPaths.Remove(path);
						else
							collapsedPaths.Add(path);
						
						needsRepaint = true;
					}
				}
			}
			else if (needsRepaint)
			{
				Event.current.Use();
			}
			
			if (needsRepaint)
			{
				needsRepaint = false;
				Repaint();
				return;
			}
		}
		
		//if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
		//{
		//	Close();
		//	editor.OwnerWindow.Focus();
		//	return;
		//}
		
		if (evenItemStyle == null)
		{
			evenItemStyle = new GUIStyle("PR Label");
			evenItemStyle.padding.top = 2;
			evenItemStyle.padding.bottom = 2;
			evenItemStyle.padding.left = 2;
			evenItemStyle.margin.right = 0;
			evenItemStyle.fixedHeight = 0;
			evenItemStyle.richText = false;
			evenItemStyle.stretchWidth = true;
			evenItemStyle.wordWrap = false;
			
			oddItemStyle = new GUIStyle(evenItemStyle);
			
			var evenBackground = (GUIStyle) "CN EntryBackEven";
			var oddBackground = (GUIStyle) "CN EntryBackodd";
			evenItemStyle.normal.background = evenBackground.normal.background;
			evenItemStyle.focused.background = evenBackground.normal.background;
			oddItemStyle.normal.background = oddBackground.normal.background;
			oddItemStyle.focused.background = oddBackground.normal.background;
			
			pingStyle = (GUIStyle) "PR Ping";
			toggleMixedStyle = (GUIStyle) "ToggleMixed";
			
			boldToolbarButton = new GUIStyle(EditorStyles.toolbarButton);
			boldToolbarButton.fontStyle = FontStyle.Bold;
		}
		
		var rcToolbar = new Rect(0f, 0f, Screen.width, 20f);
		GUI.Label(rcToolbar, GUIContent.none, EditorStyles.toolbar);
		
		GUILayout.BeginHorizontal();
		
		if (searchFunction != null)
		{
			if (GUILayout.Toggle(false, "Stop", boldToolbarButton, GUILayout.Height(20f), GUILayout.ExpandHeight(true)))
			{
				searchFunction = null;
				replaceAllAfterSearch = false;
				focusAfterReplaceAll = null;
				infoText = "Incomplete find results for '" + search.text + "'";
				foundNoResults = resultsCount == 0;
				replaceText = null;
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
				title = "Find Results";
#else
				titleContent.text = "Find Results";
#endif
			}
			EditorGUILayout.Space();
		}
		
		GUILayout.Label(infoText, GUILayout.Height(20f), GUILayout.MinWidth(0f));
		EditorGUILayout.Space();
		GUILayout.Label(resultsCountText, GUILayout.Height(20f));
		GUILayout.FlexibleSpace();
		
		GUI.enabled = !foundNoResults;
		var newGroupByFile = GUILayout.Toggle(GroupByFile, "Group by file", EditorStyles.toolbarButton, GUILayout.Height(20f), GUILayout.ExpandHeight(true));
		GUI.enabled = true;
		
		if (replaceText != null)
		{
			EditorGUILayout.Space();
			
			GUI.enabled = searchFunction == null;
			if (GUILayout.Toggle(false, foundNoResults || searchFunction != null ? "Close" : "Replace all selected", boldToolbarButton, GUILayout.Height(20f), GUILayout.ExpandHeight(true)))
			{
				ReplaceAll();
			}
			GUI.enabled = true;
		}
		else
		{
			GUI.enabled = !foundNoResults;
			keepResults = GUILayout.Toggle(keepResults, "Keep this results", EditorStyles.toolbarButton, GUILayout.Height(20f), GUILayout.ExpandHeight(true));
			GUI.enabled = true;
		}
		
		EditorGUILayout.Space();
		GUILayout.EndHorizontal();
		
		if (newGroupByFile != GroupByFile)
		{
			var results = GroupByFile ? this.results : flatResults;
			GroupByFile = newGroupByFile;
			var newResults = GroupByFile ? this.results : flatResults;
			if (currentItem < results.Count)
			{
				var currentResult = results[currentItem];
				if (currentResult.description == null)
					currentResult = results[currentItem + 1];
				currentItem = Mathf.Max(0, newResults.IndexOf(currentResult));
			}
			else
			{
				currentItem = 0;
			}
			
			needsRepaint = true;
			scrollToCurrentItem = true;
		}
		
		listViewHeight = Screen.height - rcToolbar.height - 20f;
		
		Vector2 scrollToPosition;
		try
		{
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			scrollToPosition = scrollPosition;
			
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
			
			//resultsLock.EnterReadLock();
			
			if (foundNoResults)
			{
				GUILayout.Label("No Results Found!");
			}
			else
			{
				var currentPath = "";
				bool isExpanded = true;
				int drawnItemIndex = 0;
				var results = GroupByFile ? this.results : flatResults;
				for (var i = 0; i < results.Count; ++i)
				{
					var result = results[i];
					if (result.description != null && !isExpanded)
						continue;
					
					var itemStyle = (drawnItemIndex & 1) == 0 ? evenItemStyle : oddItemStyle;
					
					++drawnItemIndex;
					
					var rc = GUILayoutUtility.GetRect(GUIContent.none, itemStyle, GUILayout.Height(21f), GUILayout.ExpandWidth(true));
					rc.xMin = 0f;
					
					if (Event.current.type == EventType.Repaint)
						itemStyle.Draw(rc, GUIContent.none, false, false, i == currentItem, this == focusedWindow);
					
					if (result.description == null)
					{
						currentPath = result.assetPath;
						isExpanded = !collapsedPaths.Contains(currentPath);
						var rcToggle = rc;
						rcToggle.xMax = 18f;
						rcToggle.yMin += 2f;
						bool expand = GUI.Toggle(rcToggle, isExpanded, GUIContent.none, EditorStyles.foldout);
						if (expand != isExpanded)
						{
							currentItem = i;
							if (expand && !isExpanded)
								collapsedPaths.Remove(currentPath);
							else if (!expand && isExpanded)
								collapsedPaths.Add(currentPath);
							needsRepaint = true;
						}
						if (replaceText != null)
						{
							rc.xMin += 18f;
							rcToggle = rc;
							rcToggle.width = 18f;
							rcToggle.yMin += 2f;
							
							var hasSelected = false;
							var hasUnselected = false;
							for (var j = i + 1; j < results.Count && !(hasSelected && hasUnselected); ++j)
							{
								var child = results[j];
								if (child.description == null)
									break;
								if (child.selected)
									hasSelected = true;
								else
									hasUnselected = true;
							}
							var selected = hasSelected;
							if (selected != GUI.Toggle(rcToggle, selected, GUIContent.none, hasSelected && hasUnselected ? toggleMixedStyle : EditorStyles.toggle))
							{
								selected = !selected;
								for (var j = i + 1; j < results.Count; ++j)
								{
									var child = results[j];
									if (child.description == null)
										break;
									child.selected = selected;
								}
							}
						}
					}
					else if (replaceText != null)
					{
						if (GroupByFile)
							rc.xMin += 36f;
						else
							rc.xMin += 4f;
						var rcToggle = rc;
						rcToggle.width = 18f;
						rcToggle.yMin += 2f;
						result.selected = GUI.Toggle(rcToggle, result.selected, GUIContent.none);
					}
					
					if (scrollToCurrentItem && i == currentItem && Event.current.type == EventType.Repaint)
					{
						if (rc.yMin < scrollPosition.y)
						{
							scrollToPosition.y = rc.yMin;
							needsRepaint = true;
						}
						else if (rc.yMax > scrollPosition.y + listViewHeight)
						{
							scrollToPosition.y = rc.yMax - listViewHeight;
							needsRepaint = true;
						}						
					}
					
					if (rc.yMax < scrollPosition.y || rc.yMin > scrollPosition.y + listViewHeight)
					{
						continue;
					}
					
					if (Event.current.type == EventType.MouseDown && rc.Contains(Event.current.mousePosition))
					{
						if (i == currentItem && Event.current.button == 0 && Event.current.clickCount == 2)
						{
							if (result.description == null)
							{
								if (collapsedPaths.Contains(result.assetPath))
									collapsedPaths.Remove(result.assetPath);
								else
									collapsedPaths.Add(result.assetPath);
								
								needsRepaint = true;
							}
							else
							{
								FGCodeWindow.OpenAssetInTab(result.assetGuid, result.line, result.characterIndex, result.length);
							}
						}
						else if (Event.current.button == 1 && result.description == null)
						{
							GenericMenu menu = new GenericMenu();
							menu.AddItem(new GUIContent("Expand All"), false, () => {
								collapsedPaths.Clear();
							});
							menu.AddItem(new GUIContent("Collapse All"), false, () => {
								foreach (var r in results)
									if (r.description == null)
										collapsedPaths.Add(r.assetPath);
							});
							menu.ShowAsContext();
						}
						currentItem = i;
						needsRepaint = true;
						scrollToCurrentItem = true;
						
						Event.current.Use();
					}
					
					GUIContent contentContent;
					int lineInfoLength = 0;
					if (result.description == null)
					{
						contentContent = new GUIContent(result.assetPath, AssetDatabase.GetCachedIcon(result.assetPath));
						rc.xMin += 16f;
					}
					else
					{
						string lineInfo;
						if (GroupByFile)
							lineInfo = (result.line + 1).ToString() + ": ";
						else
							lineInfo = System.IO.Path.GetFileName(result.assetPath) + " (" + (result.line + 1).ToString() + "): ";
						lineInfoLength = lineInfo.Length;
						contentContent = new GUIContent(lineInfo + result.description);
						if (GroupByFile)
							rc.xMin += 18f;
						else if (replaceText != null)
							rc.xMin += 18f;
						else
							rc.xMin += 2f;
					}
					
					if (Event.current.type == EventType.Repaint)
					{
						if (result.description != null)
						{
							var dotContent = new GUIContent(".");
							var preContent = new GUIContent(contentContent.text.Substring(0, lineInfoLength + result.characterIndex - result.trimOffset) + '.');
							var resultContent = new GUIContent('.' + contentContent.text.Substring(0, lineInfoLength + result.characterIndex + result.length - result.trimOffset) + '.');
							var dotSize = itemStyle.CalcSize(dotContent);
							var preSize = itemStyle.CalcSize(preContent); preSize.x -= dotSize.x;
							var resultSize = itemStyle.CalcSize(resultContent); resultSize.x -= dotSize.x * 2f;
							var rcHighlight = new Rect(rc.x + preSize.x - 4f, rc.y + 2f, resultSize.x - preSize.x + 14f, rc.height - 4f);
							GUI.color = new Color(1f, 1f, 1f, 0.4f);
							pingStyle.Draw(rcHighlight, false, false, false, false);
							GUI.color = Color.white;
						}
						
						GUI.backgroundColor = Color.clear;
						itemStyle.Draw(rc, contentContent, false, false, i == currentItem, this == focusedWindow);
						GUI.backgroundColor = Color.white;
					}					
				}
			}
			
			GUILayout.FlexibleSpace();
		}
		finally
		{
			//resultsLock.ExitReadLock();
			GUILayout.EndScrollView();
		}
		
		if (Event.current.type == EventType.Repaint)
		{
			if (needsRepaint)
			{
				scrollToCurrentItem = false;
				scrollPosition = scrollToPosition;
				Repaint();
			}
			else
			{
				scrollToCurrentItem = false;
			}
		}
	}
}

}
