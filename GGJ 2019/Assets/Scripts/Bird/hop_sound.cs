using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hop_sound : MonoBehaviour {
	public void play_hop_sound() {
		SoundManager.instance.playHopSound();
	}
}
