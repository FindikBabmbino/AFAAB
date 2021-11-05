using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicSettingsManager : MonoBehaviour
{
    //THIS IS A PORT OF THE LILEAN GRAPHICS SETTINGS WE MIGHT WANT TO CHANGE THIS LATER 
    
    //Since we are going to use this to save to prefs it is easier to have it as a singelton 
    public static GraphicSettingsManager instance;

    private int currentScreenHeight;
    private int currentScreenWidth;
    private int qualityLevel;

    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else if(instance!=this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        LoadPrefs();
    }

    //This will get the players current screen settings and load them in when he launches the game
    private int GetScreenMode()
    {
        if(PlayerPrefs.GetInt("ScreenMode")==1)
        {
            return(int)FullScreenMode.ExclusiveFullScreen;
        }
        else if(PlayerPrefs.GetInt("ScreenMode")==0)
        {
            return(int)FullScreenMode.Windowed;
        }
        else if(PlayerPrefs.GetInt("ScreenMode")==2)
        {
            return(int)FullScreenMode.FullScreenWindow;
        }
        //default is fullscrean
        return(int)FullScreenMode.ExclusiveFullScreen;
    }

    //This will load the saved settings of the player
    private void LoadPrefs()
    {
        //This will get the quality settings of the game default will be the highest
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality",3),true);
        //This gets the height of the screen the default is the current height of the player this applies to the width as well
        currentScreenHeight=PlayerPrefs.GetInt("screenHeight",Screen.currentResolution.height);
        currentScreenWidth=PlayerPrefs.GetInt("screenWidth",Screen.currentResolution.width);
        //This sets the players screen to match the settings, we convert the int that comes back into a fullscreenmode
        Screen.SetResolution(currentScreenWidth,currentScreenHeight,(FullScreenMode)GetScreenMode());
    }

    //These funtions will help us get these values.
    public int GetScreenWidth()
    {
        return currentScreenWidth;
    }
    public int GetScreenHeight()
    {
        return currentScreenHeight;
    }
    public int GetQualityMode()
    {
        return qualityLevel;
    }

    public void ChangeScreenWidth(int var)
    {
        currentScreenWidth=var;
    }
    
    public void ChangeScreenHeight(int var)
    {
        currentScreenHeight=var;
    }
}