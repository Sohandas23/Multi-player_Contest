using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SheepScripts {
	public class BattleManager : MonoBehaviour {
		public SheepFighter ramA, ramB;
		public Text battleTimerText;
		public TMP_Text alertTxt;
		public Transform winnerHolder;
		public GameObject winnerItem,winningPanel,alertPanel,fightPanel;
		public Button restartButton,exitToLobby;
		
		
		private IEnumerator Start()
		{
			
			restartButton.onClick.AddListener(RestartGame);
			exitToLobby.onClick.AddListener(ExitToLobby);
			alertTxt.text = "Let's Begin the fight";
			yield return new WaitForSeconds(2f);
			alertTxt.text = "";
			alertPanel.SetActive(false);
			fightPanel.SetActive(true);
			ramA.sheepName.text = $"First Sheep";
			ramB.sheepName.text = $"Second Sheep";
			StartCoroutine(FightRoutine());
			
			
		}
			

		private IEnumerator FightRoutine() {
			float battleDuration = Random.Range(30f, 50f);
			float endTime = Time.time + battleDuration;
			//battleTimerText.text = $"Time Left: {Mathf.Ceil(endTime - Time.time)}s";
			StartCoroutine(UpdateBattleTimer(endTime));

			while (Time.time < endTime && !ramA.IsDead() && !ramB.IsDead()) {
				yield return StartCoroutine(ramA.Attack(ramB));
				if (ramB.IsDead()) break;
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(ramB.Attack(ramA));
				if (ramA.IsDead()) break;
				yield return new WaitForSeconds(0.5f);
				battleTimerText.text = $"Time Left: {Mathf.Ceil(endTime - Time.time)}s";
			}
			Debug.Log($"game over, ramA: {ramA.currentHealth}, ramB: {ramB.currentHealth}");
			StopCoroutine(UpdateBattleTimer(endTime));
			battleTimerText.text = "Time Left: 00:00";
			  ShowWinner();
		}
		IEnumerator UpdateBattleTimer(float endTime) {
			while (Time.time < endTime) {
				float remaining = endTime - Time.time;
				int minutes = Mathf.FloorToInt(remaining / 60);
				int seconds = Mathf.FloorToInt(remaining % 60);
				battleTimerText.text = $"Time Left: {minutes:00}:{seconds:00}";
				yield return null;
			}

			battleTimerText.text = "Time Left: 00:00";
		}

		private void ShowWinner() {
			foreach (Transform child in winnerHolder) Destroy(child.gameObject);
			SheepFighter winner;
			if (ramA.currentHealth > ramB.currentHealth)
				winner = ramA;
			else if (ramB.currentHealth > ramA.currentHealth)
				winner = ramB;
			else
				winner = null; 
			var item = Instantiate(winnerItem, winnerHolder);
			var go = item.GetComponent<WinnerItem>();
			go.gameObject.SetActive(true);
			if (winner != null)
				go.SetWinner(winner.sheepName.text + $" (HP: {winner.currentHealth})");
			else
				go.SetWinner("It's a Tie!");
			
			winningPanel.SetActive(true);
		}

		private void RestartGame()
		{
			NavigationManager.Instance.LoadSheepScene();
		}
		private void ExitToLobby()
		{
			NavigationManager.Instance.LoadSplashScene();
		}
		
	}
}