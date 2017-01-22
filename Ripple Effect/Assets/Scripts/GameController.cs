
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

[DisallowMultipleComponent]
public class GameController : MonoBehaviour
{
	#region Singleton

	public static GameController Instance {
		get {
			return sInstance;
		}
	}

	public static bool TryGetInstance (out GameController returnVal)
	{
		returnVal = sInstance;
		if (returnVal == null) {
			Debug.LogWarning (string.Format ("Couldn't access {0}", typeof(GameController).Name));
		}
		return (returnVal != null);
	}

	private static GameController sInstance;

	#endregion

	#region Enums and Constants

	public enum ERoomStates
	{
		Room_1,
		Room_1_Loop,
		Room_2,
		Room_3,
	}

	#endregion

	#region Events

	public delegate void OnDoorUnlockedHandler ();

	public static event OnDoorUnlockedHandler DoorUnlockedEvent;

	#endregion

	#region Properties

	public bool IsInStairwell {
		get {
			return m_IsInStairwell;
		}
	}

	public GameObject NextRoomGameObject {
		get {
			switch (m_NextRoom) {
			case ERoomStates.Room_1:
			case ERoomStates.Room_1_Loop:
				return m_Room1;
			case ERoomStates.Room_2:
				return m_Room2;
			case ERoomStates.Room_3:
				return m_Room3;
			default:
				throw new UnityException (string.Format ("invalid case:{0}", m_CurrentRoom));
			}
		}
	}

	public ERoomStates NextRoom {
		get {
			return m_NextRoom;
		}
	}

	public Door FirstRoomDoor {
		get {
			return m_FirstRoomsDoor;
		}
	}

	#endregion

	#region Inspectables

	[Tooltip ("")]
	[SerializeField]
	private FirstPersonController m_FirstPersonController;

	[Tooltip ("First Rooms door")]
	[SerializeField]
	private Door m_FirstRoomsDoor;

	[Tooltip ("")]
	[SerializeField]
	private GameObject m_Room1;

	[Tooltip ("")]
	[SerializeField]
	private GameObject m_Room2;

	[Tooltip ("")]
	[SerializeField]
	private GameObject m_Room3;

	[Tooltip ("The Room1 open door items")]
	[SerializeField]
	private Interactable[] m_Room1OpenDoorItems;

	[Tooltip ("The set of all Room1 items")]
	[SerializeField]
	private Interactable[] m_AllRoom1Items;

	[Tooltip ("The Room3 die items")]
	[SerializeField]
	private Interactable[] m_Room3DieItems;

	[Tooltip ("The Room3 win items")]
	[SerializeField]
	private Interactable[] m_Room3WinItems;

	[Tooltip ("")]
	[SerializeField]
	private InfoPanel m_InfoPanel;

	[Tooltip ("")]
	[SerializeField]
	private OnReenterRoom[] m_OnReenterRooms;

	[Tooltip ("")]
	[SerializeField]
	private OnBedroomTeleport m_OnBedroomTeleport;

	[Tooltip ("")]
	[SerializeField]
	private Transform m_BedroomTeleportTransform;

	#endregion

	#region Private Member Variables

	private bool m_HasFoundStairwellItemsRoom1 = false;
	private bool m_HasFoundAllItemsRoom1 = false;
	private bool m_IsInStairwell;
	private bool m_DoorUnlocked = false;
	private ERoomStates m_CurrentRoom = ERoomStates.Room_1;
	private ERoomStates m_NextRoom = ERoomStates.Room_1;
	private Dictionary<Interactable, bool> m_HasInteracted = new Dictionary<Interactable, bool> ();

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		if (sInstance == null) {
			sInstance = this;
		} else if (sInstance != this) {
			DestroyObject (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);
	}

