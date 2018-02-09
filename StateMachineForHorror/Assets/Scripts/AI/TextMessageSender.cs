using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMessageSender : MonoBehaviour {

    public GameObject player;

    public Text playerPhoneTextScreen;

    public Transform[] zonePoint1;
    public Transform[] zonePoint2;

    int[] sentTextsPerZone;
    public int[] totalTextsPerZone;

    public string[] texts;

    float timeInZone;
    float timeTillNextText;

    int currZoneID;

    // Use this for initialization
    void Start ()
    {
        timeTillNextText = Random.Range(3, 12.5f);

        sentTextsPerZone = new int[zonePoint1.Length];
        

	}

    string ChooseTextFromZone(int zoneID)
    {
        string textToSend = "";
        int textID = 0;
        int minusOtherZones = 0;
        for (int i = 0; i <= zoneID - 1; i++)
        { 
            textID += totalTextsPerZone[i];
            minusOtherZones = totalTextsPerZone[i];
        }

        textID += sentTextsPerZone[zoneID];

        if (textID - minusOtherZones >= totalTextsPerZone[zoneID])
        {
            textID--;
        }
        else
        {
            sentTextsPerZone[zoneID]++;
        }
        Debug.Log(textID);
        textToSend = texts[textID];

        return textToSend;
    }
	
    void SendText(string textToSend)
    {
        playerPhoneTextScreen.text = textToSend;
    }

    void CountDownText()
    {
        if(timeInZone >= timeTillNextText)
        {
            SendText(ChooseTextFromZone(currZoneID));
            timeInZone = 0;
        }

        timeInZone += Time.deltaTime;
    }

	// Update is called once per frame
	void Update ()
    {
        Vector3 currPlayerPos = player.transform.position;
        
        for(int i = 0; i < zonePoint1.Length; i++)
        {
            if(currPlayerPos.x <= zonePoint1[i].position.x && currPlayerPos.z <= zonePoint1[i].position.z)
            {
                if(currPlayerPos.x >= zonePoint2[i].position.x && currPlayerPos.z >= zonePoint2[i].position.z)
                {
                    currZoneID = i;
                    
                    CountDownText();
                }
            }
            else
            {
                currZoneID = 0;
            }
        }
        
	}
}
