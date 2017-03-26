using UnityEngine;
using System.Collections;

public class Menu_GUI : MonoBehaviour {
    //
    public static Menu_GUI instance;
    //
    public delegate void NextLevel();
    public static event NextLevel loadNextLevel;
    //
    int currentWindow;
    //
    public bool loadReady;
    //
    Texture currentTexture;
    //
    void Awake () {
        instance = this;
        currentWindow = 1;
	}
    //
	void Update () {
        //
	}
    //
    void CheckWindow() {
        switch (currentWindow) {
            case 1:
                currentTexture = Game_LoadLevel_0.instance.firstWindow;
                break;
            case 2:
                currentTexture = Game_LoadLevel_0.instance.secondWindow;
                break;
            case 3:
                currentTexture = Game_LoadLevel_0.instance.thirdWindow;
                break;
            case 4:
                currentTexture = Game_LoadLevel_0.instance.forthWindow;
                break;
            default:
                currentWindow = 1;
                break;
        }
    }
    //
    void OnGUI() {
        float rx = Screen.width / Game_Resolution.instance.nativeScreenWidth;
        float ry = Screen.height / Game_Resolution.instance.nativeScreenHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        //
        float width = Game_Resolution.instance.nativeScreenWidth;
        float height = Game_Resolution.instance.nativeScreenHeight;
        //
        if (loadReady == false && GameObject.Find("PlayerRig") == null) {
            CheckWindow();
            //
            GUI.Box(new Rect(0, 0, width, height), "Menu");
            GUI.DrawTexture(new Rect(Game_Manager.instance.spacing.x * 2, Game_Manager.instance.spacing.x * 2,
                width - Game_Manager.instance.spacing.x * 2, height - Game_Manager.instance.spacing.x * 2), currentTexture, ScaleMode.ScaleToFit);
            if (currentWindow > 1) {
                if (GUI.Button(new Rect(0, height - (height / 8), width / 3, height / 8), "Previous")) {
                    currentWindow = currentWindow - 1;
                }
            }
            if (currentWindow < 4) {
                if (GUI.Button(new Rect(width - (width / 3), height - (height / 8), width / 3, height / 8), "Next")) {
                    currentWindow = currentWindow + 1;
                }
            }
            if (currentWindow == 4) {
                if (GUI.Button(new Rect(width / 3, height - (height / 8), width / 3, height / 8), "Start")) {
                    if (loadNextLevel != null) {
                        loadNextLevel();
                        loadReady = true;
                    }
                }
            }
        }
    }
    //
    void OnValidate() {
        currentWindow = Mathf.Clamp(currentWindow, 1, 4);
    }
}
