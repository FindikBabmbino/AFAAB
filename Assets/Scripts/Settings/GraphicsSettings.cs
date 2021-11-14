using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    //THIS IS A PORT OF THE LILEAN GRAPHICS SETTINGS WE MIGHT WANT TO CHANGE THIS LATER 
    //-TODO change the screenmode from toggle to dropdown so we can add maximised windowed mode.

    //These will be the interfaces that will help the player controll the settings
    [SerializeField]private Dropdown resolutionDropdown;
    [SerializeField]private TMP_Dropdown graphicQualityDropDown;
    [SerializeField]private Toggle screenModeToggle;
    //This will keep all the current available resolutions and put them into the list
    private List<Resolution>screens=new List<Resolution>();
    private FullScreenMode screenMode;
    private int graphicDropDownValue;

    void Start()
    {
        //These functions will be called at the start to set everything up
        SetUpGraphicQualityDropDown();
        SetResolution();
        GetCurrentResolution();
        screenModeToggle.isOn = SetToggleValue();

        //With the help of the delegate we will use the functions on the buttons
        resolutionDropdown.onValueChanged.AddListener(delegate { ChangeAndSaveResolution(); });
        screenModeToggle.onValueChanged.AddListener(delegate { SetScreenMode(); });
        graphicQualityDropDown.onValueChanged.AddListener(delegate { ChangeGraphicQuality(); });
    }

    //This will get every resolution that is available to the player and putt them all into the dropdown
    private void SetResolution()
    {
        for(int i=0;i<Screen.resolutions.Length;i++)
        {
            screens.AddRange(Screen.resolutions);
            resolutionDropdown.options.Add(new Dropdown.OptionData(ResolutionString(screens[i])));
        }
    }
    //This will convert the resolution to the readable string
    private string ResolutionString(Resolution screenRes)
    {
        return screenRes.width+"x"+screenRes.height+"@"+screenRes.refreshRate+"Hz";
    }

    //This will set the current resolution to display on the dropdown
    private void GetCurrentResolution()
    {
        for(int i=0;i<resolutionDropdown.options.Count;i++)
        {
            if(screens[i].width==GraphicSettingsManager.instance.GetScreenWidth()&&screens[i].height==GraphicSettingsManager.instance.GetScreenHeight())
            {
                resolutionDropdown.value=i;
            }
        }
    }

    //This will handle changing the resolution and also savinging it in the prefs
    private void ChangeAndSaveResolution()
    {
        //We get the list of screens and feed in the dropdowns value which should match the lists we don't set the screenmode yet because it does not change the aspect ratio if we do it like this
        Screen.SetResolution(screens[resolutionDropdown.value].width,screens[resolutionDropdown.value].height,FullScreenMode.ExclusiveFullScreen);
        //This is done to reset the aspect ratio
        if(PlayerPrefs.GetInt("ScreenMode")==1)
        {
            Screen.fullScreen=false;
            Screen.fullScreen=true;
        }
        //Now it is time to save
        PlayerPrefs.SetInt("screenWidth",screens[resolutionDropdown.value].width);
        PlayerPrefs.SetInt("screenHeight",screens[resolutionDropdown.value].height);
        PlayerPrefs.Save();
        //This is so that we can see the changes in the manager as well
        GraphicSettingsManager.instance.ChangeScreenWidth(screens[resolutionDropdown.value].width);
        GraphicSettingsManager.instance.ChangeScreenHeight(screens[resolutionDropdown.value].height);
    }

    //This controls the toggle if it is on it is fullscrean if not windowed
    //But we will probably change this into a dropdown so we can add maximised window.
    private void SetScreenMode()
    {
        if (screenModeToggle.isOn)
        {
            PlayerPrefs.SetInt("ScreenMode", 1);
            PlayerPrefs.Save();
            Screen.fullScreen = true;
        }
        if(!screenModeToggle.isOn)
        {
            PlayerPrefs.SetInt("ScreenMode", 0);
            PlayerPrefs.Save();
            Screen.fullScreen = false;
        }
    }

    //This funtion is used to update toggle when we launch the game again
    private bool SetToggleValue()
    {
        if (PlayerPrefs.GetInt("ScreenMode") == 1)
        {
            return true;
        }
        else if (PlayerPrefs.GetInt("ScreenMode") == 0)
        {
            return false;
        }
        return true;
    }

    //This will handle the changes in the premade graphic qualities
    private void ChangeGraphicQuality()
    {
        //When you select one of them it will check what the value corresponds to in the system and change to that
        //Lot of reused same code but it works so I am not complaning maybe in the future I'll change it
        if(graphicQualityDropDown.value==0)
        {
            graphicDropDownValue = graphicQualityDropDown.value;
            PlayerPrefs.SetInt("Quality", graphicDropDownValue);
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"), true);
            PlayerPrefs.Save(); 
        }
        else if(graphicQualityDropDown.value==1)
        {
            graphicDropDownValue = graphicQualityDropDown.value;
            PlayerPrefs.SetInt("Quality", graphicDropDownValue);
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"), true);
            PlayerPrefs.Save(); 
        }
        else if(graphicQualityDropDown.value==2)
        {
            graphicDropDownValue = graphicQualityDropDown.value;
            PlayerPrefs.SetInt("Quality", graphicDropDownValue);
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"), true);
            PlayerPrefs.Save(); 
        }
    }

    //This will set the graphic quality of the game when you start it up
    private void SetUpGraphicQualityDropDown()
    {
        graphicDropDownValue =PlayerPrefs.GetInt("Quality");
        graphicQualityDropDown.value = graphicDropDownValue;
    }
}