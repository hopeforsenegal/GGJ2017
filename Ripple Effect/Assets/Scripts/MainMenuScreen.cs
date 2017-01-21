
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class MainMenuScreen : MonoBehaviour 
{
    #region Singleton

	public static MainMenuScreen Instance {
		get {
			return sInstance;
		}
	}

	public static bool TryGetInstance (out MainMenuScreen returnVal)
	{
		returnVal = sInstance;
		if (returnVal == null) {
			Debug.LogWarning (string.Format ("Couldn't access {0}", typeof(MainMenuScreen).Name));
		}
		return (returnVal != null);
	}

	private static MainMenuScreen sInstance;

	#endregion
	
	#region Enums and Constants

	#endregion 

	#region Events

	#endregion

	#region Properties

	#endregion 
	
	#region Inspectables

	#endregion

	#region Private Member Variables

	#endregion

    #region Monobehaviours

	protected void Awake () 
	{
		if (sInstance == null) {
			sInstance = this;
		}
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
		sInstance = null;
	}

	#endregion

	#region Public Methods

	#endregion

	#region Private Methods

	#endregion
}
