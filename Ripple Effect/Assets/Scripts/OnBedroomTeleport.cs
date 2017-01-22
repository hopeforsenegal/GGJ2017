

using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class OnBedroomTeleport : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events
	
	public delegate void OnBedroomTeleportHandler ();

	public static event OnBedroomTeleportHandler BedroomTeleportEvent;

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

	protected void OnTriggerEnter (Collider coll)
	{
		Debug.Log ("OnTriggerEnter---");
		if (!enabled)
			return;

		Debug.Log ("OnTriggerEnter2---");
		Player player = coll.gameObject.GetComponentInChildren<Player> ();
		if (player != null) {

			Debug.Log ("OnTriggerEnter3---");

			var InvokeBedroomTeleportEvent = BedroomTeleportEvent;
			if (InvokeBedroomTeleportEvent != null) {
				InvokeBedroomTeleportEvent ();
			}

			enabled = false;
		}
	}

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
