
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class TeleportPlayer : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	#endregion

	#region Inspectables

	[Tooltip ("The linked teleport location")]
	[SerializeField]
	private TeleportPlayer m_TeleportLocation;

	#endregion

	#region Private Member Variables

	private static int sLastTeleportedHashCode = 0;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
	}

	protected void Start ()
	{
		if (m_TeleportLocation == null) {
			Debug.LogWarning ("m_TeleportLocation is null");
		}
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
		if (sLastTeleportedHashCode != 0 && this.GetHashCode () != sLastTeleportedHashCode) {
			sLastTeleportedHashCode = 0;
			return;
		}

		CharacterController player = coll.gameObject.GetComponent<CharacterController> ();
		if (player != null) {
			Transform pTransform = player.GetComponent<Transform> ();
			pTransform.position = new Vector3( pTransform.position.x, m_TeleportLocation.transform.position.y, pTransform.position.z );
			sLastTeleportedHashCode = this.GetHashCode ();
		}
	}

	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
