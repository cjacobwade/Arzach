using UnityEngine;
using System.Collections;

public static class Utilities
{
	public static void PlayAnimation(this Animation animation, string clip, float speed, float fade)
	{
		animation[clip].speed = speed;
		animation[clip].enabled = true;
		animation.Sample();
		animation.CrossFade(clip,fade);
	}

	public static void PlayAnimation(this Animation animation, string clip, float speed)
	{
		animation[clip].speed = speed;
		animation.CrossFade(clip, .2f);
	}

	public static void PlayAnimation(this Animation animation, string clip)
	{
		animation.CrossFade(clip,.2f);
	}
}
