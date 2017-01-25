
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
		Player player = coll.gameObject.GetComponentInChildren<Player> ();
		if (player != null) {
			player.FocusCamera ();
		}
	}

	protected void OnTriggerExit (Collider coll)
	{
		Player player = coll.gameObject.GetComponentInChildren<Player> ();
		if (player != null) {
			player.UnFocusCamera ();
		}
	}

	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
