using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;
using HoloToolkit.Unity.Collections;
using HoloToolkit.Unity.Receivers;
using System.IO;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

public class MenuManager : MonoBehaviour, ISpeechHandler
{

    // Initiate variables 
    private GameObject parent;
    private InteractionReceiver receiver;
    private int counter = 0;
    private string[] dummyData = { "PXL", "Scapta", "Microsoft", "UHasselt", "PXL", "Scapta", "Microsoft", "UHasselt", "PXL" };

    // Use this for initialization
    void Start()
    {
        // Assigning parent object
        parent = this.gameObject;
        Debug.Log(parent.name);

        // Disable movement capabillities
        parent.GetComponent<TapToPlace>().enabled = false;
        parent.GetComponent<BoxCollider>().enabled = false;
        parent.GetComponent<InteractionReceiver>().enabled = true;
        GameObject.Find("Screen").SetActive(true);
        GameObject.Find("Video").SetActive(true);

        createNewMenu(dummyData, "ProductMenu");
    }

    public void PlacementOn()
    {
        parent.GetComponent<TapToPlace>().enabled = true;
        parent.GetComponent<InteractionReceiver>().enabled = false;
        GameObject screen = GameObject.Find("Screen");
        screen.SetActive(true);
        screen.GetComponent<TapToPlace>().enabled = true;

    }

    public void PlacementOff()
    {
        parent.GetComponent<BoxCollider>().enabled = false;
        parent.GetComponent<InteractionReceiver>().enabled = true;
        GameObject screen = GameObject.Find("Screen");
        screen.SetActive(false);
        screen.GetComponent<TapToPlace>().enabled = false;
    }

    public void ResetPlacement()
    {
        parent.transform.localPosition = new Vector3(0f, 0f, 2f);
    }

    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        //if (eventData.RecognizedText.Equals("Move Menu"))
        //{
        //    parent.GetComponent<TapToPlace>().enabled = true;
        //    parent.GetComponent<BoxCollider>().enabled = true;

        //}
        //else if (eventData.RecognizedText.Equals("Done Moving"))
        //{
        //    parent.GetComponent<TapToPlace>().enabled = false;
        //    parent.GetComponent<BoxCollider>().enabled = false;

        //}
        //else if (eventData.RecognizedText.Equals("Reset Menu"))
        //{
        //    parent.transform.localPosition = new Vector3(0f, 0f, 2f);
        //}
        //else
        //{
        //    return;
        //}

        //eventData.Use();
    }

    private static Texture2D LoadFromImage(string path)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        return tex;
    }

    private static float CalculateAspectRatio(float width, float height)
    {
        float aspect_ratio = width / height;
        return aspect_ratio;
    }

    public void createNewMenu(string[] dummyData, string menuType)
    {
        parent = this.gameObject;
        Vector3 currentLocation = transform.localPosition;
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Setting up ObjectCollection on parent -- takes care of placement in relation to other gameObjects
        ObjectCollection buttonCollection = parent.GetComponent<ObjectCollection>();
        buttonCollection.CellWidth = 0.5f;
        buttonCollection.CellHeight = 0.5f;
        buttonCollection.SurfaceType = SurfaceTypeEnum.Plane;
        buttonCollection.Rows = 1;

        gameObject.tag = menuType;

        parent.GetComponent<BoxCollider>().size = new Vector3(buttonCollection.CellWidth * dummyData.Length, buttonCollection.CellHeight - 0.2f, 0.1f);

        foreach (string name in dummyData)
        {
            Debug.Log(name);

            // Instantiate the prefab button
            GameObject button = Instantiate(Resources.Load("HolographicButton")) as GameObject;
            button.name = name;
            Debug.Log(button.name);

            button.transform.localScale = new Vector3(3f, 3f, 1f);

            // Change the button text
            CompoundButtonText buttonText = button.GetComponent<CompoundButtonText>();
            buttonText.Text = name;
            buttonText.Size = 75;
            buttonText.OverrideSize = true;

            buttonText.Style = FontStyle.Bold;


            // Creating the Texture2D from the logo
            Texture2D logo = (Texture2D)Resources.Load(name);
            float aspect_ratio = CalculateAspectRatio(logo.width, logo.height);

            Debug.Log(aspect_ratio);

            // Change the button icon to the appropriate logo
            CompoundButtonIcon buttonIcon = button.GetComponent<CompoundButtonIcon>();
            buttonIcon.OverrideIcon = true;
            buttonIcon.IconName = "Ready";
            buttonIcon.iconOverride = logo;

            // Change button to appropriate scale -- Might wanna do this based on resolution
            GameObject iconObject = button.transform.Find("UIButtonSquareIcon").gameObject;
            // iconObject.transform.localScale.Set(2f, 2f, 1);
            buttonIcon.IconMeshFilter.transform.localScale = new Vector3(2.5f, 2.5f / aspect_ratio, 1f); // what if width < height

            // Initialize Receiver
            receiver = parent.GetComponent<InteractionReceiver>();

            // Add button to the receiver
            receiver.interactables.Add(button);

            // Add the button to the menu
            button.transform.SetParent(parent.transform);
            Debug.Log(parent.transform.childCount);
            Debug.Log(button.name);

            counter++;
        }
        buttonCollection.UpdateCollection();
        transform.localPosition = currentLocation;
    }

    public void destroyCurrentMenu()
    {
        int i = 0;
        transform.localScale = new Vector3(0f, 0f, 0f);
        GameObject[] allChildren = new GameObject[transform.childCount];
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i++;
        }

        foreach(GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
        Debug.Log(transform.childCount);
        transform.DetachChildren();
    }
}
