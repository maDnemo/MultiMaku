using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BasicAI : NetworkBehaviour
{
    public GameObject player;
    public double rateOfPrimaryFire;
    public double nextPrimaryFire;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
	public AudioClip hitSound;
	private AudioSource source;

	public float speed = 2.0f;
	private Vector3 heading = new Vector3 (1, 0, 0);
	private Vector3 displacement = new Vector3 ();
	private float arc = 0;
	private float arcSize = 1.5f;

    // Use this for initialization
    void Start ()
	{
        nextPrimaryFire = Time.time + 5;
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		move ();
        if (Time.time > nextPrimaryFire)
        {
            nextPrimaryFire = Time.time + rateOfPrimaryFire;
            CmdFire();
        }
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
				source.PlayOneShot(hitSound,1F);
			} 
		}
	}

	void move ()
	{
		arc -= Mathf.PI / 6 * arcSize * Time.deltaTime;
		if (arc > Mathf.PI * 2) {
			arc -= Mathf.PI * 2;
		}

		heading.x = Mathf.Cos (arc);
		heading.y = Mathf.Sin (arc);


		displacement.x = Time.deltaTime * heading.x * speed;
		displacement.y = Time.deltaTime * heading.y * speed;
		transform.Translate (displacement);



	}

    [Command]
    void CmdFire()
    {
        player = GameObject.FindWithTag("Player");
        Debug.Log(player.ToString());
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        Vector3 directon = player.transform.position - bullet.transform.position;
        directon = directon / directon.magnitude;
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = directon * 6;
        // Ignore Collision
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        // Spawn Bullets
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
