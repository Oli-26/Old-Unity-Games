using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTag : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 1.8f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(0f, 0.1f*Time.deltaTime, 0f);
	}
}
