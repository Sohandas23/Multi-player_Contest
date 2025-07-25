using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPlayerItem : MonoBehaviour
{
    public Text serialNumber,playerNameText, playerNumber;
    
    public void SetPlayerInfo(int serial, string playerName, int number)
    {
        serialNumber.text =  $"#{serial}";
        playerNameText.text = playerName;
        playerNumber.text = number.ToString();
    }
}
