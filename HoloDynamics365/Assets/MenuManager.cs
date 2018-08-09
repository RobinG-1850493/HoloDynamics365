using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;
using HoloToolkit.Unity.Collections;
using HoloToolkit.Unity.Receivers;
using System.IO;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using HoloDynamics365.Models;
using Assets;
using HoloToolkit.UI.Keyboard;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    // Initiate variables 
    private GameObject parent;
    private InteractionReceiver receiver;
    private Texture2D urlLogo;
    private int counter = 0;

    // Use this for initialization
    public void Start()
    {

        // Assigning parent object
        parent = this.gameObject;
        Debug.Log(parent.name);

        PlayerPrefs.SetString("Username", "robin.goos@scapta.com");
        PlayerPrefs.SetString("Password", "Welcome@Scapta");
        PlayerPrefs.Save();

        GameObject.Find("Settings").SetActive(true );
        // Disable movement capabillities -- Enable using Voice Commands
        GameObject.Find("MenuObject").GetComponent<TapToPlace>().enabled = false;
        GameObject.Find("MenuObject").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("VideoPlayers").GetComponent<TapToPlace>().enabled = false;

        // enable interactions
        parent.GetComponent<InteractionReceiver>().enabled = true;

        // create product based menu
        CreateProductMenu().GetAwaiter();
    }

    // Called by Voice Command : Move Menu
    public void PlacementMenu()
    {
        PlacementOff();
        GameObject.Find("MenuObject").GetComponent<TapToPlace>().enabled = true;
        GameObject.Find("MenuObject").GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("BackButton").GetComponent<InteractionReceiver>().enabled = false;
        GameObject.Find("Menu").GetComponent<InteractionReceiver>().enabled = false;
        GameObject screen = GameObject.Find("YoutubePlayer");
        screen.transform.localScale = new Vector3(1f, 0.58f, 1);
        screen.GetComponent<TapToPlace>().enabled = true;
        screen.GetComponent<BoxCollider>().enabled = true;
    }

    // Called by Voice Command : Move Video
    public void PlacementVideo()
    {
        PlacementOff();
        GameObject.Find("VideoPlayers").GetComponent<TapToPlace>().enabled = true;
        GameObject.Find("VideoPlayers").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("YoutubePlayer").transform.localScale = new Vector3(1f, 0.58f, 0.01f);
    }

    // Called by Voice Command : Done Moving
    public void PlacementOff()
    {
        GameObject.Find("MenuObject").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("MenuObject").GetComponent<TapToPlace>().enabled = false;
        GameObject.Find("BackButton").GetComponent<InteractionReceiver>().enabled = true;
        GameObject.Find("Menu").GetComponent<InteractionReceiver>().enabled = true;
        GameObject.Find("VideoPlayers").GetComponent<TapToPlace>().enabled = false;
        GameObject.Find("VideoPlayers").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("YoutubePlayer").transform.localScale = new Vector3(0f, 0f, 0f);
        GameObject.Find("VideoPlayers").GetComponent<TapToPlace>().enabled = false;
        GameObject.Find("Settings").SetActive(false);
    }

    // Called by Voice Command : Reset Menu
    public void ResetPlacement()
    {
        GameObject.Find("MenuObject").transform.localPosition = new Vector3(0f, 0f, 2f);
        GameObject.Find("VideoPlayers").transform.localPosition = new Vector3(0f, 0f, 2f);
    }

    // Loads in an image from a url and converts it to a Texture2D
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

    // Calculates image aspect ratio
    private static float CalculateAspectRatio(float width, float height)
    {
        float aspect_ratio = width / height;
        return aspect_ratio;
    }

    // Creates the Product menu
    public async Task CreateProductMenu()
    {
        // Populate products list with web api get call
        List<Product> products = await DataController.getProducts();

        parent = gameObject;

        // Save position and scale and minize the menu untill loading is done
        //Vector3 currentLocation = GameObject.Find("MenuObject").transform.localPosition;
        transform.localScale = new Vector3(1f, 1f, 1f);
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(0f, 0f, 0f);

        // Setting up ObjectCollection on parent -- takes care of placement in relation to other gameObjects
        ObjectCollection buttonCollection = parent.GetComponent<ObjectCollection>();
        buttonCollection.CellWidth = 0.45f;
        buttonCollection.CellHeight = 0.45f;
        buttonCollection.SurfaceType = SurfaceTypeEnum.Plane;
        buttonCollection.Rows = 1; // Change this based on amount of products? !!

        // Tag the menu as a product menu
        gameObject.tag = "ProductMenu";

        // Size the collider based on the size of product list
        parent.GetComponent<BoxCollider>().size = new Vector3(buttonCollection.CellWidth * products.Count, buttonCollection.CellHeight - 0.2f, 0.1f);
        GameObject.Find("MenuObject").GetComponent<BoxCollider>().size = new Vector3(buttonCollection.CellWidth * products.Count, buttonCollection.CellHeight - 0.2f, 0.1f);

        // Iterate through the products list
        foreach (Product p in products)
        {
            Debug.Log(name);

            // Instantiate the prefab button
            GameObject button = Instantiate(Resources.Load("HolographicButton")) as GameObject;
            button.name = p.productId;
            Debug.Log(button.name);

            // Load the image from url and scale it
            StartCoroutine(loadImageFromUrl(p.productLogo, button));

            // Change the button text value and style
            CompoundButtonText buttonText = button.GetComponent<CompoundButtonText>();
            buttonText.Text = p.productNaam;
            buttonText.Size = 75;
            buttonText.OverrideSize = true;
            buttonText.Style = FontStyle.Bold;

            // Initialize Receiver
            receiver = parent.GetComponent<InteractionReceiver>();

            // Add button to the receiver
            receiver.interactables.Add(button);

            // Add the button to the menu
            button.transform.SetParent(parent.transform);
            Debug.Log(parent.transform.childCount);
            Debug.Log(p.productLogo);

            // Scale the button
            button.transform.localScale = new Vector3(currentScale.x * 3f, currentScale.x * 3f, 1f);

            counter++;
        }
        // Update the button placements
        buttonCollection.UpdateCollection();

        // Scale the menu
        //GameObject.Find("MenuObject").transform.localPosition = currentLocation;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    // Create the customer menu based on productId
    public async Task CreateCustomerMenu(string id)
    {
        // Populate accounts list using web api call
        List<Account> accounts = await DataController.getCustomers(id);

        parent = gameObject;

        // Save position and scale and minize the menu untill loading is done
        //Vector3 currentLocation = GameObject.Find("MenuObject").transform.localPosition;
        transform.localScale = new Vector3(1f, 1f, 1f);
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(0f, 0f, 0f);

        // Setting up ObjectCollection on parent -- takes care of placement in relation to other gameObjects
        ObjectCollection buttonCollection = parent.GetComponent<ObjectCollection>();
        buttonCollection.CellWidth = 0.45f;
        buttonCollection.CellHeight = 0.45f;
        buttonCollection.SurfaceType = SurfaceTypeEnum.Plane;
        buttonCollection.Rows = 1;

        // Tagging the menu as a customer menu
        gameObject.tag = "CustomerMenu";
        
        // Sizing the box collider based on amount of customers
        parent.GetComponent<BoxCollider>().size = new Vector3(buttonCollection.CellWidth * accounts.Count, buttonCollection.CellHeight - 0.2f, 0.1f);
        GameObject.Find("MenuObject").GetComponent<BoxCollider>().size = new Vector3(buttonCollection.CellWidth * accounts.Count, buttonCollection.CellHeight - 0.2f, 0.1f);

        // Iterate through the accounts list
        foreach (Account a in accounts)
        {
            Debug.Log(name);

            // Instantiate the prefab button
            GameObject button = Instantiate(Resources.Load("HolographicButton")) as GameObject;
            button.name = id + "/" + a.id;
            Debug.Log(button.name);

            // Load image from url -- This needs to be changed in CRM (No url field)
            //StartCoroutine(loadImageFromUrl(p.productLogo, button)); 

            // Change the button text
            CompoundButtonText buttonText = button.GetComponent<CompoundButtonText>();
            buttonText.Text = a.naam;
            buttonText.Size = 75;
            buttonText.OverrideSize = true;
            buttonText.Style = FontStyle.Bold;

            // Initialize Receiver
            receiver = parent.GetComponent<InteractionReceiver>();

            // Add button to the receiver
            receiver.interactables.Add(button);

            // Add the button to the menu
            button.transform.SetParent(parent.transform);
            Debug.Log(parent.transform.childCount);

            // Scale the button
            button.transform.localScale = new Vector3(currentScale.x * 3f, currentScale.x * 3f, 1f);
            counter++;
        }
        // Update button placements
        buttonCollection.UpdateCollection();

        // Scale the menu
        //GameObject.Find("MenuObject").transform.localPosition = currentLocation;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    // This destorys the currently showing menu to make place for a new one
    public void destroyCurrentMenu()
    {
        int i = 0;

        // Hide the menu
        transform.localScale = new Vector3(0f, 0f, 0f);

        // Get all the children of the menu (buttons)
        GameObject[] allChildren = new GameObject[transform.childCount];
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i++;
        }

        // Destroy each child
        foreach(GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
        Debug.Log(transform.childCount);
        transform.DetachChildren();
    }

    // Load image from an url and use it as the button icon
    IEnumerator loadImageFromUrl(string url, GameObject button)
    {
        urlLogo = new Texture2D(4, 4, TextureFormat.DXT1, false);
        using (WWW www = new WWW(url))
        {
            // Retrieve the image 
            yield return www;
            
            // Convert the image to a texture
            www.LoadImageIntoTexture(urlLogo);
            CompoundButtonIcon icon = button.GetComponent<CompoundButtonIcon>();
            icon.iconOverride = urlLogo;
            icon.OverrideIcon = true;
            icon.IconName = "Ready";

            float aspect_ratio = CalculateAspectRatio(urlLogo.width, urlLogo.height);

            // Scale the icon
            icon.IconMeshFilter.transform.localScale = new Vector3(2.5f, 2.5f / aspect_ratio, 1f);
        }
    }

    public void SaveUser()
    {
        string username = GameObject.Find("Username").GetComponentInChildren<Text>().text;
        string password = GameObject.Find("Password").GetComponentInChildren<Text>().text;

        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("Password", password);
        PlayerPrefs.Save();
    }

    public void showSettings()
    {
        GameObject.Find("Settings").SetActive(true);
    }
    public void closeSettings()
    {
        GameObject.Find("Settings").SetActive(false);
    }
}
