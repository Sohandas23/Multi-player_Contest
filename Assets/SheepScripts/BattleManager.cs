using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SheepScripts {
	public class BattleManager : MonoBehaviour {
		public SheepFighter ramA, ramB;
		public Text battleTimerText;
		

		//public HealthBarUI healthBarA, healthBarB;
		//public TextMeshProUGUI resultText;
		// Start is called before the first frame update
		private void Start() => StartCoroutine(FightRoutine());

		IEnumerator FightRoutine() {
			float battleDuration = Random.Range(30f, 50f);
			float endTime = Time.time + battleDuration;
			//battleTimerText.text = $"Time Left: {Mathf.Ceil(endTime - Time.time)}s";
			StartCoroutine(UpdateBattleTimer(endTime));

			while (Time.time < endTime && !ramA.IsDead() && !ramB.IsDead()) {
				yield return StartCoroutine(ramA.Attack(ramB));
				//healthBarB.UpdateHealth(ramB.currentHealth, ramB.maxHealth);
				if (ramB.IsDead()) break;
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(ramB.Attack(ramA));
				 //healthBarA.UpdateHealth(ramA.currentHealth, ramA.maxHealth);
				if (ramA.IsDead()) break;

				yield return new WaitForSeconds(0.5f);
				battleTimerText.text = $"Time Left: {Mathf.Ceil(endTime - Time.time)}s";
			}
			Debug.Log($"game over, ramA: {ramA.currentHealth}, ramB: {ramB.currentHealth}");
			StopCoroutine(UpdateBattleTimer(endTime));
			battleTimerText.text = "Time Left: 00:00";
			//  ShowWinner();
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
	}
}