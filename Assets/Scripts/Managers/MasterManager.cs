using UnityEngine;
using System.Collections;

public class MasterManager : MonoBehaviour 
{
	public GameObject[] managers;

	// Use this for initialization
	void Start () 
	{
		//Use this model for spawning managers
		//That way we can start in any scene and have what we need
		foreach(GameObject manager in managers)
		{
			GameObject tmpObj;

			//Check if what we want exists already
			tmpObj = GameObject.Find(manager.name);

			//If not, make it
			if(!tmpObj) 
			{
				tmpObj = (GameObject)Instantiate(manager);
				tmpObj.name = manager.name;
			}
		}
	}
}
