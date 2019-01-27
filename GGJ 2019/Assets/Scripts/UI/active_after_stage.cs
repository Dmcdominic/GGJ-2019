using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class active_after_stage : MonoBehaviour {

	public move_stage activation_stage;
	public GameObject to_toggle_active;

	// Update is called once per frame
	void Update() {
		if (movement.move_Stage >= activation_stage) {
			to_toggle_active.SetActive(true);
		} else {
			to_toggle_active.SetActive(false);
		}
	}
}
