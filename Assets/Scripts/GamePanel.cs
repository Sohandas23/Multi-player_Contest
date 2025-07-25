using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamePanel : MonoBehaviourPunCallbacks {
	public Text timerText;
	public Button foldButton, contestButton;
	public GameObject resultPanel;
	public LobbyPanel lobbyPanel;
	public Text winnerText,gameStatusTxt;
	private Dictionary<int, int> playerNumbers = new();
	private Dictionary<int, bool> decisions = new();
	public Transform resultListContainer;
	public GameObject resultPlayerItemPrefab;
	private PhotonView photonView;
	private float _roundStartTime;
	public Button lobbyBtn;

	private void Awake() {
		photonView = GetComponent<PhotonView>();
	}

	private void Start()
	{
		lobbyBtn.onClick.AddListener(BackToLobby);
	}

	private void BackToLobby()
	{
		NavigationManager.Instance.LoadSplashScene();
	}


	public void StartNewRound() {
		foldButton.interactable = true;
		contestButton.interactable = true;
		resultPanel.SetActive(false);
		decisions.Clear();

		if (PhotonNetwork.IsMasterClient) {
			playerNumbers.Clear();

			foreach (var p in PhotonNetwork.PlayerList) {
				int randomNum = Random.Range(1, 101);
				playerNumbers[p.ActorNumber] = randomNum;
			}

			photonView.RPC(nameof(SyncAllPlayerNumbers), RpcTarget.All,
				playerNumbers.Select(kv => $"{kv.Key}:{kv.Value}").ToArray());

			photonView.RPC(nameof(StartTimerRPC), RpcTarget.All, PhotonNetwork.Time);
		}
	}

	[PunRPC]
	public void SyncAllPlayerNumbers(string[] data) {
		playerNumbers.Clear();

		foreach (string pair in data) {
			var parts = pair.Split(':');
			int actorId = int.Parse(parts[0]);
			int number = int.Parse(parts[1]);
			playerNumbers[actorId] = number;
		}
	}

	[PunRPC]
	public void StartTimerRPC(double serverStartTime) {
		_roundStartTime = (float)serverStartTime;
		StartCoroutine(SyncedTimer());
	}

	private IEnumerator SyncedTimer() {
		float countdown = 10f;
		float endTime = _roundStartTime + countdown;

		while (PhotonNetwork.Time < endTime) {
			float timeLeft = endTime - (float)PhotonNetwork.Time;
			timerText.text = $"Time: {Mathf.Ceil(timeLeft)}s";
			yield return null;
		}

		timerText.text = "0";

		if (PhotonNetwork.IsMasterClient) {
			photonView.RPC(nameof(EvaluateWinner), RpcTarget.All);
		}
	}

	public void OnFoldClicked() {
		SendDecision(false);
		gameStatusTxt.text = "You folded! 😣😣";
	}

	public void OnContestClicked() {
		SendDecision(true);
		gameStatusTxt.text = "You are In contesting! 🤩🤩";
	}

	private void SendDecision(bool isContesting) {
		foldButton.interactable = false;
		contestButton.interactable = false;
		photonView.RPC(nameof(SubmitDecision), RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber,
			isContesting);
	}

	[PunRPC]
	public void SubmitDecision(int playerId, bool isContesting) {
		decisions[playerId] = isContesting;
	}

	[PunRPC]
	public void EvaluateWinner()
	{
		if (!PhotonNetwork.IsMasterClient) return;

		var active = decisions.Where(d => d.Value).ToList();
		var validActive = active
			.Where(d => playerNumbers.ContainsKey(d.Key))
			.OrderByDescending(x => playerNumbers[x.Key])
			.ToList();

		List<string> resultList = new();
		string winnerTextString;

		if (validActive.Count == 0)
		{
			winnerTextString = "No valid contestants!";
		}
		else if (validActive.Count == 1)
		{
			int winnerId = validActive[0].Key;
			string winnerName = PhotonNetwork.CurrentRoom.GetPlayer(winnerId).NickName;
			int winnerNumber = playerNumbers[winnerId];
			winnerTextString = $"{winnerName} wins by default!";
			resultList.Add($"1:{winnerName}:{winnerNumber}");
		}
		else
		{
			for (int i = 0; i < validActive.Count; i++)
			{
				int actorId = validActive[i].Key;
				string name = PhotonNetwork.CurrentRoom.GetPlayer(actorId).NickName;
				int number = playerNumbers[actorId];
				resultList.Add($"{i + 1}:{name}:{number}");
			}

			var topPlayer = PhotonNetwork.CurrentRoom.GetPlayer(validActive[0].Key);
			int topNumber = playerNumbers[validActive[0].Key];
			winnerTextString = $"{topPlayer.NickName} wins with {topNumber}!";
		}

		photonView.RPC(nameof(DisplayResults), RpcTarget.All, winnerTextString, resultList.ToArray());
		gameStatusTxt.text = "New round started!";
	}
	[PunRPC]
	public void DisplayResults(string winnerMessage, string[] results)
	{
		foreach (Transform child in resultListContainer)
			Destroy(child.gameObject);

		winnerText.text = winnerMessage;

		foreach (var entry in results)
		{
			var parts = entry.Split(':');
			int rank = int.Parse(parts[0]);
			string name = parts[1];
			int number = int.Parse(parts[2]);

			CreateResultItem(rank, name, number);
		}

		resultPanel.SetActive(true);
	}



	private void CreateResultItem(int rank, string name, int number) {
		var item = Instantiate(resultPlayerItemPrefab, resultListContainer);
		var ui = item.GetComponent<ResultPlayerItem>();
		ui.gameObject.SetActive(true);
		ui.SetPlayerInfo(rank, name, number);
	}

	public void OnNextRoundClicked() {
		StartNewRound();
	}

	[PunRPC]
	public void StartGameForAll() {
		lobbyPanel.lobbyPanel.SetActive(false);
		lobbyPanel.gamePanel.SetActive(true);
		StartNewRound();
	}

	public void GameStart() {
		photonView.RPC(nameof(StartGameForAll), RpcTarget.All);
	}
}
[System.Serializable]
public class PlayerResultData {
	public int rank;
	public string name;
	public int number;
}
