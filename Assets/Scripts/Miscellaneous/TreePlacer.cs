using UnityEngine;
using System.Collections;

public class TreePlacer : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(SetSpawn());
	}

	IEnumerator SetSpawn()
	{
		yield return new WaitForSeconds(Random.Range(0, 0.5f));

		RaycastHit hit;
		if(Physics.Raycast(transform.position, -transform.up, out hit))
		{
			transform.position = hit.point;
			transform.rotation = Quaternion.Euler(hit.normal);
		}
	}
}
