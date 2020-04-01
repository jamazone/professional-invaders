using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    void Start()
    {
        foreach (HealthScript pixel in GetComponentsInChildren<HealthScript>())
        {
            pixel.Spawn();
        }
    }


}
