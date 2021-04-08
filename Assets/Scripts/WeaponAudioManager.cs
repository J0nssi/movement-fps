using System;
using UnityEngine.Audio;
using UnityEngine;

public class WeaponAudioManager : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float stereoPan = 0f;
    [Range(0f, 1f)]
    public float spatialBlend = 1f;
    [Range(1f, 500f)]
    public float minDistance = 1f;
    [Range(1f, 500f)]
    public float maxDistance = 500f;


    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in sounds)
		{
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.panStereo = stereoPan;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.spatialBlend = spatialBlend;
            s.source.minDistance = minDistance;
            s.source.maxDistance = maxDistance;

		}
    }

    public void Play(string name)
	{
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
		{
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
		}
		
        s.source.Play();
	}
}
