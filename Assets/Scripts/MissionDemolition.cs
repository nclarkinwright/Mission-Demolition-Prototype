﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // a private Singleton

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitLowestShots;
    public Text uitButton;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    private int? lowestShot;

    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;

        StartLevel();
    }

    void StartLevel()
    {
        // Get rid of the old castle if one exists
        if(castle != null)
        {
            Destroy(castle);
        }

        // Destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // Destroy broken pieces
        GameObject[] bps = GameObject.FindGameObjectsWithTag("BrokenPiece");
        foreach(GameObject bpTemp in bps)
        {
            Destroy(bpTemp);
        }
        
        // Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // Reset the camera
        SwitchView("Show Both");

        // Reset the goal
        Goal.goalMet = false;

        // Get the best score
        if(PlayerPrefs.HasKey("LowestShot_" + level))
        {
            lowestShot = PlayerPrefs.GetInt("LowestShot_" + level);
        }
        else
        {
            lowestShot = null;
        }

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        // Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;

        if (lowestShot != null)
        {
            uitLowestShots.text = "Lowest Shots: " + lowestShot;
        }
        else
        {
            uitLowestShots.text = "Lowest Shots: N/A";
        }
    }
    private void Update()
    {
        UpdateGUI();

        // Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            if(shotsTaken < lowestShot || lowestShot == null)
            {
                lowestShot = shotsTaken;
                PlayerPrefs.SetInt("LowestShot_" + level, (int)lowestShot);
            }
            
            Debug.Log(PlayerPrefs.GetInt("LowestShot_" + level).ToString());
            
            // Change mode to stop checking for level end
            mode = GameMode.levelEnd;
            // Zoom out
            SwitchView("Show Both");
            // Start the next level in 3 seconds
            Invoke("NextLevel", 3f);
        }
    }

    void NextLevel()
    { 
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if(eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    // Static method that allows the code anywhere to increment shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
