using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersTracker : MonoBehaviour {

    public List<Playerstats> players;

	// Use this for initialization
	void Awake () {
		players = new List<Playerstats>();
    }

    // Rolls stats for any amount of players, clears the old stats and stores the new ones.
    public void GenerateStats(int numPlayers, string[] names)
    {
        players.Clear();

        //Generate stats for each player
        for (int x = 0; x < numPlayers; x++)
        {
            int[] Passion = { -5, -5, -5, -5, -5 };
            int[] Skill = { 0, 0, 0, 0, 0 };
            float Greed, Need;

            List<int> pList = new List<int> { -5, -2, 0, 2, 5 };
            List<int> sList = new List<int> { 1, 3, 5, 7, 9 };

            // Randomly assign which stat gets which number
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

            // Goes through 5 rounds of "Mutations" which add to 1 stat and subtract from another.
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

            // Greed and need are from 0-1
            Greed = Random.Range(0, 10) / 10f;
            Need = Random.Range(0, 10) / 10f;

            // set the values and then generates a display
            Playerstats playerstats = new Playerstats();
            playerstats.Passion = Passion;
            playerstats.Skill = Skill;
            playerstats.Greed = Greed;
            playerstats.Need = Need;
            playerstats.name = names[x];
            playerstats.GenerateDisplay();
            players.Add(playerstats);

        }
    }
}
