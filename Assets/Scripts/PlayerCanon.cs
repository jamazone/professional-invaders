using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]

public class PlayerCanon : MonoBehaviour, IStartable
{

// VARIABLES ---------------------------------------------------------------------
    public bool autoFire;
    public ShootType shootStyle;
    public float fireRate;
    public int maxMissilesOnScreen;
    [HideInInspector]public bool canShoot;
    public enum ShootType{fireRate,maxMissilesOnScreen, Both}

    PlayerInput inputs;
    public GameObject missilePrefab;
    public GameObject sfxFire;
    public Transform muzzle;
    [HideInInspector]public Transform myMissiles;
    float fireChrono;
//--------------------------------------------------------------------------------


    void Awake()
    {
        inputs = GetComponent<PlayerInput>();
        myMissiles = new GameObject().transform;
        myMissiles.name = this.gameObject.name + " MISSILES";
        myMissiles.position = Vector3.zero;
    }

    public void StartNewGame()
    {
        ClearAllMyMissiles();
        fireChrono = 0;
    }

    void Mat()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fireRate = 1;
            maxMissilesOnScreen = 1;
            Debug.Log("ça marche");

        }
    }
    void Leon()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            fireRate = 1;
            maxMissilesOnScreen = 1;

        }
    }

    // BOUCLE DE GAMEPLAY ------------------------------------------------------------
    void Update()
    {
        fireChrono -= Time.deltaTime;

        if (canShoot)
        {
            bool wantToShoot = false;

            switch (inputs.controlType)
            {
                case PlayerInput.ControlType.KeyboardOrController:
                    if (autoFire)
                    {
                        if (Input.GetButton(inputs.FireButtonName)) wantToShoot = true;
                    }     
                    else if (Input.GetButtonDown(inputs.FireButtonName)) wantToShoot = true;
                break;


                case PlayerInput.ControlType.Mouse:
                    if (autoFire)
                    {
                        if (Input.GetMouseButton(0)) wantToShoot = true;
                    }     
                    else if (Input.GetMouseButtonDown(0)) wantToShoot = true;
                break;
            }


            if (wantToShoot)
            {
                switch (shootStyle)
                {
                    case ShootType.fireRate:
                        if (fireChrono < 0) FireMissile();
                    break;

                    case ShootType.maxMissilesOnScreen:
                        if (myMissiles.childCount < maxMissilesOnScreen) FireMissile();
                    break;

                    case ShootType.Both:
                        if (fireChrono < 0 && myMissiles.childCount < maxMissilesOnScreen) FireMissile();
                    break;
                }
            }
        }
    } // FIN DE UPDATE
      //--------------------------------------------------------------------------------


   
    void FireMissile()
    {
        fireChrono = 1f/fireRate;

        if (sfxFire) Instantiate(sfxFire, muzzle.position, muzzle.rotation);

        if (missilePrefab)
        {
            GameObject newMissile = Instantiate(missilePrefab, muzzle.position, transform.rotation, myMissiles);
            newMissile.GetComponent<MissileScript>().shooter = this.transform;
        }
        else Debug.Log("Missing prefab. Nothing to shoot!");
    }


    public void ClearAllMyMissiles()
    {
        foreach (Transform missile in myMissiles) Destroy(missile.gameObject);
    }




} // FIN DU SCRIPT
