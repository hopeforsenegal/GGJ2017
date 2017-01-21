
using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[DisallowMultipleComponent]
public class BlurCamera : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	#endregion

	#region Inspectables

	[Tooltip ("")]
	[SerializeField]
	private float m_Duration = 1f;

	#endregion

	#region Private Member Variables

	private bool m_DoAction = false;
	private bool m_Blur;
	private float m_DurationRemaining;
	private TiltShift m_TiltShift;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		m_TiltShift = GetComponent<TiltShift> ();

		Debug.Assert (m_TiltShift != null);
	}

	protected void Start ()
	{
	}

	protected void Update ()
	{
		if (m_DoAction) {
			// animate the position of the game object...
			if (m_Blur) {
				m_TiltShift.blurArea = Mathf.Lerp (1f, 15f, (m_Duration - m_DurationRemaining) / m_Duration);
			} else {
				m_TiltShift.blurArea = Mathf.Lerp (15f, 1f, (m_Duration - m_DurationRemaining) / m_Duration);
			}
			Debug.LogFormat ("blurArea:{0} m_Duration:{1} m_DurationRemaining:{2} {3}", m_TiltShift.blurArea, m_Duration, m_DurationRemaining, (m_Duration - m_DurationRemaining) / m_Duration);

			m_DurationRemaining -= Time.deltaTime;

			if (m_DurationRemaining <= 0f) {
				m_DurationRemaining = 0f;
				m_DoAction = false;
				Debug.Log ("Reached BlurVision");
			}
		} else {
			if (Input.GetKey (KeyCode.H)) {
				BlurVision ();
			}
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

	public void BlurVision ()
	{
		Debug.Log ("BlurVision");
		m_DurationRemaining = m_Duration;
		m_DoAction = true;
		m_Blur = true;
	}

	public void ClearVision ()
	{
		Debug.Log ("BlurVision");
		m_DurationRemaining = m_Duration;
		m_DoAction = true;
		m_Blur = false;
	}

	#endregion

	#region Private Methods

	#endregion
}