	protected void Start ()
	{
		if (m_FirstPersonController == null) {
			Debug.LogWarning ("m_FirstPersonController is null");
		}
		if (m_Room1OpenDoorItems == null || m_Room1OpenDoorItems.Length <= 0) {
			Debug.LogWarning ("m_Room1OpenDoorItems is null");
		}
		if (m_AllRoom1Items == null || m_AllRoom1Items.Length <= 0) {
			Debug.LogWarning ("m_AllRoom1Items is null");
		}
		if (m_Room3DieItems == null) {
			Debug.LogWarning ("m_Room3DieItems is null");
		}
		if (m_Room3WinItems == null) {
			Debug.LogWarning ("m_Room3WinItems is null");
		}
		if (m_FirstRoomsDoor == null) {
			Debug.LogWarning ("m_FirstRoomsDoor is null");
		}
		if (m_InfoPanel == null) {
			Debug.LogWarning ("m_InfoPanel is null");
		}
		if (m_Room1 == null) {
			Debug.LogWarning ("m_Room1 is null");
		} 
		if (m_Room2 == null) {
			Debug.LogWarning ("m_Room2 is null");
		} else {
			m_Room2.SetActive (false);
		}
		if (m_Room3 == null) {
			Debug.LogWarning ("m_Room3 is null");
		} else {
			m_Room3.SetActive (false);
		}
		if (m_OnReenterRooms == null) {
			Debug.LogWarning ("m_OnReenterRoom is null");
		}
		if (m_OnBedroomTeleport == null) {
			Debug.LogWarning ("m_OnBedroomTeleport is null");
		}

		Interactable.DoInteractEvent += Interactable_DoInteractEvent;
		OnStairWell.EnterStairWellEvent += OnStairWell_EnterStairWellEvent;
		OnBedroomTeleport.BedroomTeleportEvent += OnBedroomTeleport_BedroomTeleportEvent;
		OnReenterRoom.ReenterRoomEvent += OnReenterRoom_ReenterRoomEvent;
	}

	protected void Update ()
	{
		switch (m_CurrentRoom) {
		case ERoomStates.Room_1:
			if (IsInStairwell)
				return;
				
			if (CheckForAllItems ())
				return;

			if (CheckForDoorOpenItems ())
				return;
			break;
		case ERoomStates.Room_1_Loop:
			if (IsInStairwell)
				return;
			
			if (CheckForAllItems ())
				return;
			break;
		case ERoomStates.Room_2:
			break;
		case ERoomStates.Room_3:
			if (CheckForDieItems ()) {
				Debug.Log ("Game over");
				Debug.Log ("You lost");
			} else if (CheckForWinItems ()) {
				Debug.Log ("Game over");
				Debug.Log ("You won");
			}
			break;
		default:
			throw new UnityException (string.Format ("invalid case:{0}", m_CurrentRoom));
		}
	}

	protected void OnEnable ()
	{
	}

	protected void OnDisable ()
	{
	}

	protected void OnDestroy ()
	{
		Interactable.DoInteractEvent -= Interactable_DoInteractEvent;
		OnStairWell.EnterStairWellEvent -= OnStairWell_EnterStairWellEvent;
		OnBedroomTeleport.BedroomTeleportEvent -= OnBedroomTeleport_BedroomTeleportEvent;

		if (sInstance == this) {
			sInstance = null;
		}
	}

	#endregion

	#region Public Methods

	public void EnterStairWell ()
	{
		m_IsInStairwell = true;
		m_FirstPersonController.IsInStairwell = true;
		m_DoorUnlocked = false;
		foreach (var item in m_OnReenterRooms) {
			item.enabled = true;
		}

		Debug.LogFormat ("EnterStairWell m_Room:{0} m_HasFoundStairwellItemsRoom1:{1} m_HasFoundAllItemsRoom1:{2}", m_CurrentRoom, m_HasFoundStairwellItemsRoom1, m_HasFoundAllItemsRoom1);
		
		switch (m_CurrentRoom) {
		case ERoomStates.Room_1:
			m_Room1.SetActive (false);
			if (m_HasFoundAllItemsRoom1) {
				m_NextRoom = ERoomStates.Room_2;
			} else {
				m_NextRoom = ERoomStates.Room_1_Loop;
			}
			break;
		case ERoomStates.Room_1_Loop:
			if (m_HasFoundAllItemsRoom1) {
				m_NextRoom = ERoomStates.Room_2;
			}
			break;
		case ERoomStates.Room_2:
			m_NextRoom = ERoomStates.Room_3;
			break;
		case ERoomStates.Room_3:
			Debug.Log ("How are you here?");
			break;
		default:
			throw new UnityException (string.Format ("invalid case:{0}", m_CurrentRoom));
		}
	}

