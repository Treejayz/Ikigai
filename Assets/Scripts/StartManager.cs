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

    public void GeneratePlayers()
    {
        numPlayers = int.Parse(numPlayersDropdown.captionText.text);
        string[] playerNames = new string[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            playerNames[i] = names[i].text;
        }
        playersTracker.GenerateStats(numPlayers, playerNames);
        Text[] texts = statsCanvas.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.text = "";
        }
        for (int i = 0; i < playersTracker.players.Count; i++)
        {
            texts[i].text = playersTracker.players[i].display;
        }
        GetComponent<GameLoopManager>().numPlayers = numPlayers;
        GetComponent<GameLoopManager>().RollQuests();
        startCanvas.SetActive(false);
        questCanvas.SetActive(true);
    }

    public void ToggleStats()
    {
        statsCanvas.SetActive(!statsCanvas.activeSelf);
    }

}
