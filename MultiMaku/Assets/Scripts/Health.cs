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


	// Returns whether target is still alive or not
	public bool TakeDamage(int amount)
    {
        if (!isServer)
        {
			return false;
        }

		bool alive = true;
        Debug.Log("player has been hit by: " + amount);
        Debug.Log("player has this many hp left: " + currentHealth);
        currentHealth = currentHealth - amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Something has died!");
			alive = false;

        }
		return alive;
    }

    void OnChangeHealth( int currrentHealth )
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }
}
