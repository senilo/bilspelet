using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {
    public static Sounds instance;
    public AudioClip[] hem;
    public AudioClip[] dagis;
    public AudioClip[] stugan;
    public AudioClip[] jobb;
    public AudioClip[] mammasJobb;
    public AudioClip[] affären;
    public AudioClip[] bra;
    public AudioClip[] nej;
    public Dictionary<BuildingType, AudioClip[]> destinationSounds;


    public void PlayRandom(AudioClip[] sounds, float delay = 0f)
    {
        Debug.Assert(sounds.Length > 0);

        var sound = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        if(delay == 0f)
        {
            //AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.Play();
        } else
        {
            StartCoroutine(PlayDelayed(delay, sound));
        }
        //AudioSource.PlayClipAtPoint(sound, position);
        //WaitForSeconds
    }

    public void PlayRandom(BuildingType building, float delay = 0f)
    {
        AudioClip[] sounds;
        if(destinationSounds.TryGetValue(building, out sounds))
        {
            PlayRandom(sounds, delay);
        } else
        {
            Debug.LogWarning("No sounds for building type");
        }
    }

    IEnumerator PlayDelayed(float delay, AudioClip clip)
    {
        yield return new WaitForSeconds(delay);
        //AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Awake()
    {
        instance = this;
        destinationSounds = new Dictionary<BuildingType, AudioClip[]> { { BuildingType.Affär, affären },
            { BuildingType.Dagis, dagis },
            { BuildingType.Hem, hem },
            { BuildingType.Jobb, jobb },
            { BuildingType.Mammas_jobb, mammasJobb },
            { BuildingType.Stugan, stugan}};
    }
}
