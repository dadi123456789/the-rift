using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public static readonly List<Collider> All = new List<Collider>();
    Collider col;

    void OnEnable()
    {
        col = GetComponent<Collider>();
        if (col != null) All.Add(col);
    }

    void OnDisable()
    {
        if (col != null) All.Remove(col);
    }
}