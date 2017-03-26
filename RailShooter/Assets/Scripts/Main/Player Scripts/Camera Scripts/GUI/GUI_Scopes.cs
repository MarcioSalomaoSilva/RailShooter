using UnityEngine;
using System.Collections;
//
public class GUI_Scopes : MonoBehaviour {
    //
    private GameObject scopes;
    public GameObject farScope;
    public GameObject nearScope;
    //
    void Start() {
        CreateObjects();
    }
    public void CreateObjects() {
        scopes = Scope();
        nearScope = NearScope();
        farScope = FarScope();
    }
    public GameObject Scope() {
        GameObject scopes = new GameObject("Scope Locations");
        scopes.transform.parent = GameObject.Find("Player").transform;
        scopes.transform.localPosition = Vector3.zero;
        return scopes;
    }
    public GameObject NearScope() {
        GameObject nearScope = new GameObject("Near Scope Location");
        nearScope.transform.parent = scopes.transform;
        nearScope.transform.localPosition = Vector3.forward * Player_Manager.instance.scopeDistance * Player_Manager.instance.universalScale;
        return nearScope;
    }
    public GameObject FarScope() {
        GameObject farScope = new GameObject("Far Scope Location");
        farScope.transform.parent = scopes.transform;
        farScope.transform.localPosition = Vector3.forward * Player_Manager.instance.scopeDistance * 1.5f * Player_Manager.instance.universalScale;
        return farScope;
    }
    public void OnGUI() {
        float textureSize = Game_Manager.instance.textureSize / 4.5f;
        float rx = Screen.width / Game_Resolution.instance.nativeScreenWidth;
        float ry = Screen.height / Game_Resolution.instance.nativeScreenHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        //
        GUI.color = Game_Manager.instance.uiGreen;
        Game_Manager.instance.uiGreen.a = Game_Manager.instance.uiTransLow.a;
        //
        Vector3 nearPos = Camera_Controller.instance.cam.WorldToScreenPoint(nearScope.transform.position);
        Vector2 nearScreenPos = GUIUtility.ScreenToGUIPoint(nearPos);
        nearScreenPos.y = Game_Resolution.instance.nativeScreenHeight - nearPos.y;
        float textureNear = textureSize;
        GUI.DrawTexture(new Rect(nearScreenPos.x - textureNear / 2, nearScreenPos.y - textureNear / 2, textureNear, textureNear), Player_Manager.instance.scopeTexture, ScaleMode.ScaleToFit);
        //
        Vector3 farPos = Camera_Controller.instance.cam.WorldToScreenPoint(farScope.transform.position);
        Vector2 farScreenPos = GUIUtility.ScreenToGUIPoint(farPos);
        farScreenPos.y = Game_Resolution.instance.nativeScreenHeight - farPos.y;
        float textureFar = textureSize * 1.2f;
        GUI.DrawTexture(new Rect(farScreenPos.x - textureFar / 2, farScreenPos.y - textureFar / 2, textureFar, textureFar), Player_Manager.instance.scopeTexture, ScaleMode.ScaleToFit);
    }
}
