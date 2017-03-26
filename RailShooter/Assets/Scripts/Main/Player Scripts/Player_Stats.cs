using UnityEngine;
using System.Collections;
//
[RequireComponent(typeof(Player_Input))]
//
public class Player_Stats : MonoBehaviour {
	//
	public static Player_Stats instance;
    
    public delegate void PlayerStats();
    public static event PlayerStats ChangeMode;
    //
    public enum CurrentStatMode 
	{
		cooling, heating
	}
	public CurrentStatMode statMode;
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
    public float modeTimer;
	//
    void OnEnabel() {
        Player_Input.FireLaser += FireLaser;
        Player_Input.FireCannon += FireCannon;
    }
	void Start () {
		instance = this;
	}
	//
	void Update () {
        CalculateVariables();
        ClampVariables();
    }
    //
    void FireLaser() {
        weaponLeftTemp += 0.05f;
        weaponRightTemp += 0.05f;
        weaponTemp += 5;
    }
    void FireCannon() {

    }
    void TiltLeft() {
        //wingLeftTemp += 0.005f;
        //wingRightTemp += 0.005f;
        //weaponLeftTemp += 0.005f;
        //weaponRightTemp += 0.005f;
    }
    void TiltRight() {
        //wingLeftTemp += 0.005f;
        //wingRightTemp += 0.005f;
        //weaponLeftTemp += 0.005f;
        //weaponRightTemp += 0.005f;
    }
    void Boost() {
        engineLeftTemp += 0.02f * engineHeatScale;
	    engineRightTemp += 0.02f * engineHeatScale;
    }
    void Brake() {
        engineLeftTemp += 0.005f * engineHeatScale;
        engineRightTemp += 0.005f * engineHeatScale;
    }
	//
	public void CalculateVariables () 
	{
		if (Player_Input.instance.curState == Player_Input.CurrentState.Cooling) {
			statMode = CurrentStatMode.cooling;
		} else {
			statMode = CurrentStatMode.heating;
		}
        if (Player_Input.instance.curState == Player_Input.CurrentState.ModeChange)
        {
            modeTimer++;
        }
        else if (Player_Motor.instance.lookAtPath)
        {
            modeTimer++;
        }
        else {
            modeTimer = 0;
        }
        if (modeTimer == 5 && ChangeMode != null) {
            ChangeMode();
        }
        switch (statMode) {
		case CurrentStatMode.heating:
            
            engineLeftTemp += 0.005f;
            engineRightTemp += 0.005f;
            
            if (Player_Motor.instance.curMode == Player_Motor.CurrentMode.Rail)
            {
                afterburnerTemp += 0.05f;
                engineLeftTemp += 0.005f * engineHeatScale;
                engineRightTemp += 0.005f * engineHeatScale;
            }
            else
            {
                afterburnerTemp -= 0.25f * coolScale;
            }
			//
			cannonPower -= 5;
			break;
		case CurrentStatMode.cooling:
                //
                if (Player_Motor.instance.curMode == Player_Motor.CurrentMode.Rail) {
                    afterburnerTemp += 0.05f;
                    engineLeftTemp += 0.005f * engineHeatScale;
                    engineRightTemp += 0.005f * engineHeatScale;
                } else {
                    afterburnerTemp -= 0.25f * coolScale;
                }
                //
                switch (Player_Input.instance.curCooling)
                {
                    case Player_Input.CurrentCooling.Shield:
                        shieldTemp += 0.15f;
                        generatorTemp += 0.002f;
                        break;
                    case Player_Input.CurrentCooling.Cannon:
                        cannonTemp -= 0.06f * coolScale;
                        generatorTemp += 0.002f;
                        break;
                    case Player_Input.CurrentCooling.Generator:
                        generatorTemp -= 0.06f * coolScale;
                        generatorTemp += 0.002f;
                        break;
                    case Player_Input.CurrentCooling.WingLeft:
                        wingLeftTemp -= 0.06f * coolScale;
                        generatorTemp += 0.002f;
                        break;
                    case Player_Input.CurrentCooling.WingRight:
                        wingRightTemp -= 0.06f * coolScale;
                        generatorTemp += 0.002f;
                        break;
                    case Player_Input.CurrentCooling.EngineLeft:
                        engineLeftTemp -= 0.06f * coolScale;
                        generatorTemp += 0.002f;
                        break;
                    case Player_Input.CurrentCooling.EngineRight:
                        engineRightTemp -= 0.06f * coolScale;
                        generatorTemp += 0.002f;
                        break;
                    case Player_Input.CurrentCooling.WeaponLeft:
                        weaponLeftTemp -= 0.04f * coolScale;
                        generatorTemp += 0.002f;
                        break;
                    case Player_Input.CurrentCooling.WeaponRight:
                        weaponRightTemp -= 0.04f * coolScale;
                        generatorTemp += 0.002f;
                        break;
                }
			break;
		}
    }
    void ClampVariables() {
        //
        shieldTemp = Mathf.Clamp(shieldTemp, 0, 101);
        generatorTemp = Mathf.Clamp(generatorTemp, 0, 101);
        cannonTemp = Mathf.Clamp(cannonTemp, 0, 101);
        if (cannonTemp > 100) {
            cannonPower = 0;
        }
        cannonPower = Mathf.Clamp(cannonPower, 0, 101);
        wingLeftTemp = Mathf.Clamp(wingLeftTemp, 0, 101);
        wingRightTemp = Mathf.Clamp(wingRightTemp, 0, 101);
        engineRightTemp = Mathf.Clamp(engineRightTemp, 0, 101);
        engineLeftTemp = Mathf.Clamp(engineLeftTemp, 0, 101);
        weaponLeftTemp = Mathf.Clamp(weaponLeftTemp, 0, 101);
        if (modeTimer > 5) {
            modeTimer = 0;
        }
        engineHeatScale = Mathf.Lerp(1, 3, afterburnerTemp / 100);
        coolScale = Mathf.Lerp(3, 1, generatorTemp / 100);
        afterburnerTemp = Mathf.Clamp(afterburnerTemp, 0, 101);
    }
}
