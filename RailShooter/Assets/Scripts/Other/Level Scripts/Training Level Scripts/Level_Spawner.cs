using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//
[RequireComponent(typeof(Level_Prefabs))]
//
public class Level_Spawner : MonoBehaviour {
	//
	public static Level_Spawner instance;
	//
	public bool spawnObstacles = true;
	public Vector2 obstacleFieldSize = new Vector2(30,30);
	//public Transform prefab;
	public int obsSpawnDistance;
	public int obstacleDensity;
	public float obstacleVelocity;
	public float spawnRate = 2f;
	private float curSpawnRate;
	public bool spawnField = true;
	public Vector2 meteorFieldSize = new Vector2(100,100);
	public Vector2 meteorSpawnSize = new Vector2(1,100);
	public int fieldSpawnDistance;
	public int fieldDensity;
	public int fieldDepth;
	private int mapSeed;
	//
	Transform prefabHolder;
	Transform chunksHolder;
	//
	List<Coord> obsTileCoords;
	List<Coord> metTileCoords;
	Queue<Coord> shuffledObsCoords;
	Queue<Coord> shuffledMetCoords;
	// Use this for initialization
	void Start () 
	{
		//
		instance = this;
		//prefab = Level_Prefabs.instance.randomPrefab;
		//
		GenerateSeed ();
		//
		InitializeGeneration ();
		//
		GenerateMeteorBelt ();
		//GenerateObstacles ();
		//
	}
	// Update is called once per frame
	void Update () 
	{
		//
		SpawnTime ();
		//
		//prefab = Level_Prefabs.instance.randomObsPrefab;
		//
		//GenerateMeteorBelt ();
	}
	//
	public void GenerateSeed ()
	{
		//
		mapSeed =  Random.Range(1,100000);
		//
	}
	//
	public void InitializeGeneration ()
	{
		//
		curSpawnRate = spawnRate;
		//
		obstacleFieldSize.x = Mathf.RoundToInt(obstacleFieldSize.x);
		obstacleFieldSize.y = Mathf.RoundToInt (obstacleFieldSize.y);
		meteorFieldSize.x = Mathf.RoundToInt(meteorFieldSize.x);
		meteorFieldSize.y = Mathf.RoundToInt (meteorFieldSize.y);
		//
		obsTileCoords = new List<Coord> ();
		for (int x = 0; x < obstacleFieldSize.x; x++)
		{
			for (int y = 0; y < obstacleFieldSize.y; y++) 
			{
				//
				obsTileCoords.Add (new Coord (x, y));
			}
		}
		//
		metTileCoords = new List<Coord> ();
		for (int x = 0; x < meteorFieldSize.x; x++)
		{
			for (int y = 0; y < meteorFieldSize.y; y++) 
			{
				//
				metTileCoords.Add (new Coord (x, y));
			}
		}
		//
		shuffledObsCoords = new Queue<Coord>(Utility.ShuffleArray(obsTileCoords.ToArray(), mapSeed));
		shuffledMetCoords = new Queue<Coord>(Utility.ShuffleArray(metTileCoords.ToArray(), mapSeed));
		//
		//noiseMap = Noise.GenerateNoiseMap (Mathf.RoundToInt (coordMapSize.x), Mathf.RoundToInt (coordMapSize.y), mapSeed, scale, octaves, persistance, lacunarity, offset);
		//
		string holderName = "Obstacles";
		if (transform.FindChild (holderName)) {
			DestroyImmediate (transform.FindChild (holderName).gameObject);
		}
		//
		prefabHolder = new GameObject (holderName).transform;
		prefabHolder.parent = transform;
		prefabHolder.position = Vector3.zero;
		//
		string holderName2 = "Chunks";
		if (transform.FindChild (holderName2)) {
			DestroyImmediate (transform.FindChild (holderName2).gameObject);
		}
		//
		chunksHolder = new GameObject (holderName2).transform;
		chunksHolder.parent = transform;
		chunksHolder.position = Vector3.zero;
	}
	//
	public void SpawnTime ()
	{
		//
		curSpawnRate -=  1 * Time.deltaTime;
		//
		if (curSpawnRate <= 0) {
			//
			GenerateObstacles();
			curSpawnRate = spawnRate;
		}
	}
	//
	public void GenerateMeteorBelt ()
	{
		if (spawnField) {
			//
			string holderMapName = "Meteor Field";
			if (transform.FindChild (holderMapName)) {
				DestroyImmediate (transform.FindChild (holderMapName).gameObject);
			}
			//
			Transform spawnPosition = new GameObject (holderMapName).transform;
			spawnPosition.parent = this.transform;
	//		spawnPosition.position = Vector3.zero;
			//
			for (int h = 0; h < fieldDepth; h++) {
				for (int i = 0; i < fieldDensity; i++) {
					//
					Transform getPrefab = Level_Prefabs.instance.metPrefabs [Random.Range (0, 9)];
					float randomNumber = Random.Range (meteorSpawnSize.x, meteorSpawnSize.y);
					getPrefab.localScale = new Vector3(randomNumber, randomNumber, randomNumber);
					Coord randomCoord = GetRandomMetCoord ();
					Vector3 obstaclePosition = MeteorCoordToPosition (randomCoord.x, randomCoord.y, fieldSpawnDistance + h*2);
					Transform newObstacle = Instantiate (getPrefab, obstaclePosition , Quaternion.identity) as Transform;
					newObstacle.parent = spawnPosition.transform;
				}
			}
		}
	}
	//
	public void GenerateObstacles()
	{
		if (spawnObstacles) 
		{
			//
			for (int i = 0; i < obstacleDensity ; i++) 
			{
				Transform getPrefab = Level_Prefabs.instance.obsPrefabs [Random.Range (0, 9)];
				float randomNumber = Random.Range (1f, 10f);
				getPrefab.localScale = new Vector3(randomNumber, randomNumber, randomNumber);
				Coord randomCoord = GetRandomObsCoord ();
				Vector3 obstaclePosition = ObstacleCoordToPosition (randomCoord.x, randomCoord.y, obsSpawnDistance);
				Transform newObstacle = Instantiate (getPrefab, obstaclePosition , Quaternion.identity) as Transform;
				newObstacle.parent = prefabHolder.transform;
			}
		}
	}
	//helpers
	Vector3 ObstacleCoordToPosition(int x, int y, float height)
	{
		return new Vector3 (-obstacleFieldSize.x / 2 + 0.5f + x, -obstacleFieldSize.y / 2 + 0.5f + y, height);
	}
	//
	Vector3 MeteorCoordToPosition (int x, int y, float height)
	{
		return new Vector3 (-meteorFieldSize.x / 2 + 0.5f + x, -meteorFieldSize.y / 2 + 0.5f + y, height);
	}
	//
	public Coord GetRandomObsCoord()
	{
		//gets first item
		Coord randomCoord = shuffledObsCoords.Dequeue ();
		//adds to the end of the queue
		shuffledObsCoords.Enqueue (randomCoord);
		return randomCoord;
	}
	public Coord GetRandomMetCoord()
	{
		//gets first item
		Coord randomCoord = shuffledMetCoords.Dequeue ();
		//adds to the end of the queue
		shuffledMetCoords.Enqueue (randomCoord);
		return randomCoord;
	}
	//
	public struct Coord 
	{
		//
		public int x;
		public int y;
		//
		public Coord (int _x, int _y)
		{
			x = _x;
			y = _y;
		}
	}
	//
	void OnValidate()
	{
		//
		if (obstacleFieldSize.x < 1) 
		{
			obstacleFieldSize.x = 1;
		}
		if (obstacleFieldSize.y < 1) 
		{
			obstacleFieldSize.y = 1;
		}
		//
		if (meteorFieldSize.x < 1) 
		{
			meteorFieldSize.x = 1;
		}
		if (meteorFieldSize.y < 1) 
		{
			meteorFieldSize.y = 1;
		}
		//
		if (obsSpawnDistance < 0)
		{
			obsSpawnDistance = 0;
		}
		//
		if (spawnRate < 0.1f)
		{
			spawnRate = 0.1f;
		}
		//
		if (obstacleDensity > (obstacleFieldSize.x * obstacleFieldSize.y)) 
		{
			obstacleDensity = Mathf.RoundToInt( obstacleFieldSize.x) * Mathf.RoundToInt( obstacleFieldSize.y);
		}
		if (obstacleDensity < 0) 
		{
			obstacleDensity = 0;
		}
		//
		if (obstacleVelocity < 0) {
			obstacleVelocity = 0;
		}
		//
		if (fieldDensity > (meteorFieldSize.x * meteorFieldSize.y)) 
		{
			fieldDensity = Mathf.RoundToInt( meteorFieldSize.x) * Mathf.RoundToInt( meteorFieldSize.y);
		}
		if (fieldDensity < 0) 
		{
			fieldDensity = 0;
		}
		//
		if (fieldDepth < 1) {
			fieldDepth = 1;
		}
	}
}
