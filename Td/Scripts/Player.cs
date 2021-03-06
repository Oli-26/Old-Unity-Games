﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameObject gameManager;
    List<Node> path ;
    bool moving = false;
	// Use this for initialization
	void Start () {
		
        
        
        
	}
	
	// Update is called once per frame
	void Update () {
		if(moving){
            
            if(path.Count == 0){
                moving = false;
            }else{
                if(Vector3.Distance(transform.position, new Vector3((int)path[0].getX() , (int)path[0].getY() , 0)) < 0.2){
                    path.Remove(path[0]);
                }
                if(path.Count == 0){
                    moving = false;
                }else{
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3((int)path[0].getX() , (int)path[0].getY() , 0), 0.32f*Time.deltaTime);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)){

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Move(targetPosition);
        }
	}
    
    
    void Move(Vector3 pos){
        
        bool bExists = false;
        bool aExists = false;
        Node b = gameManager.GetComponent<Grid>().findClosestNode(pos, ref bExists);
        Node a = gameManager.GetComponent<Grid>().findClosestNode(transform.position, ref aExists);
        if(aExists && bExists){
            bool found = gameManager.GetComponent<Grid>().Search(a,b);
            if(found){
                path = gameManager.GetComponent<Grid>().finalSearchPath;
                moving = true;
            }
        }else{
            if(!aExists){
                Debug.Log("A is null");
            }
            if(!bExists){
                Debug.Log("B is null");
            }
        }
        
    }
    
    void OnCollisionEnter2D(Collision2D col){
        Debug.Log("Entered col");
        if(col.transform.tag == "Skeleton"){
            takeDamage(5);
        }
    }
}
