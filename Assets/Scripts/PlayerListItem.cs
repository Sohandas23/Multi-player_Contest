using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviour
{
  public Text playerNameText;

  public void SetPlayerName(string playerName) {
    playerNameText.text = playerName;
  }
  
}
