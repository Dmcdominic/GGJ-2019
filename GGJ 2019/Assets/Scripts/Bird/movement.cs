using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

	// Public fields
	public move_stage init_stage; // Todo - remove

	public float x_mult;
	public float max_velo;

	public float flutter_force;
	public float flutter_delay;
	public float flutter_thresh;

	public float glide_grav_scale;
	public float glide_max_velo;

	public float jump_velo;
	public float raycast_dist;

	public float cam_adjust_time;

	public GameObject visuals;
	public GameObject feet_pos;

	// Static settings
	public static move_stage move_Stage = move_stage.glide; // Start this at flutter, if we have time
	public static bool gliding = false;

	// Private vars
	private readonly string horizontal_axis = "Horizontal";
	private readonly string vertical_axis = "Vertical";

	private float base_grav_scale;
	private float flutter_timer;

	private bool jump_held = false;
	private int jump_charges = 0;

	private int landing_layer_mask;

	// Component references
	private Rigidbody2D rb;
	private Collider2D col;


	// Init
	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();

		base_grav_scale = rb.gravityScale;
		landing_layer_mask = LayerMask.GetMask(new string[] { "platform" });
		move_Stage = init_stage;
	}

	// Called once per frame
	void FixedUpdate() {
		float x_input = Input.GetAxis(horizontal_axis);
		float y_input = Input.GetAxisRaw(vertical_axis);

		// Horizontal movement
		//rb.velocity += new Vector2(x_input * x_mult * Time.deltaTime, 0);
		transform.Translate(Mathf.Abs(x_input)*(-1f) * x_mult * Time.deltaTime, 0, 0);
		//if (x_input != 0) {
		//	rb.interpolation = RigidbodyInterpolation2D.None;
		//} else {
		//	rb.interpolation = RigidbodyInterpolation2D.Interpolate;
		//}

		// Check if you've landed, to reset jump charges
		RaycastHit2D raycast = Physics2D.Raycast(feet_pos.transform.position, Vector2.down, raycast_dist, landing_layer_mask);
		if (raycast.collider != null && rb.velocity.y <= 0) {
			if (move_Stage == move_stage.jump) {
				jump_charges = 1;
			} else if (move_Stage == move_stage.double_jump) {
				jump_charges = 2;
			}
		}

		// Vertical controls
		switch (move_Stage) {
			case move_stage.flutter:
				if (flutter_timer > 0) {
					flutter_timer -= Time.deltaTime;
				} else if (y_input > 0 && rb.velocity.y < flutter_thresh) {
					rb.AddForce(new Vector2(0, flutter_force));
					flutter_timer = flutter_delay;
				}
				break;
			case move_stage.glide:
				break;
			case move_stage.jump:
			// Fall through to double_jump, which has same behavior
			case move_stage.double_jump:
				if (y_input > 0 && !jump_held && jump_charges > 0) {
					rb.velocity = new Vector2(rb.velocity.x, jump_velo);
					jump_charges--;
				}
				break;
			case move_stage.fly:
				if (y_input > 0 && !jump_held) {
					rb.velocity = new Vector2(rb.velocity.x, jump_velo);
				}
				break;
		}

		if (y_input <= 0) {
			jump_held = false;
		} else {
			jump_held = true;
		}

		// Fall through platforms if you are holding down
		//col.enabled = y_input >= 0;

		// Glide if you're falling, and holding up
		check_glide(y_input);

		// Clamp the velocity
		float falling_clamp = gliding ? glide_max_velo : max_velo;
		rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -falling_clamp, jump_velo + 0.1f));

		// Transform flip
		if (x_input > 0) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180f, transform.rotation.z));
		} else if (x_input < 0) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
		}

		// Camera tracking
		Transform cam = Camera.main.transform;
		Vector2 displacement = transform.position - cam.position;
		Camera.main.transform.Translate(displacement * Time.deltaTime * cam_adjust_time);
	}

	// Adjust the bird's gravity scale based on y-input, to glide (if you're falling)
	private void check_glide(float y_input) {
		if (move_Stage == move_stage.flutter) {
			gliding = false;
			return;
		}

		//if (rb.velocity.y < 0 && y_input > 0) {
		if (y_input > 0) {
			rb.gravityScale = glide_grav_scale;
			gliding = rb.velocity.y < 0;
		} else {
			gliding = false;
			rb.gravityScale = base_grav_scale;
		}
	}
}

// Movement stages enum
public enum move_stage { flutter, glide, jump, double_jump, fly }
