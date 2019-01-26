using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

	// Public fields
	public float x_mult;
	public float max_velo;

	public float flutter_force;
	public float flutter_delay;
	public float flutter_thresh;

	public float glide_grav_scale;
	public float glide_max_velo;

	public float jump_velo;

	// Static settings
	public static move_stage move_Stage = move_stage.flutter;
	public static bool gliding = false;

	// Private vars
	private readonly string horizontal_axis = "Horizontal";
	private readonly string vertical_axis = "Vertical";

	private float base_grav_scale;
	private float flutter_timer;

	private bool jump_held = false;
	private int jump_charges = 12; // set high for testing

	// Component references
	private Rigidbody2D rb;
	private SpriteRenderer sr;


	// Init
	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();

		base_grav_scale = rb.gravityScale;
	}

	// Called once per frame
	void Update() {
		float x_input = Input.GetAxis(horizontal_axis);
		float y_input = Input.GetAxisRaw(vertical_axis);

		// Horizontal movement
		transform.Translate(x_input * x_mult * Time.deltaTime, 0, 0);

		// Check if you've landed, to reset jump charges
		//		TODO

		// Vertical controls
		switch (move_Stage) {
			case move_stage.flutter:
				if (flutter_timer > 0) {
					flutter_timer -= Time.deltaTime;
				} else if (y_input > 0 && rb.velocity.y < flutter_thresh) {
					print("Fluttered");
					rb.AddForce(new Vector2(0, flutter_force));
					flutter_timer = flutter_delay;
				}
				break;
			case move_stage.glide:
				break;
			case move_stage.jump:
				if (y_input > 0 && !jump_held && jump_charges > 0) {
					rb.velocity = new Vector2(rb.velocity.x, jump_velo);
					jump_held = true;
					jump_charges--;
				}
				break;
			case move_stage.double_jump:
				if (y_input > 0 && !jump_held && jump_charges > 0) {
					rb.velocity = new Vector2(rb.velocity.x, jump_velo);
					jump_held = true;
					jump_charges--;
				}
				break;
		}

		if (y_input <= 0) {
			jump_held = false;
		}

		// Glide if you're falling, and holding up
		check_glide(y_input);

		// Clamp the velocity
		float falling_clamp = gliding ? glide_max_velo : max_velo;
		rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -falling_clamp, jump_velo + 0.1f));

		// Sprite flip
		if (x_input > 0) {
			sr.flipX = true;
		} else if (x_input < 0) {
			sr.flipX = false;
		}
	}

	// Adjust the bird's gravity scale based on y-input, to glide (if you're falling)
	private void check_glide(float y_input) {
		if (move_Stage == move_stage.flutter) {
			gliding = false;
			return;
		}
		
		if (rb.velocity.y < 0 && y_input > 0) {
			gliding = true;
			rb.gravityScale = glide_grav_scale;
		} else {
			gliding = false;
			rb.gravityScale = base_grav_scale;
		}
	}
}

// Movement stages enum
public enum move_stage { flutter, glide, jump, double_jump, fly }
