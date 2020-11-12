using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement; 

public class GenButton : MonoBehaviour {
    public string type = "play";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    
    public void Clicked(){
        if(type == "play"){
            SceneManager.LoadScene("SampleScene");
        }
        if(type == "mainmenu"){
            SceneManager.LoadScene("Menu");
        }
    }

}
