using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

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

	#endregion

	#region Monobehaviours

	#endregion

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
	}

	void Update ()
	{
		if (Input.anyKeyDown && m_InfoPanel.IsShowing) {
			m_InfoPanel.Hide ();
			return;
		}
		if (Input.GetButtonDown ("Fire1")) {
			Debug.Log ("im firing my stuff");
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
			if (CurrentTarget != null) {
				CurrentTarget.Highlight ();
				if (!string.IsNullOrEmpty (CurrentTarget.PromptText)) {
					m_PromptText.enabled = true;
					m_PromptText.text = CurrentTarget.PromptText;
				}
				if (Input.GetButtonDown ("Fire1")) {
					Debug.Log ("I hit my target");
					m_InfoPanel.Show (CurrentTarget.TextToDisplay);
				}
			} else {
				m_PromptText.enabled = false;
			}
		} else {
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
