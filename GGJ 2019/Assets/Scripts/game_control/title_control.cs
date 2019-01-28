using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class title_control : MonoBehaviour {

	public int scene_index;

	private bool animating = false;


	private void Start() {
		movement.full_init();
	}

	private void Update() {
		if (!animating && Input.anyKeyDown && !(Input.GetAxisRaw("Cancel") > 0)) {
			start_anim();
		}
	}

	private void start_anim() {
		animating = true;
		GetComponent<Animator>().SetTrigger("egg cracks");
		StartCoroutine(load_scene());
	}

	IEnumerator load_scene() {
		yield return new WaitForSeconds(8f);
		black_fade.black_fade_to_scene(scene_index);
	}
}
