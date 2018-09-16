using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour {

    public GameObject[] questUI = new GameObject[3];

    List<Dropdown> playerSelects = new List<Dropdown>();

    [HideInInspector]
    public int numPlayers;

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
        List<Quest> quests = GetComponent<QuestsManager>().ChooseQuests(3, numPlayers);
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
        
    }

    public void UpdateLists()
    {

    }

}
