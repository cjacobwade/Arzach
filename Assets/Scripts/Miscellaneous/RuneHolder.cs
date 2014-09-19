using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneHolder : MonoBehaviour 
{
	private static RuneHolder _instance;
	public static RuneHolder Instance
	{
		get{ return _instance; }
		set{ _instance = value; }
	}

	// Use this for initialization
	void Start () 
	{
		_instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void SetChildrenActive(bool enable)
	{
		foreach(Transform child in transform)
			child.gameObject.SetActive(enable);
	}
}
