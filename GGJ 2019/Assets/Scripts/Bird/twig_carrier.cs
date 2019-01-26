using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twig_carrier : MonoBehaviour {

	public GameObject twig_in_beak;

	private bool carrying_twig = false;

	// Init
	void Start() {
		twig_in_beak.SetActive(false);
	}

	// Check if you can pick up a twig
	private void OnCollisionEnter2D(Collision2D collision) {
		if (!carrying_twig && collision.gameObject.tag == "twig") {
			pickup_twig(collision.gameObject);
		}
	}

	// Check if you can place a twig in the nest
	private void OnTriggerEnter2D(Collider2D collision) {
		if (carrying_twig && collision.gameObject.tag == "nest") {
			place_twig(collision.gameObject);
		}
	}

	// Pick up a twig off the ground
	private void pickup_twig(GameObject twig) {
		carrying_twig = true;
		Destroy(twig);
		twig_in_beak.SetActive(true);
		SoundManager.instance.playPickupTwig();
	}

	// Add a twig that you're carrying to the nest
	private void place_twig(GameObject nest) {
		carrying_twig = false;
		twig_in_beak.SetActive(false);
		nest.GetComponent<nest>().place_twig();
	}
}
