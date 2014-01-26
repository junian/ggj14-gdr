using UnityEngine;
using System.Collections;

public class AIMove : MonoBehaviour {
	public Vector2 speed = new Vector2(-0.75f, 0.0f);
	public Vector2 range = new Vector2(3.0f, 0.0f);

	private Vector3 movement;
	private Vector3 startPos;
	// Use this for initialization
	void Start () {
		var xDirection = transform.localScale.x;
		xDirection = xDirection < 0.0f ? -1.0f : 1.0f;
		movement = new Vector3(speed.x * xDirection, 0.0f, 0.0f);
		startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		var startpos = new Vector2(transform.position.x, transform.position.y + 0.05f);
		var nextpos = new Vector2(startpos.x + (movement.x / Mathf.Abs(movement.x) * 0.25f), startpos.y);

		//Debug.DrawLine(startpos, nextpos, Color.red, 100.0f);

		var isCollide = Physics2D.Linecast(startpos, nextpos, 1 << LayerMask.NameToLayer(Layers.Platform));

		if((transform.position.x <= startPos.x - range.x && movement.x < 0.0f) ||
		   (transform.position.x >= startPos.x + range.x && movement.x > 0.0f) || isCollide)
		{
			ChangeDirection();
		}

		this.transform.Translate(Time.deltaTime * movement);
	}

	void ChangeDirection()
	{
		movement.x = movement.x * -1;
		Vector3 scale = this.transform.localScale;
		scale.x = scale.x * -1;
		this.transform.localScale = scale;
	}
}
