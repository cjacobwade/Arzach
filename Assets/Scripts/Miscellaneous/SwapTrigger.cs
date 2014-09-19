using UnityEngine;
using System.Collections;

public class SwapTrigger : MonoBehaviour 
{
	public int targetLevel;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			GameManager.Instance.ChangeLevel(targetLevel);
			gameObject.SetActive(false);
		}
	}
}
