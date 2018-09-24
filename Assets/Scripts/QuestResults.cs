using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestResults : MonoBehaviour {
	public Quest quest;
	public Playerstats[] playerlist;
	public int[] ikigai;
	public bool unselected;
	// Use this for initialization
	void Start () {
		unselected = false;
	}
	public void addQuest(Quest Q) {
		quest = Q;
		playerlist = new Playerstats[Q.tasks.Length];
	}
	public void addPlayer (int task, Playerstats player){
		playerlist [task] = player;
		unselected = true;
	}
}
