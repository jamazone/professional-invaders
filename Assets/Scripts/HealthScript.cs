using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour, IDamageable, IStartable
{
    [Range(1,10)]public int maxHealth;
    public GameObject sfxDie, sfxSpawn;
    [HideInInspector]public int health;
    [HideInInspector]public bool vulnerable;
    IKillable lifeHandler;
    GameObject shape;
    GameObject colliders;


    void Awake()
    {
        lifeHandler = GetComponentInParent<IKillable>();
        shape = transform.Find("GRAPHICS").gameObject;
        colliders = transform.Find("COLLIDERS").gameObject;
        shape.SetActive(false);
        colliders.SetActive(false);
        vulnerable = false;
    }


    public void StartNewGame()
    {
        Spawn();
    }


    public void Spawn()
    {
        if (sfxSpawn) Instantiate(sfxSpawn, transform.position, transform.rotation);
        health = maxHealth;
        shape.SetActive(true);
        colliders.SetActive(true);
        vulnerable = true;
    }


    public void TakeDamage(int amount)
    {
        if (vulnerable == false) return;
        health -= amount;
        if (health < 1) Die();
    }


    public void Die()
    {
        health = 0;
        shape.SetActive(false);
        colliders.SetActive(false);
        if (sfxDie) Instantiate(sfxDie, transform.position, transform.rotation);
        if (lifeHandler == null) Destroy(this.gameObject); else lifeHandler.LoseLife();
    }


} // FIN DU SCRIPT
