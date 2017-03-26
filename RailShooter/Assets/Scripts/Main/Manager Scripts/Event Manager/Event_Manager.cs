using UnityEngine;
using System.Collections;
//
public class Event_Manager : MonoBehaviour {
    //
    public static Event_Manager instance;
    //
    public delegate void DialogVerification();
    //
    public static event DialogVerification Dialog_Portrait_True;//ftfd
    public static event DialogVerification Dialog_Portrait_False;
    public static event DialogVerification Dialog_True;//change thhis name
    public static event DialogVerification Dialog_False;
    //
    public delegate void DialogTextures(int number);
    //
    public static event DialogTextures Dialog_Portrait_Emma;
    //
    public static event DialogTextures Dialog_Intro;
    bool intro_bl;
    public static event DialogTextures Dialog_EngineHeat20;
    bool engineStatus20;
    //public static event DialogTextures Dialog_EngineHeat80;
    //
    float timer;
    int time = 6;
    //
    void OnEnable() {
        Player_Stats_EvHandler.EngineStatus20 += EngineStatus20;
    }
    //
    void Awake () {
        instance = this;
	}
    //
	void Update () {
        Introduction();
        //Debug.Log(intro_bl);
    }
    //
    void Introduction() {
        if (intro_bl == false) {
            StartCoroutine(OneTextBoxWait(time, Dialog_EngineHeat20, Dialog_Portrait_Emma));
            intro_bl = true;
        }
    }
    //
    void EngineStatus20() {
        //
        if (engineStatus20 == false) {
            StartCoroutine(TwoTextBoxes(5, Dialog_EngineHeat20, Dialog_Portrait_Emma));
            engineStatus20 = true;
        }
    }
    //
    IEnumerator OneTextBoxWait(int time, DialogTextures texture, DialogTextures portrait) {
        //
        yield return new WaitForSeconds(1);
        //
        if (Dialog_Portrait_True != null) {
            Dialog_Portrait_True();
        }
        if (portrait != null) {
            portrait(1);
        }
        if (Dialog_True != null) {
            Dialog_True();
        }
        if (texture != null) {
            texture(1);
        }
        yield return new WaitForSeconds(time);
        if (Dialog_False != null) {
            Dialog_False();
        }
        if (Dialog_Portrait_False != null) {
            Dialog_Portrait_False();
        }
        yield return null;
    }
    //
    IEnumerator TwoTextBoxes(float textTime, DialogTextures texture, DialogTextures portrait) {
        //
        yield return new WaitForSeconds(1);
        //
        if (Dialog_Portrait_True != null) {
            Dialog_Portrait_True();
        }
        if (portrait != null) {
            portrait(1);
        }
        if (Dialog_True != null) {
            Dialog_True();
        }
        if (texture != null) {
            texture(1);
        }
        yield return new WaitForSeconds(time);
        if (Dialog_False != null) {
            Dialog_False();
        }
        if (Dialog_True != null) {
            Dialog_True();
        }
        if (texture != null) {
            texture(2);
        }
        yield return new WaitForSeconds(time);
        if (Dialog_False != null) {
            Dialog_False();
        }
        if (Dialog_Portrait_False != null) {
            Dialog_Portrait_False();
        }
        yield return null;
    }
    //
    IEnumerator ThreeTextBoxes (float textTime, DialogTextures texture, DialogTextures portrait) {
        //
        yield return new WaitForSeconds(1);
        //
        if (Dialog_Portrait_True != null) {
            Dialog_Portrait_True();
        }
        if (portrait != null) {
            portrait(1);
        }
        if (Dialog_True != null) {
            Dialog_True();
        }
        if (texture != null) {
            texture(1);
        }
        yield return new WaitForSeconds(time);
        if (Dialog_False != null) {
            Dialog_False();
        }
        if (Dialog_True != null) {
            Dialog_True();
        }
        if (texture != null) {
            texture(2);
        }
        yield return new WaitForSeconds(time);
        if (Dialog_False != null) {
            Dialog_False();
        }
        if (Dialog_True != null) {
            Dialog_True();
        }
        if (Dialog_Intro != null) {
            texture(3);
        }
        yield return new WaitForSeconds(time);
        if (Dialog_False != null) {
            Dialog_False();
        }
        if (Dialog_Portrait_False != null) {
            Dialog_Portrait_False();
        }
        yield return null;
    }
}
