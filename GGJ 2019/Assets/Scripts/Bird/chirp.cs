using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chirp : MonoBehaviour {

	public void play_small_chirp() {
		SoundManager.instance.playSmallChirp();
	}

}
