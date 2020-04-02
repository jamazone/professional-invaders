using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    static public HealthUI Instance = null;
    public Sprite fullHeart = null;
    public Sprite emptyHeart = null;
    public Image[] lives = new Image[3];
    
    private void Awake() {
        if (!Instance)
            Instance = this;
    }

    public void UpdateUI(int hp)
    {
        foreach (var item in lives)
        {
            item.sprite = emptyHeart;
        }
        for (int i = 0; i != hp; i++)
            lives[i].sprite = fullHeart;
    }
}
