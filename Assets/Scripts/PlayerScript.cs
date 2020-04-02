using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IKillable, IStartable
{
    public int extraLives;
    int remainingLives;

    Vector3 spawnPosition;
    Quaternion spawnRotation;
    HealthScript healthScript;
    [HideInInspector] public PlayerMovement moveScript;
    [HideInInspector] public PlayerCanon shootScript;

    private Animator animator;
    public string LeonControllerPath;
    public string MatControllerPath;

    void Awake()
    {
        healthScript = GetComponent<HealthScript>();
        moveScript = GetComponent<PlayerMovement>();
        shootScript = GetComponent<PlayerCanon>();
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        animator = GetComponent<Animator>();

        GAMEMANAGER.access.players.Add(this);
    }


    public void StartNewGame()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        healthScript.Spawn();
        moveScript.canMove = true;
        shootScript.canShoot = false;
    }

    public void Die()
    {
        healthScript.TakeDamage(healthScript.health);
    }

    public void SwapLeonControllerSprite()
    {
        animator.runtimeAnimatorController = Resources.Load(LeonControllerPath) as RuntimeAnimatorController;
    }

    public void SwapMatSprite()
    {
        animator.runtimeAnimatorController = Resources.Load(MatControllerPath) as RuntimeAnimatorController;
    }



    public void LoseLife()
    {
        healthScript.vulnerable = false;
        moveScript.canMove = false;
        shootScript.canShoot = false;
        HealthUI.Instance.UpdateUI(extraLives);
        extraLives -= 1;


        if (extraLives < 0)
        {
            GAMEMANAGER.access.CheckGameOver();
        }
        else
        {
            Invoke("Respawn", 2f);
        }
    }

    public void GameOver()
    {
        extraLives = -1;
        healthScript.TakeDamage(1000);
    }

    public void Respawn()
    {
        if (GAMEMANAGER.access.isPlaying == false) return;
        Debug.Log("RESPAWNING");
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        healthScript.Spawn();
        moveScript.canMove = true;
        shootScript.canShoot = true;
    }
}
