
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(Interactable))]
public class Door : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	public bool IsDoorOpen {
		get {
			return m_IsDoorOpen;
		}
		set {
			m_IsDoorOpen = value;
		}
	}

	private bool m_IsDoorOpen;

	#endregion

	#region Inspectables

	[Tooltip ("")]
	[SerializeField]
	private Transform m_Transform;

	#endregion

	#region Private Member Variables

	private Interactable m_Interactable;
	private AudioSource[] m_Sources;
	private Animator m_Animator;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		m_Sources = GetComponents<AudioSource> ();
		m_Interactable = GetComponent<Interactable> ();
		m_Animator = GetComponent<Animator> ();

		Debug.Assert (m_Interactable != null, name);

		Interactable.DoInteractEvent += Interactable_DoInteractEvent;
		GameController.DoorUnlockedEvent += GameController_DoorUnlockedEvent;
		OnStairWell.EnterStairWellEvent += OnStairWell_EnterStairWellEvent;
		OnReenterRoom.ReenterRoomEvent += OnReenterRoom_ReenterRoomEvent;
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

	protected void OnDestroy ()
	{
		OnReenterRoom.ReenterRoomEvent -= OnReenterRoom_ReenterRoomEvent;
		OnStairWell.EnterStairWellEvent -= OnStairWell_EnterStairWellEvent;
		Interactable.DoInteractEvent -= Interactable_DoInteractEvent;
		GameController.DoorUnlockedEvent -= GameController_DoorUnlockedEvent;
	}

	#endregion

	#region Public Methods

	public void PlaySound (Object audio)
	{
		foreach (AudioSource source in m_Sources) {
			if (!source.isPlaying) {
				source.clip = audio as AudioClip;
				source.Play ();
				break;
			}
		}
	}

	#endregion

	#region Private Methods

	private void Interactable_DoInteractEvent (Interactable i)
	{
		GameController gameController;
		if (GameController.TryGetInstance (out gameController)) {
			bool isFirstRoomDoor = this.m_Interactable.ID == gameController.FirstRoomDoor.m_Interactable.ID;
			if (isFirstRoomDoor && !gameController.IsInStairwell) {
				return;
			}
		}

		if (m_Interactable != null && m_Interactable.ID == i.ID && gameController.IsInStairwell) {
			if (gameController != null) {
				Debug.LogFormat ("Teleporting room from stairwell {0}/{1}", gameController.NextRoomGameObject, gameController.NextRoom);
				gameController.NextRoomGameObject.SetActive (true);
				gameController.NextRoomGameObject.transform.position = m_Transform.position;
				gameController.NextRoomGameObject.transform.rotation = m_Transform.rotation;
				gameController.NextRoomGameObject.transform.localScale = m_Transform.localScale;
			}
			ToggleDoor ();
		}
	}

	private void ToggleDoor ()
	{
		Debug.Log ("OpenDoorState");
		if (m_Animator != null && m_Animator.isActiveAndEnabled) {
			if (!m_IsDoorOpen) {
				StartCoroutine (OpenDoorTask ());
			} else {
				m_Animator.SetInteger ("OpenDoorState", 0);
				m_IsDoorOpen = false;
			}
			m_Animator.Update (0.0f);
		}
	}

	private void GameController_DoorUnlockedEvent ()
	{
		GameController gameController;
		if (GameController.TryGetInstance (out gameController)) {
			if (m_Interactable.ID == gameController.FirstRoomDoor.m_Interactable.ID) {
				ToggleDoor ();
			}
		}
	}

	private IEnumerator OpenDoorTask ()
	{
		int explodeDoor = 2; //no explode
		GameController gameController;
		if (GameController.TryGetInstance (out gameController)) {
			if (!gameController.IsInStairwell) {
				Player player;
				if (Player.TryGetInstance (out player)) {
					player.FocusCamera ();
					explodeDoor = 1; //explode
				}
			}
		}
		yield return new WaitForSeconds (1.2f);
		m_Animator.SetInteger ("OpenDoorState", explodeDoor);
		m_IsDoorOpen = true;
	}

	private void OnStairWell_EnterStairWellEvent ()
	{
		CloseDoor ();
	}

	private void OnReenterRoom_ReenterRoomEvent ()
	{
		StartCoroutine (ReenterRoomCloseDoorTask ());
	}

	private IEnumerator ReenterRoomCloseDoorTask ()
	{
		yield return null;
		GameController gameController;
		if (GameController.TryGetInstance (out gameController)) {
			if (!gameController.IsInStairwell) {
				CloseDoor ();
			}
		}
	}

	private void CloseDoor ()
	{
		Debug.Log ("CloseDoor");
		if (m_Animator != null && m_Animator.isActiveAndEnabled) {
			if (m_IsDoorOpen) {
				m_Animator.SetInteger ("OpenDoorState", 0);
				m_IsDoorOpen = false;
			}
			m_Animator.Update (0.0f);
		}
	}

	#endregion
}
