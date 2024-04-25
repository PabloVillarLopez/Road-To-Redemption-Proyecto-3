using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
    public int id;
    public GameObject electricityFailObject;
    public Transform electricityFailSpawnPoint;
    public GameObject electricityPanel;
    public PanelFader cablePanelFader;

    private void Start()
    {
        electricityFailObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped");

        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<NewDragAnDrop>().canResetPosition = false;
            eventData.pointerDrag.GetComponent<NewDragAnDrop>().isPositioned = true;
            energyMinigameManager.globalElectricity += eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity;

            /*if (eventData.pointerDrag.GetComponent<NewDragAnDrop>().id == id)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<NewDragAnDrop>().canResetPosition = false;
                energyMinigameManager.globalElectricity += eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity;
                Debug.Log("Correct");
            }
            else
            {
                eventData.pointerDrag.GetComponent<NewDragAnDrop>().ResetPosition();
                StartCoroutine(ShowElectrictyFail());
                Debug.Log("Incorrect");
            }*/


        }
    }

    public IEnumerator ShowElectrictyFail()
    {
        cablePanelFader.Fade();
        electricityFailObject.transform.position = electricityFailSpawnPoint.position;
        electricityFailObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        cablePanelFader.Fade();
        electricityFailObject.SetActive(false);
    }
}
