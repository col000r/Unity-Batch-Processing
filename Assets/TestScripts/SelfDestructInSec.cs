using UnityEngine;
using System.Collections;

public class SelfDestructInSec : MonoBehaviour {
	
	public float sec = 3f;
	
	IEnumerator Start () {
		yield return new WaitForSeconds(sec);
		Destroy(gameObject);
	}

	
}
