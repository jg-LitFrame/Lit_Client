/* SCRIPT INSPECTOR 3
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

using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;


namespace ScriptInspector {

    public class GukParser : FGParser {
        public override string[] Keywords { get { return keywords; } }
        public override string[] BuiltInLiterals { get { return scriptLiterals; } }

        public override bool IsBuiltInType(string word) {
            return Array.BinarySearch(builtInTypes, word, StringComparer.Ordinal) >= 0;
        }

        private static readonly string[] keywords = new string[] {
        "a", "i", "btn", "scroll", "tex", "list", "hide", "eh", ".val"
        //"end",       "false"   ,  "for"   ,    "function" , "if",
        //"in"  ,      "local"   ,  "nil"  ,     "not"    ,  "or",
        //"repeat",    "return"  ,  "then"  ,    "true"   ,   "until"    , "while" , "###"
    };

    //    private static readonly HashSet<string> csOperators = new HashSet<string>{
    //    "++", "--", "->", "+", "-", "!", "~", "++", "--", "&", "*", "/", "%", "+", "-", "<<", ">>", "<", ">",
    //    "<=", ">=", "==", "!=", "&", "^", "|", "&&", "||", "??", "?", "::", ":",
    //    "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "<<=", ">>=", "=>", "..", "\\"
    //};

        private static readonly string[] preprocessorKeywords = new string[] {
        "##", "define", "elif", "else", "endif", "endregion", "error", "if", "line", "pragma", "region", "undef", "warning"
    };

        private static readonly string[] builtInTypes = new string[] {
        ".li", ".show", ".id"
    };

        static GukParser() { }

        protected override void ParseAll(string bufferName) {
            //var scanner = new LuaGrammar.Scanner(LuaGrammar.Instance, textBuffer.formatedLines, bufferName);
            //parseTree = LuaGrammar.Instance.ParseAll(scanner);
        }

        public override FGGrammar.IScanner MoveAfterLeaf(ParseTree.Leaf leaf) {
            return null;
            //var scanner = new LuaGrammar.Scanner(LuaGrammar.Instance, textBuffer.formatedLines, assetPath);
            //return leaf == null ? scanner : scanner.MoveAfterLeaf(leaf) ? scanner : null;
        }

        public override bool ParseLines(int fromLine, int toLineInclusive) {
            var formatedLines = textBuffer.formatedLines;

            for(var line = Math.Max(0, fromLine); line <= toLineInclusive; ++line) {
                var tokens = formatedLines[line].tokens;
                for(var i = tokens.Count; i-- > 0;) {
                    var t = tokens[i];

                    if(t.tokenKind == SyntaxToken.Kind.Missing) {
                        if(t.parent != null && t.parent.parent != null)
                            t.parent.parent.syntaxError = null;
                        tokens.RemoveAt(i);
                    }
                }
            }

            //var scanner = new LuaGrammar.Scanner(LuaGrammar.Instance, formatedLines, assetPath); 
            //scanner.MoveToLine(fromLine, parseTree); 

            //var grammar = LuaGrammar.Instance;
            var canContinue = true;

            for(var line = fromLine; line <= toLineInclusive; ++line)
                foreach(var t in formatedLines[line].tokens)
                    if(t.tokenKind == SyntaxToken.Kind.ContextualKeyword)
                        t.style = t.text == "value" ? textBuffer.styles.parameterStyle : textBuffer.styles.keywordStyle;

            return canContinue;
        }

        public override void FullRefresh() {
            base.FullRefresh();

            parserThread = new System.Threading.Thread(() => {
                this.OnLoaded();
                this.parserThread = null;
            });
            parserThread.Start();
        }

        public override void LexLine(int currentLine, FGTextBuffer.FormatedLine formatedLine) {
            formatedLine.index = currentLine;

            if(parserThread != null)
                parserThread.Join();
            parserThread = null;

            string textLine = textBuffer.lines[currentLine];

            if(currentLine == 0) {
                var defaultScriptDefines = UnityEditor.EditorUserBuildSettings.activeScriptCompilationDefines;
                if(scriptDefines == null || !scriptDefines.SetEquals(defaultScriptDefines)) {
                    if(scriptDefines == null) {
                        scriptDefines = new HashSet<string>(defaultScriptDefines);
                    } else {
                        scriptDefines.Clear();
                        scriptDefines.UnionWith(defaultScriptDefines);
                    }
                }
            }

            //sw2.Start();
            Tokenize(textLine, formatedLine);

            //		syntaxTree.SetLineTokens(currentLine, lineTokens);
            var lineTokens = formatedLine.tokens;

            if(textLine.Length == 0) {
                formatedLine.tokens.Clear();
            } else if(textBuffer.styles != null) {
                var lineWidth = textBuffer.CharIndexToColumn(textLine.Length, currentLine);
                if(lineWidth > textBuffer.longestLine)
                    textBuffer.longestLine = lineWidth;

                for(var i = 0; i < lineTokens.Count; ++i) {
                    var token = lineTokens[i];
                    switch(token.tokenKind) {
                        case SyntaxToken.Kind.Whitespace:
                        case SyntaxToken.Kind.Missing:
                            token.style = textBuffer.styles.normalStyle;
                            break;

                        case SyntaxToken.Kind.Punctuator:
                            token.style = IsOperator(token.text) ? textBuffer.styles.operatorStyle : textBuffer.styles.punctuatorStyle;
                            break;

                        case SyntaxToken.Kind.Keyword:
                            if(IsBuiltInType(token.text)) {
                                if(token.text == "string" || token.text == "object")
                                    token.style = textBuffer.styles.builtInRefTypeStyle;
                                else
                                    token.style = textBuffer.styles.builtInValueTypeStyle;
                            } else {
                                token.style = textBuffer.styles.keywordStyle;
                            }
                            break;

                        case SyntaxToken.Kind.Identifier:
                            if(token.text == "true" || token.text == "false" || token.text == "null") {
                                token.style = textBuffer.styles.builtInLiteralsStyle;
                                token.tokenKind = SyntaxToken.Kind.BuiltInLiteral;
                            } else {
                                token.style = textBuffer.styles.normalStyle;
                            }
                            break;

                        case SyntaxToken.Kind.IntegerLiteral:
                        case SyntaxToken.Kind.RealLiteral:
                            token.style = textBuffer.styles.constantStyle;
                            break;

                        case SyntaxToken.Kind.Comment:
                            var regionKind = formatedLine.regionTree.kind;
                            var inactiveLine = regionKind > FGTextBuffer.RegionTree.Kind.LastActive;
                            token.style = inactiveLine ? textBuffer.styles.inactiveCodeStyle : textBuffer.styles.commentStyle;
                            break;

                        case SyntaxToken.Kind.Preprocessor:
                            token.style = textBuffer.styles.preprocessorStyle;
                            break;

                        case SyntaxToken.Kind.PreprocessorSymbol:
                            token.style = textBuffer.styles.defineSymbols;
                            break;

                        case SyntaxToken.Kind.PreprocessorArguments:
                        case SyntaxToken.Kind.PreprocessorCommentExpected:
                        case SyntaxToken.Kind.PreprocessorDirectiveExpected:
                        case SyntaxToken.Kind.PreprocessorUnexpectedDirective:
                            token.style = textBuffer.styles.normalStyle;
                            break;

                        case SyntaxToken.Kind.CharLiteral:
                        case SyntaxToken.Kind.StringLiteral:
                        case SyntaxToken.Kind.VerbatimStringBegin:
                        case SyntaxToken.Kind.VerbatimStringLiteral:
                            token.style = textBuffer.styles.stringStyle;
                            break;
                    }
                    lineTokens[i] = token;
                }
            }
        }

        protected override void Tokenize(string line, FGTextBuffer.FormatedLine formatedLine) {
            var tokens = new List<SyntaxToken>();
            formatedLine.tokens = tokens;

            int startAt = 0;
            int length = line.Length;
            SyntaxToken token;

            SyntaxToken ws = ScanWhitespace(line, ref startAt);
            if(ws != null) {
                tokens.Add(ws);
                ws.formatedLine = formatedLine;
            }
             

            var inactiveLine = formatedLine.regionTree.kind > FGTextBuffer.RegionTree.Kind.LastActive;

            while(startAt < length) {
                switch(formatedLine.blockState) {
                    case FGTextBuffer.BlockState.None:
                        ws = ScanWhitespace(line, ref startAt);
                        if(ws != null) {
                            tokens.Add(ws);
                            ws.formatedLine = formatedLine;
                            continue;
                        }

                        if(inactiveLine) {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.Comment, line.Substring(startAt)) { formatedLine = formatedLine });
                            startAt = length;
                            break;
                        }

                        if(line[startAt] == '<') {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.PreprocessorSymbol, "<") { formatedLine = formatedLine });
                            startAt += 1;
                            break;
                        }
                        if(line[startAt] == '>') {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.PreprocessorSymbol, ">") { formatedLine = formatedLine });
                            startAt += 1;
                            break;
                        }
                        if(line[startAt] == '/') {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.PreprocessorSymbol, "/") { formatedLine = formatedLine });
                            startAt += 1;
                            break;
                        }
                        if(line[startAt] == '[') {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.PreprocessorSymbol, "[") { formatedLine = formatedLine });
                            startAt += 1;
                            break;
                        }
                        if(line[startAt] == ']') {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.PreprocessorSymbol, "]") { formatedLine = formatedLine });
                            startAt += 1;
                            break;
                        }
                        if(line[startAt] == '=') {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.PreprocessorSymbol, "=") { formatedLine = formatedLine });
                            startAt += 1;
                            break;
                        }

                        if(line[startAt] == '-' && startAt < length - 1) { 
                            if(line[startAt + 1] == '-' && startAt < length - 3 && line[startAt + 2] == '[' && line[startAt + 3] == '[') {
                                tokens.Add(new SyntaxToken(SyntaxToken.Kind.Comment, "--[[") { formatedLine = formatedLine });
                                startAt += 4;
                                formatedLine.blockState = FGTextBuffer.BlockState.CommentBlock;
                                break;
                            } else if(line[startAt + 1] == '-') {
                                tokens.Add(new SyntaxToken(SyntaxToken.Kind.Comment, "--") { formatedLine = formatedLine });
                                startAt += 2;
                                tokens.Add(new SyntaxToken(SyntaxToken.Kind.Comment, line.Substring(startAt)) { formatedLine = formatedLine });
                                startAt = length;
                                break;
                            }

                        }

                        if(line[startAt] == '\'') {
                            token = ScanCharLiteral(line, ref startAt);
                            tokens.Add(token);
                            token.formatedLine = formatedLine;
                            break;
                        }

                        if(line[startAt] == '\"') {
                            token = ScanStringLiteral(line, ref startAt);
                            tokens.Add(token);
                            token.formatedLine = formatedLine;
                            break;
                        }

                        if(startAt < length - 1 && line[startAt] == '@' && line[startAt + 1] == '\"') {
                            token = new SyntaxToken(SyntaxToken.Kind.VerbatimStringBegin, line.Substring(startAt, 2)) { formatedLine = formatedLine };
                            tokens.Add(token);
                            startAt += 2;
                            formatedLine.blockState = FGTextBuffer.BlockState.StringBlock;
                            break;
                        }

                        if(line[startAt] >= '0' && line[startAt] <= '9'
                            || startAt < length - 1 && line[startAt] == '.' && line[startAt + 1] >= '0' && line[startAt + 1] <= '9') {
                            token = ScanNumericLiteral(line, ref startAt);
                            tokens.Add(token);
                            token.formatedLine = formatedLine;
                            break;
                        }

                        token = ScanIdentifierOrKeyword(line, ref startAt);
                        if(token != null) {
                            tokens.Add(token);
                            token.formatedLine = formatedLine;
                            break;
                        }

                        // Multi-character operators / punctuators
                        // "++", "--", "<<", ">>", "<=", ">=", "==", "!=", "&&", "||", "??", "+=", "-=", "*=", "/=", "%=",
                        // "&=", "|=", "^=", "<<=", ">>=", "=>", "::"
                        var punctuatorStart = startAt++;
                        if(startAt < line.Length) {
                            switch(line[punctuatorStart]) {
                                case '?':
                                    if(line[startAt] == '?')
                                        ++startAt;
                                    break;
                                case '+':
                                    if(line[startAt] == '+' || line[startAt] == '=')
                                        ++startAt;
                                    break;
                                case '-':
                                    if(line[startAt] == '-' || line[startAt] == '=')
                                        ++startAt;
                                    break;
                                case '<':
                                    if(line[startAt] == '=')
                                        ++startAt;
                                    else if(line[startAt] == '<') {
                                        ++startAt;
                                        if(startAt < line.Length && line[startAt] == '=')
                                            ++startAt;
                                    }
                                    break;
                                case '>':
                                    if(line[startAt] == '=')
                                        ++startAt;
                                    //else if (startAt < line.Length && line[startAt] == '>')
                                    //{
                                    //    ++startAt;
                                    //    if (line[startAt] == '=')
                                    //        ++startAt;
                                    //}
                                    break;
                                case '=':
                                    if(line[startAt] == '=' || line[startAt] == '>')
                                        ++startAt;
                                    break;
                                case '&':
                                    if(line[startAt] == '=' || line[startAt] == '&')
                                        ++startAt;
                                    break;
                                case '|':
                                    if(line[startAt] == '=' || line[startAt] == '|')
                                        ++startAt;
                                    break;
                                case '*':
                                case '/':
                                case '%':
                                case '^':
                                case '!':
                                    if(line[startAt] == '=')
                                        ++startAt;
                                    break;
                                case ':':
                                    if(line[startAt] == ':')
                                        ++startAt;
                                    break;
                            }
                        }
                        tokens.Add(new SyntaxToken(SyntaxToken.Kind.Punctuator, line.Substring(punctuatorStart, startAt - punctuatorStart)) { formatedLine = formatedLine });
                        break;
                    case FGTextBuffer.BlockState.EmptyOrAssign:
                        int EABlockEmpty = line.IndexOf(" ", startAt, StringComparison.Ordinal);
                        int EABlockAssign = line.IndexOf(":", startAt, StringComparison.Ordinal);
                        int EABlockEnd = EABlockEmpty;
                        if(EABlockAssign >= 0 && EABlockAssign < EABlockEnd) EABlockEnd = EABlockAssign;
                        if(EABlockEnd == -1) {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.Keyword, line.Substring(startAt)) { formatedLine = formatedLine });
                            startAt = length;
                        } else {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.Comment, line.Substring(startAt, EABlockEnd + 1 - startAt)) { formatedLine = formatedLine });
                            startAt = EABlockEnd + 1;
                            formatedLine.blockState = FGTextBuffer.BlockState.None;
                        }
                        break;

                    case FGTextBuffer.BlockState.CommentBlock:
                        int commentBlockEnd = line.IndexOf("--]]", startAt, StringComparison.Ordinal);
                        if(commentBlockEnd == -1) {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.Comment, line.Substring(startAt)) { formatedLine = formatedLine });
                            startAt = length;
                        } else {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.Comment, line.Substring(startAt, commentBlockEnd + 4 - startAt)) { formatedLine = formatedLine });
                            startAt = commentBlockEnd + 4;
                            formatedLine.blockState = FGTextBuffer.BlockState.None;
                        }
                        break;

                    case FGTextBuffer.BlockState.StringBlock:
                        int i = startAt;
                        int closingQuote = line.IndexOf('\"', startAt);
                        while(closingQuote != -1 && closingQuote < length - 1 && line[closingQuote + 1] == '\"') {
                            i = closingQuote + 2;
                            closingQuote = line.IndexOf('\"', i);
                        }
                        if(closingQuote == -1) {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.VerbatimStringLiteral, line.Substring(startAt)) { formatedLine = formatedLine });
                            startAt = length;
                        } else {
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.VerbatimStringLiteral, line.Substring(startAt, closingQuote - startAt)) { formatedLine = formatedLine });
                            startAt = closingQuote;
                            tokens.Add(new SyntaxToken(SyntaxToken.Kind.VerbatimStringLiteral, line.Substring(startAt, 1)) { formatedLine = formatedLine });
                            ++startAt;
                            formatedLine.blockState = FGTextBuffer.BlockState.None;
                        }
                        break;
                }
            }
        }

        private new SyntaxToken ScanIdentifierOrKeyword(string line, ref int startAt) {
            var token = FGParser_ScanIdentifierOrKeyword(line, ref startAt);
            if(token != null && token.tokenKind == SyntaxToken.Kind.Keyword && !IsKeyword(token.text) && !IsBuiltInType(token.text))
                token.tokenKind = SyntaxToken.Kind.Identifier;
            return token;
        }

        protected static SyntaxToken FGParser_ScanIdentifierOrKeyword(string line, ref int startAt) {
            bool identifier = false;
            int i = startAt;
            if(i >= line.Length)
                return null;

            char c = line[i];
            if(c == '@') {
                identifier = true;
                ++i;
            }
            if(i < line.Length) {
                c = line[i];
                if(char.IsLetter(c) || c == '_' || c == '.') {
                    ++i;
                } else if(!ScanUnicodeEscapeChar(line, ref i)) {
                    if(i == startAt)
                        return null;
                    var partialWord = line.Substring(startAt, i - startAt);
                    startAt = i;
                    return new SyntaxToken(SyntaxToken.Kind.Identifier, partialWord);
                } else {
                    identifier = true;
                }

                while(i < line.Length) {
                    if(char.IsLetterOrDigit(line, i) || line[i] == '_' || line[i] == '.')
                        ++i;
                    else if(!ScanUnicodeEscapeChar(line, ref i))
                        break;
                    else
                        identifier = true;
                }
            }

            var word = line.Substring(startAt, i - startAt);
            startAt = i;
            return new SyntaxToken(identifier ? SyntaxToken.Kind.Identifier : SyntaxToken.Kind.Keyword, word);
        }

        private bool IsKeyword(string word) {
            return Array.BinarySearch(Keywords, word, StringComparer.Ordinal) >= 0;
        }

        private bool IsOperator(string text) {
            return false;// csOperators.Contains(text);
        }
    }

}
