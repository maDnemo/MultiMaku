using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;
	
    public void TakeDamage(int amount)
    {
        Debug.Log("player has been hit");
        currentHealth = currentHealth - amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("deaded!");
        }
        Debug.Log(currentHealth + " " + healthBar.sizeDelta.y);
    }

    void OnChangeHealth( int currrentHealth )
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }
}
