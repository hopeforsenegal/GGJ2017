
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

[DisallowMultipleComponent]
public class FocusPlayer : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	#endregion

	#region Inspectables

	#endregion

	#region Private Member Variables

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
	}

	protected void Start ()
	{
	}

	protected void Update ()
	{
	}

	protected void OnEnable ()
	{
	}

	protected void OnDisable ()
	{
	}

	#endregion

	#region Trigger Collider

	protected void OnTriggerEnter (Collider coll)
	{
		FocusCamera fcamera = coll.gameObject.GetComponentInChildren<FocusCamera> ();
		if (fcamera != null) {
			Debug.Log ("FocusPlayer");
			fcamera.enabled = true;
		}
		FirstPersonController player = coll.gameObject.GetComponentInChildren<FirstPersonController> ();
		if (player != null) {
			Debug.Log ("FocusPlayer");
			player.enabled = false;
		}
	}

	protected void OnTriggerExit (Collider coll)
	{
		FocusCamera fcamera = coll.gameObject.GetComponentInChildren<FocusCamera> ();
		if (fcamera != null) {
			Debug.Log ("UnFocusPlayer");
			fcamera.enabled = false;
		}
		FirstPersonController player = coll.gameObject.GetComponentInChildren<FirstPersonController> ();
		if (player != null) {
			Debug.Log ("UnFocusPlayer");
			player.enabled = true;
			player.UpdateView (fcamera.Target);
		}
	}

	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
