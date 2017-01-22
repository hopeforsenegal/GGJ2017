
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent (typeof(Renderer))]
public class Interactable : MonoBehaviour
{
	#region Enums and Constants

	private static readonly string kInteractText = "Press 'E' to {0} {1}";

	public static readonly string kInteractableTag = "Item";
	public static readonly string kDoorTag = "Door";

	#endregion

	#region Events

	public delegate void OnDoInteractHandler (Interactable i);

	public static event OnDoInteractHandler DoInteractEvent;

	#endregion

	#region Properties

	public string ID {
		get {
			return m_ID;
		}
	}

	public string PromptText {
		get {
			return string.Format (kInteractText, m_InteractionText, name);
		}
	}

	public string TextToShowsOnInteraction {
		get {
			if (m_InteractionAttempts % 5 == 0) {
				return m_TextToShowsOnInteraction;
			} else {
				return m_TextToShowsOnInteraction2;
			}
		}
	}

	public AudioClip AudioToPlayOnInteraction {
		get {
			if (m_InteractionAttempts % 5 == 0) {
				return m_AudioToPlayOnInteraction;
			} else {
				return m_AudioToPlayOnInteraction2;
			}
		}
	}

	#endregion

	#region Inspectables

	[Tooltip ("Text describing the interaction with the player. Press 'E' to {0} {1}")]
	[SerializeField]
	private string m_InteractionText;

	[Tooltip ("Text that shows when the player interacts")]
	[SerializeField]
	private string m_TextToShowsOnInteraction;

	[Tooltip ("Text that shows when the player interacts")]
	[SerializeField]
	private string m_TextToShowsOnInteraction2;

	[Tooltip ("Audio that plays when the player interacts")]
	[SerializeField]
	private AudioClip m_AudioToPlayOnInteraction;

	[Tooltip ("Audio that plays when the player interacts")]
	[SerializeField]
	private AudioClip m_AudioToPlayOnInteraction2;

	#endregion

	#region Private Member Variables

	private float m_FocusDurationRemaining;
	private string m_ID;
	private int m_InteractionAttempts = 0;
	private Renderer m_Renderer;
	private Color m_StartColor;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		m_Renderer = GetComponent<Renderer> ();
		m_ID = System.Guid.NewGuid ().ToString ();
	}

	protected void Start ()
	{
		m_StartColor = m_Renderer.material.color;
		if (string.IsNullOrEmpty (tag)) {
			tag = kInteractableTag;
		}
	}

	protected void Update ()
	{
		m_FocusDurationRemaining -= Time.deltaTime;

		if (m_FocusDurationRemaining <= 0f) {
			m_FocusDurationRemaining = 0f;
			Unhighlight ();
		}
	}

	protected void OnEnable ()
	{
	}

	protected void OnDisable ()
	{
	}

	#endregion

	#region Public Methods

	public void Highlight ()
	{
		m_Renderer.material.color = Color.blue;

		m_FocusDurationRemaining = 0.1f;
	}

	public void Unhighlight ()
	{
		m_Renderer.material.color = m_StartColor;
	}

	public void DoInteract ()
	{
		var InvokeDoInteractEvent = DoInteractEvent;
		if (InvokeDoInteractEvent != null) {
			InvokeDoInteractEvent (this);
		}
		m_InteractionAttempts++;
	}

	#endregion

	#region Private Methods

	#endregion
}
