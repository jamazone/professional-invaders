using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerMovement : MonoBehaviour
{
// VARIABLES ---------------------------------------------------------------------
    [HideInInspector]public bool canMove;
    public Vector2 maxSpeed;
    Vector2 movement = Vector2.zero;
    PlayerInput inputScript;
    Rigidbody2D ship;
    float lastUpdateTime;
    Animator animator;
//--------------------------------------------------------------------------------


    void Awake()
    {
        ship = GetComponent<Rigidbody2D>();
        inputScript = GetComponent<PlayerInput>();
        movement = Vector2.zero;
        animator = GetComponent<Animator>();
    }



// BOUCLE DE GAMEPLAY ------------------------------------------------------------

    void Update()
    {
        if (canMove)
            movement.x = Input.GetAxisRaw("Horizontal");
        else
            movement.x = 0;

        animator.SetFloat("Horizontal", movement.x);
        if (movement.x != 0)
        animator.SetFloat("Speed", 1);
        else
        animator.SetFloat("Speed", 0);
    } // FIN DE UPDATE


    void FixedUpdate()
    {
        if (canMove) Move();
    }

 //--------------------------------------------------------------------------------


    void Move()
    {
        Vector2 curPos = transform.position;
        ship.MovePosition(curPos + movement * maxSpeed.x * Time.fixedDeltaTime);
    }




} // FIN DU SCRIPT
