using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += Vector3.up*2*Time.deltaTime;
	}
}
