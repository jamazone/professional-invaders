using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour, IKillable
{

    MotherShipScript mother;
    public int scoreBonus;
    SpriteAnimation anim;
    bool halfMove;


    void Awake()
    {
        mother = GetComponentInParent<MotherShipScript>();
        anim = GetComponentInChildren<SpriteAnimation>();
        anim.playOnEnable=false;
        anim.Pause();
        halfMove = false;
    }

   public void ChangeSprite()
    {
        if (anim==null) return;

        if (halfMove)
        {
            halfMove = false;
        }
        else
        {
            if (anim.sprites.Length < 2) halfMove = true;
            anim.NextSprite();
        }
    }

    public void LoseLife()
    {
        GAMEMANAGER.access.AddScore(scoreBonus);
        mother.AlienDeath(this);
        Destroy(this.gameObject);
    }
}
