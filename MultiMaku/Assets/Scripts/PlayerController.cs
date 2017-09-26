using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController: NetworkBehaviour {
    public bool focus = true;
	public double rateOfPrimaryFire = 0.3;
    public double rateOfSecondaryCharge = 1.5;
    public double charge = 0;
    public double maxCharge = 4;
    public double bombCount = 3;
    private double nextPrimaryFire;
    private double nextSecondaryCharge;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
	public AudioClip hitSound;
	public AudioClip shootSound;
	public AudioClip chargeSound;
	public AudioClip bombSound;
	private AudioSource source;
	private AudioSource source2;
	private AudioSource source3;
	private AudioSource source4;
    public Text hpText;
    public Text chargeText;

	public float speed = 5.0f;


	// Use this for initialization
	void Start () {
        nextPrimaryFire = Time.time;
        nextSecondaryCharge = Time.time;
		source = GetComponent<AudioSource> ();
		source2 = GetComponent<AudioSource> ();
		source3 = GetComponent<AudioSource> ();
		source4 = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer)
        {
            return;
        }
        
        if (Input.GetButton("Fire1") && Time.time > nextPrimaryFire)
        {
            nextPrimaryFire = Time.time + rateOfPrimaryFire;
            FirstFire();
        }

        if (Time.time > nextSecondaryCharge)
        {
            if (charge < maxCharge)
            {
                charge = charge + 1;
                nextSecondaryCharge = Time.time + rateOfSecondaryCharge;
            }
        }

        if (Input.GetKeyDown("c") && charge > 0)
        {
            SecondFire();
            charge = charge - 1;
        }

        if (Input.GetKeyDown("space") && bombCount > 0)
        {
            ThirdFire();
            bombCount = bombCount - 1;
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Move();
    }
        // Move and rotate character
    void Move()
	{
		//var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

		if (Input.GetButton("Fire3"))
		{
			x = x * (float)0.5;
			z = z * (float)0.5;
		}

		//transform.Rotate(0, 0, -x);
		transform.Translate(x, 0, 0);
		transform.Translate(0, z, 0);
	}

    void FirstFire()
    {
        CmdFireBullet(new Vector3(0, 6, 0), new Vector3(.25f, 0, 0));
        CmdFireBullet(new Vector3(0, 6, 0), new Vector3(-.25f, 0, 0));
		source2.PlayOneShot(shootSound,1F);
    }

    void SecondFire()
    {
        CmdFireBullet(new Vector3(1, 6, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(-1, 6, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(2, 6, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(-2, 6, 0), new Vector3(0, 0, 0));
		source3.PlayOneShot(chargeSound,1F);
    }

    void ThirdFire()
    {
        CmdFireBullet(new Vector3(-3, 3, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(-2, 4, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(-2, 5, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(-1, 5, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(-1, 6, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(-1, 7, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(0, 6, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(0, 10, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(3, 3, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(2, 4, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(2, 5, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(1, 5, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(1, 6, 0), new Vector3(0, 0, 0));
        CmdFireBullet(new Vector3(1, 7, 0), new Vector3(0, 0, 0));
		source4.PlayOneShot(bombSound,1F);
    }

    [Command]
    void CmdFireBullet(Vector3 directon, Vector3 spawnpoint)
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position + spawnpoint,
            bulletSpawn.rotation);
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = directon;
        // Spawn Bullets
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EBullet"))
        {
            Destroy(collision.gameObject);
            var health = this.GetComponent<Health>();
            if (health != null)
            {
				health.TakeDamage(10);
				source.PlayOneShot(hitSound,1F);
            }
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().material.color = Color.blue;
    }
}
