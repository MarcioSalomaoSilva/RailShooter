using UnityEngine;
using System.Collections;
//
[RequireComponent(typeof(Rigidbody))]
//
public class Asteroid_Controller : MonoBehaviour {
	//
	public Rigidbody rb;
	//
	private float velocity;
	//private float deathDistance;
	//private float randomSpeed;
	// Use this for initialization
	void Awake () {
		//
		//deathDistance = Level_Spawner.instance.fieldSpawnDistance;
		//
		rb = this.GetComponent<Rigidbody>();
		//get random values
		velocity = Level_Spawner.instance.obstacleVelocity;
		//randomSpeed = Random.Range (0.75f, 1.5f);
		//rigidbody.AddForce (-Vector3.forward * velocity * 20, ForceMode.VelocityChange);
		//velocity *= Time.deltaTime;
	}
	// Update is called once per frame
	void Update () {
		//positon movement
		//this.transform.Translate (-Vector3.forward * velocity * randomSpeed, Space.World);
		rb.AddForce (-Vector3.forward * velocity , ForceMode.VelocityChange);
		//rotation
		this.transform.RotateAround (this.transform.position, new Vector3 (Random.Range(0,2),Random.Range(0,2),Random.Range(0,2)), 20 * Time.deltaTime);
		//destroy self
		if (this.transform.position.z < -20) {
			Destroy (this.gameObject);
		}
	}
	//
	void OnValidate () {
		//
		if (velocity < 0) {
			velocity = 0;
		}
	}
	//
	void OnCollisionEnter (Collision col) {
		//
		if (col.gameObject.tag == "Laser") {
			//
			if (this.transform.localScale.x > 3) {
				//
				Transform getPrefab = Level_Prefabs.instance.obsPrefabs [Random.Range (0, 9)];
				float randomNumber = Random.Range (0.1f, 0.3f);
				getPrefab.localScale = new Vector3(randomNumber, randomNumber, randomNumber);
				Transform newObstacle1 = Instantiate (getPrefab, this.transform.position , Quaternion.identity) as Transform;
				newObstacle1.parent = GameObject.Find("Chunks").transform;
				Transform newObstacle2 = Instantiate (getPrefab, this.transform.position , Quaternion.identity) as Transform;
				newObstacle2.parent = GameObject.Find("Chunks").transform;
				Transform newObstacle3 = Instantiate (getPrefab, this.transform.position , Quaternion.identity) as Transform;
				newObstacle3.parent = GameObject.Find("Chunks").transform;
			}
			Destroy (this.gameObject);
		}

	}
}
