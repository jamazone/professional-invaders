using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PLAYER")) {
            other.GetComponentInParent<PlayerCanon>().HasSuperPower = true;
            BonusUI.Instance.UpdateUI(true);
            Destroy(this.gameObject);
        }
    }
}
