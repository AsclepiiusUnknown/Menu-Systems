using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public static OptionsMenu optionsMenu;
    void Awake()
    {
        optionsMenu = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<OptionsMenu>();
        if (!PlayerPrefs.HasKey("Loaded"))
        {
            PlayerPrefs.DeleteAll();
            //FirstLoad Function...sets up optionsMenu Data
            FirstLoad();
            //Save Data Creates first save file in binary
            Save();
            //We now have our first save file
            PlayerPrefs.SetInt("Loaded", 0);
        }
        else
        {
            Load();
        }
    }
    void FirstLoad()
    {
        OptionsData data = SaveSystem.LoadOptions();

        optionsMenu.SetFullscreen(true);
        optionsMenu.isFullscreen = true;

        //optionsMenu.SetScreenRes(0);
        optionsMenu.activeScreenResIndex = 0;

        optionsMenu.SetQuality(3);
        optionsMenu.sharedQualityIndex = 3;

        AudioManager.instance.SetVolume(1, AudioManager.AudioChannel.Master);
        AudioManager.instance.SetVolume(1, AudioManager.AudioChannel.Music);
        AudioManager.instance.SetVolume(1, AudioManager.AudioChannel.SFX);
    }
    public void Save()
    {
        //Do when Binary is done
        OptionsBinary.SaveOptionsData(optionsMenu);
    }
    public void Load()
    {
        //Do this when Binary is done
        OptionsData data = OptionsBinary.LoadOptionsData(optionsMenu);

        optionsMenu.isFullscreen = data.isFullscreen;
        optionsMenu.SetFullscreen(data.isFullscreen);

        //optionsMenu.SetScreenRes(data.activeScreenResIndex);
        optionsMenu.activeScreenResIndex = data.activeScreenResIndex;

        optionsMenu.SetQuality(data.qualityIndex);
        optionsMenu.sharedQualityIndex = data.qualityIndex;

        optionsMenu.SetMasterVolume(data.masterVolPercent);
        optionsMenu.SetMusicVolume(data.musicVolPercent);
        optionsMenu.SetSfxVolume(data.sfxVolPercent);
    }
}