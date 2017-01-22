
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

	public void FocusCamera ()
	{
		Debug.Log ("FocusPlayer");
		m_FocusCamera.enabled = true;
		m_FocusDurationRemaining = 1f;
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
			AudioSource soundSource = gameObject.AddComponent<AudioSource>();
			soundSource.clip = audio;
			soundSource.Play();
			m_AudioSources = GetComponents<AudioSource>();
		}
	}
	#endregion
}
