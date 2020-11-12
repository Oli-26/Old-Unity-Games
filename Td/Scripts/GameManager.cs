using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    
    string mode = "none";
    
    
    public GameObject addButton;
    public GameObject removeButton;
    public GameObject startPosButton;
    public GameObject endPosButton;
    
    public GameObject platform;
    public GameObject tower1;
    
    
    
    public Node startNode = null;
    public Node endNode = null;
    public bool startSet = false;
    public bool endSet = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetMouseButtonDown(0)){
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            //if(IsPointerOverUIObject()){
            //    return;
            //}
            float clickedX = Mathf.Round(targetPosition.x);
            float clickedY = Mathf.Round(targetPosition.y);
            if(clickedX >= 0 && clickedX <= 19 && clickedY >= 0 && clickedY <= 19){
                //Debug.Log("[" + clickedX + "," + clickedY + "]");
                //Node n  = GetComponent<Grid>().grid[(int)clickedX, (int)clickedY];
                bool nExists = false;
                Node n = GetComponent<Grid>().findClosestNode(new Vector3(clickedX, clickedY, 0), ref nExists);
                
                if(mode == "add"){
                   if(!n.active){
                      n.addPlatform(Instantiate(platform, new Vector3(clickedX, clickedY, 0), Quaternion.identity));
                   }
                }
                
                if(mode == "remove" && nExists){
                    
                    GameObject p = n.getPlatform();
                    Destroy(p);
                    n.removeTower();
                    
                    n.removePlatform();
                }
                
                if(mode == "start" && nExists && n.active){
                   if(startSet){
                       startNode.getPlatform().GetComponent<SpriteRenderer>().color = Color.white;
                   }
                   startSet = true;
                   startNode = n;
                   n.getPlatform().GetComponent<SpriteRenderer>().color = Color.green;
                   
                }
                
                if(mode == "end" && nExists && n.active){
                    if(endSet){
                        endNode.getPlatform().GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    endSet = true;
                    endNode = n;
                    n.getPlatform().GetComponent<SpriteRenderer>().color = Color.red;
                }
                
               if(mode == "tower1" && nExists){
                   n.addTower(Instantiate(tower1, new Vector3(clickedX, clickedY, -1f), Quaternion.identity));
                   
               }
            }

        }
        
	}
    
    public void toggleAdd(){
        mode = "add";
        addButton.GetComponent<Image>().color = Color.red;
        removeButton.GetComponent<Image>().color = Color.white;
        startPosButton.GetComponent<Image>().color = Color.white;
        endPosButton.GetComponent<Image>().color = Color.white;
    }
    
    public void toggleRemove(){
        mode = "remove";
        addButton.GetComponent<Image>().color = Color.white;
        removeButton.GetComponent<Image>().color = Color.red;
        startPosButton.GetComponent<Image>().color = Color.white;
        endPosButton.GetComponent<Image>().color = Color.white;
    }    
    
    public void toggleStartPos(){
        mode = "start";
        addButton.GetComponent<Image>().color = Color.white;
        removeButton.GetComponent<Image>().color = Color.white;
        startPosButton.GetComponent<Image>().color = Color.red;
        endPosButton.GetComponent<Image>().color = Color.white;
    }
    
    public void toggleEndPos(){
        mode = "end";
        addButton.GetComponent<Image>().color = Color.white;
        removeButton.GetComponent<Image>().color = Color.white;
        startPosButton.GetComponent<Image>().color = Color.white;
        endPosButton.GetComponent<Image>().color = Color.red;
    }
    
    public void toggleTower1(){
        mode = "tower1";
        addButton.GetComponent<Image>().color = Color.white;
        removeButton.GetComponent<Image>().color = Color.white;
        startPosButton.GetComponent<Image>().color = Color.white;
        endPosButton.GetComponent<Image>().color = Color.white;
    }
    public void Go(){
        if(startSet && endSet){
            GetComponent<Grid>().Search(startNode, endNode);
        }
        
    }
}
