using UnityEngine;
using System.Collections.Generic;

public class ChainGenerator : MonoBehaviour {

	public GameObject chainPrefab;
	public int numberOfChain = 3;

	[ContextMenu("Generate Chains")]
	void GenerateChains()
	{
		var count = transform.childCount;
		while(count-- > 0)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}

		var chainList = new List<GameObject>();
		for(int i=0; i<numberOfChain; i++)
		{
			var chain = (GameObject) Instantiate(chainPrefab);
			chainList.Add(chain);
			//var height = chain.gameObject.GetComponentInChildren<SpriteRenderer>().sprite.rect.height;
			chain.name = chainPrefab.name + i.ToString("00");
			chain.transform.parent = this.transform;
			var pos = chain.transform.localPosition;
			pos.y = -i*0.75f;
			chain.transform.localPosition = pos;

			if(i > 0)
			{
				var hingeJoint = chain.gameObject.AddComponent<HingeJoint2D>();
				hingeJoint.connectedBody = chainList[i-1].rigidbody2D;
				var anchor = hingeJoint.anchor;
				anchor.y = 0.65f;
				hingeJoint.anchor = anchor;
				hingeJoint.collideConnected = true;
			}
			else
			{
				chain.rigidbody2D.isKinematic = true;
			}
		}
	}

	void Start () {
		//GenerateChains();
	}
}
