using UnityEngine;
using System.Collections;
//
public class Text_Manager : MonoBehaviour {
    //
    public delegate void Send(Texture texture);
    public static event Send Send_Text_Texture;
    public static event Send Send_Portrait_Texture;
    //
    Texture curPortrait;
    public Texture blankPortrait;
    //
    public Texture emmaPortrait;
    //
    Texture curTexture;
    public Texture blankScript;
    //
    public Texture testScript1;
    public Texture testScript2;
    public Texture testScript3;
    //
    public Texture EngineHeat20_1;
    public Texture EngineHeat20_2;
    //
    void OnEnable() {
        //
        Event_Manager.Dialog_Portrait_Emma += AssignTexture_Portrait_Emma;
        //
        Event_Manager.Dialog_Intro += AssignTexture_Intro;
        Event_Manager.Dialog_EngineHeat20 += AssignTexture_EngineHeat20;
    }
    //
    void AssignTexture_Portrait_Emma(int number) {
        //
        curPortrait = emmaPortrait;
        SendPortraitTexture();
    }
    //
    void AssignTexture_Intro(int number) {
        //
        if (number == 1) {
            curTexture = testScript1;
        } else if (number == 2) {
            curTexture = testScript2;
        } else if (number == 3) {
            curTexture = testScript3;
        } else {
            curTexture = blankScript;
        }
        SendTextTexture();
    }
    void AssignTexture_EngineHeat20(int number) {
        //
        if (number == 1) {
            curTexture = EngineHeat20_1;
        } else if (number == 2) {
            curTexture = EngineHeat20_2;
        }  else {
            curTexture = blankScript;
        }
        SendTextTexture();
    }
    //
    void SendTextTexture() {
        if (Send_Text_Texture != null) {
            Send_Text_Texture(curTexture);
        }
    }
    void SendPortraitTexture() {
        if (Send_Portrait_Texture != null) {
            Send_Portrait_Texture(curPortrait);
        }
    }
    //
    //public Texture[] AddItemToArray(this Texture[] original, Texture itemToAdd) {
    //    //
    //    Texture[] finalArray = new Texture[original.Length + 1];
    //    for (int i = 0; i < original.Length; i++ ) {
    //        finalArray[i] = original[i];
    //    }
    //    finalArray[finalArray.Length - 1] = itemToAdd;
    //    return finalArray;
    //}
}
