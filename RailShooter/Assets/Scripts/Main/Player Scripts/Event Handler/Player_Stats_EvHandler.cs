using UnityEngine;
using System.Collections;

public class Player_Stats_EvHandler : MonoBehaviour {
    //
    public delegate void PlayerStats();
    //
    public static event PlayerStats EngineStatus20;
    bool engineStatus20;
    //
    public float weaponTemp = 0;
    public float cannonTemp = 0;
    public float cannonPower = 0;
    public float generatorTemp = 0;
    public float shieldTemp = 100;
    public float engineLeftTemp = 0;
    public float engineRightTemp = 0;
    public float wingLeftTemp = 0;
    public float wingRightTemp = 0;
    public float weaponLeftTemp = 0;
    public float weaponRightTemp = 0;
    public float afterburnerTemp = 0;
    public float engineHeatScale;
    public float coolScale;
    public float forwardSpeed;
    //
    void Start () {
        
    }
	void Update () {
        Variables();
        EngineStatus();
    }
    //
    public void EngineStatus() {
        if (engineLeftTemp > 10 || engineRightTemp > 10) {
            if (EngineStatus20 != null ) {
                EngineStatus20();
            }
        }
    }
    //
    void Variables() {
        //
        weaponTemp = Player_Stats.instance.weaponTemp;
        cannonTemp = Player_Stats.instance.cannonTemp;
        cannonPower = Player_Stats.instance.cannonPower;
        generatorTemp = Player_Stats.instance.generatorTemp;
        shieldTemp = Player_Stats.instance.shieldTemp;
        engineLeftTemp = Player_Stats.instance.engineLeftTemp;
        engineRightTemp = Player_Stats.instance.engineRightTemp;
        wingLeftTemp = Player_Stats.instance.wingLeftTemp;
        wingRightTemp = Player_Stats.instance.wingRightTemp;
        weaponLeftTemp = Player_Stats.instance.weaponLeftTemp;
        weaponRightTemp = Player_Stats.instance.weaponRightTemp;
        afterburnerTemp = Player_Stats.instance.afterburnerTemp;
        engineHeatScale = Player_Stats.instance.engineHeatScale;
        coolScale = Player_Stats.instance.coolScale;
        forwardSpeed = Player_Stats.instance.forwardSpeed;
    }
}
