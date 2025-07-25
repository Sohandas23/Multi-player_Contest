using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance;
    public static string MultiplayerScene = "MultiplayerContest";
    public static string SheepFightScene = "RamFight";
    public static string Splash = "Splash";

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        
    }

   public void LoadMultiplayerScene()=> SceneManager.LoadScene(MultiplayerScene);
   public void LoadSheepScene()=> SceneManager.LoadScene(SheepFightScene);
   public void LoadSplashScene()=> SceneManager.LoadScene(Splash);
}
