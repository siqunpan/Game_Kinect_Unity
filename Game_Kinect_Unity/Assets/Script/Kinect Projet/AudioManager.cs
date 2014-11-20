using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

	public static AudioClip AC_BACKGROUND;
	public AudioClip ac_background;

	public static AudioClip AC_WEEP;
	public AudioClip ac_weep;

	public static AudioClip AC_HIT;
	public AudioClip ac_hit;

	private static AudioManager _instance;
	public static AudioManager Instance
	{
		get
		{
			_instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
			if(_instance == null)
			{
				_instance = GameObject.Instantiate(GlobalVariables.GO_AUDIO_MANAGER) as AudioManager;
			}
			
			return _instance;
		}
	}

	void Start()
	{
		DontDestroyOnLoad (this.gameObject);

		AC_BACKGROUND = ac_background;
		AC_WEEP = ac_weep;
		AC_HIT = ac_hit;
	}

	public void playAudioBackGround ()
	{
		AudioSource.PlayClipAtPoint(AC_BACKGROUND, Vector3.zero);
	}

	public void playAudioWeep()
	{
		AudioSource.PlayClipAtPoint(AC_WEEP, Vector3.zero);
	}

	public void playAudioHit()
	{
		AudioSource.PlayClipAtPoint(AC_HIT, Vector3.zero);
	}
}
