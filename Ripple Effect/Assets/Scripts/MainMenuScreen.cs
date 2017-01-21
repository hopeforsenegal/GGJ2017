
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

	[Tooltip ("")]
	[SerializeField]
	private string m_MainGameScene = "main";

	[Tooltip ("")]
	[SerializeField]
	private Button m_StartGame;

	[Tooltip ("")]
	[SerializeField]
	private Button m_ExitGame;

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
		if (string.IsNullOrEmpty (m_MainGameScene)) {
			Debug.LogWarning ("m_MainGameScene is null");
		}
		if (m_StartGame == null) {
			Debug.LogWarning ("m_StartGame is null");
		} else {
			m_StartGame.onClick.AddListener (() => {
				StartGame ();
			});
		}
		if (m_ExitGame == null) {
			Debug.LogWarning ("m_ExitGame is null");
		} else {
			m_ExitGame.onClick.AddListener (() => {
				ExitGame ();
			});
		}
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

	private void StartGame ()
	{
		SceneManager.LoadScene (m_MainGameScene);
	}

	private void ExitGame ()
	{
		#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	#endregion
}
