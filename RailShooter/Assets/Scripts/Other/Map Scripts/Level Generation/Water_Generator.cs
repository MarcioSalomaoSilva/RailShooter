using UnityEngine;
using System.Collections;
//
//[RequireComponent(typeof(Level_Generator))]
//
public class Water_Generator : MonoBehaviour {
	//
	public static Water_Generator instance;
	//
	public Transform waterPrefab;
	private Vector2 mapSize;
	//
	private Vector2 offset;
	public Vector2 offsetIncreae;
	private int scale;
	private int octaves;
	private float persistance;
	private float lacunarity;
	public float heightMultiplier;
	//public bool useFalloffMap;
	//
	public int waterSeed;
	public bool water;
	public float frequency;
	public float magnitude;
	//
	float[,] noiseMap;
	//
	void Start () 
	{
		//
		instance = this;
		//
		GenerateSeed ();
		GetVariables ();
	}
	
	//
	void Update () 
	{
		UpdateVariables ();
		GenerateWaterUpdate ();
	}
	//
	public void GetVariables()
	{
		//
		//
		mapSize.x = Level_Generator.instance.mapSize.x;
		mapSize.y = Level_Generator.instance.mapSize.y;
		//
		//scale = Level_Generator.instance.scale;
		scale = 1;
		//octaves = Level_Generator.instance.octaves;
		octaves = 3;
		persistance = Level_Generator.instance.persistance;
		lacunarity = Level_Generator.instance.lacunarity;
		offset = Level_Generator.instance.offset;
		heightMultiplier = Level_Generator.instance.heightMultiplier;
		//
		noiseMap = Noise.GenerateNoiseMap (Mathf.RoundToInt (mapSize.x), Mathf.RoundToInt (mapSize.y), waterSeed, scale, octaves, persistance, lacunarity, offset);
		//
	}
	//
	public void GenerateSeed ()
	{
		//
		waterSeed =  Random.Range(1,100000);
		//
	}
	//
	public void UpdateVariables()
	{
		offset = new Vector2 (offset.x + offsetIncreae.x, offset.y + offsetIncreae.y);
		noiseMap = Noise.GenerateNoiseMap (Mathf.RoundToInt (mapSize.x), Mathf.RoundToInt (mapSize.y), waterSeed, scale, octaves, persistance, lacunarity, offset);
		heightMultiplier = Mathf.Sin(Time.time * frequency) * magnitude;
		heightMultiplier = 1;
	}
	//
	public void GenerateWaterUpdate()
	{
		//
		//offset = new Vector2 (offset.x + offsetIncreae.y, offset.y + offsetIncreae.y );
		//
		string holderWaterName = "Water";
		if (transform.FindChild (holderWaterName)) {
			DestroyImmediate (transform.FindChild (holderWaterName).gameObject);
		}
		//
		Transform waterHolder = new GameObject (holderWaterName).transform;
		waterHolder.parent = transform;
		waterHolder.position = Vector3.zero;
		//
		//heightMultiplier = Mathf.Sin(Time.time * frequency) * magnitude;
		//
		for (int x = 0; x < mapSize.x; x++) 
		{
			for (int y = 0; y < mapSize.y; y++) 
			{
				//
				Vector3 lowPrefabPosition = CoordToPosition(x, y, 3.5f);
				Transform lowPrefab = Instantiate (waterPrefab, lowPrefabPosition, Quaternion.identity) as Transform;
				lowPrefab.transform.parent = waterHolder.transform;
			}
		}
	}
	//
	Vector3 CoordToPosition(int x, int y, float height)
	{
		return new Vector3 (-mapSize.x / 2 + 0.5f + x, noiseMap [x, y] * heightMultiplier + height, -mapSize.y / 2 + 0.5f + y);
	}
	void OnValidate()
	{
		//
		if (frequency < 0) 
		{
			frequency = 0;
		}		
		if (magnitude < 0) 
		{
			magnitude = 0;
		}
	}
}
