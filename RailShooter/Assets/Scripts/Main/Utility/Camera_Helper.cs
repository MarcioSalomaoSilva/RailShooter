using UnityEngine;
using System.Collections;

public class Camera_Helper { 
    //
    public static void DrawTexturesParts(Color colColorRight, Color colColorLeft, Texture texLeft, Texture texRight) {
        GUI.color = colColorLeft;
        DrawTexture(texLeft);
        GUI.color = colColorRight;
        DrawTexture(texRight);
    }
    //
    public static float TextureSize() {
        float textureSize = Game_Resolution.instance.nativeScreenHeight / 3;
        return textureSize;
    }
	//
	public static float HorizontalPlacemnet (float xSpacing){
		//
		float x =  xSpacing;
		return x;
	}
    //
    public static float HorizontalPlacemnetRadar(float xSpacing) {
        //
        float x = Game_Resolution.instance.nativeScreenWidth - xSpacing - TextureSize();
        return x;
    }
    //
    public static float VerticalPlacement (float ySpacing, float textureSize){
		//
		float x = Game_Resolution.instance.nativeScreenHeight - textureSize - ySpacing;
		return x;
	}
    //
    public static float VerticalPlacementAbove(float ySpacing, float textureSize) {
        //
        float x = Game_Resolution.instance.nativeScreenHeight - textureSize - ySpacing - textureSize * 0.73f;
        return x;
    }
    public static float CurrentFlicker() {
        float curFlicker = Mathf.Lerp(0, 0.7f, Player_Stats.instance.generatorTemp / 100);
        return curFlicker;
    }
	//
	public static void DrawTexture (Texture texture) {
		GUI.DrawTexture (new Rect (HorizontalPlacemnet(Game_Manager.instance.spacing.x), VerticalPlacement(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), texture, ScaleMode.ScaleToFit);
	}
    //
    public static void DrawTextureAbove(Texture texture) {
        GUI.DrawTexture (new Rect (HorizontalPlacemnet(Game_Manager.instance.spacing.x), VerticalPlacementAbove(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), texture, ScaleMode.ScaleToFit);
    }
    //
    public static void DrawTextureRadar(Texture texture) {
        GUI.DrawTexture(new Rect(HorizontalPlacemnetRadar(Game_Manager.instance.spacing.x), VerticalPlacement(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), texture, ScaleMode.ScaleToFit);
    }
    //
    public static void DrawTextureRadarAbove(Texture texture) {
        GUI.DrawTexture(new Rect(HorizontalPlacemnetRadar(Game_Manager.instance.spacing.x), VerticalPlacementAbove(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), texture, ScaleMode.ScaleToFit);
    }
    //
    public static void DrawTwoTextures (Texture texture1, Texture texture2) {
		GUI.DrawTexture (new Rect (HorizontalPlacemnet(Game_Manager.instance.spacing.x), VerticalPlacement(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), texture1, ScaleMode.ScaleToFit);
		GUI.DrawTexture (new Rect (HorizontalPlacemnet(Game_Manager.instance.spacing.x), VerticalPlacement(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), texture2, ScaleMode.ScaleToFit);
	}
    //
    public static void DrawThreeTextures(Texture font, Texture line, Texture texture) {
        GUI.DrawTexture(new Rect(HorizontalPlacemnet(Game_Manager.instance.spacing.x), VerticalPlacement(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), font, ScaleMode.ScaleToFit);
        GUI.DrawTexture(new Rect(HorizontalPlacemnet(Game_Manager.instance.spacing.x), VerticalPlacement(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), line, ScaleMode.ScaleToFit);
        GUI.DrawTexture(new Rect(HorizontalPlacemnet(Game_Manager.instance.spacing.x), VerticalPlacement(Game_Manager.instance.spacing.y, TextureSize()), TextureSize(), TextureSize()), texture, ScaleMode.ScaleToFit);
    }
    //
    public static Color CurrentShieldColor (float curPartTemp, Player_Input.CurrentCooling curCool) {
        Color colPart;
        Color curColPart;
        //
        if (curPartTemp > 50) {
            colPart = Color.Lerp(Game_Manager.instance.uiYellow, Game_Manager.instance.uiGreen, ((curPartTemp / 100) - 0.5f) * 2);
            colPart.a = Mathf.Lerp(Game_Manager.instance.uiTransHigh.a, Game_Manager.instance.uiTransLow.a, curPartTemp / 100);
        }
        else {
            colPart = Color.Lerp(Game_Manager.instance.uiRed, Game_Manager.instance.uiYellow, (curPartTemp / 100) * 2);
            colPart.a = Mathf.Lerp(Game_Manager.instance.uiTransHigh.a, Game_Manager.instance.uiTransLow.a, curPartTemp / 100);
        }
        if (curPartTemp < 0.1f) {
            colPart = Game_Manager.instance.uiBlack;
            colPart.a = Player_Manager.instance.transparency - Game_Manager.instance.transparencyRange;
        }
		curColPart = colPart;
		if (Player_Input.instance.curState == Player_Input.CurrentState.Cooling && Player_Input.instance.curCooling == curCool) {
            colPart = Color.Lerp(curColPart, Game_Manager.instance.uiCoolingBlue, Flicker.noise(1, CurrentFlicker(), 0, 0f));
        }
		return colPart;
	}
    //
    public static Color CurrentColor(float curPartTemp, Player_Input.CurrentCooling curCool) {
        Color colPart;
        Color curColPart;
        //
        if (curPartTemp < 60)
        {
            colPart = Color.Lerp(Game_Manager.instance.uiGreen, Game_Manager.instance.uiYellow, (curPartTemp / 100) * 2);
            colPart.a = Mathf.Lerp(Game_Manager.instance.uiTransLow.a, Game_Manager.instance.uiTransHigh.a, curPartTemp / 100);
        }
        else
        {
            colPart = Color.Lerp(Game_Manager.instance.uiYellow, Game_Manager.instance.uiRed, ((curPartTemp / 100) - 0.5f) * 2);
            colPart.a = Mathf.Lerp(Game_Manager.instance.uiTransLow.a, Game_Manager.instance.uiTransHigh.a, curPartTemp / 100);
        }
        if (curPartTemp > 100f)
        {
            colPart = Game_Manager.instance.uiBlack;
            colPart.a = Player_Manager.instance.transparency - Game_Manager.instance.transparencyRange;
        }
        curColPart = colPart;
        if (Player_Input.instance.curState == Player_Input.CurrentState.Cooling && Player_Input.instance.curCooling == curCool)
        {
            colPart = Color.Lerp(curColPart, Game_Manager.instance.uiCoolingBlue, Flicker.noise(1, CurrentFlicker(), 0, 0f));
        }
        return colPart;
    }
    public static Color CurrentColorAverage(float curPartTemp)
    {
        Color colPart;
        Color curColPart;
        //
        if (curPartTemp < 60)
        {
            colPart = Color.Lerp(Game_Manager.instance.uiGreen, Game_Manager.instance.uiYellow, (curPartTemp / 100) * 2);
            colPart.a = Mathf.Lerp(Game_Manager.instance.uiTransLow.a, Game_Manager.instance.uiTransHigh.a, curPartTemp / 100);
        }
        else
        {
            colPart = Color.Lerp(Game_Manager.instance.uiYellow, Game_Manager.instance.uiRed, ((curPartTemp / 100) - 0.5f) * 2);
            colPart.a = Mathf.Lerp(Game_Manager.instance.uiTransLow.a, Game_Manager.instance.uiTransHigh.a, curPartTemp / 100);
        }
        if (curPartTemp > 100f)
        {
            colPart = Game_Manager.instance.uiBlack;
            colPart.a = Player_Manager.instance.transparency - Game_Manager.instance.transparencyRange;
        }
        curColPart = colPart;
        if (Player_Input.instance.curState == Player_Input.CurrentState.Cooling)
        {
            colPart = Color.Lerp(curColPart, Game_Manager.instance.uiCoolingBlue, Flicker.noise(1, CurrentFlicker(), 0, 0f));
        }
        return colPart;
    }
}
