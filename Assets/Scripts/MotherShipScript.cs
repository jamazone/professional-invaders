using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MotherShipScript : MonoBehaviour, IStartable
{
    // VARIABLES ---------------------------------------------------------------
    public bool stepMovement;
    [Range(0.5f, 5f)] public float startHorizontalSpeed;
    [Range(5f, 60f)] public float lastHorizontalSpeed;
    [Range(0.1f, 1f)] public float verticalSpeed;
    [Range(2f, 100f)] public float alienSpawningSpeed;
    float speedStep;
    public int columns;
    public GameObject[] rowsAlienPrefabs;
    [Range(0.5f, 5f)] public float columnWidth, rowHeight;
    Rigidbody2D aliensMovement;
    float sens = 1f;
    [HideInInspector] public bool alive;
    AlienScript[] myAliens;
    int aliensAlive;
    int aliensTotal;
    float currentSpeed;
    float chrono;
    bool wantToGoDown;
    // ---------------------------------------------------------------



    // DEBUT DU JEU ---------------------------------------------------------------
    void Awake()
    {
        columns = Mathf.Abs(columns);
        aliensMovement = GetComponent<Rigidbody2D>();
        GAMEMANAGER.access.motherships.Add(this);
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }

    public void StartNewGame()
    {
        alive = false;
        chrono = 0;
        wantToGoDown = false;
        currentSpeed = startHorizontalSpeed;

        if (columns + rowsAlienPrefabs.Length > 0)
        {
            DestroyAllAliens();
            GenerateColumns();
        }

        StartCoroutine(GenerateAliens(columns, rowsAlienPrefabs, 1f / alienSpawningSpeed));
    }
    // ---------------------------------------------------------------



    // BOUCLE DE JEU ---------------------------------------------------------------
    void Update()
    {
        if (GAMEMANAGER.access.isPlaying && alive && stepMovement)
        {
            chrono += Time.deltaTime;

            if (chrono > 1f / currentSpeed)
            {
                AlienMove();
                chrono = 0;
            }
        }
    }


    void FixedUpdate()
    {
        if (GAMEMANAGER.access.isPlaying && alive && stepMovement == false)
        {
            chrono += Time.fixedDeltaTime;

            float alienStep = 0.5f;
            if (aliensAlive < aliensTotal / 20f) alienStep = 0.75f;
            if (aliensAlive < aliensTotal / 10f) alienStep = 1.25f;

            if (chrono > 1f / currentSpeed)
            {
                foreach (AlienScript alien in myAliens) alien.ChangeSprite();
                chrono = 0;
            }

            if (wantToGoDown) AliensGoDown();
            else aliensMovement.MovePosition(transform.position + (alienStep * Vector3.right * sens * Time.fixedDeltaTime));
        }
    }

    // ---------------------------------------------------------------



    void AlienMove()
    {
        foreach (AlienScript alien in myAliens) alien.ChangeSprite();

        float alienStep = 0.5f;
        if (aliensAlive < aliensTotal / 20f) alienStep = 0.75f;
        if (aliensAlive < aliensTotal / 10f) alienStep = 1.25f;

        if (wantToGoDown) AliensGoDown();
        else aliensMovement.MovePosition(transform.position + (alienStep * Vector3.right * sens));
    }

    void AliensGoDown()
    {
        transform.Translate(Vector3.down * verticalSpeed);
        wantToGoDown = false;
    }





    public void AlienDeath(AlienScript deadAlien)
    {
        if (deadAlien.transform.parent.childCount < 2)
        {

            Destroy(deadAlien.transform.parent.gameObject);
        }
        aliensAlive -= 1;
        ComputeNewSpeed();
        if (alive && aliensAlive < 1) Defeated();
    }





    void ComputeNewSpeed()
    {
        speedStep = (lastHorizontalSpeed - startHorizontalSpeed) / (aliensTotal - 1);
        currentSpeed += speedStep;
        //Debug.Log("New Alien Speed= "+currentSpeed);
    }




    void CountAliens()
    {
        myAliens = transform.GetComponentsInChildren<AlienScript>();
        aliensAlive = myAliens.Length;
    }


    void GenerateColumns()
    {

        if (columns + rowsAlienPrefabs.Length > 0)
        {
            for (int i = 0; i < columns; i++)
            {
                GameObject newColumn = new GameObject();
                newColumn.transform.position = transform.position + Vector3.right * i * columnWidth;
                newColumn.transform.parent = this.transform;
                newColumn.name = "Column" + (i + 1);
            }
        }
        else
        {

        }
    }


    IEnumerator GenerateAliens(int columns, GameObject[] rowsAlienPrefabs, float delay)
    {
        if (columns + rowsAlienPrefabs.Length > 0)
        {
            CenterColumns();

            for (int c = 0; c < columns; c++)
            {
                Transform thisColumn = transform.GetChild(c).transform;

                for (int r = 0; r < rowsAlienPrefabs.Length; r++)
                {
                    Vector3 newPos = thisColumn.position - Vector3.up * r * rowHeight;
                    Instantiate(rowsAlienPrefabs[r], newPos, thisColumn.rotation, thisColumn).GetComponent<HealthScript>().Spawn();
                    yield return new WaitForSeconds(delay);
                }
            }
        }

        CountAliens();
        aliensTotal = aliensAlive;
        alive = true;
    }

    void CenterColumns()
    {
        float offset = (columns - 1) * columnWidth / 2f;

        foreach (Transform column in transform)
        {
            column.position -= Vector3.right * offset;
        }
    }


    void DestroyAllAliens()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }


    void Defeated()
    {
        alive = false;
        GAMEMANAGER.access.motherships.Remove(this);
        GAMEMANAGER.access.CheckVictory();
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        // CHANGEMENT DE SENS AU BORD DE L'ECRAN
        if (col.transform.CompareTag("WALL") && Mathf.Sign(transform.position.x) == Mathf.Sign(sens))
        {
            sens = -sens;
            wantToGoDown = true;
        }

    }


    void OnTriggerEnter2D(Collider2D objetTouche)
    {
        if (objetTouche.CompareTag("SHIELD"))
        {
            objetTouche.GetComponentInParent<IDamageable>().TakeDamage(1000);
        }

        if (objetTouche.CompareTag("DEFEATLIMIT"))
        {
            GAMEMANAGER.access.GameOver();
        }

        if (objetTouche.CompareTag("PLAYER"))
        {
            objetTouche.GetComponentInParent<PlayerScript>().GameOver();
        }
    }





} // FIN DU SCRIPT