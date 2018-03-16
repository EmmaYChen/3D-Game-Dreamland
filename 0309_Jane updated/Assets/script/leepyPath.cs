using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class leepyPath : MonoBehaviour {
	public Transform player;
	public Transform head;
	Animator anim;

	private string state = "patrol";
	public GameObject[] wayPoints;
	private int currentWayPoint = 0;
	private float rotationspeed = 0.2f;
	private float speed = 1.5f;
	private float WayPointAccracy = 1.0f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = player.position - this.transform.position;
		direction.y = 0;

		if(state == "patrol" && wayPoints.Length > 0){
			anim.SetBool("isWalking",true);
			anim.SetBool("isIdle",false);
			if (Vector3.Distance(wayPoints[currentWayPoint].transform.position, transform.position) < WayPointAccracy){
				currentWayPoint ++;
				if (currentWayPoint >= wayPoints.Length){
					anim.SetBool("isWalking", false);
					anim.SetBool("isIdle", true);
					state = "relax";
				}
			}
			// rotate towards wayPoints
			direction = transform.position - wayPoints[currentWayPoint].transform.position ;
			this.transform.rotation = Quaternion.Slerp(transform.rotation,
										Quaternion.LookRotation(direction), rotationspeed * Time.deltaTime);
			this.transform.Translate(0, 0, Time.deltaTime * speed);
		}
	}
}
