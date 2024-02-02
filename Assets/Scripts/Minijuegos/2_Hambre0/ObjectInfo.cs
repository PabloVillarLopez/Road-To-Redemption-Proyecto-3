using System;
using UnityEditor;
using UnityEngine;

public class GetOjectInfo : MonoBehaviour
{
    // Variables p�blicas que describen las propiedades del objeto
    public string Name;
    public int ID;
    public float Weight;

    // M�todo p�blico para realizar acciones relacionadas con el objeto
    public Tuple<string, int, float> GetObjectInfo()
    {
        return Tuple.Create(Name, ID, Weight);

    }
}