using UnityEngine;
using System.Collections;

public enum AnimType
{
	Legacy,
	Generic
}

public class DistanceAnim : MonoBehaviour 
{
	public Vector2 minMaxDist;

	//For target distance
	public Transform target;

	//For unity anims
	Animator animator;
	public AnimType animType = AnimType.Legacy;
	public string animName;


	// Use this for initialization
	void Start () 
	{
		if(animType == AnimType.Legacy)
		{
			animation [animation.clip.name].speed = 0.0f;
			animation.Play ();
		}
		else
		{
			animator = GetComponent<Animator>();
			animator.speed = 0.0f;
			animator.Play (animName);
		}
	}
	
	// Update is called once per frame
	void Update ()         
	{
		//Find distance
		float currentDist = Vector3.Distance (target.position, Player.Instance.transform.position);
		currentDist = Mathf.Clamp (currentDist, minMaxDist.x, minMaxDist.y);

		if(animType == AnimType.Legacy)
		{
			//Find anim point
			float animTime = animation[animation.clip.name].length - (animation [animation.clip.name].length * currentDist) / (minMaxDist.y - minMaxDist.x);

			animTime = Mathf.Clamp (animTime, 0, animation [animation.clip.name].length);

			animation [animation.clip.name].time = animTime;
			animation.Sample ();
		}
		else
		{
			//Find anim point
			float animLength = animator.GetCurrentAnimationClipState(0).Length;
			float animTime = animLength - (animLength * currentDist) / (minMaxDist.y - minMaxDist.x);

			animTime = Mathf.Clamp (animTime, 0, animLength);

			animator.Play(animName, 0, animTime);
		}
	}
}
