using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public GameObject startCanvas;
    public GameObject questCanvas;
    public GameObject statsCanvas;
    public GameObject resultsCanvas;
    public GameObject jobsCanvas;

    string[] statTypes = { "strength", "charisma", "agility", "smarts", "magic" };

    public Sprite[] moneySprites;
    public Sprite[] statSprites;
    public GameObject[] taskUIs;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleStats();
        }
    }

    public void SetQuest()
    {
        startCanvas.SetActive(false);
        jobsCanvas.SetActive(false);
        questCanvas.SetActive(true);
    }
    public void SetJob()
    {
        startCanvas.SetActive(false);
        jobsCanvas.SetActive(true);
        questCanvas.SetActive(false);
    }


    public void ToggleStats()
    {
        statsCanvas.SetActive(!statsCanvas.activeSelf);
    }

    public void ToggleResults()
    {
        resultsCanvas.SetActive(!resultsCanvas.activeSelf);
    }
	public void displayNextquest()
	{
		//if end of quests turn off an "reset" results
		if(GetComponent<GameLoopManager> ().currentQuestResult >= GetComponent<GameLoopManager> ().getActiveQuestNumber()){
			ToggleResults ();
			GetComponent<GameLoopManager> ().RollJobs();
			GetComponent<GameLoopManager> ().currentQuestResult = 0; 
			return;
		}
		QuestResults result = GetComponent<GameLoopManager> ().resultsUI [GetComponent<GameLoopManager> ().currentQuestResult];
		//if quest was not given a player move on to next quest
		while (!result.unselected) {
			GetComponent<GameLoopManager> ().currentQuestResult++;
			if(GetComponent<GameLoopManager> ().currentQuestResult >= GetComponent<GameLoopManager> ().getActiveQuestNumber()){
				ToggleResults ();
				GetComponent<GameLoopManager> ().RollJobs();
				GetComponent<GameLoopManager> ().currentQuestResult = 0; 
				return;
			}
			result = GetComponent<GameLoopManager> ().resultsUI [GetComponent<GameLoopManager> ().currentQuestResult];
		}
		Quest quest = result.quest;
		//set quest name, i think this needs a fix sorry
		resultsCanvas.transform.GetChild(2).GetComponent<Text> ().text = result.quest.title;

		int taskWins = 0;
		bool questSuccess = true;
		//figuring out task results for sake of quest success/failure and also setting task success + name
		for (int taskNum = 0; taskNum < 4; taskNum++) {
            GameObject taskUI = taskUIs[taskNum];
			Text[] texts = taskUI.GetComponentsInChildren<Text> ();
			//if task doesn't exist skipp and set blank
			if (taskNum >= quest.tasks.Length) {
				texts [0].text = "";
				texts [2].text = "";
				//set image to blank
				continue;
			}
			Playerstats player = result.playerlist [taskNum];
			Task task = quest.tasks[taskNum];
			texts [0].text = player.name;
			int stat = 0;
			if (task.stat == Task.Stat.Fight) {
				stat = 0;
				//also set image
			}else if (task.stat == Task.Stat.Charm) {
				stat = 1;
				//also set image
			}else if (task.stat == Task.Stat.Agility) {
				stat = 2;
				//also set image
			}else if (task.stat == Task.Stat.Smarts) {
				stat = 3;
				//also set image
			}else if (task.stat == Task.Stat.Magic) {
				stat = 4;
				//also set image
			}
			//sucesses at task text
			if (task.difficulty <= player.Skill [stat]) {
				//Successful task
				texts [2].text = player.name + " has succeeded at their " + statTypes[stat] + " task.\n";
				taskWins++;
			} else {
				texts [2].text = player.name + " has failed their " + statTypes[stat] + " task.\n";
			}
		}
		//sets if the quest failed or succeded
		if (taskWins == 0 || taskWins * 2 < quest.tasks.Length) {
			questSuccess = false;
			GameObject.Find ("Fail/Success").GetComponent<Text> ().text = "Failed Quest"; 
		} else {
			GameObject.Find ("Fail/Success").GetComponent<Text> ().text = "Successful Quest"; 
		}

        // Set the quest result text
        if (questSuccess) {
            resultsCanvas.transform.GetChild(4).GetComponent<Text>().text = quest.succeed;
        }
        else
        {
            resultsCanvas.transform.GetChild(4).GetComponent<Text>().text = quest.fail;
        }

        // set image for money based on success/failure/ammount of money
        string output = "";
        if (!questSuccess)
        {
            output += "You got no money.\n";
            GameObject.Find("MoneyAmount").GetComponent<Image>().sprite = moneySprites[0];
        } else
        {
            if (quest.money == -5)
            {
                output += "You got no money.\n";
                GameObject.Find("MoneyAmount").GetComponent<Image>().sprite = moneySprites[0];
            }
            else if (quest.money == 5)
            {
                output += "You got a lot of money.\n";
                GameObject.Find("MoneyAmount").GetComponent<Image>().sprite = moneySprites[4];
            }
            else if (quest.money <= -2)
            {
                output += "You got very little money.\n";
                GameObject.Find("MoneyAmount").GetComponent<Image>().sprite = moneySprites[1];
            }
            else if (quest.money <= 1)
            {
                output += "You got an average amount of money.\n";
                GameObject.Find("MoneyAmount").GetComponent<Image>().sprite = moneySprites[2];
            }
            else
            {
                output += "You got a good amount of money.\n";
                GameObject.Find("MoneyAmount").GetComponent<Image>().sprite = moneySprites[3];
            }
        }

        //set world stat text based on success/failure/ammount of world change
        if (questSuccess)
        {
            if (quest.need == -5)
            {
                output += "The world is worse now.";
            }
            else if (quest.need == 5)
            {
                output += "You did a great deed!";
            }
            else if (quest.need <= -2)
            {
                output += "You did a pretty bad thing.";
            }
            else if (quest.need <= 1)
            {
                output += "Life goes on as usual.";
            }
            else
            {
                output += "You helped some people today!";
            }
        }
        else
        {
            if (quest.need == -5)
            {
                output += "The world is glad you failed.";
            }
            else if (quest.need == 5)
            {
                output += "You failed, when the world desperately needed you.";
            }
            else if (quest.need <= -2)
            {
                output += "It's probably a good thing you failed...";
            }
            else if (quest.need <= 1)
            {
                output += "Life goes on as usual anyway.";
            }
            else
            {
                output += "You missed an opportunity to help others.";
            }
        }

        GameObject.Find("Money&Need").GetComponent<Text>().text = output;

		//going through tasks to set ikigai
		for (int taskNum = 0; taskNum < 4; taskNum++) {
            GameObject taskUI = taskUIs[taskNum];
            Text[] texts = taskUI.GetComponentsInChildren<Text> ();
			if (taskNum >= quest.tasks.Length) {
				texts [1].text = "";
                //set image to blank
                taskUI.SetActive(false);
				continue;
			}
            taskUI.SetActive(true);

			Playerstats player = result.playerlist [taskNum];
			Task task = quest.tasks[taskNum];
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

            // set stat picture
            taskUI.GetComponentInChildren<Image>().sprite = statSprites[stat];

            //setting ikigai number
            int ikigai = 0;
			if (questSuccess) {
				//Successful quest
				ikigai = GetComponent<GameLoopManager> ().calculateIkigai(player.Passion[stat], quest.money, player.Greed, quest.need, player.Need);
			} else {
				//Failed quest
				ikigai = GetComponent<GameLoopManager> ().calculateIkigai(player.Passion[stat], -5, player.Greed, quest.need * -1, player.Need);
			}
            texts[1].text = ikigai.ToString();
            if (ikigai > 0)
            {
                texts[1].color = Color.green;
            } else if (ikigai < 0)
            {
                texts[1].color = Color.red;
            } else
            {
                texts[1].color = Color.white;
            }
        }
		GetComponent<GameLoopManager> ().currentQuestResult++;
	}
}
