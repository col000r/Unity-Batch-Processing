using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	
	public Vector3 axis = Vector3.forward;
	public float speed = 1f;
	
	void Update () {
		transform.localEulerAngles += axis * speed * Time.deltaTime;  
	}
}
