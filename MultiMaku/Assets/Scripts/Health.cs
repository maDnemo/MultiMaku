using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public bool destroyOnDeath;
    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;


	// Returns whether target is still alive or not
	public void TakeDamage(int amount)
    {
        if (!isServer)
        {
			return;
        }

        Debug.Log("player has been hit by: " + amount);
        Debug.Log("player has this many hp left: " + currentHealth);
        currentHealth = currentHealth - amount;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }

        }
    }

    void OnChangeHealth( int currrentHealth )
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the player’s position to origin
            transform.position = Vector3.zero;
        }
    }
}
