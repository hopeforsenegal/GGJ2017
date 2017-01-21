
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
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

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
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
	}

	protected void Update ()
	{
		m_FocusDurationRemaining -= Time.deltaTime;

		if (m_FocusDurationRemaining <= 0f) {
			m_FocusDurationRemaining = 0f;
		}

		if (m_FocusCamera.enabled && Mathf.Approximately (m_FocusDurationRemaining, 0f)) {
			UnFocusCamera ();
		}
	}

	protected void OnEnable ()
	{
	}

	protected void OnDisable ()
	{
	}

	#endregion

	#region Public Methods

	public void FocusCamera ()
	{
		Debug.Log ("FocusPlayer");
		m_FocusCamera.enabled = true;
		m_FocusDurationRemaining = 5f;
		KillControls ();
	}

	public void UnFocusCamera ()
	{
		Debug.Log ("UnFocusPlayer");
		m_FocusCamera.enabled = false;
		m_FirstPersonController.enabled = true;
		EnableControls ();
	}

	public void KillControls()
	{
		m_FirstPersonController.enabled = false;
	}

	public void EnableControls()
	{
		m_FirstPersonController.enabled = true;
	}

	#endregion

	#region Private Methods

	#endregion
}
