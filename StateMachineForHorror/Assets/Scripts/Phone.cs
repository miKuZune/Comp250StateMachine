using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour {

    bool phoneOut;

    public GameObject phoneOBJ;

    public GameObject[] apps;
    public GameObject[] textsDisplayAreas;
    public GameObject[] lightOBJs;
    public GameObject[] GPS;
    public GameObject TextsHeader;
    public Contacts[] contacts;

    GameObject[,] phone;
    int phoneArryLength;


    int currentScreen;
    int currentScroll;


    void GetPhoneOut()
    {
        phoneOBJ.SetActive(true);
        phoneOut = true;
    }

    void PutPhoneAway()
    {
        phoneOBJ.SetActive(false);
        phoneOut = false;
    }

    void SetupPhoneArray()
    {
        //Apps in column 0
        for (int j = 0; j < apps.Length; j++)
        {
            phone[0, j] = apps[j];
        }
        //Contacts in column 1
        for (int j = 0; j < textsDisplayAreas.Length; j++)
        {
            phone[1, j] = textsDisplayAreas[j];
        }
        //Light in colum 2
        for(int j = 0; j < lightOBJs.Length; j++)
        {
            phone[2, j] = lightOBJs[j];
        }
        //GPS in colum 3
        for(int j = 0; j < GPS.Length; j++)
        {
            phone[3, j] = GPS[j];
        }
    }

    void SetupContacts()
    {
        for(int i = 0; i < textsDisplayAreas.Length; i++)
        {
            phone[1, i].GetComponentInChildren<Text>().text = contacts[i].name;
        }
    }

    void ShowTexts(int contactID)
    {
        for(int i = 0; i < textsDisplayAreas.Length; i++)
        {
            phone[1, i].GetComponentInChildren<Text>().text = contacts[contactID].texts[i];
        }
    }

    void HideAll()
    {
        for(int i = 0; i < phoneArryLength; i++)
        {
            for(int j = 0; j < phoneArryLength; j++)
            {
                if(phone[i,j] != null)
                {
                    phone[i, j].SetActive(false);
                }
            }
        }
        TextsHeader.SetActive(false);
    }

    void ShowApps()
    {
        for(int i = 0; i < apps.Length; i++)
        {
            phone[0, i].SetActive(true);
        }
    }

    void ShowChosen(int currentScreen)
    {
        for(int i = 0; i < phoneArryLength; i++)
        {
            if(phone[currentScreen,i] != null)
            {
                phone[currentScreen, i].SetActive(true);
            }
        }
        if(currentScreen == 1)
        {
            TextsHeader.SetActive(true);
        }
    }

    void HighlightCurrent(int currScreen, int currScroll)
    {
        for(int i = 0; i < phoneArryLength; i++)
        {
            if(phone[currScreen,i] != null && phone[currScreen,i].GetComponent<Image>()!= null)
            {
                phone[currScreen, i].GetComponent<Image>().color = Color.cyan;
            }
        }
        if(phone[currScreen, currScroll] != null && phone[currScreen, currScroll].GetComponent<Image>() != null)
        {
            phone[currScreen, currScroll].GetComponent<Image>().color = Color.red;
        }
        
    }

    void InputListner()
    {
        //Handle the phone
        if(Input.GetKeyDown(KeyCode.E) && !phoneOut)
        {
            GetPhoneOut();
        }else if(Input.GetKeyDown(KeyCode.E) && phoneOut)
        {
            PutPhoneAway();
        }
        //Handle scrolling
        if(phoneOut)
        {
            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                currentScroll++;
                if(phone[currentScreen,currentScroll] == null)
                {
                    currentScroll--;
                }
                HighlightCurrent(currentScreen, currentScroll);
                
            }
            else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                currentScroll--;
                if(currentScroll < 0)
                {
                    currentScroll = 0;
                }
                HighlightCurrent(currentScreen, currentScroll);
                
            }

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                
                if(currentScreen == 0)
                {
                    HideAll();
                    currentScreen = currentScroll + 1;
                    ShowChosen(currentScreen);
                    currentScroll = 0;
                    HighlightCurrent(currentScreen, currentScroll);
                }
                else if (currentScreen == 1)
                {
                    ShowTexts(currentScroll);
                }
                
            }
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                HideAll();
                ShowApps();
                SetupContacts();
                currentScreen = 0;
                currentScroll = 0;
                HighlightCurrent(currentScreen, currentScroll);
            }
        }
    }

    // Use this for initialization
    void Start () {
        currentScreen = 0;
        currentScroll = 0;
        phoneArryLength = 20;
        phone = new GameObject[phoneArryLength, phoneArryLength];

        SetupPhoneArray();

        HideAll();
        ShowApps();
        SetupContacts();
        HighlightCurrent(currentScreen, currentScroll);
        PutPhoneAway();
    }
	
	// Update is called once per frame
	void Update () {
        InputListner();
	}
}
