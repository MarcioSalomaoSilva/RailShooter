using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 
//
public class Game_Manager : MonoBehaviour {
	//
	public static Game_Manager instance;
    //
    public delegate void LoadLevel();
    public static event LoadLevel LoadLevel_0;
    //
    public enum Debug {
		True, False
	}
    int curScene;
	//
	public Debug debugging;
    //
    public bool debugShip;
    public bool debugGrid;
    public bool debugResolution;
    public bool isPaused;
    //
    [Header("Text GUI")]
    public Vector2 spacing = new Vector2(20, 20);
    public float textureSize = 200;
    public float fadeSpeed;
    public Texture dialogShadow;
    [Header("Colours")]
    public float transparency = 0.3f;
    public float transparencyRange = 0.1f;
    public Color uiTransLow;
    public Color uiTransHigh;
    public Color uiGreen;
    public Color uiYellow;
    public Color uiRed;
    public Color uiBlue;
    public Color uiCoolingBlue;
    public Color uiBlack;
    // Use this for initialization
    void Awake () {
		instance = this;
        SetColors();
        CheckLevel();
        //
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Additive); // 1 for menu
    }
    void OnEnable() {
        Menu_GUI.loadNextLevel += LoadNextLevel;
    }
    //
    void Update() {
        curScene = SceneManager.GetActiveScene().buildIndex;
        UnityEngine.Debug.Log(curScene);
    }
    //
    void SetColors() {
        uiTransLow = new Color(0f, 0f, 0f, transparency - transparencyRange);
        uiTransHigh = new Color(0f, 0f, 0f, transparency + transparencyRange);
        uiGreen = new Color(0f, 1f, 0f, 1f);
        uiYellow = new Color(1f, 0.92f, 0.016f, 1f);
        uiRed = new Color(1f, 0f, 0f, 1f);
        uiBlue = new Color(0f, 0f, 1f, 1f);
        uiCoolingBlue = new Color(uiBlue.r, uiBlue.g, uiBlue.b, transparency);
        uiBlack = new Color(0f, 0f, 0f, 1f);
    }
    //
    void LoadNextLevel() {
        //
        SceneManager.LoadScene(curScene + 2, LoadSceneMode.Additive);
    }
    //
    void CheckLevel() {
        if (curScene == 0) {
            this.GetComponent<Game_LoadLevel_0>().enabled = true;
            if (LoadLevel_0 != null) {
                LoadLevel_0();
            }
        } 
    }
	//
    void OnValidate() {
        //
        if (textureSize < 200) {
            textureSize = 200;
        }
        if (textureSize > 600) {
            textureSize = 600;
        }
        if (spacing.x < 20) {
            spacing.x = 20;
        }
        if (spacing.y < 20) {
            spacing.y = 20;
        }
        if (spacing.x > Screen.height / 2) {
            spacing.x = Screen.height / 2;
        }
        if (spacing.y > Screen.height / 2) {
            spacing.y = Screen.height / 2;
        }
    }
}
