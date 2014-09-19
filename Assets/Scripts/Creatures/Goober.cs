using UnityEngine;
using System.Collections;
using Pathfinding;

public class Goober : MonoBehaviour 
{
	AIPath aiPath;
	public Transform[] path;
	int currentTarget = 0;
	
	bool seek = false;
	
	// Update is called once per frame
	void Update () 
	{
		if(animation.isPlaying && !seek)
		{
			aiPath = GetComponent<AIPath>();
			aiPath.target = path[currentTarget];
			seek = true;
		}
		
		if(seek)
			CheckPath();
	}
	
	void CheckPath()
	{	
		if(Mathf.Abs(Vector2.Distance(path[currentTarget].position, transform.position)) < 5)
		{
			if(currentTarget != path.Length-1)
			{
				currentTarget++;
				//seeker.GetNewPath(transform.position, path[currentTarget].position);
				aiPath.target = path[currentTarget];
			}
			else if(!animation["Hover"].enabled)
			{
				animation.PlayAnimation("Hover");	
			}
		}
	}
}
