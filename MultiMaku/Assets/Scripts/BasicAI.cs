using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter2D (Collider2D collision)
	{
		if (!collision.gameObject.CompareTag ("EBullet"))
        {
			Debug.Log ("Hit by a player :(");
			Destroy (collision.gameObject);
			var health = this.GetComponent<Health> ();
			if (health != null)
            {
                health.TakeDamage(10);
			} 

		}
	}
}
