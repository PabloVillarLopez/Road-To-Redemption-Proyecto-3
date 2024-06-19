using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixingGunCollide : MonoBehaviour
{
    public GameObject gun;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            gun.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            gun.SetActive(true);
        }
    }
}
