using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Receivers;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Buttons;
using UnityEngine.Video;

namespace HoloToolkit.Unity.Examples
{
    public class ButtonReceiver : InteractionReceiver
    {
        private string[] dummyData = { "PXL", "Scapta", "Microsoft", "UHasselt", "PXL", "Scapta", "Microsoft", "Microsoft"};

        protected override void FocusEnter(GameObject obj, PointerSpecificEventData eventData)
        {
            Debug.Log(obj.name + " : FocusEnter");
            Debug.Log("Entered " + obj.GetComponent<CompoundButtonText>().Text);
        }

        protected override void FocusExit(GameObject obj, PointerSpecificEventData eventData)
        {
            Debug.Log(obj.name + " : FocusExit");
            Debug.Log("Left " + obj.GetComponent<CompoundButtonText>().Text);
        }

        protected override void InputUp(GameObject obj, InputEventData eventData)
        {
            Debug.Log(gameObject.tag);
            if (gameObject.tag == "ProductMenu")
            {
                transform.localScale = new Vector3(0, 0, 0);
                gameObject.GetComponent<MenuManager>().destroyCurrentMenu();
                gameObject.GetComponent<MenuManager>().createNewMenu(dummyData, "CustomerMenu");
            }
            else if (gameObject.tag == "CustomerMenu")
            {
                if(obj.name == "Scapta")
                {
                    GameObject.Find("Screen").SetActive(true);
                    GameObject.Find("Video").SetActive(true);
                    GameObject.Find("Video").GetComponent<VideoBehaviour>().PlayVideo("http://techslides.com/demos/sample-videos/small.mp4");
                }
                Debug.Log(obj.name);
            }
        }
    }
}