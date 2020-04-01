using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MissileScript : MonoBehaviour, IDetectOffscreen
{
    public enum Side{Allied, Enemy}
    public Side missileSide;
    public float speed;
    [Range(1,10)]public int damage;
    [Range(0.1f, 10f)]public float maxLifetime;
    public GameObject sfxImpact;
    Rigidbody2D thisMissile;
    Vector3 lastPos;
    [HideInInspector]public Transform shooter;
    RaycastHit2D[] hits;
    //bool touchedSomething;

    void Awake()
    {
        thisMissile = GetComponent<Rigidbody2D>();
        //touchedSomething = false;
    }


    void Start()
    {
        thisMissile.AddForce(transform.up * speed, ForceMode2D.Impulse);
        Destroy(this.gameObject, maxLifetime);
    }


    void Update()
    {
        hits = Physics2D.RaycastAll(lastPos, transform.up, (transform.position - lastPos).magnitude);

        if (hits!=null)
        {
            for (int i=0 ; i< hits.Length; i++)
            {
                Rigidbody2D touchedRb = hits[i].collider.attachedRigidbody;
                Transform touchedObject = hits[i].collider.transform;

                if (touchedRb==null)
                {
                    if (touchedObject.CompareTag("WALL")) return;

                    if (touchedObject.CompareTag("SHIELD"))
                    {
                        Collider2D[] cratere = Physics2D.OverlapCircleAll(hits[i].point,Random.Range(0.2f,0.5f));

                        foreach (Collider2D victim in cratere)
                        {
                            if (victim.CompareTag("SHIELD"))
                            {
                                IDamageable target = victim.GetComponentInParent<IDamageable>();
                                if (target!=null) target.TakeDamage(damage);
                            }
                        }
                        transform.position = hits[i].point;
                        i=hits.Length;
                        MissileExplodes();
                    }
                    else
                    {
                        IDamageable target = touchedObject.GetComponentInParent<IDamageable>();
                        if (target!=null) target.TakeDamage(damage);
                        else Debug.Log("target is null");
                        transform.position = hits[i].point;
                        i=hits.Length;
                        MissileExplodes();
                    }
                }
                else if (touchedRb.transform != shooter && touchedRb.transform != this.transform)
                {
                        if (touchedObject.CompareTag("ENEMY") && missileSide==Side.Allied)
                        {
                            IDamageable target = touchedObject.GetComponentInParent<IDamageable>();
                            if (target!=null) target.TakeDamage(damage);
                            transform.position = hits[i].point;
                            i=hits.Length;
                            MissileExplodes();
                        }
                        if (touchedObject.CompareTag("PLAYER") && missileSide==Side.Enemy)
                        {
                            IDamageable target = touchedObject.GetComponentInParent<IDamageable>();
                            if (target!=null) target.TakeDamage(damage);
                            transform.position = hits[i].point;
                            i=hits.Length;
                            MissileExplodes();
                        }
                }

            }
        }

    } // FIN DE UPDATE


    void LateUpdate()
    {
        lastPos = transform.position;
    }

    public void OffScreen()
    {
        StartCoroutine(AutoDestroyNextFrame());
    }

    IEnumerator AutoDestroyNextFrame()
    {
        yield return 1;
        Destroy(this.gameObject);
    }

    void MissileExplodes()
    {
        if (sfxImpact) Instantiate(sfxImpact, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }


}
