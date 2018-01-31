using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //Check the current state the user is in
    bool phoneOut;
    bool inApp;
    //Store the phone GameOBJ
    public GameObject phone;
    //Store the list of software(UI s to enable and disable) and a list of apps(ways for the player to choose which software to go to)
    GameObject[] software;
    GameObject[] apps;
    //Hold the currently hovered app.
    int currSelectedApp;
    //The gameobject all the apps are held in, for disabling when the player is in a software.
    GameObject AppsHolder;
    //Hold the base contact prefab
    public GameObject ContactBase;
    public Transform texts;
    //Contacts
    public int NumOfContacts;
    public Contacts[] listOfContacts;
    public int[] numOfTexts;

    void PutPhoneAway()
    {
        
        phoneOut = false;
        currSelectedApp = 0;
        inApp = false;
        AppsHolder.SetActive(true);
        foreach(GameObject curr in software)
        {
            curr.SetActive(true);
        }
        phone.SetActive(false);
    }

    void GetPhoneOut()
    {
        phone.SetActive(true);
        phoneOut = true;
    }

    void InputListener()
    {
        //Gets the phone out or puts it away
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (phoneOut)
            {
                PutPhoneAway();
            }
            else
            {
                GetPhoneOut();
                FindSoftware();
                FindApps();
                currSelectedApp = 0;
                SelectApp(currSelectedApp, 1);
            }
        }

        //Phone activated controls
        if (phoneOut)
        {
            //Select app
            if (Input.GetAxis("Mouse ScrollWheel") < 0f && !inApp)
            {
                if(currSelectedApp < (apps.Length - 1))
                {
                    currSelectedApp++;
                    SelectApp(currSelectedApp, currSelectedApp - 1);
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f && !inApp)
            {
                if(currSelectedApp > 0)
                {
                    currSelectedApp--;
                    SelectApp(currSelectedApp, currSelectedApp + 1);
                }
            }
            //Enter app
            if(!inApp && Input.GetKeyDown(KeyCode.Mouse0))
            {
                AppsHolder.SetActive(false);
                software[currSelectedApp].SetActive(true);
                inApp = true;
                
            }
            //Exit app
            else if(inApp && Input.GetKeyDown(KeyCode.Mouse1))
            {
                AppsHolder.SetActive(true);
                software[currSelectedApp].SetActive(false);
                inApp = false;
                SelectApp(0, currSelectedApp);
                currSelectedApp = 0;
                
            }
        }
    }

    void FindApps()
    {
        AppsHolder = GameObject.Find("Apps");
        apps = GameObject.FindGameObjectsWithTag("App");
    }

    void FindSoftware()
    {

        software = GameObject.FindGameObjectsWithTag("Software");
        foreach(GameObject curr in software)
        {
            curr.SetActive(false);
        }
    }

    void SelectApp(int appToSelect, int previousApp)
    {
        apps[previousApp].GetComponent<Image>().color = Color.blue;
        apps[appToSelect].GetComponent<Image>().color = Color.red;
    }

    // Use this for initialization
    void Start () {
        phoneOut = false;
        inApp = false;
        phone.SetActive(false);
        currSelectedApp = 0;
	}
	
	// Update is called once per frame
	void Update () {
        InputListener();
	}
}
