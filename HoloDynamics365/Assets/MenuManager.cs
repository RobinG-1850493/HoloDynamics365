using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;
using HoloToolkit.Unity.Collections;
using HoloToolkit.Unity.Receivers;
using System.IO;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

public class MenuManager : MonoBehaviour
{

    // Initiate variables 
    private GameObject parent;
    private InteractionReceiver receiver;
    private Texture2D urlLogo;
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
        GameObject.Find("Screen").transform.localScale = new Vector3(0f, 0f, 0f);
        GameObject.Find("Screen").GetComponent<TapToPlace>().enabled = true;

        createNewMenu(dummyData, "ProductMenu");
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    public void PlacementOn()
    {
        parent.GetComponent<TapToPlace>().enabled = true;
        parent.GetComponent<InteractionReceiver>().enabled = false;
        GameObject screen = GameObject.Find("Screen");
        screen.transform.localScale = new Vector3(1f, 0.58f, 1);
        screen.GetComponent<TapToPlace>().enabled = true;
        screen.GetComponent<BoxCollider>().enabled = true;
    }

    public void PlacementOff()
    {
        parent.GetComponent<BoxCollider>().enabled = false;
        parent.GetComponent<InteractionReceiver>().enabled = true;
        GameObject screen = GameObject.Find("Screen");
        screen.transform.localScale = new Vector3(0, 0, 0);
        screen.GetComponent<TapToPlace>().enabled = false;
        screen.GetComponent<BoxCollider>().enabled = false;
    }

    public void ResetPlacement()
    {
        parent.transform.localPosition = new Vector3(0f, 0f, 2f);
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
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(0f, 0f, 0f);

        // Setting up ObjectCollection on parent -- takes care of placement in relation to other gameObjects
        ObjectCollection buttonCollection = parent.GetComponent<ObjectCollection>();
        buttonCollection.CellWidth = 0.45f;
        buttonCollection.CellHeight = 0.45f;
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

            //StartCoroutine(loadImageFromUrl("https://www.pxl.be/Assets/website/pxl_algemeen/afbeeldingen/grotere_versie/1314_logo_pxl_bol.png", button));

            // Change the button text
            CompoundButtonText buttonText = button.GetComponent<CompoundButtonText>();
            buttonText.Text = name;
            buttonText.Size = 75;
            buttonText.OverrideSize = true;
            buttonText.Style = FontStyle.Bold;

            Texture2D urlLogo = new Texture2D(0,0);
            urlLogo = Resources.Load(name) as Texture2D;
            // Creating the Texture2D from the logo
            //Debug.Log(aspect_ratio);

            float aspect_ratio = CalculateAspectRatio(urlLogo.width, urlLogo.height);


            // Change the button icon to the appropriate logo
            CompoundButtonIcon buttonIcon = button.GetComponent<CompoundButtonIcon>();
            buttonIcon.OverrideIcon = true;
            buttonIcon.IconName = "Ready";
            buttonIcon.iconOverride = urlLogo;

            buttonIcon.IconMeshFilter.transform.localScale = new Vector3(2.5f, 2.5f / aspect_ratio, 1f);

            // Change button to appropriate scale -- Might wanna do this based on resolution
            // what if width < height

            // Initialize Receiver
            receiver = parent.GetComponent<InteractionReceiver>();

            // Add button to the receiver
            receiver.interactables.Add(button);

            // Add the button to the menu
            button.transform.SetParent(parent.transform);
            Debug.Log(parent.transform.childCount);
            Debug.Log(button.name);

            button.transform.localScale = new Vector3(currentScale.x * 3f, currentScale.x * 3f, 1f);
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

    IEnumerator loadImageFromUrl(string url, GameObject button)
    {
        urlLogo = new Texture2D(4, 4, TextureFormat.DXT1, false);
        using (WWW www = new WWW(url))
        {
            yield return www;
  
            www.LoadImageIntoTexture(urlLogo);
            CompoundButtonIcon icon = button.GetComponent<CompoundButtonIcon>();
            icon.iconOverride = urlLogo;
            icon.OverrideIcon = true;
            icon.IconName = "Ready";

            float aspect_ratio = CalculateAspectRatio(urlLogo.width, urlLogo.height);

            icon.IconMeshFilter.transform.localScale = new Vector3(2.5f, 2.5f / aspect_ratio, 1f);
        }
    }
}
