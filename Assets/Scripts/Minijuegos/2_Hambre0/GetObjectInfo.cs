using System;
using UnityEditor;
using UnityEngine;

public class GetOjectInfo : MonoBehaviour
{
    // Variables públicas que describen las propiedades del objeto
    public string Name;
    public int ID;
    public float Weight;

    // Método público para realizar acciones relacionadas con el objeto
    public Tuple<string, int, float> GetObjectInfo()
    {
        return Tuple.Create(Name, ID, Weight);

    }
}