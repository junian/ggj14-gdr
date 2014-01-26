using UnityEngine;
using System.Collections;

public class LinearMove : MonoBehaviour {

	public Vector2 speed = new Vector2(10.0f, 0.0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(
			new Vector3(
				speed.x , 
				speed.y, 
				0.0f) * Time.deltaTime);
	}
}
