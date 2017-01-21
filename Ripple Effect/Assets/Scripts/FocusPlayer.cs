
using UnityEngine;
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
		FocusCamera player = coll.gameObject.GetComponent<FocusCamera> ();
		if (player != null) {
			FocusCamera fcamera = player.GetComponent<FocusCamera> ();
			fcamera.enabled = true;
		}
	}

	protected void OnTriggerExit (Collider coll)
	{
		FocusCamera player = coll.gameObject.GetComponent<FocusCamera> ();
		if (player != null) {
			FocusCamera fcamera = player.GetComponent<FocusCamera> ();
			fcamera.enabled = false;
		}
	}

	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
