using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviourPunCallbacks
{
	public GameObject playerListItemPrefab,lobbyPanel,gamePanel;
	public Transform playerListParent; 
	public GamePanel gamePanelScript;
	public Button startGameButton;
	private List<GameObject> _spawnedItems = new();
	public PhotonView photonView;

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
	}
	
	
	
	public void ActiveDeactiveLobbyPanel(bool isActive)
	{
		lobbyPanel.SetActive(isActive);
		if (isActive)
		{
			RefreshPlayerList();
		}
		else
		{
			foreach (var go in _spawnedItems) Destroy(go);
			_spawnedItems.Clear();
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		RefreshPlayerList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		RefreshPlayerList();
	}

	private void RefreshPlayerList()
	{
		foreach (var go in _spawnedItems) Destroy(go);
		_spawnedItems.Clear();
		Debug.Log($"List of players in the room: {PhotonNetwork.PlayerList.Length}");
		foreach (var player in PhotonNetwork.PlayerList)
		{
			var item = Instantiate(playerListItemPrefab, playerListParent);
			
			var go = item.GetComponent<PlayerListItem>();
			go.gameObject.SetActive(true);
			go.SetPlayerName(player.NickName);
			_spawnedItems.Add(item);
		}
		startGameButton.interactable = PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 2;
	}
	

	
}
