using UnityEngine;
using System.Collections;
//
public class Weapon_Controller : MonoBehaviour {
	//
	public static Weapon_Controller instance;
	//
	Rigidbody bombPrefab;
	Rigidbody laserPrefab;
	Rigidbody bigLaserPrefab;
    GameObject center;
    GameObject nearLeft;
    GameObject farLeft;
    GameObject nearRight;
    GameObject farRight;
    bool canFire = true;
    public float velocity = 30;
	public float destroyTimer = 2f;
	// Use this for initialization
	public int gunDamage = 1;
	public float cannonFireRate = .25f;
	public float fireRate = .4f;
	public float weaponRange = 50f;
	public float hitForce = 100f;
	//
	private WaitForSeconds shotDuration = new WaitForSeconds(.01f);
	private AudioSource gunAudio;
	private LineRenderer cannonLine;
	private float nextCannonFire;
	private float nextLaserFire;
	//
    void OnEnable() {
        Player_Input.FireLaser += FireLaser;
        Player_Input.FireCannon += FireCannon;
    }
	void Start () 
	{
		instance = this;
		//
		GetPrefabs ();
		//
		cannonLine = GetComponent<LineRenderer>();
		gunAudio = GetComponent<AudioSource> ();
		gunAudio.clip = Player_Manager.instance.cannonAudio;
	}
	//
    void FireLaser() {
        GetTransforms();
        if (canFire) {
            
            StartCoroutine("LeftGun");
            StartCoroutine("RightGun");
        }
    }
    void FireCannon() {
    }
    void Bullet(GameObject Pos) {
        Rigidbody newBullet = Instantiate(laserPrefab, Pos.transform.position, Pos.transform.rotation) as Rigidbody;
        newBullet.transform.parent = Player_Manager.instance.lasers.transform;
        newBullet.AddForce(Pos.transform.forward * velocity, ForceMode.VelocityChange);
        newBullet.isKinematic = false;
    }
    IEnumerator LeftGun () {
        gunAudio.Play();
        Bullet(nearLeft);
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        gunAudio.Play();
        Bullet(farLeft);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
    IEnumerator RightGun() {
        gunAudio.Play();
        Bullet(nearRight);
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        gunAudio.Play();
        Bullet(farRight);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
    //
    public void GetPrefabs () {
		bombPrefab = Player_Manager.instance.bombPrefab;
		laserPrefab = Player_Manager.instance.laserPrefab;
		bigLaserPrefab = Player_Manager.instance.bigLaserPrefab;
	}
    public void GetTransforms () {
        center = Player_Manager.instance.weaponLNear;
        nearLeft = Player_Manager.instance.weaponLNear;
        farLeft = Player_Manager.instance.weaponLFar;
        nearRight = Player_Manager.instance.weaponRNear;
        farRight = Player_Manager.instance.weaponRFar;
    }
}