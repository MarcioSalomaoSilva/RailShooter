using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//
//[RequireComponent(typeof(Water_Generator))]
//
public class Level_Generator : MonoBehaviour 
{
	public static Level_Generator instance;
	//----------------------------------------------------------------------------------Inspector
	public Transform lowerPrefab;
	public Transform middlePrefab;
	public Transform higherPrefab;
	public Transform obstaclePrefab;
	public Vector2 mapSize;
	//
	[Range (0, 1)]
	public float outLinePercent;
	//
	public Vector2 offset;
	public Vector2 offsetIncrease;
	//
	[Range(1, 100)]
	public int scale;
	//
	[Range(1, 10)]
	public int octaves;
	//
	[Range(0, 10)]
	public float persistance;
	[Range(0, 1)]
	public float lacunarity;
	[Range(0,10)]
	public float heightMultiplier;
	//public bool useFalloffMap;
	//
	public AnimationCurve meshHeightCurve;
	public int mapHeight;
	public bool randomObsHeight;
	public int obstacleDensity;
	public int obstacleHeight;
	public bool sameSeed;
	public int mapSeed;
	public int obstacleSeed;
	//------------------------------------------------------------------------------------Variables
	//
	float[,] noiseMap;
	//
	List<Coord> allTileCoords;
	Queue<Coord> shuffledTileCoords;
	//------------------------------------------------------------------------------------
	void Awake ()
	{
		//
		instance = this;
		//
		GenerateSeed ();
		InitializeGeneration ();
		//
	}
	//
	void Update ()
	{
		//
		UpdateVariables();
		//
		GenerateLowerTerrain ();
		GenerateMidTerrain ();
		GenerateHighTerrain ();
		GenerateObstacles ();
		//
	}
	//
	public void GenerateSeed ()
	{
		//
		mapSeed =  Random.Range(1,100000);
		//
		if (sameSeed) {
			mapSeed = Random.Range (1, 100000);
			obstacleSeed = mapSeed;
		} else {
			obstacleSeed =  Random.Range(1,100000);
		}
	}
	//
	public void InitializeGeneration ()
	{
		//
		mapSize.x = Mathf.RoundToInt(mapSize.x);
		mapSize.y = Mathf.RoundToInt (mapSize.y);
		//
		allTileCoords = new List<Coord> ();
		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++) 
			{
				//
				allTileCoords.Add (new Coord (x, y));
			}
		}
		//
		shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), mapSeed));
		//
		noiseMap = Noise.GenerateNoiseMap (Mathf.RoundToInt (mapSize.x), Mathf.RoundToInt (mapSize.y), mapSeed, scale, octaves, persistance, lacunarity, offset);
		//
	}
	//
	public void UpdateVariables()
	{
		offset = new Vector2 (offset.x + offsetIncrease.x, offset.y + offsetIncrease.y);
		noiseMap = Noise.GenerateNoiseMap (Mathf.RoundToInt (mapSize.x), Mathf.RoundToInt (mapSize.y), mapSeed, scale, octaves, persistance, lacunarity, offset);
	}
	//
	public void GenerateLowerTerrain()
	{
		//
		string holderMapName = "Low Level Prefabs";
		if (transform.FindChild (holderMapName)) {
			DestroyImmediate (transform.FindChild (holderMapName).gameObject);
		}
		//
		Transform lowMapHolder = new GameObject (holderMapName).transform;
		lowMapHolder.parent = transform;
		//
		for (int h = 0; h < mapHeight; h++) 
		{ 
			//
			string holderName1 = "Prefabs";
			if (transform.FindChild (holderName1)) {
				DestroyImmediate (transform.FindChild (holderName1).gameObject);
			}
			//
			Transform prefabHolder = new GameObject (holderName1).transform;
			prefabHolder.parent = lowMapHolder.transform;
			prefabHolder.position = Vector3.zero;
			//
			for (int x = 0; x < mapSize.x; x++) 
			{
				for (int y = 0; y < mapSize.y; y++) 
				{
					//
					Vector3 lowPrefabPosition = CoordToPosition(x, y, h);
					Transform lowPrefab = Instantiate (lowerPrefab, lowPrefabPosition, Quaternion.identity) as Transform;
					lowPrefab.localScale = Vector3.one * (1 - outLinePercent);
					lowPrefab.transform.parent = prefabHolder.transform;
				}
			}
		}
	}
	//
	public void GenerateMidTerrain()
	{
		//
		string holderMapName1 = "Mid Level Prefabs";
		if (transform.FindChild (holderMapName1)) {
			DestroyImmediate (transform.FindChild (holderMapName1).gameObject);
		}
		//
		Transform midMapHolder = new GameObject (holderMapName1).transform;
		midMapHolder.parent = transform;
		//
		string holderName2 = "Prefabs";
		if (transform.FindChild (holderName2)) {
			DestroyImmediate (transform.FindChild (holderName2).gameObject);
		}
		//
		Transform prefabHolder1 = new GameObject (holderName2).transform;
		prefabHolder1.parent = midMapHolder.transform;
		prefabHolder1.position = Vector3.zero;
		//
		for (int i = 0; i < mapSize.x*mapSize.y; i++)
		{
			Coord randomCoord = GetRandomCoord ();
			Vector3 obstaclePosition = CoordToPosition (randomCoord.x, randomCoord.y, 1);
			Transform newObstacle = Instantiate (middlePrefab, obstaclePosition + Vector3.up*2 , Quaternion.identity) as Transform;
			newObstacle.parent = prefabHolder1;
		}
		//	
	}
	public void GenerateHighTerrain ()
	{
		//
		string holderMapName2 = "High Level Prefabs";
		if (transform.FindChild (holderMapName2)) {
			DestroyImmediate (transform.FindChild (holderMapName2).gameObject);
		}
		//
		Transform highMapHolder = new GameObject (holderMapName2).transform;
		highMapHolder.parent = transform;
		//
		string holderName3 = "Prefabs";
		if (transform.FindChild (holderName3)) {
			DestroyImmediate (transform.FindChild (holderName3).gameObject);
		}
		//
		Transform prefabHolder2 = new GameObject (holderName3).transform;
		prefabHolder2.parent = highMapHolder.transform;
		prefabHolder2.position = Vector3.zero;
		//
		for (int i = 0; i < mapSize.x*mapSize.y; i++)
		{
			Coord randomCoord = GetRandomCoord ();
			Vector3 obstaclePosition = CoordToPosition (randomCoord.x, randomCoord.y, 1);
			Transform newObstacle = Instantiate (higherPrefab, obstaclePosition + Vector3.up*3 , Quaternion.identity) as Transform;
			newObstacle.parent = prefabHolder2;
		}
	}
	//
	public void GenerateObstacles()
	{
		if (randomObsHeight) 
		{
			//
			string holderName4 = "Obstacles";
			if (transform.FindChild (holderName4)) {
				DestroyImmediate (transform.FindChild (holderName4).gameObject);
			}
			//
			Transform prefabHolder3 = new GameObject (holderName4).transform;
			prefabHolder3.parent = transform;
			prefabHolder3.position = Vector3.zero;
			//
			for (int h = 0; h < obstacleHeight; h++) 
			{
				//
				for (int i = 0; i < obstacleDensity ; i++) 
				{
					Coord randomCoord = GetRandomCoord ();
					Vector3 obstaclePosition = CoordToPosition (randomCoord.x, randomCoord.y, -h);
					Transform newObstacle = Instantiate (obstaclePrefab, obstaclePosition + Vector3.up * 3, Quaternion.identity) as Transform;
					newObstacle.parent = prefabHolder3;
				}
			}
		}
		else 
		{
			for (int h = 0; h < obstacleHeight; h++) 
			{
				//
				string holderName4 = "Obstacles";
				if (transform.FindChild (holderName4)) {
					DestroyImmediate (transform.FindChild (holderName4).gameObject);
				}
				//
				Transform prefabHolder3 = new GameObject (holderName4).transform;
				prefabHolder3.parent = transform;
				prefabHolder3.position = Vector3.zero;
				//
				for (int i = 0; i < obstacleDensity; i++) 
				{
					Coord randomCoord = GetRandomCoord ();
					Vector3 obstaclePosition = CoordToPosition (randomCoord.x, randomCoord.y, -h);
					Transform newObstacle = Instantiate (obstaclePrefab, obstaclePosition + Vector3.up * 3, Quaternion.identity) as Transform;
					newObstacle.parent = prefabHolder3;
				}
			}
		}
	}
	//
	Vector3 CoordToPosition(int x, int y, float height)
	{
		return new Vector3 (-mapSize.x / 2 + 0.5f + x, meshHeightCurve.Evaluate (noiseMap [x, y]) * heightMultiplier - height, -mapSize.y / 2 + 0.5f + y);
	}
	//
	public Coord GetRandomCoord()
	{
		//gets first item
		Coord randomCoord = shuffledTileCoords.Dequeue ();
		//adds to the end of the queue
		shuffledTileCoords.Enqueue (randomCoord);
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
		if (mapSize.x < 1) 
		{
			mapSize.x = 1;
		}
		if (mapSize.y < 1) 
		{
			mapSize.y = 1;
		}
		//
		if(lacunarity < 0)
		{
			lacunarity = 0;
		}
		//
		if(octaves < 1)
		{
			octaves = 1;
		}
		//
		if(scale < 1)
		{
			scale = 1;
		}
		if(scale > 100)
		{
			scale = 100;
		}
		//
		if (persistance < 0 ) 
		{
			persistance = 0;
		}
		if (persistance > 10 ) 
		{
			persistance = 10;
		}
		//
		if (mapSeed < 1) 
		{
			mapSeed = 1;
		}
		if (mapSeed > 99999) 
		{
			mapSeed = 99999;
		}
		if (mapHeight < 0 ) 
		{
			mapHeight = 0;
		}
		//
		if (obstacleSeed < 1) 
		{
			obstacleSeed = 1;
		}
		if (obstacleSeed > 99999) 
		{
			obstacleSeed = 99999;
		}
		if (obstacleHeight < 1 ) 
		{
			obstacleHeight = 1;
		}
		if (obstacleDensity > (mapSize.x * mapSize.y)) 
		{
			obstacleDensity = Mathf.RoundToInt( mapSize.x) * Mathf.RoundToInt( mapSize.y);
		}
		if (obstacleDensity < 0) 
		{
			obstacleDensity = 0;
		}
	}
}