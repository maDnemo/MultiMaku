using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController: NetworkBehaviour {
    public bool focus = true;
	public double rateOfPrimaryFire = 0.3;
    public double rateOfSecondaryCharge = 1.5;
    public double charge = 0;
    public double maxCharge = 4;  
    private double nextPrimaryFire;
    private double nextSecondaryCharge;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
	// Use this for initialization
	void Start () {
        nextPrimaryFire = Time.time;
        nextSecondaryCharge = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer)
        {
            return;
        }

		Move ();

        
        if (Input.GetButton("Fire1") && Time.time > nextPrimaryFire)
        {
            nextPrimaryFire = Time.time + rateOfPrimaryFire;
            CmdFire();
        }

        if (Time.time > nextSecondaryCharge)
        {
            if (charge < maxCharge)
            {
                charge = charge + 1;
                nextSecondaryCharge = Time.time + rateOfSecondaryCharge;
            }
        }

        if (Input.GetButton("Fire2") && charge > 0)
        {
            CmdSecondFire();
            charge = charge - 1;
        }
    }


	// Move and rotate character
	void Move()
	{
		//var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		if (Input.GetButton("Fire3"))
		{
			x = x * (float)0.5;
			z = z * (float)0.5;
		}

		//transform.Rotate(0, 0, -x);
		transform.Translate(x, 0, 0);
		transform.Translate(0, z, 0);
	}

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        // Left Bullet
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position + new Vector3(-2, 0, 0),
            bulletSpawn.rotation);
        //Right Bullet
        var bullet2 = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position + new Vector3(2, 0, 0),
            bulletSpawn.rotation);
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;
        bullet2.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;

        NetworkServer.Spawn(bullet);
        NetworkServer.Spawn(bullet2);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
        Destroy(bullet2, 2.0f);
    }

    [Command]
    void CmdSecondFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        var bullet2 = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        var bullet3 = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;
        bullet2.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6 + new Vector3(-2, 0, 0);
        bullet3.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6 + new Vector3(2, 0, 0);

        NetworkServer.Spawn(bullet);
        NetworkServer.Spawn(bullet2);
        NetworkServer.Spawn(bullet3);

        // Destroy the bullet after 3 seconds
        Destroy(bullet, 3.0f);
        Destroy(bullet2, 3.0f);
        Destroy(bullet3, 3.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().material.color = Color.blue;
    }
}
