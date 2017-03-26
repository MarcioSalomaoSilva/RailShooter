using UnityEngine;
using System.Collections;

public class Ray_Debug : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (Game_Manager.instance.debugging == Game_Manager.Debug.True && Game_Manager.instance.debugShip) {
			//
			float cannonRange = Weapon_Controller.instance.weaponRange;
			Transform gunEndCenter = Player_Manager.instance.weaponC.transform;
			Debug.DrawRay (gunEndCenter.position, gunEndCenter.forward * cannonRange,
                Game_Manager.instance.uiGreen);
			//
			float farLeftRange = Weapon_Controller.instance.weaponRange;
			Transform gunEndfarleft = Player_Manager.instance.weaponLFar.transform;
			Debug.DrawRay (gunEndfarleft.position, gunEndfarleft.forward * farLeftRange,
                Game_Manager.instance.uiGreen);
			//
			float nearLeftRange = Weapon_Controller.instance.weaponRange;
			Transform gunEndNearLeft = Player_Manager.instance.weaponLNear.transform;
			Debug.DrawRay (gunEndNearLeft.position, gunEndNearLeft.forward * nearLeftRange,
                Game_Manager.instance.uiGreen);
			//
			float farRightRange = Weapon_Controller.instance.weaponRange;
			Transform gunEndFarRight = Player_Manager.instance.weaponRFar.transform;
			Debug.DrawRay (gunEndFarRight.position, gunEndFarRight.forward * farRightRange,
                Game_Manager.instance.uiGreen);
			//
			float nearRightRange = Weapon_Controller.instance.weaponRange;
			Transform gunEndNearRight = Player_Manager.instance.weaponRNear.transform;
			Debug.DrawRay (gunEndNearRight.position, gunEndNearRight.forward * nearRightRange,
                Game_Manager.instance.uiGreen);
		}
	}
}
