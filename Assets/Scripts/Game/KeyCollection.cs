using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Key
{
	public KeyColor keyColor;
	public bool enabled = false;
	public bool received = false;
	public Texture2D enabledKeyTextures;
	public Texture2D disabledKeyTextures;
}

public class KeyCollection : MonoBehaviour {

	private static KeyCollection instance;

	public Key[] colorKeys = new Key[4];

	private Dictionary<KeyColor, Key> dict;

	public static KeyCollection Instance
	{
		get
		{
			return KeyCollection.instance;
		}
	}

	public void ReceiveKey(KeyColor keyColor)
	{
		dict[keyColor].received = true;
	}

	public bool IsKeyReceived(KeyColor keyColor)
	{
		if(!dict.ContainsKey(keyColor))
			return false;
		return dict[keyColor].received;
	}

	void Awake()
	{
		KeyCollection.instance = this;

		dict = new Dictionary<KeyColor, Key>();

		foreach(var key in colorKeys)
		{
			//if(key.enabled)
			dict.Add(key.keyColor, key);
			key.enabled = false;
		}

		var keys = GameObject.FindGameObjectsWithTag(Tags.Key);
		foreach(var key in keys)
		{
			if(key.activeSelf)
			{
				dict[key.GetComponent<DoorKey>().keyColor].enabled = true;
			}
		}

	}

	void OnGUI()
	{
		float totalWidth = 0;
		foreach(var key in dict.Values)
		{
			totalWidth += key.enabledKeyTextures.width * 0.5f;
		}

		//float startX  = (Screen.width - totalWidth)/2;
		float startX = 2.0f;
		float startY = 2.0f;

		int i=0;

		foreach(var key in dict.Values)
		{
			if(key.enabled)
			{
				GUI.DrawTexture(
					new Rect(
						startX + i * key.enabledKeyTextures.width * 0.5f,
						startY,
						key.enabledKeyTextures.width * 0.5f,
						key.enabledKeyTextures.height * 0.5f),
					key.received ? key.enabledKeyTextures : key.disabledKeyTextures);
				i++;
			}
		}
	}
}
