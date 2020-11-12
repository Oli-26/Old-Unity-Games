using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public bool active = false;

    
    public float h;
    public float g;
    
    private int x;
    private int y;
    
    public List<Node> shortestPath;
    public float shortestPathValue = 1000f;
    
    GameObject platform;
    GameObject tower;
    bool towerExists = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void reset(){
        shortestPath = null;
        shortestPathValue = 1000f;
        h = 0;
        g = 0;
    }
    public float getValue(){
        return g + h;    
    }
    
    public void setPos(int xPos, int yPos){
        x = xPos;
        y = yPos;
        
    }
    
    public int getX(){
        return x;
    }
    
    public int getY(){
        return y;
    }
    
    public void addPlatform(GameObject p){
        platform = p;
        active = true;
    }
    public GameObject getPlatform(){
        return platform;
    }
    public void removePlatform(){
        platform = null;
        active = false;
    }
    
    public void addTower(GameObject ob){
        if(towerExists){
            return;
        }
        tower = ob;
    }
    
    public void removeTower(){
        Destroy(tower);
        tower = null;
    }
}
