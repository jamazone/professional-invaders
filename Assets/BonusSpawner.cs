using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public Transform[] spawnPoints = null;
    public GameObject prefab = null;

    public GameObject spawned = null;
    public float cooldown = 7.5f;
    float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(cooldown, cooldown*1.5f);
    }

    void Spawn()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        spawned = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned) {
            if (timer <= 0) {
                Spawn();
                timer = Random.Range(cooldown, cooldown*1.5f);
            } else {
                timer -= Time.deltaTime;
            }
        }
    }
}
