

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

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

	#endregion

	#region Private Member Variables

	private MovieTexture m_MovieTexture;

	#endregion

	#region Monobehaviours

	protected void Awake ()
	{
		RawImage rim = GetComponent<RawImage> ();
		m_MovieTexture = (MovieTexture)rim.mainTexture;
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

	private IEnumerator PlayMovieTask ()
	{
		m_MovieTexture.Play ();
		yield return new WaitForSeconds (10f);
		m_MovieTexture.Stop ();
		yield return null;
		StartCoroutine (PlayMovieTask ());
	}

	#endregion
}
