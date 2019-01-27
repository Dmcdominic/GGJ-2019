using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prompt : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "bird") {
			GetComponentInChildren<Animator>().SetBool("revealed", true);
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "bird") {
			GetComponentInChildren<Animator>().SetBool("revealed", false);
		}
	}
}