	public void EnterRoom ()
	{
		Debug.LogFormat ("EnterRoom m_CurrentRoom:{0} m_HasFoundStairwellItemsRoom1:{1} m_HasFoundAllItemsRoom1:{2}", m_CurrentRoom, m_HasFoundStairwellItemsRoom1, m_HasFoundAllItemsRoom1);

		m_CurrentRoom = m_NextRoom;

		m_DoorUnlocked = false;
		m_IsInStairwell = false;
		m_FirstPersonController.IsInStairwell = false;
	}

	#endregion

	#region Private Methods

	private bool CheckForAllItems ()
	{
		bool hasFoundAllItemsRoom1 = false;

		foreach (var item in m_AllRoom1Items) {
			if (m_HasInteracted.ContainsKey (item) && m_HasInteracted [item] == true) {
				hasFoundAllItemsRoom1 = true;
			} else {
				hasFoundAllItemsRoom1 = false;
				break;
			}
		}

		if (hasFoundAllItemsRoom1 && !m_InfoPanel.IsShowing) {
			m_HasFoundAllItemsRoom1 = true;
			AttemptDoorUnlock ();
			return true;
		}

		return false;
	}

	private bool CheckForDoorOpenItems ()
	{
		bool hasFoundStairwellItemsRoom1 = false;

		foreach (var item in m_Room1OpenDoorItems) {
			if (m_HasInteracted.ContainsKey (item) && m_HasInteracted [item] == true) {
				hasFoundStairwellItemsRoom1 = true;
			} else {
				hasFoundStairwellItemsRoom1 = false;
				break;
			}
		}

		if (hasFoundStairwellItemsRoom1 && !m_InfoPanel.IsShowing) {
			m_HasFoundStairwellItemsRoom1 = true;
			AttemptDoorUnlock ();
			return true;
		}

		return false;
	}

	private bool CheckForDieItems ()
	{
		bool hasFoundDieItems = false;

		foreach (var item in m_Room3DieItems) {
			if (m_HasInteracted.ContainsKey (item) && m_HasInteracted [item] == true) {
				hasFoundDieItems = true;
			} else {
				hasFoundDieItems = false;
				break;
			}
		}

		return hasFoundDieItems;
	}

	private bool CheckForWinItems ()
	{
		bool hasFoundWinItems = false;

		foreach (var item in m_Room3WinItems) {
			if (m_HasInteracted.ContainsKey (item) && m_HasInteracted [item] == true) {
				hasFoundWinItems = true;
			} else {
				hasFoundWinItems = false;
				break;
			}
		}

		return hasFoundWinItems;
	}

	private void AttemptDoorUnlock ()
	{
		if (!m_DoorUnlocked) {
			Debug.LogFormat ("Door unlocked! m_HasFoundStairwellItemsRoom1:{0} m_HasFoundAllItemsRoom1:{1}", m_HasFoundStairwellItemsRoom1, m_HasFoundAllItemsRoom1);
			m_DoorUnlocked = true;
			var InvokeDoorUnlockedEvent = DoorUnlockedEvent;
			if (InvokeDoorUnlockedEvent != null) {
				InvokeDoorUnlockedEvent ();
			}
		}
	}

	private void Interactable_DoInteractEvent (Interactable i)
	{
		if (i.tag == Interactable.kInteractableTag) {
			if (!m_HasInteracted.ContainsKey (i) || m_HasInteracted [i] == false) {
				Debug.LogFormat ("Interacted with {0}", i.name);
			}
			m_HasInteracted [i] = true;
		}
	}

	private void OnStairWell_EnterStairWellEvent ()
	{
		EnterStairWell ();
		switch (NextRoom) {
		case ERoomStates.Room_1_Loop:
			FirstRoomDoor.IsDoorOpen = false;
			break;
		case GameController.ERoomStates.Room_2:
			FirstRoomDoor.IsDoorOpen = false;
			break;
		case GameController.ERoomStates.Room_3:
			break;
		default:
			throw new UnityException (string.Format ("invalid case:{0}", NextRoom));
		}
	}

	private void OnBedroomTeleport_BedroomTeleportEvent ()
	{
		Player player;
		if (Player.TryGetInstance (out player)) {
			Transform pTransform = player.GetComponent<Transform> ();
			pTransform.position = m_BedroomTeleportTransform.position;
		}
	}

	private void OnReenterRoom_ReenterRoomEvent ()
	{
		EnterRoom ();
	}

	#endregion
}
