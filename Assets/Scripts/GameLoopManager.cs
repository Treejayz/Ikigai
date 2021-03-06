﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour {

    // The 3 quest objects which contain some text and dropdown fields for the tasks
    public GameObject[] questUI = new GameObject[3];
    public GameObject[] jobUI = new GameObject[3];
	public QuestResults[] resultsUI = new QuestResults[3];
	public int currentQuestResult;

    public Button submit;

    public Text turnOrderText;

    // list of all the dropdowns
    List<Dropdown> playerSelects = new List<Dropdown>();

    List<string> turnOrder = new List<string>();
    int orderIndex = 0;

    [HideInInspector]
    public int numPlayers;

	int round = 0;

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

    public void RollJobs()
    {
        Dictionary<string, string> jobs = GetComponent<JobManager>().ChooseJobs();
        List<string> jobTitles = new List<string>(jobs.Keys);
        GetComponent<CanvasManager>().SetJob();

        for (int i = 0; i < 3; i++)
        {
            jobUI[i].transform.GetChild(0).GetComponent<Text>().text = jobTitles[i];
            jobUI[i].transform.GetChild(1).GetComponent<Text>().text = jobs[jobTitles[i]];
            jobUI[i].transform.GetChild(2).GetComponent<Dropdown>().ClearOptions();
            jobUI[i].transform.GetChild(2).GetComponent<Dropdown>().AddOptions(playerOptions);
            jobUI[i].transform.GetChild(2).GetComponent<Dropdown>().value = 0;
        }
    }

    public void SubmitJobs()
    {

        string[] names = new string[3];
        for (int i = 0; i < 3; i++)
        {
            if (jobUI[i].transform.GetChild(2).GetComponent<Dropdown>().value != 0)
            {
                string name = jobUI[i].transform.GetChild(2).GetComponent<Dropdown>().captionText.text;
                if (name != "Select Player" && playerOptions.Contains(name))
                {
                    names[i] = name;
                    for (int j = 0; j < i; j++)
                    {
                        if (string.Compare(names[i], names[j]) == 0)
                        {
                            return;
                        }
                    }
                } else
                {
                    names[i] = name;
                }
            }
        }

        bool jobsSelected = false;

        for (int i = 0; i < 3; i++)
        {
            if (jobUI[i].transform.GetChild(2).GetComponent<Dropdown>().value != 0)
            {
                string name = jobUI[i].transform.GetChild(2).GetComponent<Dropdown>().captionText.text;
                if (name != "Select Player" && playerOptions.Contains(name))
                {
                    GetComponent<PlayersTracker>().RetirePlayer(name, jobUI[i].transform.GetChild(0).GetComponent<Text>().text);
                    numPlayers -= 1;
                    if (orderIndex > turnOrder.IndexOf(name))
                    {
                        orderIndex -= 1;
                    }
                    turnOrder.Remove(name);
                    jobsSelected = true;
                }
            }
        }

        if (jobsSelected) { GetComponent<CanvasManager>().ToggleStats(); }

        if (numPlayers == 0)
        {
            GetComponent<PlayersTracker>().Done();
        }
        else
        {
            GetComponent<CanvasManager>().SetQuest();
            RollQuests();
        }
    }

    public void RollQuests()
    {
        if (round == 0)
        {
            List<string> temp = new List<string>();
            foreach (Playerstats player in GetComponent<PlayersTracker>().players)
            {
                temp.Add(player.name);
            }

            int n = temp.Count;
            while(n > 0)
            {
                int x = Random.Range(0, n);
                turnOrder.Add(temp[x]);
                temp.RemoveAt(x);
                n -= 1;
            }
            turnOrderText.text = turnOrder[0];
            orderIndex += 1;
        }
        else
        {
            if (orderIndex >= turnOrder.Count)
            {
                orderIndex = 0;
            }
            turnOrderText.text = turnOrder[orderIndex];
            orderIndex += 1;
        }
        if (numPlayers > 1)
        {
            GetComponent<CanvasManager>().ToggleLeader();
        }
		round += 1;

		// First get a list of quests
		if (round <= 2) {
			activeQuests = GetComponent<QuestsManager> ().ChooseQuests (1, numPlayers);
		} else if (round <= 4) {
        
			activeQuests = GetComponent<QuestsManager> ().ChooseQuests (2, numPlayers);
		} else {
			activeQuests = GetComponent<QuestsManager> ().ChooseQuests (3, numPlayers);
		}

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
                selectedPlayers.Add(player.name, 0);
                playerOptions.Add(player.name);
            }

            playerSelects.Clear();
            for (int i = 0; i < activeQuests.Count; i++)
            {
                questUI[i].SetActive(true);
                questUI[i].transform.GetChild(0).GetComponent<Text>().text = activeQuests[i].title;
                /* Old thing, might be reused
                if (activeQuests[i].tasks.Length == 1)
                {
                    questUI[i].transform.GetChild(0).GetComponent<Text>().text = "Hero Wanted:";
                }
                else
                {
                    questUI[i].transform.GetChild(0).GetComponent<Text>().text = activeQuests[i].tasks.Length + " Heroes Wanted:";
                }
                */
                questUI[i].transform.GetChild(1).GetComponent<Text>().text = activeQuests[i].description + "\n<i>Reward: " + activeQuests[i].rewardDescription + "</i>";
                
                for (int j = activeQuests[i].tasks.Length; j < 4; j++) {
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
                    d.value = 0;
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
        for (int i = 0; i < playerSelects.Count; i++) {
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
	public void calculateResults()
	{
		currentQuestResult = 0;
		int questnum = 0;

		//Sorted player list
		Playerstats[] playerlist = new Playerstats[Mathf.Max(3, GetComponent<PlayersTracker>().players.Count)];
		foreach (Playerstats player in GetComponent<PlayersTracker>().players) {
			//places players in list based on order of tasks
			playerlist [selectedPlayers [player.name] - 1] = player;
		}

		for (int i = 0; i < 3; i++) {
			resultsUI [i] = new QuestResults ();
		}
		int tasknum = 0;
		//assigns each player to their task
		for (int i = 0; i < activeQuests.Count; i++) {
			//if quest doen't have players sullected cause less then 3 people skip
			if(playerlist[tasknum] == null){
				tasknum++;
				continue;
			}
			resultsUI [i] = new QuestResults(); 
			resultsUI[i].addQuest(activeQuests [i]);
			int i2 = 0;
			//adding to task per quest
			foreach(Task task in activeQuests[i].tasks){
				resultsUI [i].addPlayer (i2, playerlist [tasknum]);
				i2++;
				tasknum++;
			}
		}
        GetComponent<CanvasManager>().displayNextquest();
	}

	public int calculateIkigai(int passion, int gold, float greed, int need, float care)
	{
		return (passion  + (int)(need * care) + (int)(gold * greed));
	}
	public int getActiveQuestNumber(){
		return activeQuests.Count;
	}
}
