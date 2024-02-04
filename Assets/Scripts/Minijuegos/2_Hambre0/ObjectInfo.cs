using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    public string Name { get; set; }
    public int ID { get; set; }
    public float Weight { get; set; }

    public void SetInfo(int id)
    {
        ID = id;
    }

    public int GetID()
    {
        return ID;
    }
}
