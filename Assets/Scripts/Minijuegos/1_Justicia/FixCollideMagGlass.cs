using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCollideMagGlass : MonoBehaviour
{
    public GameObject magnifyingGlass;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            magnifyingGlass.SetActive(false);
        }

        magnifyingGlass.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            magnifyingGlass.SetActive(true);
        }

        magnifyingGlass.SetActive(true);
    }
}
