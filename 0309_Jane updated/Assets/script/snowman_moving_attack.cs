using System.Collections;
using UnityEngine;

public class snowman_moving_attack : MonoBehaviour {
	public float speed;
	public float stopDistance;
	public float retreatDistance;
	public Transform player;
	private float timeBtwShots;
	public float startTimeBtwShot;
	public GameObject projectile;
	public float rotaiondamping;

	void Start(){
		player = GameObject.FindGameObjectWithTag("shadow").transform;
		timeBtwShots = startTimeBtwShot;
	}

	void Update(){

		if(Vector3.Distance(transform.position, player.position) > stopDistance){
			transform.position = this.transform.position;
		} 
		else if (Vector3.Distance(transform.position, player.position) < stopDistance && Vector3.Distance(transform.position, player.position) > retreatDistance){
			lookAtPlayer();
			transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
		}
		else if (Vector3.Distance(transform.position, player.position) < retreatDistance){
			lookAtPlayer();
			transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
		}

		if (timeBtwShots <=0){
			Instantiate(projectile, transform.position, transform.rotation);
			timeBtwShots = startTimeBtwShot;
		}
		else
		{
			timeBtwShots -= Time.deltaTime;
		}
	}

	void lookAtPlayer(){
		
		Quaternion rotation = Quaternion.LookRotation(player.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotaiondamping);
	}
	
}
