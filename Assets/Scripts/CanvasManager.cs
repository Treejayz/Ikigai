using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    public GameObject startCanvas;
    public GameObject questCanvas;
    public GameObject statsCanvas;
    public GameObject resultsCanvas;
    public GameObject jobsCanvas;

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
}
