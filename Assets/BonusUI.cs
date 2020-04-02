using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusUI : MonoBehaviour
{
    private Image img = null;
    private Color baseColor = Color.white;

    public static BonusUI Instance = null;

    private void Awake() {
        if (!Instance)
            Instance = this;
        img = GetComponent<Image>();
        baseColor = img.color;
    }

    public void UpdateUI(bool hasBonus)
    {
        if (hasBonus) {
            img.color = Color.white;
        } else {
            img.color = baseColor;

        }
    }
}
