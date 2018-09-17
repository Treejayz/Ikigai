using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour {

    //Main Manager of all things, contains most functions and calls them.

    public Dropdown numPlayersDropdown;
    public InputField[] names;

    [HideInInspector]
    public static int numPlayers;

    public GameObject startCanvas;
    public GameObject questCanvas;
    public GameObject statsCanvas;

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
        print(numPlayers);
        string[] playerNames = new string[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            playerNames[i] = names[i].text;
        }
        // Generate those stats
        playersTracker.GenerateStats(numPlayers, playerNames);
        // set the text fields in the stat canvas to see player stats (for now)
        Text[] texts = statsCanvas.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.text = "";
        }
        for (int i = 0; i < playersTracker.players.Count; i++)
        {
            texts[i].text = playersTracker.players[i].display;
        }
        // Start the main game loop
        GetComponent<GameLoopManager>().numPlayers = numPlayers;
        GetComponent<GameLoopManager>().RollQuests();
        // turn off the player selection canvas and turn on the quest selection canvas
        startCanvas.SetActive(false);
        questCanvas.SetActive(true);
    }

    // yep
    public void ToggleStats()
    {
        statsCanvas.SetActive(!statsCanvas.activeSelf);
    }

}
