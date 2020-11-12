using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDefense : Defense {
    
    
	// Use this for initialization
	void Start () {
		popValue = 1;
        type = "Wall";
        baseDamage = 0;
        range = 0;
	}
	
	// Update is called once per frame
	void Update () {
		updateHealthText();
	}
    
    
}
