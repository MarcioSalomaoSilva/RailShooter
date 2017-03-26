using UnityEngine;
using System.Collections;
//
[RequireComponent(typeof(Map_Display))]
//
public class Map_Generator : MonoBehaviour {

	public enum DrawMode
	{
		NoiseMap, ColourMap, Mesh, Cubes, FalloffMap
	};
	public DrawMode drawMode;
	//
	private GameObject plane;
	private GameObject mesh;
	//
	public int mapChunkSize = 241;
	//
	[Range(0,6)]
	public int levelOfDetail;
	public float noiseScale;
	public int octaves;
	//
	[Range(0,1)]
	public float persistance;
	public float lacunarity;
	//
	public int seed;
	public Vector2 offset;
	//
	public bool useFalloffMap;
	//
	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;
	//
	public bool autoUpdate;
	//public array of terrain types
	public TerrainType[] regions;
	//
	float [,] falloffMap;
	//
	void Awake() {
		//
		checkGameObjects();
		//
		falloffMap = Fallloff_Generator.GenerateFalloffMap(mapChunkSize);
	}
	//
	public void GenerateMap()
	{
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
		//
		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		//
		for (int y = 0; y < mapChunkSize; y++) 
		{
			for (int x = 0; x < mapChunkSize; x++) 
			{
				//
				if (useFalloffMap) {
					noiseMap [x, y] = Mathf.Clamp01( noiseMap [x, y] - falloffMap [x, y]);
				}
				//
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Length; i++) 
				{
					if (currentHeight <= regions [i].height) 
					{
						colourMap [y * mapChunkSize + x] = regions [i].colour;
						break;
					}
				}
			}
		}
		//
		Map_Display display = FindObjectOfType<Map_Display>();
		//
		switch (drawMode) 
		{
		case DrawMode.NoiseMap:
			display.DrawTexture (Texture_Generator.TextureFromHeightMap (noiseMap));
//			mesh.SetActive (false);
//			plane.SetActive (true);
			break;
		case DrawMode.ColourMap:
			display.DrawTexture (Texture_Generator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
//			mesh.SetActive (false);
//			plane.SetActive (true);
			break;
		case DrawMode.Mesh:
			display.DrawMesh (Mesh_Generator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), Texture_Generator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
//			mesh.SetActive (true);
//			plane.SetActive (false);
			break;
		case DrawMode.Cubes:
			display.DrawMesh (Mesh_Generator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), Texture_Generator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
//			mesh.SetActive (true);
//			plane.SetActive (false);
			break;		
		case DrawMode.FalloffMap:
			display.DrawTexture (Texture_Generator.TextureFromHeightMap (Fallloff_Generator.GenerateFalloffMap(mapChunkSize)));
//			mesh.SetActive (false);
//			plane.SetActive (true);
			break;
		}
	}
	//called automatically when a variable is changed
	void OnValidate()
	{
		if(lacunarity < 1)
		{
			lacunarity = 1;
		}
		if(octaves < 1)
		{
			octaves = 1;
		}
		falloffMap = Fallloff_Generator.GenerateFalloffMap (mapChunkSize);
	}
	//checks to see if the generated stuff is childed
	public void checkGameObjects ()
	{
		//checks to see if the plane exists
		plane = GameObject.Find ("Plane");
		if (plane == null) 
		{
			Debug.LogWarning ("Create a plane called PLane and assign it in the map display script");
		}
		plane.transform.parent = this.transform;
		//checks to see if the mesh exists
		mesh = GameObject.Find ("Mesh");
		if(mesh == null)
		{
			Debug.LogWarning ("Create a empty game object called Mesh and assign it in the map display script");
		}
		mesh.transform.parent = this.transform;
	}
}
//holds data
[System.Serializable]
public struct TerrainType {
	//
	public string name;
	public float height;
	public Color colour;
}