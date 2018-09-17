using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour {

    // The 3 quest objects which contain some text and dropdown fields for the tasks
    public GameObject[] questUI = new GameObject[3];

    public Button submit;

    // list of all the dropdowns
    List<Dropdown> playerSelects = new List<Dropdown>();

    [HideInInspector]
    public int numPlayers;

    // Used to see which players are selected for tasks
    Dictionary<string, int> selectedPlayers = new Dictionary<string, int>();
    List<string> playerOptions = new List<string>();

    List<Quest> activeQuests;

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
        activeQuests = GetComponent<QuestsManager>().ChooseQuests(3, numPlayers);

        submit.interactable = false;

        // Do something if we are out of quests
        if (activeQuests == null)
        {
            print("Ran out of quests!");
        } 
        else 
        {
            for (int i = activeQuests.Count; i < 3; i++) {
                questUI[i].SetActive(false);
            }

            selectedPlayers.Clear();
            playerOptions.Clear();
            playerOptions.Add("Select Player");
            foreach (Playerstats player in GetComponent<PlayersTracker>().players)
            {
                print(player.name);
                selectedPlayers.Add(player.name, 0);
                playerOptions.Add(player.name);
            }

            playerSelects.Clear();
            for (int i = 0; i < activeQuests.Count; i++)
            {
                questUI[i].transform.GetChild(0).GetComponent<Text>().text = activeQuests[i].tasks.Length + " Heroes Wanted:";
                questUI[i].transform.GetChild(1).GetComponent<Text>().text = activeQuests[i].description + "\n\n" + activeQuests[i].rewardDescription;
                
                for (int j = activeQuests[i].tasks.Length; j < 5; j++) {
                    questUI[i].transform.GetChild(j + 2).gameObject.SetActive(false);
                }
                for (int j = 0; j < activeQuests[i].tasks.Length; j++) {
                    GameObject task = questUI[i].transform.GetChild(j + 2).gameObject;
                    task.SetActive(true);
                    task.transform.GetChild(0).gameObject.GetComponent<Text>().text = activeQuests[i].tasks[j].description;
                    Dropdown d = task.GetComponentInChildren<Dropdown>();
                    playerSelects.Add(d);
                    d.ClearOptions();
                    d.AddOptions(playerOptions);
                }
            } 
        }
    }

    

    public void UpdateLists()
    {
        List<string> keyList = new List<string>(selectedPlayers.Keys);
        foreach (string key in keyList)
        {
            selectedPlayers[key] = 0;
        }
        for (int i = 0; i < numPlayers; i++) {
            if (playerSelects[i].captionText.text != "Select Player" && playerOptions.Contains(playerSelects[i].captionText.text)) {
                selectedPlayers[playerSelects[i].captionText.text] = i + 1;
            }
        }
        bool allSelected = true;
        foreach (string key1 in keyList)
        {
            if (selectedPlayers[key1] == 0) {

                allSelected = false;
                break;
            } else {
                foreach (string key2 in keyList)
                {
                    if (string.Compare(key1, key2) != 0 && selectedPlayers[key1] == selectedPlayers[key2]) {
                        allSelected = false;
                        break;
                    }
                }
            }
        }

        submit.interactable = allSelected;

        // Below is for removing the option from other lists, disabled for now
        /*
        for (int i = 0; i < numPlayers; i++) {
            string value = playerSelects[i].captionText.text;
            List<string> temp = new List<string>(playerOptions);
            foreach (string key in selectedPlayers.Keys)
            {
                if (selectedPlayers[key] != 0 && string.Compare(playerSelects[i].captionText.text, key) != 0) {
                    temp.Remove(key);
                }
            }
            playerSelects[i].ClearOptions();
            playerSelects[i].AddOptions(temp);
        }
        */
    }

}
