using BepInEx;
using System;
using UnityEngine;
using Utilla;

namespace Elis_Stop_Watch_Mod
{
    using BepInEx;
    using System.Collections.Generic;
    using UnityEngine;

    [BepInPlugin("com.yourname.stopwatchmod", "Stopwatch Mod", "1.0.0")]
    public class StopwatchMod : BaseUnityPlugin
    {
        private float elapsedTime = 0f;
        private bool isRunning = false;

        private List<string> recordedTimes = new List<string>();  // Store added times
        private GUIStyle stopwatchStyle; // Define a GUIStyle for the stopwatch
        private GUIStyle recordedTimeStyle; // Define a GUIStyle for recorded times

        void Start()
        {
            // Initialize the stopwatch style and set font size and color
            stopwatchStyle = new GUIStyle();
            stopwatchStyle.fontSize = 24;
            stopwatchStyle.normal.textColor = Color.white;

            // Initialize the recorded time style
            recordedTimeStyle = new GUIStyle();
            recordedTimeStyle.fontSize = 28;
            recordedTimeStyle.normal.textColor = Color.yellow;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized()
        {
            Debug.Log("Game Initialized, Stopwatch ready.");
        }

        void Update()
        {
            if (isRunning)
            {
                elapsedTime += Time.deltaTime;
            }
        }

        void OnGUI()
        {
            // Update the text color based on the stopwatch state
            stopwatchStyle.normal.textColor = isRunning ? Color.red : Color.white;

            // Calculate minutes, seconds, and milliseconds
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime - Mathf.Floor(elapsedTime)) * 1000);

            // Format the time string with milliseconds
            string timeText = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);

            // Display the stopwatch time in the top-left corner of the screen
            GUI.Label(new Rect(10, 10, 250, 50), "Stopwatch: " + timeText, stopwatchStyle);

            // Create buttons for start, stop, and restart below the stopwatch
            if (GUI.Button(new Rect(10, 60, 100, 30), isRunning ? "Stop" : "Start"))
            {
                isRunning = !isRunning;
            }

            if (GUI.Button(new Rect(120, 60, 100, 30), "Restart"))
            {
                elapsedTime = 0f;
                isRunning = false;
            }

            // Add button to record the current stopwatch time
            if (!isRunning && GUI.Button(new Rect(10, 100, 100, 30), "Add Time"))
            {
                recordedTimes.Add(timeText);  // Add the current time to the list
            }

            // Remove button to remove the most recent saved time
            if (!isRunning && GUI.Button(new Rect(120, 100, 100, 30), "Remove Time"))
            {
                if (recordedTimes.Count > 0)
                {
                    recordedTimes.RemoveAt(recordedTimes.Count - 1);  // Remove the most recent time
                }
            }

            // Add a "Remove All" button to remove all saved times
            if (!isRunning && GUI.Button(new Rect(10, 140, 100, 30), "Remove All"))
            {
                recordedTimes.Clear();  // Remove all saved times
            }

            // Display the recorded times in the top-right corner, stacked below each other
            for (int i = 0; i < recordedTimes.Count; i++)
            {
                GUI.Label(new Rect(Screen.width - 150, 10 + i * 25, 200, 25), recordedTimes[i], recordedTimeStyle);
            }
        }
    }
}
