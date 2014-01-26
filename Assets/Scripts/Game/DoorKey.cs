using UnityEngine;
using System.Collections;

public class DoorKey : MonoBehaviour {

	public KeyColor keyColor;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(Game.Instance.gameState != GameState.Playing)
			return;

		if(collider.gameObject.CompareTag(Tags.Player))
		{
			KeyCollection.Instance.ReceiveKey(keyColor);
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(Game.Instance.gameState != GameState.Playing)
			return;

		if(collision.gameObject.CompareTag(Tags.Player))
		{
			if(KeyCollection.Instance.IsKeyReceived(keyColor))
			{
				Destroy(this.gameObject);
			}
		}
	}
}
