using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public GameObject electricityFailObject;
    public Transform electricityFailSpawnPoint;
    public GameObject electricityPanel;
    public PanelFader cablePanelFader;
    public Camera cableCamera;
    public Camera currentCamera;
    public int slotIndividualElectricity;
    public int initialSlotIndividualElectricity;
    public GameObject tapaCuadro;
    
    private void Start()
    {
        electricityFailObject.SetActive(false);
        cableCamera.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        //initialSlotIndividualElectricity = slotIndividualElectricity;

        Debug.Log("Item dropped");

        if (eventData.pointerDrag != null)
        {
            gameObject.transform.GetComponent<Image>().color = new Color(217, 217, 217, 0);
            //gameObject.transform.GetComponent<Image>().sprite = null;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<NewDragAnDrop>().canResetPosition = false;
            eventData.pointerDrag.GetComponent<NewDragAnDrop>().isPositioned = true;
            if (eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity > 0 && eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity != slotIndividualElectricity || eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity < 0 && eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity != slotIndividualElectricity)
            {
                slotIndividualElectricity = eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity;
            }
            else if (eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity == 0)
            {
                slotIndividualElectricity = eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity;
            }
            
            //energyMinigameManager.globalElectricity += eventData.pointerDrag.GetComponent<NewDragAnDrop>().individualElectricity;

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
        cableCamera.gameObject.SetActive(true);
        currentCamera.gameObject.SetActive(false);
        tapaCuadro.SetActive(false);
        //cableCamera.gameObject.transform.position = electricityFailSpawnPoint.position;
        electricityFailObject.transform.position = electricityFailSpawnPoint.position;
        electricityFailObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        cablePanelFader.Fade();
        electricityFailObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //gameObject.transform.GetComponent<Image>().color = new Color(190, 190, 190, 255);
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //gameObject.transform.GetComponent<Image>().color = new Color(176, 176, 176, 255);
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void ResetSlot()
    {
        gameObject.transform.GetComponent<Image>().color = new Color32(170, 170, 170, 255);
        slotIndividualElectricity = initialSlotIndividualElectricity;
    }
}
