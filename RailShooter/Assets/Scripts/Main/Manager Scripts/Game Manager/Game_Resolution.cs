using UnityEngine;
using System.Collections;

public class Game_Resolution : MonoBehaviour {
    public static Game_Resolution instance;
    //
    public enum FullScreen
    {
        Windowed, FullSCreen
    }
    public FullScreen fullScreen;
    //
    public enum AspectRatio
    {
        TV, Widescreen, HDVIDEO
    }
    public AspectRatio aspectRatio;
    //
    public enum CurrentResolution
    {
    FHD, FWXGA, HDTV, SDTV, N64, PS1
    //1920x1080, 1366x768, 1280x720, 720x480, 640x480, 320x240
    //
    //4:3 aspect ratio resolutions: 640×480, 800×600, 960×720, 1024×768, 1280×960, 1400×1050, 1440×1080 , 1600×1200, 1856×1392, 1920×1440, and 2048×1536. // TV
    //16:10 aspect ratio resolutions: - 1280×800, 1440×900, 1680×1050, 1920×1200 and 2560×1600. // Widescreen
    //16:9 aspect ratio resolutions: 1024×576, 1152×648, 1280×720, 1366×768, 1600×900, 1920×1080, 2560×1440 and 3840×2160. // HDVIDEO
    }
    //
    public CurrentResolution resolution;
    //
    [HideInInspector]
    public float nativeScreenWidth = 1920;
    [HideInInspector]
    public float nativeScreenHeight = 1080;
    //
    void Start () {
        //
        instance = this;
        //
        Resolution[] resolutions = Screen.resolutions;
        //foreach (Resolution res in resolutions)
        //{
            //print(res.width + "x" + res.height);
        //}
        //
        print(Screen.currentResolution);
    }
	//
	public void Update () {
        //
        switch (resolution) {
            case CurrentResolution.FHD:
                Screen.SetResolution(1920,1080,false);
                break;
            case CurrentResolution.FWXGA:
                Screen.SetResolution(1366,768,false);
                break;
            case CurrentResolution.HDTV:
                Screen.SetResolution(1280, 720, false);
                break;
            case CurrentResolution.SDTV:
                Screen.SetResolution(720, 480, false);
                break;
            case CurrentResolution.N64:
                Screen.SetResolution(640, 480, false);
                break;
            case CurrentResolution.PS1:
                Screen.SetResolution(320, 240, false);
                break;
        }
	}
    void OnGUI()
    {
        float rx = Screen.width / nativeScreenWidth;
        float ry = Screen.height / nativeScreenHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        //
        if (Game_Manager.instance.debugging == Game_Manager.Debug.True && Game_Manager.instance.debugResolution == true)
        {
            if (GUI.Button(new Rect(800, 800, 200, 130), "SDTV"))
            {
                resolution = CurrentResolution.SDTV;
                Debug.Log("Clicked the button with text");
            }
        }
    }
}
