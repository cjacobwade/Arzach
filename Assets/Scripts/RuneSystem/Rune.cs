using UnityEngine;
using System.Collections;

public class Rune : MonoBehaviour 
{
	Vector3 initPos;
	bool doReset = false;

	// Use this for initialization
	void Start () 
	{
		initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!doReset)
		{
			//Only visible when in first person
			renderer.enabled = Player.Instance.firstPerson;

			//Only visible when nearby
			Color runeColor = renderer.material.color;
			runeColor.a = (255 - Vector3.Distance(transform.position, Player.Instance.transform.position)*1.7f)/255;
			renderer.material.color = runeColor;
		}
	}

	public IEnumerator Reset()
	{
		transform.parent = null;
		transform.position = initPos;
		renderer.enabled = false;
		collider.enabled = false;
		doReset = true;

		yield return new WaitForSeconds (3);

		renderer.enabled = Player.Instance.firstPerson;
		collider.enabled = true;
		doReset = false;
	}
}
