using UnityEngine;
using System.Collections;

public class GUI_Health : MonoBehaviour {
    //
    public static GUI_Health instance;
    //
    Color colAverage;
    //
    Color uiGreen;
    Color uiCoolingBlue;
    Color uiBlack;
    //
    void Awake() {
        instance = this;
    }
    //
    public void OnGUI() {
        //
        uiGreen = Game_Manager.instance.uiGreen;
        uiCoolingBlue = Game_Manager.instance.uiCoolingBlue;
        uiBlack = Game_Manager.instance.uiBlack;
        //
        float rx = Screen.width / Game_Resolution.instance.nativeScreenWidth;
        float ry = Screen.height / Game_Resolution.instance.nativeScreenHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        //
        float textureSize = Game_Resolution.instance.nativeScreenHeight / 3;//Player_Manager.instance.textureSize;
        //-----------------------------------------------------------------------------------------------------------------------test--------------
        GUI.Box(new Rect(0, 0, Screen.width / 5, 30), "");
        GUI.Box(new Rect(0, 0, Screen.width / 5 / (100 / Player_Stats.instance.shieldTemp), 30), "");
        GUI.Label(new Rect(10, 0, Screen.width / 5, 30), "Shield = " + Player_Stats.instance.shieldTemp + "/" + 100);
        //
        GUI.Box(new Rect(0, 30, Screen.width / 5, 30), "");
        GUI.Box(new Rect(0, 30, Screen.width / 5 / (100 / Player_Stats.instance.modeTimer), 30), "");
        GUI.Label(new Rect(10, 30, Screen.width / 5, 30), "Mode Change Time = " + Player_Stats.instance.modeTimer + "/" + 100);
        //
        GUI.skin = Player_Manager.instance.healthBar;
        //-------------------------------------------------------------------------------------------------------------------------test------------
        Color colShield = Camera_Helper.CurrentShieldColor(Player_Stats.instance.shieldTemp, Player_Input.CurrentCooling.Shield);
        GUI.color = colShield;
        GUI.Box(new Rect(Game_Manager.instance.spacing.x + 34, Game_Resolution.instance.nativeScreenHeight - textureSize / 3 - 178, 27, -(textureSize * .91f / (100 / Player_Stats.instance.shieldTemp))), "");
        //
        Color colGenerator = Camera_Helper.CurrentColor(Player_Stats.instance.generatorTemp, Player_Input.CurrentCooling.Generator);
        //
        Color colCannon = Camera_Helper.CurrentColor(Player_Stats.instance.cannonTemp, Player_Input.CurrentCooling.Cannon);
        GUI.color = colCannon;
        GUI.Box(new Rect(Game_Manager.instance.spacing.x + 222, Game_Resolution.instance.nativeScreenHeight - textureSize / 3 - 150, 27, -(textureSize * .21f / (100 / Player_Stats.instance.cannonPower))), "");
        //
        Color colWingLeft = Camera_Helper.CurrentColor(Player_Stats.instance.wingLeftTemp, Player_Input.CurrentCooling.WingLeft);
        //
        Color colWingRight = Camera_Helper.CurrentColor(Player_Stats.instance.wingRightTemp, Player_Input.CurrentCooling.WingRight);
        //
        Color colEngineLeft = Camera_Helper.CurrentColor(Player_Stats.instance.engineLeftTemp, Player_Input.CurrentCooling.EngineLeft);
        //
        Color colEngineRight = Camera_Helper.CurrentColor(Player_Stats.instance.engineRightTemp, Player_Input.CurrentCooling.EngineRight);
        //
        Color colWeaponLeft = Camera_Helper.CurrentColor(Player_Stats.instance.weaponLeftTemp, Player_Input.CurrentCooling.WeaponLeft);
        //
        Color colWeaponRight = Camera_Helper.CurrentColor(Player_Stats.instance.weaponRightTemp, Player_Input.CurrentCooling.WeaponRight);
        //
        float tempAverage = (Player_Stats.instance.engineLeftTemp + Player_Stats.instance.engineRightTemp + Player_Stats.instance.generatorTemp + Player_Stats.instance.cannonTemp +
            Player_Stats.instance.wingLeftTemp + Player_Stats.instance.wingRightTemp + Player_Stats.instance.weaponLeftTemp + Player_Stats.instance.weaponRightTemp) / 8;
        colAverage = Camera_Helper.CurrentColorAverage(tempAverage);
        //
        if (tempAverage > 0 && Player_Input.instance.curState == Player_Input.CurrentState.Cooling || Player_Input.instance.curState == Player_Input.CurrentState.Cooling) {
            GUI.color = Color.Lerp(colAverage, uiCoolingBlue, Flicker.noise(1, Camera_Helper.CurrentFlicker(), 0, 0f));
            Camera_Helper.DrawTexture(Player_Manager.instance.texWarningCooling);
        } else if (tempAverage > 30) {
            GUI.color = colAverage;
            Camera_Helper.DrawTexture(Player_Manager.instance.texWarningDanger);
        } else {
            GUI.color = new Color(uiBlack.r, uiBlack.g, uiBlack.b, Player_Manager.instance.transparency - Game_Manager.instance.transparencyRange);
            Camera_Helper.DrawTexture(Player_Manager.instance.texWarningDanger);
        }
        //
        GUI.color = uiGreen;
        uiGreen.a = Game_Manager.instance.uiTransLow.a;
        Camera_Helper.DrawTwoTextures(Player_Manager.instance.fontShield, Player_Manager.instance.lineShield);
        Camera_Helper.DrawTextureAbove(Player_Manager.instance.uiBarExShield);
        GUI.color = colShield;
        Camera_Helper.DrawTexture(Player_Manager.instance.texShield);
        //
        GUI.color = uiGreen;
        uiGreen.a = Game_Manager.instance.uiTransLow.a;
        Camera_Helper.DrawTwoTextures(Player_Manager.instance.fontGenerator, Player_Manager.instance.lineGenerator);
        GUI.color = colGenerator;
        Camera_Helper.DrawTexture(Player_Manager.instance.texGenerator);
        //
        GUI.color = uiGreen;
        uiGreen.a = Game_Manager.instance.uiTransLow.a;
        Camera_Helper.DrawTwoTextures(Player_Manager.instance.fontCannon, Player_Manager.instance.lineCannon);
        GUI.DrawTexture(new Rect(Game_Manager.instance.spacing.x, Game_Resolution.instance.nativeScreenHeight - textureSize - Game_Manager.instance.spacing.y, textureSize, textureSize),
            Player_Manager.instance.uiBarExCannon, ScaleMode.ScaleToFit);
        GUI.color = colCannon;
        Camera_Helper.DrawTexture(Player_Manager.instance.texCannon);
        //
        Camera_Helper.DrawTexturesParts(colWingRight, colWingLeft, Player_Manager.instance.texWingLeft, Player_Manager.instance.texWingRight);
        GUI.color = uiGreen;
        uiGreen.a = Game_Manager.instance.uiTransLow.a;
        Camera_Helper.DrawTwoTextures(Player_Manager.instance.lineWingLeft, Player_Manager.instance.lineWingRight2);
        Camera_Helper.DrawTexture(Player_Manager.instance.fontWing);
        //
        Camera_Helper.DrawTexturesParts(colEngineRight, colEngineLeft, Player_Manager.instance.texEngineLeft, Player_Manager.instance.texEngineRight);
        GUI.color = uiGreen;
        uiGreen.a = Game_Manager.instance.uiTransLow.a;
        Camera_Helper.DrawTwoTextures(Player_Manager.instance.lineEngineLeft, Player_Manager.instance.lineEngineRight2);
        Camera_Helper.DrawTexture(Player_Manager.instance.fontEngine);
        //
        Camera_Helper.DrawTexturesParts(colWeaponRight, colWeaponLeft, Player_Manager.instance.texWeaponLeft, Player_Manager.instance.texWeaponRight);
        GUI.color = uiGreen;
        uiGreen.a = Game_Manager.instance.uiTransLow.a;
        Camera_Helper.DrawTwoTextures(Player_Manager.instance.lineWeaponLeft, Player_Manager.instance.lineWeaponRight2);
        Camera_Helper.DrawTexture(Player_Manager.instance.fontWeapon);
    }
}
