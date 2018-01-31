using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Texts : MonoBehaviour {

    public int numOfConversations;
    int currentSelected;
    GameObject[] conversations;
    public PhoneUImanager PUIM;

    bool veiwingTexts;

    void ScrollThrough()
    {

        if(!veiwingTexts)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                GetConversations();
                if (currentSelected < (conversations.Length - 1))
                {
                    currentSelected++;
                    GetConversations();
                    SelectConversation(currentSelected, currentSelected - 1);
                    Debug.Log(currentSelected);
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                GetConversations();
                if (currentSelected > 0)
                {
                    currentSelected--;
                    GetConversations();
                    SelectConversation(currentSelected, currentSelected + 1);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) && !veiwingTexts)
        {
            PUIM.ShowTexts();
            PUIM.HideContacts();
            veiwingTexts = true;
        }
        if(Input.GetKeyDown(KeyCode.Mouse1) && veiwingTexts)
        {
            PUIM.ShowContacts();
            PUIM.HideTexts();
            veiwingTexts = false;
        }

    }

    void SelectConversation(int current, int old)
    {
        foreach(GameObject curr in conversations)
        {
            curr.GetComponent<Image>().color = Color.cyan;
        }
        conversations[current].GetComponent<Image>().color = Color.red;
    }

    void GetConversations()
    {
        for(int i = 0; i < conversations.Length; i++)
        {
            string find = "Contact (" + (i + 1) + ")";
            conversations[i] = GameObject.Find(find+"");
        }
    }

    void Awake()
    {
        conversations = new GameObject[numOfConversations];
        GetConversations();
        conversations[0].GetComponent<Image>().color = Color.red;
        veiwingTexts = false;
    }

	// Update is called once per frame
	void Update () {
        ScrollThrough();
	}
}
