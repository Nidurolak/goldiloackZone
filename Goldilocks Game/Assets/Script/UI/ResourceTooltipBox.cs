using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResourceTooltipBox : MonoBehaviour
{
    public GameObject TooltipBox;

    [SerializeField]
    private Camera Cam;

    [SerializeField]
    private RectTransform canvasRectTransform;

    [SerializeField]
    public CanvasGroup canvasGroup;

    public RectTransform backgroundRectTransform;
    public Text TextBox; 
    public RectTransform rectTransform;
    public bool Open_RightNow = false;
    public bool Open_MousePosition = false;
    private float TextPaddingSize = 15f;

    private Sequence sequence0 = null;

    private void OnEnable()
    {
        sequence0 = DOTween.Sequence();
    }

    private void OnDisable()
    {
        canvasGroup.DOFade(0, 0f);
    }

    public void SetText(string a)
    {
        sequence0.Kill();

        canvasGroup.alpha = 0.0f;
        sequence0.Append(canvasGroup.DOFade(0, 0f));
        sequence0.Append(canvasGroup.DOFade(1, 2f));
        sequence0.Play();
        
        TextBox.text = a;
        
        Vector2 TextSize = new Vector2(TextBox.preferredWidth + (TextPaddingSize * 2), TextBox.preferredHeight + TextPaddingSize);
        backgroundRectTransform.sizeDelta = TextSize;


        Vector2 localPoint = new Vector2(Input.mousePosition.x + 25f, Input.mousePosition.y - 25f);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Cam, out localPoint);
        transform.position = localPoint;
    }
}
