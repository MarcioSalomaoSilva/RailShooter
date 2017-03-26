using UnityEngine;
using System.Collections;
//
public static class Flicker {
	//
	public static float sinWave (float baseStart, float amplitude, float phase, float frequency) {
		//
		float x = (Time.time + phase) * frequency;
		float y;
		x = x - Mathf.Floor (x); // normalized value (0..1)
		//
		y = Mathf.Sin (x * 2 * Mathf.PI);
		//
		return (y * amplitude) + baseStart;
	}
	public static float triWave (float baseStart, float amplitude, float phase, float frequency) {
		//
		float x = (Time.time + phase) * frequency;
		float y;
		x = x - Mathf.Floor (x); // normalized value (0..1)
		//
		if (x < 0.5f) {
			y = 4.0f * x - 1.0f;
		} else {
			y = -4.0f * x + 3.0f;
		}
		//
		return (y * amplitude) + baseStart;
	}
	public static float sqrWave (float baseStart, float amplitude, float phase, float frequency) {
		//
		float x = (Time.time + phase) * frequency;
		float y;
		x = x - Mathf.Floor (x); // normalized value (0..1)
		//
		if (x < 0.5f) {
			y = 1.0f;
		} else {
			y = -1.0f;
		}
		//
		return (y * amplitude) + baseStart;
	}
	public static float sawWave (float baseStart, float amplitude, float phase, float frequency) {
		//
		float x = (Time.time + phase) * frequency;
		float y;
		x = x - Mathf.Floor (x); // normalized value (0..1)
		//
		y = x;
		//
		return (y * amplitude) + baseStart;
	}
	public static float invWave (float baseStart, float amplitude, float phase, float frequency) {
		//
		float x = (Time.time + phase) * frequency;
		float y;
		x = x - Mathf.Floor (x); // normalized value (0..1)
		//
		y = 1.0f - x;
		//
		return (y * amplitude) + baseStart;
	}
	public static float noise (float baseStart, float amplitude, float phase, float frequency) {
		//
		float x = (Time.time + phase) * frequency;
		float y;
		x = x - Mathf.Floor (x); // normalized value (0..1)
		//
		y = 1f - (Random.value * 2);
		//
		return (y * amplitude) + baseStart;
	}
}
