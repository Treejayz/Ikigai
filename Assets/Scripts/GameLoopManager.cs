using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour {

    // The 3 quest objects which contain some text and dropdown fields for the tasks
    public GameObject[] questUI = new GameObject[3];

    // list of all the dropdowns
    List<Dropdown> playerSelects = new List<Dropdown>();

    [HideInInspector]
    public int numPlayers;

    // Used to see which players are selected for tasks
    Dictionary<string, bool> selectedPlayers;

    private void Awake()
    {
        foreach (GameObject quest in questUI)
        {
            Dropdown[] temp = quest.GetComponentsInChildren<Dropdown>();
            for (int i = 0; i < temp.Length; i++)
            {
                playerSelects.Add(temp[i]);
            }
        }
    }

    public void RollQuests()
    {
        // First get a list of quests
        List<Quest> quests = GetComponent<QuestsManager>().ChooseQuests(3, numPlayers);

        // Do something if we are out of quests
        if (quests == null)
        {
            print("Ran out of quests!");
        }
        /*
        foreach (Playerstats player in GetComponent<PlayersTracker>().players)
        {
            selectedPlayers.Add(player.name, false);
        }
        for (int i = quests.Count; i < questUI.Length; i++)
        {
            questUI[i].SetActive(false);
        }
        for (int i = 0; i < quests.Count; i++)
        {

        }
        */
    }

    public void UpdateLists()
    {

    }

}
