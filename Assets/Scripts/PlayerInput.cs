using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public enum ControlType { KeyboardOrController, Mouse }
    public ControlType controlType;
    public string HorizontalAxisName, VerticalAxisName, FireButtonName;
    [HideInInspector] public Vector2 move;

    void Awake()
    {
        move = Vector2.zero;
    }


    public Vector2 Movement(float deltaTime)
    {
        switch (controlType)
        {
            case ControlType.Mouse:
                move.x = Input.GetAxisRaw("Mouse X");
                move.y = Input.GetAxisRaw("Mouse Y");
                break;

            case ControlType.KeyboardOrController:
                move.x = Input.GetAxisRaw(HorizontalAxisName);
                move.y = Input.GetAxisRaw(VerticalAxisName);
                if (move.magnitude > 1f) move.Normalize();
                break;
        }
        move *= deltaTime;
        return move;
    }


} // FIN DU SCRIPT
