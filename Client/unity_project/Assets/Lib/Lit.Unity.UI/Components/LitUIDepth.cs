using UnityEngine;
using System.Collections;
using Lit.Unity;
namespace Lit.Unity{

    public class LitUIDepth : LitBehaviour {
        public int order;
        public bool isUI = true;
        public LayerMask layer = 3;
        void Start()
        {
            float layerValue = Mathf.Log(layer.value, 2);
            if(layerValue % 1 > float.MinValue)
            {
                LitLogger.ErrorFormat("LitUIDepth => Set Layer Error {0}", gameObject.name);
            }
            gameObject.layer = (int)layerValue;

            if (isUI)
            {
                Canvas canvas = GetComponent<Canvas>();
                if (canvas == null)
                {
                    canvas = gameObject.AddComponent<Canvas>();
                }
                canvas.overrideSorting = true;
                canvas.sortingOrder = order;
            }
            else
            {
                Renderer[] renders = GetComponentsInChildren<Renderer>();
                foreach (Renderer render in renders)
                {
                    render.sortingOrder = order;
                }
                if (isAttach<Renderer>())
                {
                    var render = GetComponent<Renderer>();
                    render.sortingOrder = order;
                }
            }
        }
    }

}
