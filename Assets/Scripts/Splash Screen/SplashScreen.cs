using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public float loadingWait = 3.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine(LoadMainMenu());
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator LoadMainMenu()
	{
		yield return new WaitForSeconds(loadingWait);
		Application.LoadLevel(Scenes.MainMenu);
	}
}
