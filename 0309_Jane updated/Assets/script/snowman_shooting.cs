using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowman_shooting : MonoBehaviour {

	public float speed;
	private Transform player; 
	private Vector3 target;

	void Start () {
		player = GameObject.FindGameObjectWithTag("leepy").transform;
		target = new Vector3(player.position.x, player.position.y, player.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
		if(transform.position.x == target.x && transform.position.z == target.z && transform.position.y == target.y ){
			DestroyProjectile();
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "leepy") {
			DestroyProjectile();
			//// To do: when the ball hit leept, add health bar 
		}
	}

	void DestroyProjectile(){
		Destroy(gameObject);
	}
}
