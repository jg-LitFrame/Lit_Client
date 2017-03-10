using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public static partial class UIUtility
    {

        public static Sprite LoadSprite(string path)
        {
            if (path == null) return null;
            return Resources.Load<Sprite>(path);
        }
    }
}
