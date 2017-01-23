

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class MoviePlayer : MonoBehaviour
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
	private string m_sceneToLoad;

	[Tooltip ("")]
	[SerializeField]
	private float m_LoopTime;

	[Tooltip ("")]
	[SerializeField]
	private bool m_TransitionAfterClip = false;

	#endregion

	#region Private Member Variables

	private MovieTexture m_MovieTexture;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		RawImage rim = GetComponent<RawImage> ();
		try
		{
			m_MovieTexture = ( MovieTexture )rim.mainTexture;
		} catch (System.Exception e)
		{
			Application.Quit();
		}
	}

	protected void Start ()
	{
		StartCoroutine (PlayMovieTask ());
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

	protected void OnMouseDown ()
	{
		SceneTransition ();
	}

	private void SceneTransition ()
	{
		if (!string.IsNullOrEmpty (m_sceneToLoad)) {
			StopAllCoroutines ();
			SceneManager.LoadScene (m_sceneToLoad);
		}
	}

	private IEnumerator PlayMovieTask ()
	{
		m_MovieTexture.Play ();
		yield return new WaitForSeconds (m_LoopTime);
		m_MovieTexture.Stop ();
		yield return null;
		if (m_TransitionAfterClip) {
			SceneTransition ();
		} else {
			StartCoroutine (PlayMovieTask ());
		}
	}

	#endregion
}
