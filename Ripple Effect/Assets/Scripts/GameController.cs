﻿
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

[DisallowMultipleComponent]
[
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

	public GameObject CurrentRoom {
		get {
			switch (m_CurrentRoom) {
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

	#endregion

	#region Inspectables

	[Tooltip ("")]
	[SerializeField]
	private FirstPersonController m_FirstPersonController;

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
		if (m_Room1OpenDoorItems == null || m_Room1OpenDoorItems.Length < 0) {
			Debug.LogWarning ("m_Room1OpenDoorItems is null");
		}
		if (m_AllRoom1Items == null || m_AllRoom1Items.Length < 0) {
			Debug.LogWarning ("m_AllRoom1Items is null");
		}

		Interactable.DoInteractEvent += Interactable_DoInteractEvent;
	}

	protected void Update ()
	{
		switch (m_CurrentRoom) {
		case ERoomStates.Room_1:
			if (CheckForAllItems ())
				return;

			if (CheckForDoorOpenItems ())
				return;
			break;
		case ERoomStates.Room_1_Loop:
			if (CheckForAllItems ())
				return;
			break;
		case ERoomStates.Room_2:
			break;
		case ERoomStates.Room_3:
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

		Debug.LogFormat ("m_Room:{0} m_HasFoundStairwellItemsRoom1:{1} m_HasFoundAllItemsRoom1:{2}", m_CurrentRoom, m_HasFoundStairwellItemsRoom1, m_HasFoundAllItemsRoom1);
		
		switch (m_CurrentRoom) {
		case ERoomStates.Room_1:
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
		m_DoorUnlocked = false;
		m_IsInStairwell = false;
		m_FirstPersonController.IsInStairwell = false;
		m_CurrentRoom =	m_NextRoom;
	}

	#endregion

	#region Private Methods

	private void Interactable_DoInteractEvent (Interactable i)
	{
		if (i.tag == Interactable.kInteractableTag) {
			m_HasInteracted [i] = true;
		}
	}

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
		if (hasFoundAllItemsRoom1) {
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
		if (hasFoundStairwellItemsRoom1) {
			m_HasFoundStairwellItemsRoom1 = true;
			AttemptDoorUnlock ();
			return true;
			;
		}

		return false;
	}

	private void AttemptDoorUnlock ()
	{
		if (!m_DoorUnlocked) {
			m_DoorUnlocked = true;
			var InvokeDoorUnlockedEvent = DoorUnlockedEvent;
			if (InvokeDoorUnlockedEvent != null) {
				InvokeDoorUnlockedEvent ();
			}
		}
	}

	#endregion
}
