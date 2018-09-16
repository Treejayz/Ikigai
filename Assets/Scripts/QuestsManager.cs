using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsManager : MonoBehaviour {

    List<Quest>[] easyQuests = { new List<Quest>(), new List<Quest>(), new List<Quest>(), new List<Quest>(), new List<Quest>() };
    List<Quest>[] mediumQuests = { new List<Quest>(), new List<Quest>(), new List<Quest>(), new List<Quest>(), new List<Quest>() };
    List<Quest>[] hardQuests = { new List<Quest>(), new List<Quest>(), new List<Quest>(), new List<Quest>(), new List<Quest>() };

    // Use this for initialization
    void Awake () {
        Object[] allQuests = Resources.LoadAll("Quests", typeof(Quest));
        foreach(Object q in allQuests)
        {
            Quest quest = (Quest)q;
            // Sort by difficulty overall
            if (quest.overallDifficulty == 1)
            {
                // sort by number of tasks
                int numTasks = quest.tasks.Length;
                easyQuests[numTasks-1].Add(quest);

            }
            else if (quest.overallDifficulty == 2)
            {
                // sort by number of tasks
                int numTasks = quest.tasks.Length;
                mediumQuests[numTasks - 1].Add(quest);
            }
            else
            {
                // sort by number of tasks
                int numTasks = quest.tasks.Length;
                hardQuests[numTasks - 1].Add(quest);
            }
        }
    }
     
    public List<Quest> ChooseQuests(int maxDifficulty, int numPlayers)
    {
        int numTasks = 0;

        List<Quest> quests = new List<Quest>();

        while (numTasks < numPlayers)
        {
            int difficulty = Random.Range(0, maxDifficulty) + 1;
            int questTasks;
            if (quests.Count == 2)
            {
                questTasks = numPlayers - numTasks;
            }
            else
            {
                questTasks = Random.Range(1, Mathf.Clamp((numPlayers - numTasks + 1), 0, 5));
            }

            if (difficulty == 1)
            {
                if (easyQuests[questTasks].Count != 0)
                {
                    int quest = Random.Range(0, easyQuests[questTasks].Count);
                    quests.Add(easyQuests[questTasks][quest]);
                    numTasks += questTasks;
                }
            }
            else if (difficulty == 2)
            {
                if (mediumQuests[questTasks].Count != 0)
                {
                    int quest = Random.Range(0, mediumQuests[questTasks].Count);
                    quests.Add(mediumQuests[questTasks][quest]);
                    numTasks += questTasks;
                }
            }
            else
            {
                if (hardQuests[questTasks].Count != 0)
                {
                    int quest = Random.Range(0, hardQuests[questTasks].Count);
                    quests.Add(hardQuests[questTasks][quest]);
                    numTasks += questTasks;
                }
            }
        }

        return quests;
    }
}
