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
    
    // Choose which quests the players will have, given a max difficulty and number of players.
    public List<Quest> ChooseQuests(int maxDifficulty, int numPlayers)
    {
        // Keeps track of how many total tasks combined are in all the quests.
        // Must be equal to numPlayers by the end.
        int numTasks = 0;

        List<Quest> quests = new List<Quest>();

        // To prevent infinite loops,
        // AKA theres not enough quests for the players so it cant find any.
        int numRolls = 100;

        while (numTasks < numPlayers && numRolls > 0)
        {

            // Choose a difficulty and number of tasks for this quest
            int difficulty = Random.Range(0, maxDifficulty) + 1;
            int questTasks;
            // If we already have 2 quests, then the last one should have enough tasks for the remainder.
            if (quests.Count == 2)
            {
                questTasks = numPlayers - numTasks;
            }
            else
            {
                questTasks = Random.Range(1, Mathf.Clamp((numPlayers - numTasks + 1), 1, 5));
            }

            // Sort by difficulty
            if (difficulty == 1)
            {
                // If we have tasks of the corresponding difficulty and task amount,
                // Then grab a random one of them. Same for each difficulty
                if (easyQuests[questTasks-1].Count != 0)
                {
                    int quest = Random.Range(0, easyQuests[questTasks-1].Count);
                    if (!quests.Contains(easyQuests[questTasks-1][quest])) {
                        quests.Add(easyQuests[questTasks-1][quest]);
                        numTasks += questTasks;
                    }
                }
            }
            else if (difficulty == 2)
            {
                if (mediumQuests[questTasks-1].Count != 0)
                {
                    int quest = Random.Range(0, mediumQuests[questTasks-1].Count);
                    if (!quests.Contains(mediumQuests[questTasks-1][quest])) {
                        quests.Add(mediumQuests[questTasks-1][quest]);
                        numTasks += questTasks;
                    }
                }
            }
            else
            {
                if (hardQuests[questTasks-1].Count != 0)
                {
                    int quest = Random.Range(0, hardQuests[questTasks-1].Count);
                    if (!quests.Contains(hardQuests[questTasks-1][quest])) {
                        quests.Add(hardQuests[questTasks-1][quest]);
                        numTasks += questTasks;
                    }
                }
            }
            // Subtract 1 to make sure we dont loop forever
            numRolls -= 1;
        }

        // If we couldnt find enough quests, return null
        if (numRolls == 0)
        {
            return null;
        }
        // Otherwise return the quests!
        return quests;
    }
}
