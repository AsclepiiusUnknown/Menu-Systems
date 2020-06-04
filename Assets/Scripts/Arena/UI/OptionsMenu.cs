using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    //Refernces to GameObjects to switch on/off depending on what we need visible
    public GameObject mainMenuHolder;
    public GameObject pauseUiHolder;
    public GameObject optionsUiHolder;

    //Used to check if the game is or isnt in fullscreen at any moment in time
    [HideInInspector]
    public bool isFullscreen = false;

    public Slider[] volumeSliders; //Array of sliders for volume control
    public Toggle[] resToggles; //An array of toggles that acts as a m=toggle group for selecting the resolution
    public int[] screenWidths; //An array of integers used to control the width of the screen when changing resolutions
    [HideInInspector]
    public int activeScreenResIndex; //the index value of the screen resolution currently being used
    [HideInInspector]
    public int sharedQualityIndex = 3; //index value of the screen quality

    public bool canCheer = true; //Used to check if the game is or isnt able to cheer at any moment in time (currently unused)

    //Called on the first frame the script is active
    private void Start()
    {
        activeScreenResIndex = PlayerPrefs.GetInt("Screen Res Index");

        //bool isFullscreen = (PlayerPrefs.GetInt("Full Screen") == 1) ? true : false;

        //Set the values of each volume slider being used to the appropriate volume level from the Audio Manager
        volumeSliders[0].value = AudioManager.instance.masterVolPercent;
        volumeSliders[1].value = AudioManager.instance.musicVolPercent;
        volumeSliders[2].value = AudioManager.instance.sfxVolPercent;

        for (int i = 0; i < resToggles.Length; i++) //For each of the resolution toggles
        {
            //Used to set the correct display of the toggles
            resToggles[i].isOn = i == activeScreenResIndex;
        }

        SetFullscreen(isFullscreen);
    }
    public void GoToMainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsUiHolder.SetActive(false);
    }
    public void SetScreenRes(int i)
    {
        if (resToggles[i] == null)
        {
            Debug.Log("resToggles[i]: " + resToggles[i] + ". Res Toggles length: " + resToggles.Length);

            return;
        }

        if (resToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("Screen Res Index", activeScreenResIndex);
            PlayerPrefs.Save();
        }
    }
    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resToggles.Length; i++)
        {
            resToggles[i].interactable = !isFullscreen;
        }

        if (isFullscreen)
        {
            Resolution[] allRes = Screen.resolutions;
            Resolution maxRes = allRes[allRes.Length - 1];
            Screen.SetResolution(maxRes.width, maxRes.height, true);
        }
        else
        {
            Debug.Log(activeScreenResIndex.ToString());
            if (0 <= activeScreenResIndex && activeScreenResIndex <= resToggles.Length)
            {
                SetScreenRes(activeScreenResIndex);
            }
            else
            {
                SetScreenRes(0);
            }
        }

        PlayerPrefs.SetInt("Full Screen", ((isFullscreen) ? 1 : 0));
        PlayerPrefs.Save();
    }
    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }
    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }
    public void SetSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.SFX);
    }
    public void SetCheers(bool _isCheering)
    {
        if (_isCheering)
        {
            _isCheering = false;
            canCheer = false;
            PlayerPrefs.SetInt("Can Cheer", ((canCheer) ? 1 : 0));
            PlayerPrefs.Save();
        }
        else
        {
            _isCheering = true;
            canCheer = true;
        }
    }

    public void SetQuality(int qualityIndex)
    {
        sharedQualityIndex = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void BackToPause()
    {
        Time.timeScale = 0;
        pauseUiHolder.SetActive(true);
        optionsUiHolder.SetActive(false);
    }
}
