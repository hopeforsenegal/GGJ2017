
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent( typeof( Animator ) )]
[RequireComponent( typeof( AudioSource ) )]
[RequireComponent( typeof( Interactable ) )]
public class Door : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	#endregion

	#region Inspectables

	private Interactable m_Interactable;

	#endregion

	#region Private Member Variables
	private AudioSource[] m_Sources;

	private Animator m_Animator;

	private bool m_IsDoorOpen = false;

	

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		m_Sources = GetComponents<AudioSource>();
		m_Interactable = GetComponent<Interactable>();
		m_Animator = GetComponent<Animator>();
		Interactable.DoInteractEvent += Interactable_DoInteractEvent;
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

	#region Public Methods

	public void PlaySound( Object audio )
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( !source.isPlaying )
			{
				source.clip = audio as AudioClip;
				source.Play();
				break;
			}
		}
	}

	#endregion

	#region Private Methods

	private void Interactable_DoInteractEvent( Interactable i )
	{
		if ( m_Interactable != null && m_Interactable.ID == i.ID )
		{
			if ( m_Animator != null && m_Animator.isActiveAndEnabled )
			{
				if ( !m_IsDoorOpen )
				{
					m_Animator.SetBool( "OpenDoor", true );
					m_IsDoorOpen = true;
				} 
				else
				{
					m_Animator.SetBool( "OpenDoor", false );
					m_IsDoorOpen = false;
				}
				m_Animator.Update( 0.0f );
			}
		}

		GameController gameController;
		if (GameController.TryGetInstance (out gameController)) {
			gameController.CurrentRoom.transform.position = m_Transform.position;
			gameController.CurrentRoom.transform.rotation = m_Transform.rotation;
			gameController.CurrentRoom.transform.localScale = m_Transform.localScale;
	}
	#endregion
}
