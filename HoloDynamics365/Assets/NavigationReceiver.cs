using HoloToolkit.Unity.Examples;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationReceiver : InteractionReceiver {

    protected override void InputUp(GameObject obj, InputEventData eventData)
    {
        int currentPage = ButtonReceiver.currentPage;
        List<Texture2D> pages = ButtonReceiver.pdfPages;
        if (obj.name == "PageUp")
        {
            if(currentPage != pages.Count - 1)
            {
                GameObject.Find("PdfView").GetComponent<RawImage>().texture = pages[currentPage + 1];
                ButtonReceiver.currentPage++;
            }
        }
        else if (obj.name == "PageDown")
        {
            if (currentPage != 0)
            {
                GameObject.Find("PdfView").GetComponent<RawImage>().texture = pages[currentPage - 1];
                ButtonReceiver.currentPage--;
            }
        }
    }
}
