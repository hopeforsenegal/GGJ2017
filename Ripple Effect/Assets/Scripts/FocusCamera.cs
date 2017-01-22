
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

	public Transform Target {
		get {
			return m_Target;
		}
		set {
			m_Target = value;
		}
	}

	private Transform m_Target;

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
			m_Target = m_TransformTarget;
		} else {
			Debug.LogWarning ("Not set go check");
		}

		if (m_Target == null) {
			Debug.LogWarning ("m_Target is null");
		}
	}

	protected void Update ()
	{
		if ( m_Target != null )
		{
			SmoothLook( m_Target.position );
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

	#endregion

	#region Private Methods

	private void SmoothLook (Vector3 newDirection)
	{
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (newDirection), Time.deltaTime);
	}

	#endregion
}
