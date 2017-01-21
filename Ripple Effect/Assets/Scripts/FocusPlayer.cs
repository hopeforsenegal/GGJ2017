
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
		FocusCamera fcamera = coll.gameObject.GetComponentInChildren<FocusCamera> ();
		if (fcamera != null) {
			fcamera.enabled = true;
		}
	}

	protected void OnTriggerExit (Collider coll)
	{
		FocusCamera fcamera = coll.gameObject.GetComponentInChildren<FocusCamera> ();
		if (fcamera != null) {
			fcamera.enabled = false;
		}
	}

	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
