using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
	[SerializeField] AudioMixer Mixer;
	[SerializeField] AudioMixerGroup  MusicMixerGroup;
	[SerializeField] AudioMixerGroup  SfxMixerGroup;
	[SerializeField] AudioMixerGroup  AmbienceMixerGroup;
	public static AudioManger Instance;

	Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();
	List<AudioSource> CurrentSources = new List<AudioSource>();

	const float DefaultMusicVolume = 0.8f;
	static float _MusicVolume;
	public static float MusicVolume {get {return _MusicVolume;} set {SetMusicVolume(value);}}
	public static void SetMusicVolume(float value)
	{
		_MusicVolume = value;
		Instance.Mixer.SetFloat("MusicVol", VolumeToDb(value));
	}

	const float DefaultSfxVolume = 0.8f;
	static float _SfxVolume;
	public static float SfxVolume {get {return _SfxVolume;} set {SetSfxVolume(value);}}
	public static void SetSfxVolume(float value)
	{
		_SfxVolume = value;
		Instance.Mixer.SetFloat("SfxVol", VolumeToDb(value));
	}

	const float DefaultAmbienceVolume = 0.8f;
	static float _AmbienceVolume;
	public static float AmbienceVolume {get {return _AmbienceVolume;} set {SetAmbienceVolume(value);}}
	public static void SetAmbienceVolume(float value)
	{
		_AmbienceVolume = value;
		Instance.Mixer.SetFloat("AmbienceVol", VolumeToDb(value));
	}

	public static float VolumeToDb(float volume)
	{
		if (volume != 0)
			return Mathf.Log10(volume) * 20;
		else
			return -144.0f;
	}

	void Awake()
	{
		if (Instance != null)
		{
			return;
		}

		Instance = this;
		LoadSounds();

		MusicVolume = DefaultMusicVolume;
		SfxVolume = DefaultSfxVolume;
		AmbienceVolume = DefaultAmbienceVolume;
	}

	void LoadSounds()
	{
		var soundArray = Resources.LoadAll<Sound>("");
		
		foreach (var sound in soundArray)
		{
			Sounds[sound.Name] = sound;
		}
	}

	public static AudioSource PlayEvent(string path, Transform root=null)
	{
		if (!IsPathValid(path))
		{
			Logger.LogError($"PlayEvent called with path: \"{path}\" that not valid");
			return null;
		}

		var sound = Instance.Sounds[path];

		
		if (root == null)
		{
			root = Instance.transform;
		}
		
		var sources = root.gameObject.GetComponents<AudioSource>();
		AudioSource source = null;
		foreach (var item in sources)
		{
			if (!item.enabled)
			{
				source = item;
				break;
			}
		}

		if (source == null)
		{
			source = root.gameObject.AddComponent<AudioSource>();
		}
		source.enabled = true;
		
		Instance.CurrentSources.Add(source);

		source.clip = sound.GetAudioClip();
		source.outputAudioMixerGroup = Instance.GetGetAudioBus(sound.AudioBus);
		source.playOnAwake = false;
		source.Play();
		return source;
	}

	AudioMixerGroup GetGetAudioBus(eAudioBusType busType)
	{
		switch (busType)
		{
			case eAudioBusType.Music:
				return MusicMixerGroup;
			case eAudioBusType.Ambience:
				return AmbienceMixerGroup;
			default:
				return SfxMixerGroup;
		}
	}

	void Update()
	{
		var loop = 0;
		while (loop < CurrentSources.Count)
		{
			var source = CurrentSources[loop];
			if (!source.isPlaying)
			{
				source.enabled = false;
				CurrentSources.RemoveAt(loop);
			}
			else
			{
				loop += 1;
			}
		}
	}

	public static bool IsPathValid(string path)
	{
		return Instance.Sounds.ContainsKey(path);
	}
}