using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour 
{
	public Transform[] chainLinks;
	LineRenderer lr;
	
	void Start()
	{
		lr = GetComponent<LineRenderer>();	
		lr.SetVertexCount(chainLinks.Length);
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i = 0; i < chainLinks.Length; i++)
		{
			lr.SetPosition(i, chainLinks[i].position);
		}
	}
}
