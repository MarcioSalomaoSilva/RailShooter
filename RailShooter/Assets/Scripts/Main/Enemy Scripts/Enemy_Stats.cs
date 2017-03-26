using UnityEngine;
using System.Collections;
//
public class Enemy_Stats : MonoBehaviour {
	int currentHealth = 3;
	// Use this for initialization
	public void Damage (int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			gameObject.SetActive (false);
		}
	}
}
