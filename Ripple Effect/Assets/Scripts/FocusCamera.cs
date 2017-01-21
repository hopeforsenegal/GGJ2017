
using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[DisallowMultipleComponent]
public class FocusCamera : MonoBehaviour
{
	#region Enums and Constants

	#endregion

	#region Events

	#endregion

	#region Properties

	public Vector3 Target {
		get {
			return m_Target;
		}
		set {
			m_Target = value;
		}
	}

	private Vector3 m_Target;

	#endregion

	#region Inspectables

	[Tooltip ("The target transform")]
	[SerializeField]
	private Transform m_TransformTarget;

	#endregion

	#region Private Member Variables

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
	}

	protected void Start ()
	{
		if (m_TransformTarget != null) {
			m_Target = m_TransformTarget.position;
		} else {
			Debug.LogWarning ("Not set go check");
		}
	}

	protected void Update ()
	{
		SmoothLook (m_Target);
	}

	protected void OnEnable ()
	{
	}

	protected void OnDisable ()
	{
	}

	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	private void SmoothLook (Vector3 newDirection)
	{
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (newDirection), Time.deltaTime);
	}

	#endregion
}
