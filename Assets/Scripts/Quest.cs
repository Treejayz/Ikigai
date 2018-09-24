using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A task is one role in a quest,
// which has a stat, difficulty, and description.
[System.Serializable]
public class Task
{
    public enum Stat
    {
        Fight,
        Charm,
        Agility,
        Smarts,
        Magic
    };

    public Stat stat = Stat.Fight;
    [Range(1, 10)]
    public int difficulty = 1;

    public string description;
}


// A quest has a description, a reward, a need, and some number of tasks.
// DO NOT CHANGE

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 1)]
public class Quest : ScriptableObject {

    [TextArea(3,10)]
    public string description = "Default description";
    [TextArea(3, 10)]
    public string rewardDescription = "Default reward description";
    [Range(-5, 5)]
    public int money = 0;
    [Range(-5, 5)]
    public int need = 0;
    [Range(1, 3)]
    public int overallDifficulty = 1;

    public Task[] tasks;

    [TextArea(2, 10)]
    public string succeed = "You succeed";
    [TextArea(2, 10)]
    public string fail = "You fail";

    [HideInInspector]
    public string title = "Default Title";

    [HideInInspector]
    public bool completed = false;
}
