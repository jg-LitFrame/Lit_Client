using UnityEngine;
using System.Collections;
using UnityEditor;

public class UIMenu : MonoBehaviour {

    [MenuItem("GameObject/Create Manager", false, 1)]
    static void CreateMgrs(MenuCommand menuCommand)
    {
        var mgrs = LoadTempPrefab("GameManager");
        var go = GameObject.Instantiate<GameObject>(mgrs);
        go.name = "GameManagers";
    }


    [MenuItem("GameObject/LitUI/btn <Button>", false, 1)]
    static void CreateLitBtn(MenuCommand menuCommand)
    {
        Debug.Log("Doing Something..."+ menuCommand.context.name);
    }

    private static GameObject LoadTempPrefab(string name)
    {
        string path = string.Format("Prefab/Template/{0}", name);
        return Resources.Load<GameObject>(path);
    }
}
