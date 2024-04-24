using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrash : MonoBehaviour
{
    public LayerMask Trash;
    public Vector3 relocationPosition;
    public int points;
    public Camera playerCamera;

    public GameObject trashFeedbackPanel;

    // Start is called before the first frame update
    void Start()
    {
        trashFeedbackPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.GetChild(0).transform.forward); //playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Trash))
        {
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            hit.transform.gameObject.transform.position = relocationPosition; //+ new Vector3(Random.Range(10f, 20f),0,0) ;
            relocationPosition += new Vector3(2f, 0, 0);
            StartCoroutine(ShowTrashFeedbackPanel());
            points++;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.GetChild(0).transform.rotation.eulerAngles, transform.GetChild(0).transform.forward * Mathf.Infinity, Color.yellow);
    }

    private IEnumerator ShowTrashFeedbackPanel()
    {
        trashFeedbackPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        trashFeedbackPanel.SetActive(false);
    }
}
