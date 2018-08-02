using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    
	// Use this for initialization
	void Start () {
        pdfConversion converter = gameObject.GetComponent<pdfConversion>();
        converter.Convert(@"C:/Eindwerk_Goos_Robin_final", @"C:/texting.jpg", 1, 10, "jpeg", 800, 600);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
