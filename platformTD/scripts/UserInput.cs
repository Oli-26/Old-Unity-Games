using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {
    public GameObject spawner;
    public GameObject characterMenu;
    public GameObject UITab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
        Building();
        Other();
        UITab.SetActive(true);
	}
    
    void Movement(){
        if(Input.GetKeyDown("w")){ // Jump
            GetComponent<Character>().setDirection("none");
            GetComponent<Character>().Jump();
        }
        if(Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            GetComponent<Character>().setMovement(false);
        }
        if(Input.GetKey("a")){
            GetComponent<Character>().setDirection("left");
            GetComponent<Character>().setMovement(true);            
        }
        if(Input.GetKey("d")){
            GetComponent<Character>().setDirection("right");
            GetComponent<Character>().setMovement(true); 
        }
    }
    void Building(){
        if(Input.GetKeyDown("2")){
            GetComponent<Character>().createDefense("2");
            
        }
        if(Input.GetKeyDown("1")){
            GetComponent<Character>().createDefense("1");
        }
        if(Input.GetKeyDown("3")){
            GetComponent<Character>().createDefense("3");
        }
        if(Input.GetKeyDown("e")){
           GetComponent<Character>().finishDefense();
        }
        if(Input.GetKeyDown("q")){
            GetComponent<Character>().selectDefense();
        }
        if(Input.GetKeyDown("r")){
            GetComponent<Character>().rotateDefense();
        }
        if(Input.GetKeyDown("z")){
            GetComponent<Character>().cancleDefense();
        }

    }
    
    void Other(){
        if(Input.GetKeyDown("g")){
            spawner.GetComponent<Spawner>().startNextWave();
            
        }
        if(Input.GetMouseButtonDown(0)){
            GetComponent<Character>().swingWeapon();
        }
        if(Input.GetMouseButtonDown(1)){
            GetComponent<Character>().chargeWeapon(true);
        }
        if(Input.GetMouseButtonUp(1)){
            GetComponent<Character>().chargeWeapon(false);
        }
        if(Input.GetKeyDown("c")){
            characterMenu.SetActive(true);
        }
        
    }
}
