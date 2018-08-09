using HoloToolkit.UI.Keyboard;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class OkButton : InteractionReceiver {

    protected override void InputUp(GameObject obj, InputEventData eventData)
    {
        PlayerPrefs.SetString("Username", GameObject.Find("Username").GetComponent<KeyboardInputField>().text);
        PlayerPrefs.SetString("Password", GameObject.Find("Password").GetComponent<KeyboardInputField>().text);
        GameObject.Find("Settings").SetActive(false);
    }
}
