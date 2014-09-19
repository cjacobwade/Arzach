using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerHolder : MonoBehaviour 
{
	private static TriggerHolder _instance;
	public static TriggerHolder Instance
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
