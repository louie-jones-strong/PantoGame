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

#region Volumes
#region MusicVolume
	const float DefaultMusicVolume = 0.8f;
	const string MusicPlayerPrefKey = "MusicVol";
	static float _MusicVolume;
	public static float MusicVolume {get {return _MusicVolume;} set {SetMusicVolume(value);}}
	public static void SetMusicVolume(float value)
	{
		if (Instance == null)
		{
			Logger.LogError($"Trying to set Music Volume but Instance == null");
			return;
		}
		Logger.Log($"Setting Music Volume to {value}");
		_MusicVolume = value;
		Instance.Mixer.SetFloat("MusicVol", VolumeToDb(value));
		PlayerPrefs.SetFloat(MusicPlayerPrefKey, value);
	}
#endregion

#region SfxVolume
	const float DefaultSfxVolume = 0.8f;
	const string SfxPlayerPrefKey = "SfxVol";
	static float _SfxVolume;
	public static float SfxVolume {get {return _SfxVolume;} set {SetSfxVolume(value);}}
	public static void SetSfxVolume(float value)
	{
		if (Instance == null)
		{
			Logger.LogError($"Trying to set Sfx Volume but Instance == null");
			return;
		}
		Logger.Log($"Setting Sfx Volume to {value}");
		_SfxVolume = value;
		Instance.Mixer.SetFloat("SfxVol", VolumeToDb(value));
		PlayerPrefs.SetFloat(SfxPlayerPrefKey, value);
	}
#endregion

#region AmbienceVolume
	const float DefaultAmbienceVolume = 0.8f;
	const string AmbiencePlayerPrefKey = "AmbienceVol";
	static float _AmbienceVolume;
	public static float AmbienceVolume {get {return _AmbienceVolume;} set {SetAmbienceVolume(value);}}
	public static void SetAmbienceVolume(float value)
	{
		if (Instance == null)
		{
			Logger.LogError($"Trying to set Ambience Volume but Instance == null");
			return;
		}
		Logger.Log($"Setting Ambience Volume to {value}");
		_AmbienceVolume = value;
		Instance.Mixer.SetFloat("AmbienceVol", VolumeToDb(value));
		PlayerPrefs.SetFloat(AmbiencePlayerPrefKey, value);
	}
#endregion
	public static float VolumeToDb(float volume)
	{
		if (volume != 0)
			return Mathf.Log10(volume) * 20;
		else
			return -144.0f;
	}
#endregion

	void Awake()
	{
		if (Instance != null)
		{
			return;
		}

		Instance = this;
		LoadSounds();
	}

	void Start()
	{
		MusicVolume = PlayerPrefs.GetFloat(MusicPlayerPrefKey, DefaultMusicVolume);
		SfxVolume = PlayerPrefs.GetFloat(SfxPlayerPrefKey, DefaultSfxVolume);
		AmbienceVolume = PlayerPrefs.GetFloat(AmbiencePlayerPrefKey, DefaultAmbienceVolume);
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
		if (Instance == null)
		{
			Logger.LogError($"Cannot check if audio path valid as instance == null");
			return null;
		}
		
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
		// foreach (var item in sources)
		// {
		// 	if (!item.enabled && !item.isPlaying && item.clip == null)
		// 	{
		// 		Logger.Log("Found free source");
		// 		source = item;
		// 		break;
		// 	}
		// }

		if (source == null)
		{
			Logger.Log("didn't find free source so making one");
			source = root.gameObject.AddComponent<AudioSource>();
		}
		source.enabled = true;
		
		Instance.CurrentSources.Add(source);

		var clipAndVolume = sound.GetAudioClip();
		source.clip = clipAndVolume.Clip;
		source.volume = clipAndVolume.Volume;
		source.outputAudioMixerGroup = Instance.GetGetAudioBus(sound.AudioBus);
		source.playOnAwake = false;
		source.loop = sound.LoopClip;
		source.Play();

		Logger.Log($"Playing Audio event: \"{path}\" on source: {source.GetInstanceID()} on gameobject: {source}");

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
				source.clip = null;
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
		if (Instance == null)
		{
			Logger.LogError($"Cannot check if audio path valid as instance == null");
			return false;
		}
		return Instance.Sounds.ContainsKey(path);
	}
}