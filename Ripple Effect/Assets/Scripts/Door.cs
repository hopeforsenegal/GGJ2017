
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class Door : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	#endregion

	#region Inspectables

	[Tooltip ("The transform of where we pull a room to")]
	[SerializeField]
	private Transform m_Transform;

	#endregion

	#region Private Member Variables


	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
	}

	protected void Start ()
	{
		if (m_Transform == null) {
			Debug.LogWarning ("m_Transform is null");
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

	#region Public Methods

	public void RoomTransfer ()
	{
		GameController gameController;
		if (GameController.TryGetInstance (out gameController)) {
			gameController.CurrentRoom.transform.position = m_Transform.position;
			gameController.CurrentRoom.transform.rotation = m_Transform.rotation;
			gameController.CurrentRoom.transform.localScale = m_Transform.localScale;
		}
	}

	#endregion

	#region Private Methods

	#endregion
}
