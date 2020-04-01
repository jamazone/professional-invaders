using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MotherShipScript))]

public class MotherShipWeapons : MonoBehaviour
{
    public GameObject missilePrefab;
    MotherShipScript mother;
    public float minTime;
    public float maxTime;
    float nextShootIn;
    float chrono;

    void Awake()
    {
        mother = GetComponent<MotherShipScript>();
        NextShootDelay();
        chrono = 0;
    }
 

    void Update()
    {
        if (mother.alive)
        {
            chrono +=Time.deltaTime;
            if (chrono > nextShootIn)
            {
                ShootMissile( LowestAlien( RandomColumn() ).position );
                NextShootDelay();
                chrono = 0;
            }
        }
    }

    Transform LowestAlien(Transform column)
    {
        Transform chosenOne = column.GetChild(column.childCount-1);
        return chosenOne;
    }

    Transform RandomColumn()
    {
        Transform chosenOne = transform.GetChild(Random.Range(0,transform.childCount));
        return chosenOne;
    }

    void NextShootDelay()
    {
        nextShootIn = Random.Range(minTime, maxTime);
    }

    void ShootMissile(Vector3 desiredPosition)
    {
        if (missilePrefab) Instantiate(missilePrefab, desiredPosition, missilePrefab.transform.rotation);
    }

}
