using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public float voice_volume = 1.0f;
    public float sound_volume = 1.0f;
    public float music_volume = 1.0f;
    // Audio Source
    public AudioSource audioSource;

    // sound clips
    public AudioClip[] clips;

    /*
    // sound effects
    public AudioClip fireSound;
    public AudioClip waterSound;
    public AudioClip earthSound;
    public AudioClip airSound;
    public AudioClip darkSound;
    public AudioClip energySound;

    public AudioClip checkpointReached;
    public AudioClip spellReady1;
    public AudioClip spellReady2;

    // voices
    public AudioClip introSpeech;
    public AudioClip chooseCarefully;
    public AudioClip claimSpellbooks;
    public AudioClip openSpellbooks;
    public AudioClip sanctify;
    public AudioClip followDirections;
    public AudioClip exhausted;

    public AudioClip evilLaugh1;
    public AudioClip evilLaugh2;
    public AudioClip evilLaugh3;
    public AudioClip evilLaugh4;
    public AudioClip spellSucceed1;
    public AudioClip spellSucceed2;
    public AudioClip spellSucceed3;
    public AudioClip spellSucceed4;
    public AudioClip spellFail1;
    public AudioClip spellFail2;
    public AudioClip spellFail3;
    public AudioClip encourage1;
    public AudioClip encourage2;
    public AudioClip encourage3;*/

    // Background Music
    public AudioClip introMusic;
    public AudioClip ritualMusic;


    void Awake () {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {

    }

    // play a clip by name, one time
    public void playSoundEffect(string clipToPlay)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.name == clipToPlay)
            {
                audioSource.volume = sound_volume;
                audioSource.PlayOneShot(clip);
            }
        }
    }
    
    /*
    public void playSoundEffect(string e)
    {
        Debug.Log("called");
        switch (e)
        {
            case "F": case "Fire": case "Fire Hat": case "Fire Cloak":
                audioSource.PlayOneShot(fireSound);
                break;
            case "W": case "Water": case "Water Hat": case "Water Cloak":
                audioSource.PlayOneShot(waterSound);
                break;
            case "E": case "Earth": case "Earth Hat": case "Earth Cloak":
                audioSource.PlayOneShot(earthSound);
                break;
            case "A": case "Air": case "Air Hat": case "Air Cloak":
                audioSource.PlayOneShot(airSound);                
                break;
            case "D": case "Dark": case "Dark Hat": case "Dark Cloak":
                audioSource.PlayOneShot(darkSound);
                break;
            case "N": case "N-ergy": case "N-ergy Hat": case "N-ergy Cloak":
                audioSource.PlayOneShot(energySound);
                break;
            case "checkPoint":
                audioSource.PlayOneShot(checkpointReached); 
                break;
            case "spellReady1":
                audioSource.PlayOneShot(spellReady1);
                break;
            case "spellReady2":
                audioSource.PlayOneShot(spellReady2);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            case "introSpeech":
                audioSource.PlayOneShot(introSpeech);
                break;
            default:
                break;
        }

    }*/


    // background music control, choose music clip, play, pause, and stop
    public void playMusic(string m)
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.loop = true;
            switch (m)
            {
                case "intro":
                    audioSource.clip = introMusic;
                    break;
                case "ritual":
                    audioSource.clip = ritualMusic;
                    break;
                default:
                    break;

            }
            audioSource.volume = music_volume;
            audioSource.Play();
        }
            
    }

    public void pauseMusic(string m)
    {
        switch (m)
        {
            case "intro":
                audioSource.clip = introMusic;
                break;
            case "ritual":
                audioSource.clip = ritualMusic;
                break;
            default:
                break;

            }
        if (audioSource.isPlaying == false)
        {
            audioSource.Pause();
        }     

    }

    public void stopMusic(string m)
    {
        switch (m)
        {
            case "intro":
                audioSource.clip = introMusic;
                break;
            case "ritual":
                audioSource.clip = ritualMusic;
                break;
            default:
                break;
        }
        if(audioSource.isPlaying)
            audioSource.Stop();
    }


    // Update is called once per frame
    void Update () {
		
	}
}
