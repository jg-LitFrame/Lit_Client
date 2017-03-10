using UnityEngine;
using System.Collections;
using UnityEditor;

public class UIMenu : MonoBehaviour {

    [MenuItem("GameObject/LitUI/btn <Button>", false, 1)]
    static void CreateLitBtn(MenuCommand menuCommand)
    {
        Debug.Log("Doing Something..."+ menuCommand.context.name);
    }
}
