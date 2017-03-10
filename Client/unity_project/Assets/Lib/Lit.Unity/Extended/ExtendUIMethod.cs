using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lit.Unity;
namespace Lit.Unity
{
    public static  class ExtendUIMethod{

	    public static void SetInfo(this Image img, string path)
        {
            img.sprite = UIUtility.LoadSprite(path);
        }
    }

}
