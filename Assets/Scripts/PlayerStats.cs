using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerstats
{
    public string name = "Name Namington";
    // For both Passion and Skill, the indexing goes as such:
    // 0 = Fight
    // 1 = Charm
    // 2 = Agility
    // 3 = Smarts
    // 4 = Magic
    public int[] Passion;
    public int[] Skill;
    public float Greed;
    public float Need;

    public string display;


    // Generates a long string to be displayed in a text box.
    public void GenerateDisplay()
    {
        display = "<b>" + name + "</b>\nStats\tPassion\tSkill\nFight\t\t" + Passion[0] + "\t\t" + Skill[0] + "\n";
        display += "Charm\t\t" + Passion[1] + "\t\t" + Skill[1] + "\n";
        display += "Agility\t\t" + Passion[2] + "\t\t" + Skill[2] + "\n";
        display += "Smarts\t\t" + Passion[3] + "\t\t" + Skill[3] + "\n";
        display += "Magic\t\t" + Passion[4] + "\t\t" + Skill[4] + "\n";
        display += "Greed:" + Greed + "\nNeed: " + Need;
    }
}
