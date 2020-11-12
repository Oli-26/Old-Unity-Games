using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    float spawnCooldownReset = 15f; //seconds
    float spawnCooldown = 0;
    
    public GameObject groupPrefab;
    
    public GameObject cargoShip;
    
    float width = 5f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(gameObject);
		if(spawnCooldown > 0){
            spawnCooldown -= Time.deltaTime;
        }else{
            spawnGroup();
            spawnCooldown = spawnCooldownReset;
        }
        
	}
    
    void spawnGroup(){
        Vector3 position = choosePosition();
        Vector3 dest = chooseDestination(position);
        int type = 1;
        
        
        GameObject newGroup = Instantiate(groupPrefab, position, Quaternion.identity);
        Group groupScript = newGroup.GetComponent<Group>();
        
        switch(type){
            case 1:
                    GameObject newShip = Instantiate(cargoShip, position, Quaternion.identity);
                    groupScript.addObject(newShip);
                    newShip.GetComponent<CargoShip>().setDestination(dest);
                    newGroup.transform.parent = newShip.transform;
                    newGroup.SetActive(true);
                    groupScript.setTarget(newShip);
                    break;
            default:
                    break;
            
        }
        
    }
    
    Vector3 choosePosition(){
        GameObject playerShip = gameObject.GetComponent<UserInput>().playerShip;
        
        float xOffset = Random.Range(-15f, 15f);
        float yOffset = Random.Range(-15f,15f);
        while(Mathf.Abs(xOffset) < 3f){
            xOffset = Random.Range(-15f, 15f);
        }
        while(Mathf.Abs(yOffset) < 3f){
            yOffset = Random.Range(-15f, 15f);
        }
        return (playerShip.transform.position + new Vector3(xOffset,yOffset,0));
    }
    
    Vector3 chooseDestination(Vector3 pos){
        float xOffset = Random.Range(-15f, 15f);
        float yOffset = Random.Range(-15f,15f);
        while(Mathf.Abs(xOffset) < 3f){
            xOffset = Random.Range(-15f, 15f);
        }
        while(Mathf.Abs(yOffset) < 3f){
            yOffset = Random.Range(-15f, 15f);
        }
        return pos + new Vector3(xOffset, yOffset, 0);
    }
}
