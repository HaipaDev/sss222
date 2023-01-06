using UnityEngine.Audio;
using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class AudioManager : MonoBehaviour{	public static AudioManager instance;
	public AudioMixer audioMixer;
	public AudioMixerGroup mixerGroup;
	[AssetsOnly,Searchable]public Sound[] sounds;

	void Awake(){
		if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}

		foreach(Sound s in sounds){
			s.source=gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.playOnAwake=false;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}
	void Update(){
		if(SaveSerial.instance!=null){
			var ss=SaveSerial.instance.settingsData;
			float _currentMasterVolume;audioMixer.GetFloat("MasterVolume",out _currentMasterVolume);
			if(Application.isFocused){
				var _minVolume=-50;
				if(ss.masterVolume>0){_minVolume=-50;}else{_minVolume=-80;}
				if(_currentMasterVolume==-80&&_minVolume==-50){_currentMasterVolume=-50;}
				if(_currentMasterVolume>AssetsManager.InvertNormalizedMin(ss.masterVolume,_minVolume)+0.5f){audioMixer.SetFloat("MasterVolume", _currentMasterVolume-0.5f);}
				else if(_currentMasterVolume<AssetsManager.InvertNormalizedMin(ss.masterVolume,_minVolume)-0.5f){audioMixer.SetFloat("MasterVolume", _currentMasterVolume+0.5f);}
				else {audioMixer.SetFloat("MasterVolume", AssetsManager.InvertNormalizedMin(ss.masterVolume,_minVolume));}
			}else{
				var _minVolume=-50;
				if(ss.masterOOFVolume>0){_minVolume=-50;}else{_minVolume=-80;}
				//if(_currentMasterVolume==-80&&_minVolume==-50){_currentMasterVolume=-50;}
				if(_currentMasterVolume>AssetsManager.InvertNormalizedMin(ss.masterOOFVolume,_minVolume)+0.5f){audioMixer.SetFloat("MasterVolume", _currentMasterVolume-0.5f);}
				else if(_currentMasterVolume<AssetsManager.InvertNormalizedMin(ss.masterOOFVolume,_minVolume)-0.5f){audioMixer.SetFloat("MasterVolume", _currentMasterVolume+0.5f);}
				else {audioMixer.SetFloat("MasterVolume", AssetsManager.InvertNormalizedMin(ss.masterOOFVolume,_minVolume));}
			}
			if(ss.soundVolume>0){audioMixer.SetFloat("SoundVolume", AssetsManager.InvertNormalizedMin(ss.soundVolume,-50));}
			else{audioMixer.SetFloat("SoundVolume", -80);}
			if(ss.ambienceVolume>0){audioMixer.SetFloat("AmbienceVolume", AssetsManager.InvertNormalizedMin(ss.ambienceVolume,-50));}
			else{audioMixer.SetFloat("AmbienceVolume", -80);}
			if(ss.musicVolume>0){audioMixer.SetFloat("MusicVolume", AssetsManager.InvertNormalizedMin(ss.musicVolume,-50));}
			else{audioMixer.SetFloat("MusicVolume", -80);}
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