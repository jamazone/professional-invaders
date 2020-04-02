﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerMovement : MonoBehaviour
{
// VARIABLES ---------------------------------------------------------------------
    [HideInInspector]public bool canMove;
    public Vector2 maxSpeed;
    Vector2 movement;
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
        {
            movement += inputScript.Movement(Time.deltaTime);
        }
        else movement = Vector2.zero;

        animator.SetFloat("Horizontal", movement.x);
        if (movement.sqrMagnitude > 0)
        animator.SetFloat("Speed", 1);
        else
        animator.SetFloat("Speed", 0);
        lastUpdateTime = Time.time;
    } // FIN DE UPDATE


    void FixedUpdate()
    {
        movement += inputScript.Movement(Time.time - lastUpdateTime);
        if (canMove) Move();
    }

 //--------------------------------------------------------------------------------




    void ApplySpeed()
    {
        movement.x *= maxSpeed.x;
        movement.y *= maxSpeed.y;
    }


    void Move()
    {
        ApplySpeed();
        Vector2 curPos = transform.position;
        ship.MovePosition(curPos + movement);
        movement = Vector2.zero;
    }




} // FIN DU SCRIPT
