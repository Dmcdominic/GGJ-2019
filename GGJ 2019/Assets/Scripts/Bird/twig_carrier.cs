using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twig_carrier : MonoBehaviour {

	public GameObject twig_in_beak;

	private bool carrying_twig = false;
	private Animator animator;
	private GameObject to_destroy;

	// Init
	void Start() {
		animator = GetComponent<Animator>();
		twig_in_beak.SetActive(false);
	}

	// Check if you can pick up a twig
	private void OnCollisionEnter2D(Collision2D collision) {
		if (!carrying_twig && collision.gameObject.tag == "twig") {
			pickup_twig(collision.gameObject);
		}
		if (collision.gameObject.tag == "big bug") {
			eat_big_bug(collision.gameObject);
		}
	}

	// Check if you can place a twig in the nest
	private void OnTriggerEnter2D(Collider2D collision) {
		if (carrying_twig && collision.gameObject.tag == "nest") {
			place_twig(collision.gameObject);
		}
	}

	// Pick up a twig off the ground
	private void pickup_twig(GameObject the_twig) {
		to_destroy = the_twig;
		StartCoroutine(ensure_destroyed(the_twig));
		carrying_twig = true;
		StartCoroutine(ensure_twig_revealed());
		SoundManager.instance.playPickupTwig();
		animator.SetTrigger("peck");
	}

	// Destroy the twig from the ground
	public void delete_twig() {
		if (to_destroy != null) {
			Destroy(to_destroy);
		}
	}

	// Reveal the twig in the beak
	public void reveal_twig_in_beak() {
		if (carrying_twig) {
			twig_in_beak.SetActive(true);
		}
	}

	// Add a twig that you're carrying to the nest
	private void place_twig(GameObject nest) {
		carrying_twig = false;
		twig_in_beak.SetActive(false);
		nest.GetComponent<nest>().place_twig();
		animator.SetTrigger("peck");
	}

	// Eat the big bug to unlock jumping
	private void eat_big_bug(GameObject big_bug) {
		to_destroy = big_bug;
		StartCoroutine(ensure_destroyed(big_bug));
		twig_in_beak.SetActive(false);
		StartCoroutine(ensure_twig_revealed());
		SoundManager.instance.playEat();
		animator.SetTrigger("peck");
		movement.move_Stage = move_stage.jump;
	}

	// Start this coroutine to make sure an object gets destroyed
	IEnumerator ensure_destroyed(GameObject obj) {
		yield return new WaitForSeconds(0.4f);
		if (obj != null) {
			Destroy(obj);
		}
	}

	// Start this coroutine to make sure the twig in your beak shows up
	IEnumerator ensure_twig_revealed() {
		yield return new WaitForSeconds(0.6f);
		if (carrying_twig) {
			twig_in_beak.SetActive(true);
		}
	}
}
