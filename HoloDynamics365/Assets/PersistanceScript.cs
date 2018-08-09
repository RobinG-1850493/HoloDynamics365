using HoloToolkit.UI.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistanceScript : MonoBehaviour {

    void Awake()
    {
        GameObject.Find("Username").GetComponent<KeyboardInputField>().text = PlayerPrefs.GetString("Username");
        GameObject.Find("Password").GetComponent<KeyboardInputField>().text = PlayerPrefs.GetString("Password");
        PlayerPrefs.Save();
    }
    // Use this for initialization
    void Start () {
        GameObject.Find("Username").GetComponent<KeyboardInputField>().text = PlayerPrefs.GetString("Username");
        GameObject.Find("Password").GetComponent<KeyboardInputField>().text = PlayerPrefs.GetString("Password");
        PlayerPrefs.Save();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
