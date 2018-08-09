using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPauseOnTap : InteractionReceiver
{

    protected override void InputUp(GameObject obj, InputEventData eventData)
    {
        GameObject.Find("YoutubePlayer").GetComponent<SimplePlayback>().Play_Pause();
    }
}