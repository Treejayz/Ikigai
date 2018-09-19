using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour {

    // The 3 quest objects which contain some text and dropdown fields for the tasks
    public GameObject[] questUI = new GameObject[3];
	public GameObject[] resultUI = new GameObject[3];

    public Button submit;

    // list of all the dropdowns
    List<Dropdown> playerSelects = new List<Dropdown>();

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

    public void RollQuests()
    {
		round += 1;

		// First get a list of quests
		if (round <= 3) {
			activeQuests = GetComponent<QuestsManager> ().ChooseQuests (1, numPlayers);
		} else if (round <= 5) {
        
			activeQuests = GetComponent<QuestsManager> ().ChooseQuests (2, numPlayers);
		} else {
			activeQuests = GetComponent<QuestsManager> ().ChooseQuests (3, numPlayers);
		}

        submit.interactable = false;

        print(activeQuests.Count);
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
                questUI[i].transform.GetChild(0).GetComponent<Text>().text = activeQuests[i].tasks.Length + " Heroes Wanted:";
                questUI[i].transform.GetChild(1).GetComponent<Text>().text = activeQuests[i].description + "\n\n<i>Reward: " + activeQuests[i].rewardDescription + "</i>";
                
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
		int questnum = 0;

		//makes sure all Text is cleared to start
		foreach (GameObject result in resultUI)
		{ 
			for (int tcount = 0; tcount < result.GetComponentsInChildren<Text>().Length; tcount ++)
			{
				print (result.name);
				result.GetComponentsInChildren<Text>()[tcount].text = "";
			}
		}
		//Sorted player list
		Playerstats[] playerlist = new Playerstats[numPlayers];
		foreach (Playerstats player in GetComponent<PlayersTracker>().players) {
			//places players in list based on order of tasks
			playerlist [selectedPlayers [player.name] - 1] = player;
		}
		//i is quest count
	 	int i = 0;
		int playercount = 0;
		foreach (Quest quest in activeQuests) {
			//i2 is task conut
			int i2 = 0;
			int taskSuccesses = 0;
			bool questSuccess = true;
			//check for if quest as a whole failed
			foreach (Task task in quest.tasks) {
				//player for the task
				Playerstats player = playerlist [playercount];
				int stat = 0;
				if (task.stat == Task.Stat.Fight) {
					stat = 0;
				}else if (task.stat == Task.Stat.Charm) {
					stat = 1;
				}else if (task.stat == Task.Stat.Agility) {
					stat = 2;
				}else if (task.stat == Task.Stat.Smarts) {
					stat = 3;
				}else if (task.stat == Task.Stat.Magic) {
					stat = 4;
				}
				string taskText = "";
				if (task.difficulty <= player.Skill [stat]) {
					//Successful task
					taskText = player.name + " has succeeded at their task.\n";
					taskSuccesses++;
				} else {
					taskText = player.name + " has failed their task.\n";
				}
				resultUI [i].GetComponentsInChildren<Text> () [i2 + 1].text = taskText; 
				i2++;
				playercount++;
			}
			if (taskSuccesses == 0 || taskSuccesses < quest.tasks.Length / 2 ) {
				questSuccess = false;
				resultUI [i].GetComponentsInChildren<Text> () [0].text = "Failed Quest"; 
			} else {
				resultUI [i].GetComponentsInChildren<Text> () [0].text = "Successful Quest"; 
			}
			playercount -= i2;
			i2 = 0;
			//calculated task ikigai and success for text purposes
			foreach (Task task in quest.tasks) {
				//player for the task
				Playerstats player = playerlist [playercount];
				int stat = 0;
				if (task.stat == Task.Stat.Fight) {
					stat = 0;
				}else if (task.stat == Task.Stat.Charm) {
					stat = 1;
				}else if (task.stat == Task.Stat.Agility) {
					stat = 2;
				}else if (task.stat == Task.Stat.Smarts) {
					stat = 3;
				}else if (task.stat == Task.Stat.Magic) {
					stat = 4;
				}
				string taskText = "";
				if (questSuccess) {
					//Successful quest
					int ikigai = calculateIkigai(player.Passion[stat], quest.money, player.Greed, quest.need, player.Need);
					taskText +=  player.name + " has recived "+ ikigai +" ikiai points for their work\n\n";
				} else {
					//Failed quest
					int ikigai = calculateIkigai(player.Passion[stat], -5, player.Greed, quest.need * -1, player.Need);
					taskText +=  player.name + " has recived "+ ikigai +" ikiai points for their work\n\n";
				}
				resultUI [i].GetComponentsInChildren<Text> () [i2 + 1].text += taskText; 
				i2++;
				playercount++;
			}
			i++;
		}
	}

	public int calculateIkigai(int passion, int gold, float greed, int need, float care)
	{
		return (passion  + (int)(need * care) + (int)(gold * greed));
	}
}
