//using UnityEngine;
//using System.Collections;
//
//public class Mesh_Generator2 : MonoBehaviour 
//{
//	//
//	public Vector2 mapSize;
//	public Vector2 offset;
//	public Vector2 offsetIncrease;
//	//
//	[Range(1, 100)]
//	public int scale;
//	[Range(1, 10)]
//	public int octaves;
//	[Range(0, 10)]
//	public float persistance;
//	[Range(0, 1)]
//	public float lacunarity;
//	[Range(0,10)]
//	public float heightMultiplier;
//	public AnimationCurve meshHeightCurve;
//	public int mapSeed;
//	//
//	private int vertexIndex;
//	private float[,] noiseMap;
//	// Use this for initialization
//	void Awake () 
//	{
//		//
//		Initialize();
//		//
//		GenerateSeed();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		//
//		GenerateMesh();
//	}
//	//
//	public void Initialize()
//	{
//		//
//		mapSize = new Vector2 (Mathf.RoundToInt(mapSize.x),Mathf.RoundToInt(mapSize.x));
//		//
//		noiseMap = Noise.GenerateNoiseMap (Mathf.RoundToInt (mapSize.x), Mathf.RoundToInt (mapSize.y), mapSeed, scale, octaves, persistance, lacunarity, offset);
//		//
//		//
//		MeshData meshData = new MeshData (mapSize.x, mapSize.y);
//		vertexIndex = 0;
//	}
//	//
//	public void GenerateSeed ()
//	{
//		//
//		mapSeed =  Random.Range(1,100000);
//		//
//	}
//	//
//	public void GenerateMesh ()
//	{
//		for (int y = 0; y < mapSize.y; y++) 
//		{
//			for (int x = 0; x < mapSize.x; x++) 
//			{
//				MeshData [vertexIndex] = new Vector3 (x,heightMultiplier, y);
//				vertexIndex++;
//
//			}
//		}
//	}
//	//
//	Vector3 CoordToPosition(int x, int y, float height)
//	{
//		return new Vector3 (-mapSize.x / 2 + 0.5f + x, meshHeightCurve.Evaluate (noiseMap [x, y]) * heightMultiplier - height, -mapSize.y / 2 + 0.5f + y);
//	}
//	//
//	void OnValidate ()
//	{
//		if (mapSize.x < 1) 
//		{
//			mapSize.x = 1;
//		}
//		if (mapSize.y < 1) 
//		{
//			mapSize.y = 1;
//		}
//		//
//		if(lacunarity < 0)
//		{
//			lacunarity = 0;
//		}
//		//
//		if(octaves < 1)
//		{
//			octaves = 1;
//		}
//		//
//		if(scale < 1)
//		{
//			scale = 1;
//		}
//		if(scale > 100)
//		{
//			scale = 100;
//		}
//		//
//		if (persistance < 0 ) 
//		{
//			persistance = 0;
//		}
//		if (persistance > 10 ) 
//		{
//			persistance = 10;
//		}
//		//
//		if (mapSeed < 1) 
//		{
//			mapSeed = 1;
//		}
//		if (mapSeed > 99999) 
//		{
//			mapSeed = 99999;
//		}
//	}
//}
