using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Lit.Unity
{
    //TODO 后期做缓存
    public class UIFileLoader : FileLoader {

        //TODO 暂时写法，后期统一各种Manager管理
        public static UIFileLoader Instance = null;
        private void Awake()
        {
            Instance = this;
        }

        const string ROOT_SPRITE = "Sprite/";

        public Sprite LoadSprite(string name)
        {
            string path = ROOT_SPRITE + name;
            return RawLoadSprite(path);
        }

        public Image LoadImage(string name)
        {
            string path = ROOT_SPRITE + name;
            return RawLoadImage(path);
        }
    }
}
