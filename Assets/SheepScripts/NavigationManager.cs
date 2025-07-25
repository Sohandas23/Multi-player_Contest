using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance;
    public static string MultiplayerScene = "";
    public static string SheepFightScene = "";

    private void Start()
    {
        Instance = this;
        
    }
}
