using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class SheepFighter : MonoBehaviour {
	public int maxHealth = 100;
	public int currentHealth;
	public Animator animator;
	public float moveTowardsSpeed, moveBackwardsSpeed;
	public Image healthBar;
	private Text _damageText;
	public Text sheepName;


	public event Action<int> OnDamageTaken;

	void Awake() {
		currentHealth = maxHealth;
		_damageText = healthBar.GetComponentInChildren<Text>();
		if (animator == null) {
			animator = GetComponent<Animator>();
			//Attack(this);
		}
	}

	void Start() {
		_damageText.text =  $"{currentHealth.ToString()} %";
	}

	public IEnumerator Attack(SheepFighter opponent) {
		yield return StartCoroutine(PerformAttack(opponent)); // existing logic
	}

	private IEnumerator PerformAttack(SheepFighter opponent) {
		Vector3 originalPos = transform.position;
		Vector3 targetPos = opponent.transform.position +
		                    (transform.position - opponent.transform.position).normalized * 1.5f;

		// Run towards opponent
		animator.SetBool("IsRunning", true);
		while (Vector3.Distance(transform.position, targetPos) > 0.1f) {
			transform.position = Vector3.MoveTowards(transform.position, targetPos, moveTowardsSpeed * Time.deltaTime);
			yield return null;
		}
		animator.SetBool("IsRunning", false);
		animator.SetBool("Attack", true);
		yield return new WaitForSeconds(0.8f); 
		int damage = Random.Range(5, 15);
		if (Random.value < 0.2f) damage *= 2;
		StartCoroutine(opponent.TakeDamage(damage));
		yield return new WaitForSeconds(0.8f); // small delay before retreat
		animator.SetBool("Attack",false);
		animator.SetBool("IsWalkingBack", true);
		while (Vector3.Distance(transform.position, originalPos) > 0.1f) {
			transform.position = Vector3.MoveTowards(transform.position, originalPos, moveBackwardsSpeed * Time.deltaTime);
			yield return null;
		}
		animator.SetBool("IsWalkingBack", false);
	}


	public IEnumerator TakeDamage(int amount) {
		animator.SetBool("Attack",true);
		yield return new WaitForSeconds(0.5f); 
		currentHealth -= amount;
		OnDamageTaken?.Invoke(currentHealth);
		healthBar.DOFillAmount( (float)currentHealth / maxHealth, 0.5f).SetEase(Ease.InOutQuad);
		_damageText.text = $"{currentHealth.ToString()} %";
		animator.SetBool("Attack",false);
		//animator.SetBool("Attack",true);
		// Instantiate(damagePopupPrefab, hitEffectSpawn.position, Quaternion.identity);
		//.GetComponent<DamagePopup>().Setup(amount);
	}

	public bool IsDead() => currentHealth <= 0;
}