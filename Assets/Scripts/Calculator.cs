using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// This is mostly old stuff but is used in the test scene.

public class Calculator : MonoBehaviour {

    // For calculations
    public InputField passionInput;
    public InputField skillInput;
    public InputField difficultyInput;
    public InputField needInput;
    public InputField greedInput;
    public InputField needCareInput;
    public InputField greedCareInput;
    public GameObject output;

    //For generating Stats
    public GameObject passionText;
    public GameObject skillText;
    public GameObject otherText;

    public struct Stats
    {
        public int[] Passion;
        public int[] Skill;
        public float Greed;
        public float Need;

        public Stats(int[] passion, int[] skill, float greed, float need)
        {
            Passion = passion;
            Skill = skill;
            Greed = greed;
            Need = need;
        }
    }

    public void CalculateIkigai()
    {
        int passion = int.Parse(passionInput.text);
        int skill = int.Parse(skillInput.text);
        int difficulty = int.Parse(difficultyInput.text);
        int need = int.Parse(needInput.text);
        int greed = int.Parse(greedInput.text);
        float needCare = float.Parse(needCareInput.text);
        float greedCare = float.Parse(greedCareInput.text);

        int success;
        if (skill >= difficulty) { success = 1; }
        else { success = 0; }

        int total = passion + (success * (difficulty + (int)(need * needCare) + (int)(greed * greedCare)));
        output.GetComponent<Text>().text = "Total: " + total;

    }

    public void GenerateStats()
    {
        int[] Passion = { -5, -5, -5, -5, -5 };
        int[] Skill = { 0, 0, 0, 0, 0 };
        float Greed, Need;

        List<int> pList = new List<int>{ -5, -2, 0, 2, 5 };
        List<int> sList = new List<int> { 1, 3, 5, 7, 9 };

        for (int i = 5; i > 1; i--)
        {
            int index = Random.Range(0, i);
            print(index);
            Passion[i - 1] = pList[index];
            pList.RemoveAt(index);
            index = Random.Range(0, i);
            Skill[i - 1] = sList[index];
            sList.RemoveAt(index);
        }
        Passion[0] = pList[0];
        Skill[0] = sList[0];

        for (int i = 0; i < 5; i++)
        {
            int indexA = Random.Range(0, 5);
            while (Passion[indexA] <= -5)
            {
                indexA = Random.Range(0, 5);
            }
            int indexB = indexA;
            while (indexB == indexA || Passion[indexB] >= 5)
            {
                indexB = Random.Range(0, 5);
            }
            Passion[indexA] -= 1;
            Passion[indexB] += 1;

            indexA = Random.Range(0, 5);
            while (Skill[indexA] <= 0)
            {
                indexA = Random.Range(0, 5);
            }
            indexB = indexA;
            while (indexB == indexA || Skill[indexB] >= 10)
            {
                indexB = Random.Range(0, 5);
            }
            Skill[indexA] -= 1;
            Skill[indexB] += 1;

        }

        // Old system, too average
        /*
        for (int i = 0; i < 25; i++)
        {
            while (true)
            {
                int index = Random.Range(0, 5);
                if (Passion[index] < 5)
                {
                    Passion[index] += 1;
                    break;
                }
            }
            while (true)
            {
                int index = Random.Range(0, 5);
                if (Skill[index] < 10)
                {
                    Skill[index] += 1;
                    break;
                }
            }
        }
        */



        string passiontext, skilltext;
        passiontext = "Passion:";
        skilltext = "Skill:";
        for (int i = 0; i < 5; i++)
        {
            passiontext += "\n" + Passion[i];
            skilltext += "\n" + Skill[i];
            print("Passion: " + Passion[i] + "   Skill: " + Skill[i]);
        }

        passionText.GetComponent<Text>().text = passiontext;
        skillText.GetComponent<Text>().text = skilltext;

        Greed = Random.Range(0, 10) / 10f;
        Need = Random.Range(0, 10) / 10f;
        otherText.GetComponent<Text>().text = "Greed: " + Greed + "\nNeed: " + Need;

        print("Greed: " + Greed);
        print("Need: " + Need);
    
        Stats stats = new Stats(Passion, Skill, Greed, Need);
    }

}
