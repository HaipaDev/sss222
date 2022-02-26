using UnityEngine.Audio;
using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class AudioManager : MonoBehaviour{	public static AudioManager instance;
	public AudioMixer audioMixer;
	public AudioMixerGroup mixerGroup;
	[AssetsOnly,Searchable]public Sound[] sounds;

	void Awake(){
		if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}

		foreach(Sound s in sounds){
			s.source=gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.playOnAwake=false;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound){
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null){
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.loop = s.loop;

		s.source.Play();
	}
	public void PlayOnce(string sound){
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null){
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		if(!s.source.isPlaying){
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
			s.source.loop = s.loop;

			s.source.Play();
		}
	}
	public void PlayOnceRPitch(string sound){
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null){
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.loop = s.loop;

		if(!s.source.isPlaying)s.source.Play();
		
	}
	public void PlayFromInstance(string sound){AudioManager.instance.Play(sound);}

	public void StopPlaying(string sound){
		Sound s = Array.Find(sounds, item => item.name == sound);
		if(s == null){
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
  		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Stop ();
 	}
	public AudioClip Get(string sound){
		Sound s = Array.Find(sounds, item => item.name == sound);
		if(s == null){
			Debug.LogWarning("Sound: " + sound + " not found!");
			return null;
		}else{return s.clip;}
	}
	public AudioSource GetSource(string sound){
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null){
			Debug.LogWarning("Sound: " + sound + " not found!");
			return null;
		}else{return s.source;}
	}
	void Update(){
		audioMixer.SetFloat("MasterVolume", SaveSerial.instance.settingsData.masterVolume);
		audioMixer.SetFloat("SoundVolume", SaveSerial.instance.settingsData.soundVolume);
		audioMixer.SetFloat("MusicVolume", SaveSerial.instance.settingsData.musicVolume);
	}
}

[System.Serializable]
public class Sound {
	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false;

	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;

}