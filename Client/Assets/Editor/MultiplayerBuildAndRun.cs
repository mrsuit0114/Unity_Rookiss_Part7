using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiplayerBuildAndRun
{
    [MenuItem("Tools/Run Multiplayer/2 Players")]
    static void PerforWin64Build2()
    {
        PerformWind64Build(2);
    }
    [MenuItem("Tools/Run Multiplayer/3 Players")]
    static void PerforWin64Build3()
    {
        PerformWind64Build(3);

    }
    [MenuItem("Tools/Run Multiplayer/4 Players")]
    static void PerforWin64Build4()
    {
        PerformWind64Build(4);

    }

    static void PerformWind64Build(int playerCount)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        for(int i =1; i<=playerCount; i++)
        {
            BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/TestBuild/" + GetProjectName() + i.ToString() + "/" + GetProjectName() + i.ToString() + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);
        }
    }

    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for(int i =0; i<scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }
}
