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
	}

	// Update is called once per frame
	void Update() {
		if (col.IsTouchingLayers(bird_layer_mask)) {
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
		// todo - visual improvement of nest
	}

	public static void eat_bug() {
		bugs_needed--;
		SoundManager.instance.playEat();
	}

	private void complete_stage() {
		int new_stage = (int)movement.move_Stage + 1;
		movement.move_Stage = (move_stage)new_stage;
		print("Completed stage! New move stage: " + movement.move_Stage);
		// todo - fade to black while bird rests and grows

		twigs_needed += stage_twigs_needed[new_stage - 1];
		bugs_needed += stage_bugs_needed[new_stage - 1];
	}
}
