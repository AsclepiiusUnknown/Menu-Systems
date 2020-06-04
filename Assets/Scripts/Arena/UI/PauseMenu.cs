using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //Refernces to GameObjects to switch on/off depending on what we need visible
    public GameObject gameUiHolder;
    public GameObject pauseUiHolder;
    public GameObject optionsUiHolder;

    public bool isPaused;

    //Called every frame
    private void Update()
    {
        //Checking constantly if the escape key has been pressed
        if (Input.GetKeyDown(KeyBindManager.keys["Pause"]))
        {
            TogglePause(); //if so then call the TogglePause function
        }
    }

    //used to quit the application
    public void Quit()
    {
#if UNITY_EDITOR //If it is being run in unity editor
        UnityEditor.EditorApplication.isPlaying = false; //Stop editor simulation
#endif

        Application.Quit(); //Stop the application if there is one
    }

    //Used to acess the options menu
    public void GoToOptionsMenu()
    {
        pauseUiHolder.SetActive(false); //Disable pause UI
        optionsUiHolder.SetActive(true); //enable options UI
        gameUiHolder.SetActive(false); //disable game UI
    }

    //Used to load the main menu scene
    public void GoToMenu()
    {
        Cursor.visible = true; //Make the cursor visible to the player again
        SceneManager.LoadScene("Menu");//Load the menu scene
    }

    //used to toggle pause on/off
    public void TogglePause()
    {
        if (isPaused) //if the game is currently paused
        {
            isPaused = false; //unpause
            Time.timeScale = 1; //set time to normal
            gameUiHolder.SetActive(true); //enable game UI
            pauseUiHolder.SetActive(false); //disable pause UI
        }
        else //otherwise if the game isnt paused
        {
            isPaused = true; //pause it
            Time.timeScale = 0; //stop time
            gameUiHolder.SetActive(false); //Disable the game UI
            pauseUiHolder.SetActive(true); //Enable the pause UI
        }
    }

    //Used to resume the game from the pause menu (same as if we were toggling pause off)
    public void ResumeGame()
    {
        isPaused = false; //unpause
        Time.timeScale = 1; //set time to normal
        gameUiHolder.SetActive(true); //enable the game UI
        pauseUiHolder.SetActive(false); //disable the pause UI
    }

    //used to return to the pause menu (usually from options. this gets rid of time bugs that occured during another approach)
    public void BackToPause()
    {
        Time.timeScale = 0; //Stop time
        gameUiHolder.SetActive(false); //disable the game UI
        pauseUiHolder.SetActive(true); //enable the pause UI
        optionsUiHolder.SetActive(false); //disable the options UI
    }
}
