using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Lit.Unity;

namespace Lit.Unity.UI
{

    [AddComponentMenu("LitUI/LitButton")]
    public partial class  LitButton : Button {
        public enum ClickAnimation
        {
            None = 0,
            Shrink = 1,
        }

        public ClickAnimation animationType = ClickAnimation.Shrink;
        public string audioName = "btn_click";
        public float scaleFactor = 0.9f;
        public float clickInterval = 0.1f;
        public float tweenDuration = 0.1f;

        private float lastClickTime = 0f;
        private Tweener tweener = null;
        private Vector3 initScale = Vector3.one;
        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(OnClick);
            initScale = transform.localScale;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (tweener != null) { tweener.Kill(); tweener = null; }
            switch (animationType)
            {
                case ClickAnimation.Shrink:
                    tweener = transform.DOScale(scaleFactor, tweenDuration);
                    break;
                case ClickAnimation.None:
                    break;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (tweener != null) { tweener.Kill(); tweener = null; }
            switch (animationType)
            {
                case ClickAnimation.Shrink:
                    tweener = transform.DOScale(initScale, tweenDuration);
                    break;
            }
        }

        public void OnClick()
        {
            LitLogger.Log(this.gameObject.name + "OnClick");

            if(lastClickTime + clickInterval < Time.time)
            {
                lastClickTime = Time.time;
                LitLua lit = GetComponent<LitLua>();
                if (lit != null)
                    lit.GenerateEvent(LitEventType.LE_OnClick,lit);
            }
        }
    }
}
