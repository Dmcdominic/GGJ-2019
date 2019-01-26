using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource flap;
    public AudioSource chirp;
    public static SoundManager instance = null;
    public float lowPitchRange = .95f;             
    public float highPitchRange = 1.05f;           


    // Start is called before the first frame update
    void Awake ()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

        StartCoroutine("repeat_chrip");
    }

    IEnumerator repeat_chrip()
    {
        while (true)
        {
            chirp.Play();
            yield return new WaitForSeconds(20);
        }
    }


    public void playSingle (AudioClip clip)
    {
        flap.clip = clip;
        flap.Play();
    }


}
