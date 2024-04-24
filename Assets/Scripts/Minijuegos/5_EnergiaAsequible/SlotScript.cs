using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
    public int id;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped");

        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<NewDragAnDrop>().id == id)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<NewDragAnDrop>().canResetPosition = false;
                Debug.Log("Correct");
            }
            else
            {
                eventData.pointerDrag.GetComponent<NewDragAnDrop>().ResetPosition();
                Debug.Log("Incorrect");
            }

            
        }
    }
}
