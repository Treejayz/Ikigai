using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour {

    public TextAsset jobsTextFile;

    [HideInInspector]
    public Dictionary<string, string> jobs = new Dictionary<string, string>();
    List<string> jobTitles = new List<string>();

	// Use this for initialization
	void Awake () {
        string temp = jobsTextFile.text;
        string[] templist = temp.Split('\n');
        foreach (string job in templist)
        {
            string[] data = job.Split('-');
            jobTitles.Add(data[0]);
            jobs.Add(data[0], data[1]);
        }
	}

    public Dictionary<string, string> ChooseJobs()
    {
        Dictionary<string, string> chosenJobs = new Dictionary<string, string>();

        int job1 = 0;
        int job2 = 0;
        int job3 = 0;

        job1 = Random.Range(0, jobs.Count);
        job2 = Random.Range(0, jobs.Count);
        job3 = Random.Range(0, jobs.Count);

        while (job2 == job1)
        {
            job2 = Random.Range(0, jobs.Count);
        }

        while (job3 == job1 || job3 == job2)
        {
            job3 = Random.Range(0, jobs.Count);
        }

        string jobTitle1 = jobTitles[job1];
        string jobTitle2 = jobTitles[job2];
        string jobTitle3 = jobTitles[job3];

        print(jobTitle1 + ", " + jobTitle2 + ", " + jobTitle3);

        chosenJobs.Add(jobTitle1, jobs[jobTitle1]);
        chosenJobs.Add(jobTitle2, jobs[jobTitle2]);
        chosenJobs.Add(jobTitle3, jobs[jobTitle3]);

        return chosenJobs;
    }

    public void RemoveJob(string job)
    {
        jobTitles.Remove(job);
        jobs.Remove(job);
    }

}
