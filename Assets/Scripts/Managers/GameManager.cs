using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	private static GameManager _instance;
	public static GameManager Instance
	{
		get{ return _instance; }
		set{ _instance = value; }
	}

	public LevelInfo[] levels;
	LevelInfo currentLevel;

	bool playerSetup = true, levelSetup = false;

	void Start()
	{
		_instance = this;

		DontDestroyOnLoad(gameObject);

		//Figure out which level we're starting on
		foreach(LevelInfo level in levels)
		{
			if(Application.loadedLevelName == level.name)
				currentLevel = level;
		}
	}

	void Update()
	{
		if(!Application.isLoadingLevel)
		{
			if(!playerSetup)
			{
				Player.Instance.Setup(currentLevel);
				playerSetup = true;
			}

			if(!levelSetup )
			{
				if(currentLevel.name == "Desert")
					RuneHolder.Instance.SetChildrenActive(true);
				else
					RuneHolder.Instance.SetChildrenActive(false);
				
				if(currentLevel.name == "Tower")
					TriggerHolder.Instance.SetChildrenActive(true);
				else
					TriggerHolder.Instance.SetChildrenActive(false);
			}
		}
	}

	// Jump to a specific level
	public void ChangeLevel(int nextLevel)
	{
		currentLevel.lastPos = Player.Instance.lastGroundPos;
		currentLevel.lastCamPos = Player.Instance.cam.position;
		currentLevel.lastCamRot = Player.Instance.cam.rotation;

		currentLevel = levels[nextLevel];
		Application.LoadLevel(currentLevel.name);
		playerSetup = false;
	}
}

[System.Serializable]
public class LevelInfo
{
	public string name;
	internal Vector3? lastPos;
	internal Vector3 lastCamPos;
	internal Quaternion lastCamRot;
}
