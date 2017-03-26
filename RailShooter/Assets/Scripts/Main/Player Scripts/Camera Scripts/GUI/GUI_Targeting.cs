using UnityEngine;
using System.Collections;

public class GUI_Targeting : MonoBehaviour {
    UnityEngine.Camera cam;
    //
    void Start() {
        cam = this.gameObject.GetComponent<UnityEngine.Camera>();
    }
    //
    void OnGUI() {
        float rx = Screen.width / Game_Resolution.instance.nativeScreenWidth;
        float ry = Screen.height / Game_Resolution.instance.nativeScreenHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        //
        Vector3 targetPos = cam.WorldToScreenPoint(Player_Manager.instance.target.position); // move this to its own component (this.transform.position)
        Vector2 targetScreenPos = GUIUtility.ScreenToGUIPoint(targetPos);
        targetScreenPos.y = Game_Resolution.instance.nativeScreenHeight - targetPos.y;
        //
        float textureTarget = Game_Manager.instance.textureSize / 4.5f;
        //
        GUI.color = Game_Manager.instance.uiGreen;
        Game_Manager.instance.uiGreen.a = Game_Manager.instance.uiTransLow.a;
        //
        if (targetPos.z < 0) {
            if (targetScreenPos.x > Game_Resolution.instance.nativeScreenWidth / 2) {
                Camera_Helper.DrawTextureAbove(Player_Manager.instance.arrowLeft);
            }
            if (targetScreenPos.x < Game_Resolution.instance.nativeScreenWidth / 2) {
                Camera_Helper.DrawTextureRadarAbove(Player_Manager.instance.arrowRight);
            }
        } else if (targetScreenPos.x > Game_Resolution.instance.nativeScreenWidth || targetScreenPos.x < 0) {
            if (targetScreenPos.x > Game_Resolution.instance.nativeScreenWidth / 2) {
                Camera_Helper.DrawTextureRadarAbove(Player_Manager.instance.arrowRight);
            }
            if (targetScreenPos.x < Game_Resolution.instance.nativeScreenWidth / 2) {
                Camera_Helper.DrawTextureAbove(Player_Manager.instance.arrowLeft);
            }
        } else {
            GUI.DrawTexture(new Rect(targetScreenPos.x - textureTarget / 2, targetScreenPos.y - textureTarget / 2, textureTarget, textureTarget), Player_Manager.instance.scopeTexture, ScaleMode.ScaleToFit);
        }
    }
}
