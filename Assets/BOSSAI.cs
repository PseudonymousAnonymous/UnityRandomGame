using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSAI : MonoBehaviour
{
	
	public GameObject player;
	
	public float bossSpeed = 50.0f;
	
	private Vector3 directionToMove;
	
	private Vector3 playerPosition;
	
	private float maxDistance;
	

    void Update()
    {	
		
		playerPosition = player.transform.position;
		
		directionToMove = playerPosition - transform.position;
		
		
		
		maxDistance = Vector3.Distance(transform.position, playerPosition);
		
		
		directionToMove = directionToMove.normalized * Time.deltaTime * bossSpeed;
		
		
		transform.position = transform.position + Vector3.ClampMagnitude(new Vector3(directionToMove.x * maxDistance, directionToMove.y * maxDistance, directionToMove.z * maxDistance), maxDistance * maxDistance );
    }
}
