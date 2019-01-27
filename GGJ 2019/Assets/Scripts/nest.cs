using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nest : MonoBehaviour {

	// Public fields
	public List<int> stage_twigs_needed;
	public List<int> stage_bugs_needed;

	// SO references
	public bool_var twig_thought;
	public bool_var bug_thought;
	public bool_var sleeping_var;

	// Static vars
	public static int twigs_needed;
	public static int bugs_needed;

	// Private vars
	private int bird_layer_mask;

	private Collider2D col;

	// Start is called before the first frame update
	void Start() {
		col = GetComponent<Collider2D>();
		bird_layer_mask = LayerMask.GetMask(new string[] { "bird" });

		twigs_needed = stage_twigs_needed[0];
		bugs_needed = stage_bugs_needed[0];
		sleeping_var.val = false;
	}

	// Update is called once per frame
	void Update() {
		if (!sleeping_var.val && col.IsTouchingLayers(bird_layer_mask)) {
			twig_thought.val = twigs_needed > 0;
			bug_thought.val = bugs_needed > 0;
			if (twigs_needed <= 0 && bugs_needed <= 0) {
				complete_stage();
			}
		} else {
			twig_thought.val = false;
			bug_thought.val = false;
		}
	}

	public void place_twig() {
		twigs_needed--;
	}

	public static void eat_bug() {
		bugs_needed--;
		SoundManager.instance.playEat();
	}

	private void complete_stage() {
		int new_stage = (int)movement.move_Stage + 1;
		movement.move_Stage = (move_stage)new_stage;
		twigs_needed += stage_twigs_needed[new_stage - 2];
		bugs_needed += stage_bugs_needed[new_stage - 2];

		print("Completed stage! New move stage: " + movement.move_Stage);

		// todo - visual improvement of nest

		movement.bird_instance.set_movement_enabled(false);
		twig_thought.val = false;
		bug_thought.val = false;
		sleeping_var.val = true;
		StartCoroutine(bird_sleeps());
	}

	IEnumerator bird_sleeps() {
		yield return new WaitForSeconds(0.4f);
		movement.bird_instance.transform.position = transform.position + (Vector3.up * 0.3f);
		movement.bird_instance.GetComponent<Animator>().SetTrigger("sleep");
		yield return new WaitForSeconds(3f);
		black_fade.fade_to_black(3f);
		yield return new WaitForSeconds(3f);
		yield return new WaitForSeconds(2f);
		sleeping_var.val = false;
		movement.bird_instance.set_movement_enabled(true);
		movement.bird_instance.GetComponent<Animator>().SetTrigger("wake up");
		black_fade.fade_from_black(1.5f);
	}
}
