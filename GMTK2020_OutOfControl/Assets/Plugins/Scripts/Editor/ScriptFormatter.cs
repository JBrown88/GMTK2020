#region Usings

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

#endregion

public class ScriptFormatter : UnityEditor.AssetModificationProcessor
{
    //=====================================================================================================================//
    //============================================= Unity Callback Methods ================================================//
    //=====================================================================================================================//

    #region Unity Callback Methods

    private static void OnWillCreateAsset(string assetPath)
    {
        if (string.IsNullOrEmpty(assetPath))
            return;

        assetPath = assetPath.Replace(".meta", "");

        if (string.IsNullOrEmpty(assetPath))
            return;

        var idx = assetPath.LastIndexOf(".", StringComparison.Ordinal);

        if (idx == -1)
            return;

        string file = assetPath.Substring(idx);
        if (file != ".cs" && file != ".js" && file != ".boo") return;
        idx = Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal);
        assetPath = Application.dataPath.Substring(0, idx) + assetPath;
        file = File.ReadAllText(assetPath);

        file = file.Replace("#CREATIONDATE#", DateTime.Now + "");
        file = file.Replace("#PROJECTNAME#", PlayerSettings.productName.Replace(" ", ""));
        file = file.Replace("#COMPANYNAME#", PlayerSettings.companyName);

        File.WriteAllText(assetPath, file);
        AssetDatabase.Refresh();
    }

    #endregion
}