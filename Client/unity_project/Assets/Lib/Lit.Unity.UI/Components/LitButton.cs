using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Lit.Unity;
[AddComponentMenu("LitUI/LitButton")]
public class LitButton : Button {
    public enum ClickAnimation
    {
        None,
        Shrink,
    }
    public ClickAnimation animationType = ClickAnimation.Shrink;
    public float scaleFactor = 0.9f;
    public float tweenDuration = 0.1f;
    public string audioName = "btn_click";

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
        
    }
}
