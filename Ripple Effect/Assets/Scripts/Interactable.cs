
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class Interactable : MonoBehaviour 
{
	#region Enums and Constants

	#endregion 

	#region Events

	#endregion

	#region Properties

	#endregion 

	#region Inspectables

	#endregion

	#region Private Member Variables

	[Tooltip ("")]
	[SerializeField]
	private int m_ID;

	[Tooltip ("")]
	[SerializeField]
	private string m_SceneToLoad;

	[Tooltip ("")]
	[SerializeField]
	private AudioClip m_InteractSound;

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

	#endregion

	#region Private Methods

	#endregion
}
