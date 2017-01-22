

using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class OnReenterRoom : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	public delegate void OnReenterRoomHandler ();

	public static event OnReenterRoomHandler ReenterRoomEvent;

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
		Debug.Log ("I was turned on!");
	}

	protected void OnDisable ()
	{
	}

	#endregion

	protected void OnTriggerEnter (Collider coll)
	{
		Debug.Log ("===OnTriggerEnter");
		Player player = coll.gameObject.GetComponentInChildren<Player> ();
		if (player != null) {

			Debug.Log ("===OnTriggerEnter3");

			var InvokeReenterRoomEvent = ReenterRoomEvent;
			if (InvokeReenterRoomEvent != null) {
				InvokeReenterRoomEvent ();
			}
		}
	}

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
