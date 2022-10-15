using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject player;
	// Update is called once per frame
	void Update()
    {
		if (player.transform.position.x >= transform.position.x + 10)
			transform.position = new Vector3(transform.position.x + 20, transform.position.y, transform.position.z);
		if (player.transform.position.x <= transform.position.x - 10)
			transform.position = new Vector3(transform.position.x - 20, transform.position.y, transform.position.z);
		if (player.transform.position.y >= transform.position.y + 7)
			transform.position = new Vector3(transform.position.x, transform.position.y + 14, transform.position.z);
		if (player.transform.position.y <= transform.position.y - 7)
			transform.position = new Vector3(transform.position.x, transform.position.y - 14, transform.position.z);
	}
}
