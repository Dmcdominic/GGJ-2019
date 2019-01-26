using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_thought : MonoBehaviour {

	public bool_var showing_var;

	// Init var to false
	private void Awake() {
		showing_var.val = false;
	}

	// Update visibility based on showing_var
	void Update() {
		foreach (Transform child in transform) {
			child.gameObject.SetActive(showing_var.val);
		}
	}
}
