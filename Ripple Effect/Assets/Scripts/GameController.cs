
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

	public enum ERoom
	{
		Room_1,
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

	public GameObject Room {
		get {
			switch (m_Room) {
			case ERoom.Room_1:
				m_Room = ERoom.Room_2;
				return m_Room1;
			case ERoom.Room_2:
				m_Room = ERoom.Room_3;
				return m_Room2;
			case ERoom.Room_3:
				return m_Room3;
			default:
				throw new UnityException (string.Format ("invalid case:{0}", m_Room));
			}
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

	[Tooltip ("The Room1 To Room2 Progress Items")]
	[SerializeField]
	private Interactable[] m_Room1ToRoom2ProgressItems;

	#endregion

	#region Private Member Variables

	private bool m_IsInStairwell;
	private bool m_DoorUnlocked = false;
	private ERoom m_Room = ERoom.Room_1;
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
		if (m_Room1ToRoom2ProgressItems == null || m_Room1ToRoom2ProgressItems.Length < 0) {
			Debug.LogWarning ("m_Room1ToRoom2ProgressItems is null");
		}

		Interactable.DoInteractEvent += Interactable_DoInteractEvent;
	}

	protected void Update ()
	{
		switch (m_Room) {
		case ERoom.Room_1:
			bool hasFoundStairwellItems = false;
			foreach (var item in m_Room1ToRoom2ProgressItems) {
				if (m_HasInteracted.ContainsKey (item) && m_HasInteracted [item] == true) {
					hasFoundStairwellItems = true;
				}
			}
			if (hasFoundStairwellItems) {
				m_DoorUnlocked = true;
				var InvokeDoorUnlockedEvent = DoorUnlockedEvent;
				if (InvokeDoorUnlockedEvent != null) {
					InvokeDoorUnlockedEvent ();
				}
				return;
			}
			break;
		case ERoom.Room_2:
			break;
		case ERoom.Room_3:
			break;
		default:
			throw new UnityException (string.Format ("invalid case:{0}", m_Room));
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
	}

	public void EnterRoom ()
	{
		m_IsInStairwell = false;
		m_FirstPersonController.IsInStairwell = false;
	}

	#endregion

	#region Private Methods


	private void Interactable_DoInteractEvent (Interactable i)
	{
		if (i.tag == Interactable.kInteractableTag) {
			m_HasInteracted [i] = true;
		}
	}

	#endregion
}
