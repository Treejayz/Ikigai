﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour {

    //Main Manager of all things, contains most functions and calls them.

    public Dropdown numPlayersDropdown;
    public InputField[] names;

    [HideInInspector]
    public static int numPlayers;

    PlayersTracker playersTracker;

	// Use this for initialization
	void Awake () {
        playersTracker = GetComponent<PlayersTracker>();
	}

    private void Start()
    {
        UpdatePlayers();
    }

    // Updates the number of input fields based on how many players are selected
    public void UpdatePlayers()
    {
        numPlayers = int.Parse(numPlayersDropdown.captionText.text);
        for (int i = 0; i <  names.Length; i++)
        {
            if (i <  numPlayers)
            {
                names[i].gameObject.SetActive(true);
            } else
            {
                names[i].gameObject.SetActive(false);
            }
        }

    }

    // calls to generate a list of players, then calls the next functions.
    public void GeneratePlayers()
    {
        // First, get all the player names
        numPlayers = int.Parse(numPlayersDropdown.captionText.text);
        string[] playerNames = new string[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            playerNames[i] = names[i].text;
            for (int j = 0; j < i; j++)
            {
                if (string.Compare(playerNames[i], playerNames[j]) == 0)
                {
                    return;
                }
            }
        }
        // Generate those stats
        playersTracker.GenerateStats(numPlayers, playerNames);
        // Start the main game loop
        GetComponent<GameLoopManager>().numPlayers = numPlayers;
        GetComponent<GameLoopManager>().RollQuests();
        // turn off the player selection canvas and turn on the quest selection canvas
        GetComponent<CanvasManager>().SetQuest();
    }
}
