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
    public bool isBigOne = false;
    public float BigOneRadius = 5;
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

    private void OnTriggerEnter2D(Collider2D other) {
        Vector2 point = transform.position;
        Rigidbody2D touchedRb = other.attachedRigidbody;
        Transform touchedObject = other.transform;

        if (touchedRb==null)
        {
            if (touchedObject.CompareTag("WALL")) return;

            if (touchedObject.CompareTag("SHIELD"))
            {
                Collider2D[] cratere = Physics2D.OverlapCircleAll(point,Random.Range(0.2f,0.5f));

                foreach (Collider2D victim in cratere)
                {
                    if (victim.CompareTag("SHIELD"))
                    {
                        IDamageable target = victim.GetComponentInParent<IDamageable>();
                        if (target!=null) target.TakeDamage(damage);
                    }
                }
                transform.position = point;
                MissileExplodes();
            }
            else
            {
                IDamageable target = touchedObject.GetComponentInParent<IDamageable>();
                if (target!=null) target.TakeDamage(damage);
                else Debug.Log("target is null");
                transform.position = point;
                MissileExplodes();
            }
        }
        else if (touchedRb.transform != shooter && touchedRb.transform != this.transform)
        {
                if (touchedObject.CompareTag("ENEMY") && missileSide==Side.Allied)
                {
                    if (isBigOne) {
                        Collider2D[] cratere = Physics2D.OverlapCircleAll(point,BigOneRadius);

                        foreach (Collider2D victim in cratere)
                        {
                            if (victim.CompareTag("ENEMY") || victim.CompareTag("SHIELD"))
                            {
                                IDamageable target = victim.GetComponentInParent<IDamageable>();
                                if (target!=null) target.TakeDamage(damage);
                            }
                        }
                    } else {

                        IDamageable target = touchedObject.GetComponentInParent<IDamageable>();
                        if (target!=null) target.TakeDamage(damage);
                    }
                    transform.position = point;
                    MissileExplodes();
                }
                if (touchedObject.CompareTag("PLAYER") && missileSide==Side.Enemy)
                {
                    IDamageable target = touchedObject.GetComponentInParent<IDamageable>();
                    if (target!=null) target.TakeDamage(damage);
                    transform.position = point;
                    MissileExplodes();
                }
        }
    }


    void Update()
    {
        hits = Physics2D.RaycastAll(lastPos, transform.up, (transform.position - lastPos).magnitude);

        if (hits!=null)
        {
            for (int i=0 ; i< hits.Length; i++)
            {


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
