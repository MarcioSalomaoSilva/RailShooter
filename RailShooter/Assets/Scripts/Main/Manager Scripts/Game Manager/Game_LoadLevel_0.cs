using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_LoadLevel_0 : MonoBehaviour {
    //
    public static Game_LoadLevel_0 instance;
    //
    [Header("Menu Textures")]
    public Texture firstWindow;
    public Texture secondWindow;
    public Texture thirdWindow;
    public Texture forthWindow;
    //
    void Awake () {
        instance = this;
	}
    //
    void OnEnable() {
        Game_Manager.LoadLevel_0 += Level_0;
    }
    void OnDisable() {
        Game_Manager.LoadLevel_0 -= Level_0;
    }
	// Update is called once per frame
	void Update () {
        
	}
    //
    void Level_0 () {
        GameObject Menu = new GameObject("Menu Manager");
        Menu.transform.parent = this.transform;
        Menu.transform.localPosition = Vector3.zero;
        Menu.AddComponent<Menu_GUI>();
    }
}
