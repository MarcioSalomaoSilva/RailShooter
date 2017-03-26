using UnityEngine;
using System.Collections;
//
public class Player_Manager : MonoBehaviour {
	//
	public static Player_Manager instance;
	//
	[Header("Pathfinding")]
	public Transform target;
    [Header("Player Controller Stuff")]
    public float universalScale = 1;
	public Vector3 startPosition;
	public bool invertedAxis;
	public float controllerDeadZone = 0.25f;
	public float triggerDeadZone = 0.1f;
	public float forwardSpeed = 15;
	public float movementSpeed = 5;
	public float movementSmooth = 5f;
	public float rotationSmooth = 5f;
	public float cameraSmooth = 1;
	public float scopeDistance = 5f;
	public Vector2 horizantalClamp = new Vector2(11,11);
	public Vector2 verticalClamp = new Vector2 (7,7);
	public Vector3 nearTransformPosition;
	public Vector3 farTransformPosition;
    [Header("Camera_Helper Prefabs")]
    public GameObject mainCamera;
	public GameObject cameraReference;
    public float cameraDistance = 5;
    public GUISkin healthBar;
    public GUISkin autopilotBar;
	[Header("Lapyx Prefabs")]
    [HideInInspector]
    public GameObject ship;
    public GameObject hull0;
	public GameObject hullu0;
	public GameObject wingLeft0;
//	public GameObject wingLeft1;
//	public GameObject wingLeft2;
//	public GameObject wingLeft3;
//	public GameObject wingLeft4;
	public GameObject wingRight0;
//	public GameObject wingRight1;
//	public GameObject wingRight2;
//	public GameObject wingRight3;
//	public GameObject wingRight4;
	public GameObject generator0;
	public GameObject engineLeft0;
	public GameObject engineRight0;
	public GameObject cannon0;
	public GameObject lasers0;
	public GameObject exhaustFire;
	public GameObject exhaustSmokeSmall;
	public GameObject exhaustSmokeLarge;
	[Header("Weapons Prefabs")]
	public Rigidbody bombPrefab;
	public Rigidbody laserPrefab;
	public Rigidbody bigLaserPrefab;
	[Header("GUI Textures")]
	[Header("Placement and stuff")]
	public float transparency = 0.3f;
    public Texture testRight;
    public Texture uiBarAutopilot;
    public Texture fontAutopilotOn;
    public Texture fontAutopilotOff;
    [Header("Scopes")]
    public Texture scopeTexture;
    public Texture arrowLeft;
    public Texture arrowRight;
    [Header("Generator")]
	public Texture test;
	public Texture texGenerator;
	public Texture fontGenerator;
	public Texture lineGenerator;
	[Header("Hull")]
	public Texture texShield;
	public Texture fontShield;
	public Texture lineShield;
	public Texture uiBarExShield;
	[Header("Cannon")]
	public Texture texCannon;
	public Texture fontCannon;
	public Texture lineCannon;
	public Texture uiBarExCannon;
	[Header("Lasers")]
	public Texture texWeaponLeft;
	public Texture texWeaponRight;
	public Texture fontWeapon;
	public Texture lineWeaponLeft;
	//public Texture lineWeaponLeft2;
	//public Texture lineWeaponRight;
	public Texture lineWeaponRight2;
	[Header("Engines")]
	public Texture texEngineLeft;
	public Texture texEngineRight;
	public Texture fontEngine;
	public Texture lineEngineLeft;
	//public Texture lineEngineLeft2;
	//public Texture lineEngineRight;
	public Texture lineEngineRight2;
	[Header("Wings")]
	public Texture texWingLeft;
	public Texture texWingRight;
	public Texture fontWing;
	public Texture lineWingLeft;
	//public Texture lineWingLeft2;
	//public Texture lineWingRight;
	public Texture lineWingRight2;
	[Header("Warnings")]
	public Texture texWarningDanger;
	public Texture texWarningCooling;
	[Header("Laser Colours")]
	public Color green = new Color (0f, 1f, 0f, 1f);
	public Color yellow = new Color (1f, 0.92f, 0.016f, 1f);
	[HideInInspector]
	public GameObject weaponC;
	[HideInInspector]
	public GameObject weaponLFar;
	[HideInInspector]
	public GameObject weaponLNear;
	[HideInInspector]
	public GameObject weaponRFar;
    [HideInInspector]
    public GameObject weaponRNear;
    [HideInInspector]
    public GameObject lasers;
    [HideInInspector]
    public GameObject player;
    [Header("Materials")]
	public Material cannonMaterial;
	public Material laserMaterial;
	[Header("Audio")]
	public AudioClip cannonAudio;
	// Use this for initialization
	void Awake () {
		//
		instance = this;
		//
		CreateObjects();
	}
	// Update is called once per frame
	void Update () {
		//
		GetVariables();
	}
	//
	public void GetVariables () {
		//
		nearTransformPosition = this.transform.forward * Player_Manager.instance.scopeDistance;
		farTransformPosition = this.transform.forward * Player_Manager.instance.scopeDistance * 1.5f;
	}
	//
	public void CreateObjects()
	{
		//
		player = new GameObject ("Player");
		player.transform.parent = this.transform;
		player.transform.localPosition = Vector3.zero;
		player.AddComponent<Player_Input> ();
		player.AddComponent<Player_Motor> ();
		//
		GameObject cameras = new GameObject ("Cameras");
		cameras.transform.parent = GameObject.Find("PlayerRig").transform;
		cameras.transform.localPosition = Vector3.zero;
		//
		GameObject sCamera = Instantiate (cameraReference, this.transform.position, Quaternion.identity) as GameObject;
		sCamera.transform.parent = cameras.transform;
		sCamera.transform.localPosition = Vector3.zero;
		//
		GameObject vCamera = Instantiate (mainCamera, this.transform.position, Quaternion.identity) as GameObject;
		vCamera.transform.parent = cameras.transform;
		vCamera.transform.localPosition = Vector3.zero;
		//
		GameObject weapons = new GameObject ("Weapons");
		weapons.transform.parent = player.transform;
		weapons.transform.localPosition = Vector3.zero;
		weapons.AddComponent<Weapon_Controller> ();
		weapons.AddComponent<AudioSource> ();
		//
		weaponC = new GameObject ("Center");
		weaponC.transform.parent = weapons.transform;
		weaponC.transform.localPosition = new Vector3 (0, -0.14f, 0.5f);
		//
		weaponLFar = new GameObject ("farLeft");
		weaponLFar.transform.parent = weapons.transform;
        weaponLFar.transform.localPosition = new Vector3(-0.3466f, 0.020f, 0.06f);
		weaponLNear = new GameObject ("nearLeft");
		weaponLNear.transform.parent = weapons.transform;
        weaponLNear.transform.localPosition = new Vector3(-0.28f, 0.039f, 0.06f);
		//
		weaponRFar = new GameObject ("farRight");
		weaponRFar.transform.parent = weapons.transform;
        weaponRFar.transform.localPosition = new Vector3(0.313f, 0.018f, 0.06f);
		weaponRNear = new GameObject ("nearRight");
		weaponRNear.transform.parent = weapons.transform;
        weaponRNear.transform.localPosition = new Vector3(0.25f, 0.036f, 0.06f);
		//
		GameObject bombs = new GameObject ("Bombs");
		bombs.transform.localPosition = Vector3.zero;
		//
		lasers = new GameObject ("Lasers");
		lasers.transform.localPosition = Vector3.zero;
		//
		ship = new GameObject ("Ship");
		ship.transform.parent = GameObject.Find("Player").transform;
		ship.transform.localPosition = Vector3.zero;
		//
		GameObject hull = Instantiate (hull0, this.transform.position, Quaternion.identity) as GameObject;
		hull.transform.parent = ship.transform;
		hull.transform.localPosition = Vector3.zero;
		//
		GameObject hullu = Instantiate (hullu0, this.transform.position, Quaternion.identity) as GameObject;
		hullu.transform.parent = ship.transform;
		hullu.transform.localPosition = Vector3.zero;
		//
		GameObject topCannon = Instantiate (lasers0, this.transform.position, Quaternion.identity) as GameObject;
		topCannon.transform.parent = ship.transform;
		topCannon.transform.localPosition = Vector3.zero;
		//
		GameObject bottomCannon = Instantiate (cannon0, this.transform.position, Quaternion.identity) as GameObject;
		bottomCannon.transform.parent = ship.transform;
		bottomCannon.transform.localPosition = Vector3.zero;
        //
        GameObject wingLeft = Instantiate (wingLeft0, this.transform.position, Quaternion.identity) as GameObject;
		wingLeft.transform.parent = ship.transform;
		wingLeft.transform.localPosition = Vector3.zero;
        //
        GameObject wingRight = Instantiate (wingRight0, this.transform.position, Quaternion.identity) as GameObject;
		wingRight.transform.parent = ship.transform;
		wingRight.transform.localPosition = Vector3.zero;
        //
        GameObject engineLeft = Instantiate (engineLeft0, this.transform.position, Quaternion.identity) as GameObject;
		engineLeft.transform.parent = ship.transform;
		engineLeft.transform.localPosition = Vector3.zero;
        //
        GameObject engineRight = Instantiate (engineRight0, this.transform.position, Quaternion.identity) as GameObject;
		engineRight.transform.parent = ship.transform;
		engineRight.transform.localPosition = Vector3.zero;
        //
        GameObject generator = Instantiate (generator0, this.transform.position, Quaternion.identity) as GameObject;
		generator.transform.parent = ship.transform;
		generator.transform.localPosition = Vector3.zero;
        //
        GameObject exhausts = new GameObject ("Exhausts");
		exhausts.transform.parent = ship.transform;
		exhausts.transform.localPosition = Vector3.zero;
        //
        GameObject exhaustF = Instantiate (exhaustFire, this.transform.position, Quaternion.identity) as GameObject;
		exhaustF.transform.parent = exhausts.transform;
		exhaustF.transform.localPosition = Vector3.zero;
        //
        GameObject exhaustSS = Instantiate (exhaustSmokeSmall, this.transform.position, Quaternion.identity) as GameObject;
		exhaustSS.transform.parent = exhausts.transform;
		exhaustSS.transform.localPosition = Vector3.zero;
        //
        GameObject exhaustSL = Instantiate (exhaustSmokeLarge, this.transform.position, Quaternion.identity) as GameObject;
		exhaustSL.transform.parent = exhausts.transform;
		exhaustSL.transform.localPosition = Vector3.zero;
    }
	void OnValidate ()
	{
		//
		if (transparency > 1) {
			transparency = 1;
		} else if (transparency < 0) {
			transparency = 0;
		}
	}
}
