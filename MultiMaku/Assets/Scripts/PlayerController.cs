using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController: NetworkBehaviour {
    public double rateOfPrimaryFire = 0.3;
    public double rateOfSecondaryFire = 3.0;
    private double nextPrimaryFire;
    private double nextSecondaryFire;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
	// Use this for initialization
	void Start () {
        nextPrimaryFire = Time.time;
        nextSecondaryFire = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer)
        {
            return;
        }

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

        if (Input.GetButton("Fire1") && Time.time > nextPrimaryFire)
        {
            nextPrimaryFire = Time.time + rateOfPrimaryFire;
            CmdFire();
        }

        if (Input.GetButton("Fire2") && Time.time > nextSecondaryFire)
        {
            nextSecondaryFire = Time.time + rateOfSecondaryFire;
            CmdSecondFire();
        }
    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;

        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    [Command]
    void CmdSecondFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;

        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().material.color = Color.blue;
    }
}
