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
	private Animator m_PromptAnimator;

	[Tooltip ("")]
	[SerializeField]
	private Animator m_cachedFaderAnimator;

	[Tooltip ("")]
	[SerializeField]
	private Text m_promptText;

	#endregion

	#region Private Member Variables

	private static Interactable CurrentTarget;

	private string lastPromptPlayed;

	#endregion

	#region Monobehaviours

	#endregion

	void Update ()
	{
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
				if (Input.GetButtonDown ("Fire1")) {
					Debug.Log ("I hit my target");
				}
			}
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
		if (coll.tag == "Interactables") {
			Interactable interact = coll.GetComponentInChildren<Interactable> ();
		}
	}

	void OnTriggerExit (Collider coll)
	{
		if (coll.tag == "Interactables") {
			Interactable interact = coll.GetComponentInChildren<Interactable> ();
			m_promptText.text = "";
			m_PromptAnimator.SetBool ("HasPrompt", false);
		}
	}


}
