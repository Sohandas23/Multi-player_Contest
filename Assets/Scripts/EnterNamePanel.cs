using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterNamePanel : MonoBehaviourPunCallbacks
{
	public GameObject loginPanel;
	public LobbyPanel lobbyPanel;
	public InputField nameInput;
	public void OnContinueClicked()
	{
		if (string.IsNullOrEmpty(nameInput.text)) return;

		PhotonNetwork.NickName = nameInput.text;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("No room found. Creating one...");
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Successfully joined a room!");
		loginPanel.SetActive(false);
		lobbyPanel.ActiveDeactiveLobbyPanel(true); 
	}
}
