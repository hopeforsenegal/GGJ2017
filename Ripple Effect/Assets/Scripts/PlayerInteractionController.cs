using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(Player))]
public class PlayerInteractionController : MonoBehaviour
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
	private Camera m_PlayerCamera;

	[Tooltip ("")]
	[SerializeField]
	private LayerMask collisionMask;

	[Tooltip ("")]
	[SerializeField]
	private Text m_PromptText;

	[Tooltip ("")]
	[SerializeField]
	private InfoPanel m_InfoPanel;

	#endregion

	#region Private Member Variables

	private static Interactable CurrentTarget;

	private string lastPromptPlayed;
	private Player m_Player;

	#endregion

	#region Monobehaviours

	#endregion

	protected void Awake ()
	{
		m_Player = GetComponent<Player> ();

		Debug.Assert (m_Player != null);
	}

	protected void Start ()
	{
		if (m_PlayerCamera == null) {
			Debug.LogWarning ("m_PlayerCamera is null");
		}
		if (m_PromptText == null) {
			Debug.LogWarning ("m_PromptText is null");
		}
		if (m_InfoPanel == null) {
			Debug.LogWarning ("m_InfoPanel is null");
		}

		Interactable.DoInteractEvent += HandleOnDoInteractHandler;
	}

	private void HandleOnDoInteractHandler (Interactable i)
	{
		Debug.Log ("HandleOnDoInteractHandler");
		if (i.tag == Interactable.kInteractableTag && m_PromptText != null ) {
			m_PromptText.enabled = false;
			m_InfoPanel.Show (CurrentTarget.TextToShowsOnInteraction);
			m_Player.KillControls ();
		} else if (i.tag == Interactable.kDoorTag) {
			Debug.Log (i.tag);
			GameController gameController;
			if (GameController.TryGetInstance (out gameController) && gameController.FirstRoomDoor.GetComponent<Interactable> () == i && m_PromptText != null ) {
				m_PromptText.enabled = false;
				m_InfoPanel.Show (CurrentTarget.TextToShowsOnInteraction);
				m_Player.KillControls ();
			} else {
				i.GetComponentInParent<Animator> ().enabled = true;
				m_Player.WalkForward ();
			}
		}
	}

	private void OnDestroy ()
	{
		Interactable.DoInteractEvent -= HandleOnDoInteractHandler;
	}

	private void Update ()
	{
		if (Input.anyKeyDown && m_InfoPanel != null && m_InfoPanel.IsShowing) {
			m_InfoPanel.Hide ();
			m_Player.EnableControls ();
			return;
		}
		FireRayToInteractable ();
	}

	private void FireRayToInteractable ()
	{
		Transform cameraTransform = m_PlayerCamera.transform;
		Ray ray = new Ray (cameraTransform.position, cameraTransform.forward);
		RaycastHit hit;
		bool hitInteractable = Physics.Raycast (ray, out hit, 10.0f, collisionMask);

		if (hitInteractable) {
			CurrentTarget = hit.collider.GetComponentInParent<Interactable> ();
			if (CurrentTarget != null && CurrentTarget.enabled) {
				CurrentTarget.Highlight ();
				if (m_PromptText != null && !string.IsNullOrEmpty (CurrentTarget.PromptText) && m_InfoPanel != null && !m_InfoPanel.IsShowing) {
					m_PromptText.enabled = true;
					m_PromptText.text = CurrentTarget.PromptText;
				}
				if (Input.GetButtonDown ("Fire1")) {
					Debug.Log ("interact");
					CurrentTarget.DoInteract ();
				}
			} else if ( m_PromptText != null ){
				m_PromptText.enabled = false;
			}
		} else if ( m_PromptText != null ) {
			m_PromptText.enabled = false;
		}
	}

	public static void AddSceneAdditive ()
	{
		if (CurrentTarget != null) {
			CurrentTarget = null;
		}
	}

	void OnTriggerEnter (Collider coll)
	{
		if (coll.tag == Interactable.kInteractableTag) {
			Interactable interact = coll.GetComponentInChildren<Interactable> ();
		}
	}

	void OnTriggerExit (Collider coll)
	{
		if (coll.tag == Interactable.kInteractableTag) {
			Interactable interact = coll.GetComponentInChildren<Interactable> ();
			m_PromptText.text = string.Empty;
		}
	}
}
