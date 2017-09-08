using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    void Start()
    {
        Debug.Log("Im alive!");
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I hit a player :)");
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(1);
        }
        Destroy(gameObject);
    }
}
