using UnityEngine;
using System.Collections;

public class TextFloat : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		transform.position += Vector3.up * 5 * Time.deltaTime;
	}
}
