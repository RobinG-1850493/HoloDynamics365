using HoloToolkit.Unity.Receivers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

public class BackReceiver : InteractionReceiver
{
    // Handles Input events of the BackButton
    protected override void InputUp(GameObject obj, InputEventData eventData)
    {
        // Make sure the Application is ready to create a new menu
        GameObject.Find("MenuObject").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("MenuObject").GetComponent<TapToPlace>().enabled = false;
        GameObject.Find("Menu").GetComponent<InteractionReceiver>().enabled = true;
        GameObject.Find("VideoPlayers").GetComponent<TapToPlace>().enabled = false;
        GameObject.Find("VideoPlayers").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("YoutubePlayer").transform.localScale = new Vector3(0f, 0f, 0f);
        GameObject.Find("YoutubePlayer").GetComponent<SimplePlayback>().PlayerPause();
        GameObject.Find("VideoPlayers").GetComponent<TapToPlace>().enabled = false;

        // Destroy the current menu and create a new Product menu
        GameObject.Find("Menu").GetComponent<MenuManager>().destroyCurrentMenu();
        GameObject.Find("Menu").GetComponent<MenuManager>().CreateProductMenu().GetAwaiter();
    }
}
