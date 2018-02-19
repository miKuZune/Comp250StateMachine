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

	//Gets the text message to send based on which zone the player is in and which texts in that zone have been sent.
    string ChooseTextFromZone(int zoneID)
    {
        //Variables
		string textToSend = "";
        int textID = 0;
        int minusOtherZones = 0;

		//Go through the zones until the zone you are in and count the texts to ignore.
        for (int i = 0; i <= zoneID - 1; i++)
        { 
            textID += totalTextsPerZone[i];
            minusOtherZones += totalTextsPerZone[i];
        }

		//Add on the texts that have already been sent from this zone.
        textID += sentTextsPerZone[zoneID];

		//Check if the AI has sent all the texts from that zone.
		//Re send the previous text if true
        if (textID - minusOtherZones >= totalTextsPerZone[zoneID])
        {
            textID--;
            textToSend = texts[textID];
        }
        else
        {
            textToSend = texts[textID];
            sentTextsPerZone[zoneID]++;
        }
        
        

        return textToSend;
    }

	//Set the text on the phone to be the text that the AI sends.
    void SendText(string textToSend)
    {
        playerPhoneTextScreen.text = textToSend;
    }

	//Counts how long the player has been in a zone and checks if it's time to send a new text.
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
		//Get the player position
        Vector3 currPlayerPos = player.transform.position;
        
		//Check if the player is in one of the zones.
        for(int i = 0; i < zonePoint1.Length; i++)
        {
            if(currPlayerPos.x <= zonePoint1[i].position.x && currPlayerPos.z <= zonePoint1[i].position.z)
            {
                if(currPlayerPos.x >= zonePoint2[i].position.x && currPlayerPos.z >= zonePoint2[i].position.z)
                {
					//Sets the current zone and checks for when to send a new text message.
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
