﻿using UnityEngine;
using System.Collections;
using System.IO;

public static class FileTools {
   
    private static string ToLuaRoot = Application.dataPath + "/3rd_Lib/ToLua/";

    public static string ToLuaSystemInitPath = ToLuaRoot + "ToLua/Lua/";



    public static string DocsRoot = Application.dataPath + "/Docs/";

    public static string GetDocFilePath(string child_path)
    {
        return DocsRoot + child_path;
    }

    public static StreamReader GetDocFileReader(string child_path)
    {
        string fullPath = DocsRoot + child_path;
        return new StreamReader(fullPath);
    }

}
