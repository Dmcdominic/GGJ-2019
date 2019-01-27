using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class zone_connector : MonoBehaviour {

	public int connected_zone;
	public GameObject local_spawn;


	void Start() {
		if (movement.current_scene == connected_zone) {
			Vector2 pos = local_spawn.transform.position;
			movement.bird_instance.transform.position = local_spawn.transform.position;
			Camera.main.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "bird") {
			black_fade.black_fade_to_scene(connected_zone);
		}
	}
}
