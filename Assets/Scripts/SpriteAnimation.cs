using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    public bool playOnEnable;
    [Range(1,60)]public int FPS;
    public LoopingType loopType;
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    public enum LoopingType {Restart, Yoyo, Pause, AutoDestroy}
    int index;
    float chrono;
    float sens;
    bool paused;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        index = 0;
        sens = 1f;
        paused = true;
        if (FPS<1) FPS=1;
    }



    public void Play()
    {
        paused = false;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Restart()
    {
        index = 0;
    }

    void OnEnable()
    {
        if (loopType==LoopingType.Pause) Restart();
        if (playOnEnable) Play();
    }


    void Update()
    {
        if (paused==false)
        {
            chrono += Time.deltaTime;
            if (chrono > 1f/FPS)
            {
                NextSprite();
            }
        }
    }

    public void NextSprite()
    {
        if (sprites.Length<2) return;

        if (sens>0)index ++; else index--;

        if (index >= sprites.Length)
        {
            switch (loopType)
            {
                case LoopingType.Restart:
                    Restart();
                break;
                
                case LoopingType.Yoyo:
                    sens = -1f;
                    index = sprites.Length-2;
                break;

                case LoopingType.Pause:
                    index=sprites.Length-1;
                    Pause();
                break;
                
                case LoopingType.AutoDestroy:
                    Destroy(this.gameObject);
                break;
            }
        }
        if (index < 0)
        {
            sens = 1f;
            index = 1;
        }

        spriteRenderer.sprite = sprites[index];
        chrono = 0;
    }
}
