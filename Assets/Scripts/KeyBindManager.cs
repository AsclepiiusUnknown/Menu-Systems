using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindManager : MonoBehaviour
{
    [SerializeField]
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    [System.Serializable] //Allows use to see & use what we create below
    public struct KeyUISetup
    {
        public string keyName;
        public TextMeshProUGUI keyDisplayText;
        public string defaultKey;
    }

    public KeyUISetup[] baseSetup; //Array of the elements created above in KeyUISetup
    public GameObject currentKey; //Stores the key we are currently using and ppossibly changing
    public Color32 changedKey = new Color32(39, 171, 249, 255); //Color we change the button to once we have changed the associated key
    public Color32 selectedKey = new Color32(239, 116, 36, 255); //Color we change the button to when its selected

    // Start is called before the first frame update
    void Start()
    {
        //Loop adds the keys to the dictionary (created above) with either save or default (depending on load)
        for (int i = 0; i < baseSetup.Length; i++) //For all the keys in the base setup array
        {
            //add keys according to the sved string or default
            keys.Add(baseSetup[i].keyName, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(baseSetup[i].keyName, baseSetup[i].defaultKey)));

            //Change the display to what the Bind is for each UI Text component
            baseSetup[i].keyDisplayText.text = keys[baseSetup[i].keyName].ToString();
        }
    }

    //used when we want to save the keys and changes
    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    //used to change the passed key
    public void ChangeKey(GameObject clickedKey)
    {
        currentKey = clickedKey;
        if (clickedKey != null) //if we have clicked a key and its currently selected
        {
            currentKey.GetComponent<Image>().color = selectedKey; //Create a visual element that shows the user the button was succesfully pressed
        }
    }

    //Used beacuse it allows us to run events
    private void OnGUI()
    {
        string newKey = "";
        Event e = Event.current;

        if (currentKey == null) //Fixes issues for later on by exiting the function when we dont need to use it
            return;

        if (e.isKey)
        {
            newKey = e.keyCode.ToString();
        }

        //The following fixes an issue with setting the shift keys by hard coding it im
        if (Input.GetKey(KeyCode.LeftShift))
        {
            newKey = "LeftShift";
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            newKey = "RightShift";
        }

        if (newKey != "") //if a key has been set
        {
            keys[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey); //Changes out the jey in the dictionary to the new one we just pressed
            currentKey.GetComponentInChildren<TextMeshProUGUI>().text = newKey; //Changes the display text to match the new key
            currentKey.GetComponent<Image>().color = changedKey; //Changes the color to show we have changed the key
            currentKey = null; //Reset the variable and wait until another has been pressed and the cycle repeats
        }
    }
}
