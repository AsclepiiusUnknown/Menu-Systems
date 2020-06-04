using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PAB : MonoBehaviour
{
    public GameObject menuUI, PABUI;
    private bool isPAB;
    public TextMeshProUGUI PabTitle;
    private Color lerpedFlash;
    [TextArea]
    public string flashText;

    private void Start()
    {
        isPAB = true;
        PabTitle.text = flashText;

        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && isPAB == true)
        {
            Debug.Log("A key or mouse click has been detected");
            if (PABUI != null && menuUI != null)
            {
                PABUI.SetActive(false);
                menuUI.SetActive(true);
                isPAB = false;
                Debug.Log("PAB -> Menu Successful");
            }
            else
            {
                if (PABUI == null)
                {
                    Debug.LogError("PABUI for PAB.cs not found");
                }
                else if (menuUI == null)
                {
                    Debug.LogError("menuUI for PAB.cs not found");
                }
            }
            //SceneManager.LoadScene("Arena Game");
        }
    }

    public IEnumerator BlinkText()
    {
        //blink it forever. You can set a terminating condition depending upon your requirement
        while (isPAB)
        {
            //set the Text's text to blank
            PabTitle.text = "";
            //display blank text for 0.5 seconds
            yield return new WaitForSeconds(.5f);
            //display “I AM FLASHING TEXT” for the next 0.5 seconds
            PabTitle.text = flashText;
            yield return new WaitForSeconds(.5f);
        }
    }
}
