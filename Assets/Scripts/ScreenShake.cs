using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    public Transform cam;

    public static float shake;




    void Update()
    {
        shake -= (3f*shake*Time.unscaledDeltaTime) + Time.unscaledDeltaTime;
        if (shake < 0) shake = 0;
    }

    void LateUpdate()
    {
        if (shake > 0)
        {
            Vector2 newPos = Random.insideUnitCircle * shake/10f;
            newPos.y /= 2f;
            cam.position = newPos;
        }
        else ResetCamera();
    }

    public static void AddShake(float amount)
    {
        if (amount > shake) shake = amount;
    }



    void ResetCamera()
    {
        cam.position = Vector3.zero;
    }
}
