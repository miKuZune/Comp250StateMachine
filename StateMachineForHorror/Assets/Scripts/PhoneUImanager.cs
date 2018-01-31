using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PhoneUImanager : MonoBehaviour {

    public GameObject textsHeader;
    public GameObject contactsHeader;
    public Contacts[] contacts;
    public GameObject[] shownContactList;
    public GameObject[] shownTextList;

    void SetContacts(int startPoint, int endPoint)
    {
        for (int i = startPoint; i < endPoint; i++)
        {
            shownContactList[i].GetComponentInChildren<Text>().text = contacts[i].name;
        }
        contactsHeader.SetActive(true);
    }

    public void ShowContacts()
    {
        foreach (GameObject curr in shownContactList)
        {
            curr.SetActive(true);
        }
    }

    public void HideContacts()
    {
        foreach(GameObject curr in shownContactList)
        {
            curr.SetActive(false);
        }
    }

    public void ShowTexts()
    {
        SetContacts(0, shownContactList.Length);
        foreach (GameObject curr in shownTextList)
        {
            curr.SetActive(true);
        }
        textsHeader.SetActive(true);
    }

    public void HideTexts()
    {
        foreach (GameObject curr in shownTextList)
        {
            curr.SetActive(false);
        }
        textsHeader.SetActive(false);
    }

    public void BackToTexts()
    {
        foreach (GameObject curr in shownTextList)
        {
            curr.SetActive(false);
        }
        textsHeader.SetActive(false);
        foreach (GameObject curr in shownContactList)
        {
            curr.SetActive(true);
        }
    }

    public void SetTexts(int contactID,int startPoint, int endPoint)
    {
        for(int i = startPoint; i < endPoint; i++)
        {
            shownTextList[i].GetComponent<Text>().text = contacts[contactID].texts[i];
        }
    }

	// Use this for initialization
	void Start () {
        SetContacts(0, shownContactList.Length);
        foreach(GameObject curr in shownTextList)
        {
            curr.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
