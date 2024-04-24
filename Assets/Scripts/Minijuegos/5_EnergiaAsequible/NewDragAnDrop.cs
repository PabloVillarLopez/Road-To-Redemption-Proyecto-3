using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewDragAnDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTrans;
    public Canvas myCanvas;
    private CanvasGroup canvasGroup;
    public int id;
    private Vector2 initPosition;
    [HideInInspector]
    public bool canResetPosition = true;

    // Start is called before the first frame update
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / myCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (canResetPosition)
        {
            ResetPosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void ResetPosition()
    {
        transform.position = initPosition;
    }
}
