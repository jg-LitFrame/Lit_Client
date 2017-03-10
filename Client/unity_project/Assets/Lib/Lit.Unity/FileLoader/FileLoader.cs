using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Lit.Unity
{
    //后期可能会用bundle
    public class FileLoader : LitBehaviour
    {
	    public GameObject RawLoadRes(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public Sprite RawLoadSprite(string path)
        {
            return Resources.Load<Sprite>(path);
        }

        public Image RawLoadImage(string path)
        {
            return Resources.Load<Image>(path);
        }
    }
}
