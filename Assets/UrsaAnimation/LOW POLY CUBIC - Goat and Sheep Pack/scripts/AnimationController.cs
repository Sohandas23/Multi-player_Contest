using UnityEngine;

namespace Ursaanimation.CubicFarmAnimals {
	public class AnimationController : MonoBehaviour {
		public Animator animator;
		public string walkForwardAnimation = "walk_forward";
		public string walkBackwardAnimation = "walk_backwards";
		public string runForwardAnimation = "run_forward";
		public string turn90LAnimation = "turn_90_L";
		public string turn90RAnimation = "turn_90_R";
		public string trotAnimation = "trot_forward";
		public string sittostandAnimation = "sit_to_stand";
		public string standtositAnimation = "stand_to_sit";
		public string GoatSheep_attack01 = "GoatSheep_attack01";
		public string GoatSheep_death = "GoatSheep_death";

		void Start() {
			animator = GetComponent<Animator>();
		}

		void Update() {
			if (Input.GetKeyDown(KeyCode.W)) {
				animator.Play(walkForwardAnimation);
			} else if (Input.GetKeyDown(KeyCode.S)) {
				animator.Play(walkBackwardAnimation);
			} else if (Input.GetKeyDown(KeyCode.P)) {
				animator.Play(GoatSheep_death);
			} else if (Input.GetKeyDown(KeyCode.E)) {
				animator.Play(GoatSheep_attack01);
			} else if (Input.GetKeyDown(KeyCode.Space)) {
				animator.Play(runForwardAnimation);
			} else if (Input.GetKeyDown(KeyCode.A)) {
				animator.Play(turn90LAnimation);
			} else if (Input.GetKeyDown(KeyCode.D)) {
				animator.Play(turn90RAnimation);
			} else if (Input.GetKeyDown(KeyCode.N)) {
				animator.Play(trotAnimation);
			} else if (Input.GetKeyDown(KeyCode.G)) {
				animator.Play(sittostandAnimation);
			} else if (Input.GetKeyDown(KeyCode.C)) {
				animator.Play(standtositAnimation);
			}
		}
	}
}