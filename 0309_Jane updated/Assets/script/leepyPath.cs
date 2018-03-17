using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class leepyPath : MonoBehaviour {
	public Transform player;
	Animator anim;

	private string state = "patrol";
	public GameObject[] wayPoints;
	private int currentWayPoint = 0;
	private float rotationspeed = 0.2f;
	public float speed;
	private float WayPointAccracy = 1f;
	public bool start;

	// Use this for initialization
	void Start () {
		state = "patrol";
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (start) {
			if (state == "patrol" && wayPoints.Length > 0) {
				anim.SetBool ("isWalking", true);
				anim.SetBool ("isIdle", false);
				if (Vector3.Distance (wayPoints [currentWayPoint].transform.position, transform.position) < WayPointAccracy) {
					currentWayPoint++;
					if (currentWayPoint >= wayPoints.Length) {
						anim.SetBool ("isWalking", false);
						anim.SetBool ("isIdle", true);
						start = false;
						return;
					}
				}
				// rotate towards wayPoints
				Vector3 direction = transform.position - wayPoints [currentWayPoint].transform.position;
				if (start && currentWayPoint < wayPoints.Length - 1) {
					this.transform.rotation = Quaternion.Slerp (transform.rotation,
						Quaternion.LookRotation (direction), rotationspeed * Time.deltaTime);
					this.transform.position = Vector3.MoveTowards (transform.position, wayPoints [currentWayPoint].transform.position, speed * Time.deltaTime);
				} 
			}
		}
	}
}
