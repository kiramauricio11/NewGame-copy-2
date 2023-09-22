using UnityEngine;
using System.Collections;

public class AI_trol : AI_mob {
	private int buildup;
	public float stompRange;
	public string yellAnim;
	public AudioClip yellSound;
	public string bunkAnim;
	public AudioClip bunkSound;
	public int ybTime;
	public int baTime;
	private AudioSource audioC;

	public override void Start ()
	{
		base.Start();
		audioC = GetComponent<AudioSource>();
	}

	// Use this for initialization
	public override void GetShot () {
		buildup ++;
	}

	public override void LUopen ()
	{
		base.LUopen();
		if(buildup > 150)
		{
			base.canAttack = true;
			buildup = 0;
			StartCoroutine(SpecialAttack ());
		}
	}
	IEnumerator SpecialAttack ()
	{
		a.CrossFade (yellAnim);
		audioC.clip = yellSound;
		audioC.Play();
		yield return new WaitForSeconds(ybTime);
		a.CrossFade (bunkAnim);
		audioC.clip = bunkSound;
		audioC.Play();
		yield return new WaitForSeconds(baTime);
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject t in targets)
		{
			float distance = (transform.position - t.transform.position).sqrMagnitude;
			if(distance < stompRange)
			{
				//attack
			}
		}
		//do shit
	}
}