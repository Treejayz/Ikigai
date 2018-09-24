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
		GameObject.Find ("QuestName").GetComponent<Text> ().text = result.quest.name;

		int taskWins = 0;
		bool questSuccess = true;
		//figuring out task results for sake of quest success/failure and also setting task success + name
		for (int taskNum = 0; taskNum < 4; taskNum++) {
			print (taskNum);
			string taskname = "Task";
			taskname += taskNum.ToString();
			GameObject taskUI = GameObject.Find (taskname);
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
				texts [2].text = player.name + " has succeeded at their task.\n";
				taskWins++;
			} else {
				texts [2].text = player.name + " has failed their task.\n";
			}
		}
		//sets if the quest failed or succeded
		if (taskWins == 0 || taskWins < quest.tasks.Length / 2 ) {
			questSuccess = false;
			GameObject.Find ("Fail/Success").GetComponent<Text> ().text = "Failed Quest"; 
		} else {
			GameObject.Find ("Fail/Success").GetComponent<Text> ().text = "Successful Quest"; 
		}
		// set image for money based on success/failure/ammount of money
		//set world stat text based on success/failure/ammount of world change

		//going through tasks to set ikigai
		for (int taskNum = 0; taskNum < 4; taskNum++) {
			string taskname = "Task";
			taskname += taskNum.ToString();
			GameObject taskUI = GameObject.Find (taskname);
			Text[] texts = taskUI.GetComponentsInChildren<Text> ();
			if (taskNum >= quest.tasks.Length) {
				texts [1].text = "";
				//set image to blank
				continue;
			}

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
			//setting ikigai number
			if (questSuccess) {
				//Successful quest
				texts [1].text = GetComponent<GameLoopManager> ().calculateIkigai(player.Passion[stat], quest.money, player.Greed, quest.need, player.Need).ToString();
			} else {
				//Failed quest
				texts [1].text = GetComponent<GameLoopManager> ().calculateIkigai(player.Passion[stat], -5, player.Greed, quest.need * -1, player.Need).ToString();
			}
		}
		GetComponent<GameLoopManager> ().currentQuestResult++;
	}
}
