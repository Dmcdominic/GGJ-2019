using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public float updraft_accel;

	public float cam_adjust_time;

	public GameObject visuals;
	public GameObject feet_pos;

	public bool_var gliding_var;
	public bool_var updrafting_var;

	// Static settings
	public static move_stage move_Stage = move_stage.glide; // Start this at flutter, if we have time
	public static bool gliding = false;
	public static bool movement_enabled = true;
	public static int current_scene = 1;
	public static movement bird_instance;

	// Private vars
	private readonly string horizontal_axis = "Horizontal";
	private readonly string vertical_axis = "Vertical";

	private float base_grav_scale;
	private float flutter_timer;

	private bool jump_held = false;
	private int jump_charges = 0;

	private int landing_layer_mask;

	private bool updrafting = false;

	// Component references
	private Rigidbody2D rb;
	private Collider2D col;
	private Animator animator;


	// Init
	void Awake() {
		if (bird_instance == null) {
			bird_instance = this;
		} else if (bird_instance != this) {
			Destroy(gameObject);
			return;
		}

		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();

		base_grav_scale = rb.gravityScale;
		landing_layer_mask = LayerMask.GetMask(new string[] { "platform" });
		move_Stage = init_stage;
		gliding = false;
		gliding_var.val = false;
	}

	// Called once per frame
	void FixedUpdate() {
		// Reset the current_scene
		current_scene = SceneManager.GetActiveScene().buildIndex;

		if (!movement_enabled) {
			camera_track();
			rb.velocity = Vector3.zero;
			return;
		}

		// Get input
		float x_input = Input.GetAxis(horizontal_axis);
		float x_input_raw = Input.GetAxisRaw(horizontal_axis);
		float y_input_raw = Input.GetAxisRaw(vertical_axis);

		// Horizontal movement
		transform.Translate(Mathf.Abs(x_input) * (-1f) * x_mult * Time.deltaTime, 0, 0);
		//rb.velocity += new Vector2(x_input * x_mult * Time.deltaTime, 0);

		// Check if you've landed, to reset jump charges
		RaycastHit2D raycast = Physics2D.Raycast(feet_pos.transform.position, Vector2.down, raycast_dist, landing_layer_mask);
		if (raycast.collider != null && rb.velocity.y <= 0) {
			if (move_Stage == move_stage.jump) {
				jump_charges = 1;
			} else if (move_Stage == move_stage.double_jump) {
				//jump_charges = 2;
				jump_charges = 1; // todo - make lvl3 w/ double jump?
			}
		}

		// Vertical controls
		switch (move_Stage) {
			case move_stage.flutter:
				if (flutter_timer > 0) {
					flutter_timer -= Time.deltaTime;
				} else if (y_input_raw > 0 && rb.velocity.y < flutter_thresh) {
					rb.AddForce(new Vector2(0, flutter_force));
					flutter_timer = flutter_delay;
				}
				break;
			case move_stage.glide:
				break;
			case move_stage.jump:
			// Fall through to double_jump, which has same behavior
			case move_stage.double_jump:
				if (y_input_raw > 0 && !jump_held && jump_charges > 0) {
					jump();
				}
				break;
			case move_stage.fly:
				if (y_input_raw > 0 && !jump_held) {
					jump();
				}
				break;
		}

		if (y_input_raw <= 0) {
			jump_held = false;
		} else {
			jump_held = true;
		}

		// Fall through platforms if you are holding down
		//col.enabled = y_input >= 0;

		// Updraft check
		if (col.IsTouchingLayers(LayerMask.GetMask(new string[] { "updraft" }))) {
			rb.velocity += new Vector2(0, updraft_accel * Time.deltaTime);
			updrafting = true;
			updrafting_var.val = true;
		} else {
			updrafting = false;
			updrafting_var.val = false;
		}

		// Glide if you're falling, and holding up
		check_glide(y_input_raw);
		gliding_var.val = gliding;
		animator.SetBool("glide", gliding);

		// Clamp the velocity
		float falling_clamp = gliding ? glide_max_velo : max_velo;
		float max_y_velo = updrafting ? jump_velo * 1.8f : float.MaxValue;
		float new_x_velo = (gliding && rb.velocity.y < 0.5f && x_input == 0) ? rb.velocity.x / 2f : rb.velocity.x;
		rb.velocity = new Vector2(new_x_velo, Mathf.Clamp(rb.velocity.y, -falling_clamp, max_y_velo));

		// Check if you should be hopping
		if (Mathf.Abs(x_input_raw) > 0 && Mathf.Abs(rb.velocity.y) < 0.05f) {
			animator.SetBool("hopping", true);
		} else {
			animator.SetBool("hopping", false);
		}

		// Check if you should be flying
		if (rb.velocity.y > 0.5f && !updrafting && !gliding) {
			animator.SetBool("flying", true);
		} else {
			animator.SetBool("flying", false);
		}

		// Check if you should be falling
		if (rb.velocity.y < 0 && !gliding) {
			animator.SetBool("falling", true);
		} else {
			animator.SetBool("falling", false);
		}

		// Transform flip
		if (x_input > 0) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180f, transform.rotation.z));
		} else if (x_input < 0) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
		}

		// Camera tracking
		camera_track();
	}

	private void camera_track() {
		Transform cam = Camera.main.transform;
		Vector2 displacement = transform.position - cam.position;
		Camera.main.transform.Translate(displacement * Time.deltaTime * cam_adjust_time);
	}

	public void set_movement_enabled(bool enabled) {
		movement_enabled = enabled;
		rb.gravityScale = enabled ? base_grav_scale : 0;
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "bouncy leaf" && rb.velocity.y < 0) {
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Pow(Mathf.Abs(rb.velocity.y), 2.5f) / 10f);
		}
	}

	// Adjust the bird's gravity scale based on y-input, to glide (if you're falling)
	private void check_glide(float y_input) {
		if (updrafting) {
			rb.gravityScale = glide_grav_scale;
			gliding = true;
			return;
		}

		if (move_Stage == move_stage.flutter) {
			gliding = false;
			gliding_var.val = false;
			rb.gravityScale = base_grav_scale;
			return;
		}

		//if (rb.velocity.y < 0 && y_input > 0) {
		if (y_input > 0 && rb.velocity.y <= jump_velo) {
			rb.gravityScale = glide_grav_scale;
			gliding = rb.velocity.y < 0;
		} else {
			gliding = false;
			rb.gravityScale = base_grav_scale;
		}
	}

	// Jump!
	private void jump() {
		rb.velocity = new Vector2(rb.velocity.x, jump_velo);
		SoundManager.instance.playFlap();
		jump_charges--;
		// todo - flap animation
	}
}

// Movement stages enum
public enum move_stage { flutter, glide, jump, double_jump, fly }
