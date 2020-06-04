using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class OptionsData
{
    public bool isFullscreen;
    public int activeScreenResIndex;

    public int qualityIndex;

    public float masterVolPercent;
    public float musicVolPercent;
    public float sfxVolPercent;

    public OptionsData (OptionsMenu optionsMenu)
    {
        isFullscreen = optionsMenu.isFullscreen;
        activeScreenResIndex = optionsMenu.activeScreenResIndex;

        qualityIndex = optionsMenu.sharedQualityIndex;

        masterVolPercent = AudioManager.instance.masterVolPercent;
        musicVolPercent = AudioManager.instance.musicVolPercent;
        sfxVolPercent = AudioManager.instance.sfxVolPercent;
    }
}
