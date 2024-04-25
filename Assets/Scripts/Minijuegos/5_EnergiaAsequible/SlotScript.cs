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

    private void Start()
    {
        electricityFailObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped");

        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<NewDragAnDrop>().id == id)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<NewDragAnDrop>().canResetPosition = false;
                energyMinigameManager.globalElectricity++;
                Debug.Log("Correct");
            }
            else
            {
                eventData.pointerDrag.GetComponent<NewDragAnDrop>().ResetPosition();
                StartCoroutine(ShowElectrictyFail());
                Debug.Log("Incorrect");
            }

            
        }
    }

    private IEnumerator ShowElectrictyFail()
    {
        electricityPanel.SetActive(false);
        electricityFailObject.transform.position = electricityFailSpawnPoint.position;
        electricityFailObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        electricityFailObject.SetActive(false);
        electricityPanel.SetActive(true);
    }
}
