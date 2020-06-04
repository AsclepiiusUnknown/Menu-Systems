using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Refernces to GameObjects to switch on/off depending on what we need visible
    public GameObject mainMenuHolder;
    public GameObject optionsUiHolder;

    //Used to start a game
    public void Play()
    {
        SceneManager.LoadScene("Arena Game");
    }

    //Used to quit game
    public void Quit()
    {
#if UNITY_EDITOR //If it is being run in unity editor
        UnityEditor.EditorApplication.isPlaying = false; //Stop editor simulation
#endif

        Application.Quit(); //Stop the application if there is one
    }

    //used to go to the options menu
    public void GoToOptionsMenu()
    {
        mainMenuHolder.SetActive(false); //disable the main menu UI
        optionsUiHolder.SetActive(true); //enable the options UI
    }
}
