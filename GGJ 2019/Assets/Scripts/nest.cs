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

	// Nest sprites
	public List<Sprite> nest_sprites;

	// Static vars
	public static int twigs_needed;
	public static int bugs_needed;
	public static bool inited = false;

	// Private vars
	private int bird_layer_mask;

	private Collider2D col;
	private SpriteRenderer sr;

	// Start is called before the first frame update
	void Start() {
		col = GetComponent<Collider2D>();
		sr = GetComponent<SpriteRenderer>();
		bird_layer_mask = LayerMask.GetMask(new string[] { "bird" });

		if (!inited) {
			twigs_needed = stage_twigs_needed[0];
			bugs_needed = stage_bugs_needed[0];
			sleeping_var.val = false;
			inited = true;
		}

		update_nest_sprite();
	}

	public static void nest_full_init() {
		inited = false;
	}

	// Update is called once per frame
	void Update() {
		if (!sleeping_var.val && col.IsTouchingLayers(bird_layer_mask)) {
			twig_thought.val = twigs_needed > 0;
			bug_thought.val = bugs_needed > 0;
			if (twigs_needed <= 0 && bugs_needed <= 0) {
				if (movement.move_Stage < move_stage.momma) {
					complete_stage();
				}
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

		int stage_needed_index = new_stage - 2;
		if (stage_needed_index < stage_twigs_needed.Count) {
			twigs_needed += stage_twigs_needed[new_stage - 2];
			bugs_needed += stage_bugs_needed[new_stage - 2];
		}

		print("Completed stage! New move stage: " + movement.move_Stage);

		update_nest_sprite();

		movement.bird_instance.set_movement_enabled(false);
		twig_thought.val = false;
		bug_thought.val = false;
		sleeping_var.val = true;
		StartCoroutine(bird_sleeps());
	}

	IEnumerator bird_sleeps() {
		yield return new WaitForSeconds(0.4f);
		movement.bird_instance.transform.position = transform.position + (Vector3.up * 0.2f);
		if (sr.sprite == nest_sprites[2]) {
			movement.bird_instance.transform.position = transform.position + (Vector3.up * 0.1f);
		}
		movement.bird_instance.GetComponent<Animator>().SetTrigger("sleep");
		yield return new WaitForSeconds(3f);
		if (movement.move_Stage == move_stage.momma) {
			if (Random.value < 0.33f) {
				black_fade.black_fade_to_scene(5);
			} else {
				black_fade.black_fade_to_scene(1);
			}
			sleeping_var.val = false;
		} else {
			black_fade.fade_to_black(3f);
			yield return new WaitForSeconds(3f);
			yield return new WaitForSeconds(2f);
			sleeping_var.val = false;
			movement.bird_instance.set_movement_enabled(true);
			movement.bird_instance.GetComponent<Animator>().SetTrigger("wake up");
			black_fade.fade_from_black(1.5f);
		}
	}

	private void update_nest_sprite() {
		if (movement.move_Stage == move_stage.flutter || movement.move_Stage == move_stage.glide || movement.move_Stage == move_stage.jump) {
			sr.sprite = nest_sprites[0];
		} else if (movement.move_Stage == move_stage.double_jump) {
			sr.sprite = nest_sprites[1];
		} else {
			sr.sprite = nest_sprites[2];
		}
	}
}
