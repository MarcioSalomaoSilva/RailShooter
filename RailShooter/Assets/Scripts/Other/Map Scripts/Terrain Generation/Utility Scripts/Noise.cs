using UnityEngine;
using System.Collections;
//
public static class Noise {//don't neeed to inherit from mono and can be static because it wont have more than one instance{

	//noise map method
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
	{
		//2D float array
		float[,] noiseMap = new float [mapWidth, mapHeight];
		//random number for the seed (psuedo random number generator)
		System.Random mapSeed = new System.Random (seed);
		//sample each octave from different location
		Vector2[] octaveOffsets = new Vector2[octaves];
		//limit octave values
		for (int i = 0; i < octaves; i++)
		{
			float offSetX = mapSeed.Next (-10000, 10000) + offset.x;
			float offSetY = mapSeed.Next (-10000, 10000) + offset.y;
			octaveOffsets [i] = new Vector2 (offSetX, offSetY);
		}
		//check zero division
		if (scale <= 0) 
		{
			scale = 0.0001f;
		}
		//
		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;
		//finds the center so the noise can be scaled later
		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;
		//loop through the noise map
		for (int y = 0; y < mapHeight; y++) 
		{
			for (int x = 0; x < mapWidth; x++) {
				//variables
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;
				//octave loop
				for(int i = 0; i < octaves; i++)
				{
					//coordinates and centering
					float sampleX = (x- halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y- halfHeight) / scale * frequency + octaveOffsets[i].y;
					//pass coordinates into perlin value
					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY)* 2 - 1;
					//
					noiseHeight += perlinValue * amplitude;
					amplitude *= persistance;
					frequency *= lacunarity;
				}
				if(noiseHeight > maxNoiseHeight)
				{
					maxNoiseHeight = noiseHeight;
				} 
				else if (noiseHeight < minNoiseHeight)
				{
					minNoiseHeight = noiseHeight;
				}
				//
				noiseMap[x, y] = noiseHeight;
			}
		}
		//normalize because of previous perlin noise multiplication
		for (int y = 0; y < mapHeight; y++) 
		{
			for (int x = 0; x < mapWidth; x++) 
			{
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
			}
		}
		return noiseMap;
	}
}
