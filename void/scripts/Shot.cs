using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {
    public float damage = 0;
    GameObject owner;
	// Use this for initialization
	void Start () {
		Destroy(gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void setOwner(GameObject g){
        owner = g;
    }
}
