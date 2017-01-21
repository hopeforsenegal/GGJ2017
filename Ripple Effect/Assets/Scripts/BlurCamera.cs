
using UnityEngine;
using System.Collections;

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

	private float m_DurationRemaining;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
	}

	protected void Start ()
	{
	}

	protected void Update ()
	{
		// animate the position of the game object...
		transform.position = new Vector3 (Mathf.Lerp (1, 15, m_DurationRemaining / m_Duration), 0, 0);

		m_DurationRemaining -= Time.deltaTime;

		if (m_DurationRemaining <= 0f) {
			m_DurationRemaining = 0f;
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			BlurVision ();
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
		m_DurationRemaining = m_Duration;
	}

	#endregion

	#region Private Methods

	#endregion
}
