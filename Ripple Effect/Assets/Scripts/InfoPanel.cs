
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class InfoPanel : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	[Tooltip ("")]
	[SerializeField]
	private Text m_Text;

	[Tooltip ("")]
	[SerializeField]
	private Image m_Image;

	#endregion

	#region Inspectables

	public bool IsShowing {
		get {
			return m_IsShowing;
		}
		set {
			m_IsShowing = value;
		}
	}

	#endregion

	#region Private Member Variables

	private bool m_IsShowing;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
	}

	protected void Start ()
	{
		if (m_Text == null) {
			Debug.LogWarning ("m_Text is null");
		}
		if (m_Image == null) {
			Debug.LogWarning ("m_Image is null");
		}

		Hide ();
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
	}

	#endregion

	#region Public Methods

	public void Show (string text)
	{
		m_Image.enabled = true;
		m_Text.enabled = true;
		m_Text.text = text;
		m_IsShowing = true;
	}

	public void Hide ()
	{
		m_Image.enabled = false;
		m_Text.enabled = false;
		m_IsShowing = false;
	}

	#endregion

	#region Private Methods

	#endregion
}
