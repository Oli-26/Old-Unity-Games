using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaShot : MonoBehaviour {
    public float damage = 1f;
    public int remainingHits = 1;
	// Use this for initialization
	void Start () {
		Destroy(gameObject, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += -2*transform.right*Time.deltaTime;
        
	}
    
    public void setDamage(float d){
        damage = d;
    }
    
    public void setHits(int h){
        remainingHits = h;
    }
    public void reduceHits(){
        if(remainingHits == 1){
            Destroy(gameObject);
            return;
        }
        remainingHits--;
    }
}
