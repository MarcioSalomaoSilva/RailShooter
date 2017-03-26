using UnityEngine;
using System.Collections;
//
public class GUI_Radar : MonoBehaviour {
    //
    void OnGUI() {
        float rx = Screen.width / Game_Resolution.instance.nativeScreenWidth;
        float ry = Screen.height / Game_Resolution.instance.nativeScreenHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        //
        GUI.color = Game_Manager.instance.uiGreen;
        Game_Manager.instance.uiGreen.a = Game_Manager.instance.uiTransLow.a;
        //
        Camera_Helper.DrawTextureRadar(Player_Manager.instance.testRight);
        //
        if (Player_Motor.instance.curMode == Player_Motor.CurrentMode.Rail) {
            Camera_Helper.DrawTextureRadarAbove(Player_Manager.instance.fontAutopilotOn);
        }
        if (Player_Motor.instance.curMode == Player_Motor.CurrentMode.Range) {
            Camera_Helper.DrawTextureRadarAbove(Player_Manager.instance.fontAutopilotOff);
        }
        //
        GUI.skin = Player_Manager.instance.healthBar;
        Camera_Helper.DrawTextureRadarAbove(Player_Manager.instance.uiBarAutopilot);
        //
        Color colPart;
        Color curColPart;
        if (Player_Motor.instance.curMode == Player_Motor.CurrentMode.Rail) {
            if (Player_Stats.instance.afterburnerTemp < 65) {
                colPart = Color.Lerp(Game_Manager.instance.uiGreen, Game_Manager.instance.uiYellow, (Player_Stats.instance.afterburnerTemp / 100) * 2);
                colPart.a = Mathf.Lerp(Game_Manager.instance.uiTransLow.a, Game_Manager.instance.uiTransHigh.a, Player_Stats.instance.afterburnerTemp / 100);
            } else {
                colPart = Color.Lerp(Game_Manager.instance.uiYellow, Game_Manager.instance.uiRed, ((Player_Stats.instance.afterburnerTemp / 100) - 0.5f) * 2);
                colPart.a = Mathf.Lerp(Game_Manager.instance.uiTransLow.a, Game_Manager.instance.uiTransHigh.a, Player_Stats.instance.afterburnerTemp / 100);
            }
        } else {
            colPart = Game_Manager.instance.uiGreen;
            colPart.a = Game_Manager.instance.uiTransLow.a;
        }
        curColPart = colPart;
        if (Player_Motor.instance.curMode == Player_Motor.CurrentMode.Range) {
            colPart = Color.Lerp(curColPart, Game_Manager.instance.uiCoolingBlue, Flicker.noise(1, Camera_Helper.CurrentFlicker(), 0, 0f));
        }
        GUI.color = colPart;
        GUI.Box(new Rect(Game_Resolution.instance.nativeScreenWidth - Game_Manager.instance.spacing.x - 61,
            Game_Resolution.instance.nativeScreenHeight - Game_Manager.instance.spacing.y - Game_Manager.instance.textureSize - 168, 27,
            -(Game_Manager.instance.textureSize * 1.17f / (100 / Player_Stats.instance.afterburnerTemp))), "");
    }
}