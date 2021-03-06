﻿
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

[DisallowMultipleComponent]
[RequireComponent( typeof( AudioSource) )]
public class Player : MonoBehaviour
{
	#region Singleton

	public static Player Instance {
		get {
			return sInstance;
		}
	}

	public static bool TryGetInstance (out Player returnVal)
	{
		returnVal = sInstance;
		if (returnVal == null) {
			Debug.LogWarning (string.Format ("Couldn't access {0}", typeof(Player).Name));
		}
		return (returnVal != null);
	}

	private static Player sInstance;

	#endregion

	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	#endregion

	#region Inspectables

	[Tooltip ("")]
	[SerializeField]
	private CharacterController m_CharacterController;
	[Tooltip ("")]
	[SerializeField]
	private FirstPersonController m_FirstPersonController;
	[Tooltip ("")]
	[SerializeField]
	private FocusCamera m_FocusCamera;
	[Tooltip ("")]
	[SerializeField]
	private BlurCamera m_BlurCamera;

	#endregion

	#region Private Member Variables

	private float m_FocusDurationRemaining;
	private float m_WalkDurationRemaining;

	private AudioSource[] m_AudioSources;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		if (sInstance == null) {
			sInstance = this;
		}
	}

	protected void Start ()
	{
		if (m_CharacterController == null) {
			Debug.LogWarning ("m_CharacterController is null");
		}
		if (m_FirstPersonController == null) {
			Debug.LogWarning ("m_FirstPersonController is null");
		}
		if (m_FocusCamera == null) {
			Debug.LogWarning ("m_FocusCamera is null");
		}
		if (m_BlurCamera == null) {
			Debug.LogWarning ("m_BlurCamera is null");
		}

		m_AudioSources = GetComponents<AudioSource>();
	}

	protected void Update ()
	{
		m_FocusDurationRemaining -= Time.deltaTime;
		m_WalkDurationRemaining -= Time.deltaTime;

		if (m_FocusDurationRemaining <= 0f) {
			m_FocusDurationRemaining = 0f;
		}

		if (m_WalkDurationRemaining <= 0f) {
			m_WalkDurationRemaining = 0f;
			m_FirstPersonController.inputSwitcher = false;
		}

		if (m_FocusCamera.enabled && Mathf.Approximately (m_FocusDurationRemaining, 0f)) {
			UnFocusCamera ();
		}

		if (m_WalkDurationRemaining > 0f) {
			m_FirstPersonController.inputSwitcher = true;
		}
	}

	protected void OnEnable ()
	{
	}

	protected void OnDisable ()
	{
	}

	protected void OnDestroy ()
	{
		sInstance = null;
	}

	#endregion

	#region Public Methods

	public void BlurCamera()
	{
		m_BlurCamera.BlurVision ();
	}

	public void FocusCamera ()
	{
		Debug.Log ("FocusPlayer");
		m_FocusCamera.enabled = true;
		m_FocusDurationRemaining = 4f;
		KillControls ();
	}

	public void UnFocusCamera ()
	{
		Debug.Log ("UnFocusPlayer");
		m_FocusCamera.enabled = false;
		m_FirstPersonController.enabled = true;
		EnableControls ();
	}

	public void KillControls ()
	{
		m_FirstPersonController.enabled = false;
	}

	public void EnableControls ()
	{
		m_FirstPersonController.enabled = true;
	}

	public void WalkForward ()
	{
		Debug.Log ("WalkForward");
		StartCoroutine (WalkForwardFunc ());
	}

	private IEnumerator WalkForwardFunc ()
	{
		yield return new WaitForSeconds (1.5f);
		m_WalkDurationRemaining = 1f;
	}

	#endregion

	#region Private Methods

	public void PlaySound( AudioClip audio )
	{
		bool soundPlayed = false;
		foreach ( AudioSource source in m_AudioSources )
		{
			if (!source.isPlaying)
			{
				source.clip = audio;
				source.Play();
				soundPlayed = true;
				break;
			}
		}
		if (!soundPlayed)
		{
			if (m_AudioSources != null && m_AudioSources.Length > 0)
			{
				m_AudioSources[0].clip = audio;
				m_AudioSources[0].Play();
			}
		}
	}

	public void PlaySoundDelay( AudioClip[] audio, float delay )
	{
		int n = Random.Range(0, audio.Length);
		PlaySoundDelay (audio[n], delay);
	}

	public void PlaySoundDelay( AudioClip audio, float delay )
	{
		StartCoroutine (SoundPlaying (audio, delay));
	}

	private IEnumerator SoundPlaying(AudioClip audio, float delay)
	{
		yield return new WaitForSeconds (delay);
		PlaySound (audio);
	}
	#endregion
}
