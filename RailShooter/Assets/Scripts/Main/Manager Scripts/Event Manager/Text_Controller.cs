using UnityEngine;
using System.Collections;

public class Text_Controller : MonoBehaviour {
    //
    float transparencyText;
    float transparencyPortrait;
    //
    Texture textureText;
    Texture texturePortrait;
    //
    void OnEnable() {
        Event_Manager.Dialog_True += ShowDialog;
        Event_Manager.Dialog_False += HideDialog;
        Event_Manager.Dialog_Portrait_True += ShowPortrait;
        Event_Manager.Dialog_Portrait_False += HidePortrait;
        Text_Manager.Send_Text_Texture += AssignTextureText;
        Text_Manager.Send_Portrait_Texture += AssignTexturePortrait;
    }
    void AssignTextureText(Texture sentTexture) {
        textureText = sentTexture;
    }
    void AssignTexturePortrait(Texture sentTexture) {
        texturePortrait = sentTexture;
    }
    //
    private void ShowPortrait() {
        transparencyPortrait = 1f;
    }
    private void HidePortrait() {
        transparencyPortrait = 0f;
    }
    //
    private void ShowDialog() {
        transparencyText = 0.8f;
    }
    private void HideDialog() {
        transparencyText = 0f;
    }
    IEnumerator FadeIN() {
        //
        yield return transparencyText = Mathf.Lerp(transparencyText, 0.8f, Game_Manager.instance.fadeSpeed * Time.deltaTime);
    }
    IEnumerator FadeOUT() {
        //
        yield return transparencyText = Mathf.Lerp(transparencyText, 0f, Game_Manager.instance.fadeSpeed*2 * Time.deltaTime);
    }
    IEnumerator Wait() {
        //
        yield return new WaitForSeconds(1);
    }
    //
    void OnGUI() {
        float rx = Screen.width / Game_Resolution.instance.nativeScreenWidth;
        float ry = Screen.height / Game_Resolution.instance.nativeScreenHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        //
        //GUI.DrawTexture(new Rect(0, Game_Resolution.instance.nativeScreenHeight - Game_Resolution.instance.nativeScreenHeight/6, Game_Resolution.instance.nativeScreenWidth, 
        //Game_Resolution.instance.nativeScreenHeight / 6), Game_Manager.instance.dialogShadow, ScaleMode.StretchToFill);
        //
        GUI.color = new Color(1, 1, 1, transparencyText);
        if (textureText != null) {
            GUI.DrawTexture(new Rect(Game_Manager.instance.spacing.x * 3 + Camera_Helper.TextureSize(), Game_Resolution.instance.nativeScreenHeight - Game_Resolution.instance.nativeScreenHeight / 7,
                   Game_Resolution.instance.nativeScreenHeight, Game_Resolution.instance.nativeScreenHeight / 9), textureText, ScaleMode.ScaleToFit);
        }
        //
        GUI.color = new Color(1, 1, 1, transparencyPortrait);
        if (texturePortrait != null) {
            Camera_Helper.DrawTextureRadar(texturePortrait);
        }
    }
    //
    void OnDisable() {
        Event_Manager.Dialog_False -= HideDialog;
        Event_Manager.Dialog_True -= ShowDialog;
        Event_Manager.Dialog_Portrait_True -= ShowPortrait;
        Event_Manager.Dialog_Portrait_False -= HidePortrait;
        Text_Manager.Send_Text_Texture -= AssignTextureText;
        Text_Manager.Send_Portrait_Texture -= AssignTexturePortrait;
    }
}
