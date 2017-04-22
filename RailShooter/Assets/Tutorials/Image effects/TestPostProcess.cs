using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TestPostProcess : MonoBehaviour {
    #region Variables
    public Shader curShader;
    public float depthPower = 1f;
    public float brightnessAmount = 1;
    public float saturationAmount = 1;
    public float contrastAmount = 1;
    public Texture overlayTexture;
    public float overlayOpacity = 1;
    public Texture blendTexture;
    public float blendOpacity = 1;
    private Material curMaterial;
    #endregion
    #region Properties
    Material material { 
    get {
            if (curMaterial == null) {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial; 
        }
    }
    #endregion
    void Awake () {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
        if (!SystemInfo.supportsImageEffects) {
            enabled = false;
            return;
        }
        if (!curShader && !curShader.isSupported) {
            enabled = false;
        }
	}
	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture) {
        if (curShader != null) {
            material.SetFloat("_DepthPower", depthPower);
            material.SetFloat("_BrightnessAmount", brightnessAmount);
            material.SetFloat("_SatAmount", saturationAmount);
            material.SetFloat("_ConAmount", contrastAmount);
            material.SetTexture("_OverlayTex", overlayTexture);
            material.SetFloat("_OverlayOpacity", overlayOpacity);
            material.SetTexture("_BlendTex", blendTexture);
            material.SetFloat("_BlendOpacity", blendOpacity);
            Graphics.Blit(sourceTexture, destTexture, material);
        } else {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }
    void Update() {
        //depth
        //Camera.main.depthTextureMode = DepthTextureMode.Depth; 
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
        depthPower = Mathf.Clamp(depthPower,0,5);
        //
        brightnessAmount = Mathf.Clamp(brightnessAmount, 0, 2);
        saturationAmount = Mathf.Clamp(saturationAmount, 0, 2);
        contrastAmount = Mathf.Clamp(contrastAmount, 0, 3);
        blendOpacity = Mathf.Clamp(blendOpacity, 0, 1);
        overlayOpacity = Mathf.Clamp(overlayOpacity, 0, 1);
    }
    void OnDisable() {
        if (curMaterial) {
            DestroyImmediate(curMaterial); 
        }
    }
}
