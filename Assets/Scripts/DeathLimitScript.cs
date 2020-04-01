using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLimitScript : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D objetSorti)
    {
        IDetectOffscreen foundObject = objetSorti.GetComponentInParent<IDetectOffscreen>();
        if (foundObject!=null) foundObject.OffScreen();
    }
}
