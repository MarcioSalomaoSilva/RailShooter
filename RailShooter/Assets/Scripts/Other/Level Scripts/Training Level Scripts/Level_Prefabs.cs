using UnityEngine;
using System.Collections;
//
[RequireComponent(typeof(Level_Spawner))]
//
public class Level_Prefabs : MonoBehaviour {
	//
	public static Level_Prefabs instance;
	//
	public Transform[] obsPrefabs = new Transform[9];
	//
	public Transform randomObsPrefab;
	//
	public Transform obsPrefab1;
	public Transform obsPrefab2;
	public Transform obsPrefab3;
	public Transform obsPrefab4;
	public Transform obsPrefab5;
	public Transform obsPrefab6;
	public Transform obsPrefab7;
	public Transform obsPrefab8;
	public Transform obsPrefab9;
	//
	public Transform[] metPrefabs = new Transform[9];
	//
	public Transform randomMetPrefab;
	//
	public Transform metPrefab1;
	public Transform metPrefab2;
	public Transform metPrefab3;
	public Transform metPrefab4;
	public Transform metPrefab5;
	public Transform metPrefab6;
	public Transform metPrefab7;
	public Transform metPrefab8;
	public Transform metPrefab9;
	//
	private int randomNumber;
	// Use this for initialization
	void Awake () {
		//
		instance = this;
		//
		obsPrefabs[0] = obsPrefab1;
		obsPrefabs[1] = obsPrefab2;
		obsPrefabs[2] = obsPrefab3;
		obsPrefabs[3] = obsPrefab4;
		obsPrefabs[4] = obsPrefab5;
		obsPrefabs[5] = obsPrefab6;
		obsPrefabs[6] = obsPrefab7;
		obsPrefabs[7] = obsPrefab8;
		obsPrefabs[8] = obsPrefab9;
		//
		metPrefabs[0] = metPrefab1;
		metPrefabs[1] = metPrefab2;
		metPrefabs[2] = metPrefab3;
		metPrefabs[3] = metPrefab4;
		metPrefabs[4] = metPrefab5;
		metPrefabs[5] = metPrefab6;
		metPrefabs[6] = metPrefab7;
		metPrefabs[7] = metPrefab8;
		metPrefabs[8] = metPrefab9;
//		randomNumber = Random.Range(0,prefabs.Length);
//		randomPrefab = prefabs[randomNumber];
	}
	// Update is called once per frame
	void Update () {
		//
		randomNumber = Random.Range(0,obsPrefabs.Length);
		//
		randomObsPrefab = obsPrefabs[randomNumber];
		//
		randomMetPrefab = metPrefabs[randomNumber];
	}
}
