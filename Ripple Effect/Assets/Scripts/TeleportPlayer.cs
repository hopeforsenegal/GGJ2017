
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

	[Tooltip ("The teleport location")]
	[SerializeField]
	private Vector3 m_TeleportLocation;

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
	
	protected void OnBecameVisible ()
	{
		enabled = true;
	}

	protected void OnBecameInvisible ()
	{
		enabled = false;
	}
	
	#endregion

	#region Trigger Collider

	protected void OnTriggerEnter (Collider coll)
	{
		CharacterController player = coll.gameObject.GetComponent<CharacterController> ();
		if (player != null) {
			Transform pTransform = player.GetComponent<Transform> ();
			pTransform.position = m_TeleportLocation;
		}
	}

	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
