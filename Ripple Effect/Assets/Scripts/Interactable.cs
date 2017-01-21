
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent (typeof(Renderer))]
public class Interactable : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	public int ID {
		get {
			return m_ID;
		}
	}

	public string PromptText{
		get{
			return m_PromptText;
		}
	}

	public AudioClip InteractSound {
		get {
			return m_InteractSound;
		}
	}

	#endregion

	#region Inspectables

	[Tooltip ("")]
	[SerializeField]
	private int m_ID;

	[Tooltip ("The text display when the player is highliting on it")]
	[SerializeField]
	private string m_PromptText;

	[Tooltip ("")]
	[SerializeField]
	private AudioClip m_InteractSound;

	#endregion

	#region Private Member Variables

	private float m_FocusDurationRemaining;
	private Renderer m_Renderer;
	private Color m_StartColor;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		m_Renderer = GetComponent<Renderer> ();

		Debug.Assert (m_Renderer != null);
	}

	protected void Start ()
	{
		m_StartColor = m_Renderer.material.color;
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

	protected void OnBecameVisible ()
	{
		enabled = true;
	}

	protected void OnBecameInvisible ()
	{
		enabled = false;
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

	#endregion

	#region Private Methods

	#endregion
}
