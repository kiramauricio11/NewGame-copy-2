using UnityEngine;
using System.Collections;

public class AI_mob : MonoBehaviour {

	public float speed = 3.0f;
	public float rotationSpeed = 5.0f;
	private float normRange = 15.0f;
	public float attackRange = 30.0f;
	public float shootAngle = 4.0f;
	public float dontComeCloserRange = 5.0f;
	public float delayHitTime = 0.35f;
	public float pickNextWaypointDistance = 2.0f;
	public GameObject[] targets;
	public Transform target;
	public float HitDistance = 0.0f;
	public float damage = 0.0f;
	public AudioClip attack;
	private float lastShot = -10.0f;
	public GameObject anim;
	private bool hunting;
	[HideInInspector]
	public bool canAttack;
	private bool looking;

	public string idleAnim;
	public string walkAnim;
	public string attackAnim;

	public LayerMask mask;

	private CharacterController c;
	[HideInInspector]
	public Animation a;
	private bool isMoving;
	private Vector3 oldPos;
	// Use this for initialization
	public virtual void Start () {
		c = GetComponent<CharacterController>();
		a = anim.GetComponent<Animation>();
		StartCoroutine(LowUpdate());
		normRange = attackRange;
		canAttack = true;
		StartCoroutine(DownToGround ());
	}
	
	// Update is called once per frame
	IEnumerator LowUpdate () { while(true){
		LUopen ();
		yield return new WaitForSeconds(0.04f);
	}}

	public virtual void LUopen ()
	{
		if(hunting == true)
			AttackPlayer();
		else
			Patrol();

		Vector3 tempPos = transform.position;
		if((tempPos - oldPos).sqrMagnitude < 0.0001)
			isMoving = false;
		else
			isMoving = true;
		oldPos = tempPos;

		if(isMoving == true)
			a.CrossFade (walkAnim);
		else
			a.CrossFade (idleAnim);
	}

	void Patrol ()
	{
		target = null;
		target = lookForTarget();
		if(target != null)
			hunting = true;
	}

	public virtual void GetShot()
	{
		if(attackRange == normRange)
			attackRange = attackRange * 3;
	}

	public virtual void AttackPlayer () {
		Vector3 lastVisiblePlayerPosition = target.position;
			if (CanSeeTarget (target) && canAttack == true) {
				// Target is dead - stop hunting
				if (target == null)
					return;
				
				// Target is too far away - give up   
				float distance = Vector3.Distance(transform.position, target.position);
				if (distance > attackRange)
					return;
				
				if (distance > dontComeCloserRange)
					MoveTowards (lastVisiblePlayerPosition);
				else
					RotateTowards(lastVisiblePlayerPosition);
				
				Vector3 forward = transform.TransformDirection(Vector3.forward);
				Vector3 targetDirection = lastVisiblePlayerPosition - transform.position;
				targetDirection.y = 0;
				
				float angle = Vector3.Angle(targetDirection, forward);
				
				// Start shooting if close and play is in sight
				if (distance < HitDistance && angle < shootAngle)
					StartCoroutine(Attack());
			} else {
				if(looking == false)
					StartCoroutine(SearchPlayer(lastVisiblePlayerPosition));
			}
	}

	IEnumerator Attack () {
		// Start shoot animation
		//   animation.CrossFade("hit", 0.3);
		a.Play (attackAnim);
		yield return new WaitForSeconds(delayHitTime);
		target.SendMessageUpwards("Damage", damage);
		// Wait until half the animation has played
		
		//   AudioSource.PlayClipAtPoint(attack, transform.position);
		// Fire gun
		//BroadcastMessage("Fire");
		
		
		// Wait for the rest of the animation to finish
		//  yield WaitForSeconds(animation["hit"].length - delayShootTime);
		yield return new WaitForSeconds(0.2f);
		//  animation.CrossFade("hit", 0.3);
	}

	IEnumerator SearchPlayer (Vector3 position) {
		looking = true;
		// Run towards the player but after 3 seconds timeout and go back to Patroling
		float timeout = 10.0f;
		while (timeout > 0.0f) {
			MoveTowards(position);

			timeout -= Time.deltaTime;
			yield return new WaitForSeconds(1);
		}
		if(!CanSeeTarget(target))
			hunting = false;
		looking = false;
	}

	Transform lookForTarget () {
		targets = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject t in targets)
		{
			float distance = (transform.position - t.transform.position).sqrMagnitude;
			if(distance < attackRange)
			{
				if(CanSeeTarget(t.transform))
					return t.transform;
			}
		}
		return null;
	}

	bool CanSeeTarget (Transform t) {
		RaycastHit hit;
		if (Physics.Linecast (transform.position, t.position, out hit, mask))
			return true;
		return false;
	}

	void RotateTowards (Vector3 position) {
		//   SendMessage("SetSpeed", 0.0);
		
		Vector3 direction = position - transform.position;
		direction.y = 0;
		if (direction.magnitude < 0.1)
			return;

		// Rotate towards the target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}
	
	void MoveTowards (Vector3 position) {
		Vector3 direction = position - transform.position;
		direction.y = 0;
		if (direction.magnitude < 0.5) {
			//a.CrossFade (idleAnim);
			return;
		}

		//a.CrossFade (walkAnim);
		
		// Rotate towards the target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		
		// Modify speed so we slow down when we are not facing the target
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		float speedModifier = Vector3.Dot(forward, direction.normalized);
		speedModifier = Mathf.Clamp01(speedModifier);
		
		// Move the character
		direction = forward * speed * speedModifier;
		c.SimpleMove(direction);
	}

	IEnumerator DownToGround ()
	{
		float tt = 0.4f;
		while(tt > 0)
		{
			tt -= Time.deltaTime;
			yield return new WaitForSeconds(0.02f);
			c.SimpleMove(transform.position);
		}
	}
}
