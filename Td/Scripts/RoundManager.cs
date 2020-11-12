using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {
    
    int roundNumber = 1;
    public GameObject enemy1;
    List<GameObject> enemySpawnList;
    float enemySpawnRate;
    float enemySpawnCooldown;
    
	// Use this for initialization
	void Start () {
		enemySpawnList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if(enemySpawnList.Count != 0){
            enemySpawnCooldown -= Time.deltaTime;
            if(enemySpawnCooldown <= 0){
                spawnEnemy(enemySpawnList[0]);
                enemySpawnList.Remove(enemySpawnList[0]);
                enemySpawnCooldown = enemySpawnRate;
            }
            
        }
        
	}
    
    public void startRound(){
        if(!GetComponent<GameManager>().startSet || !GetComponent<GameManager>().endSet){
            Debug.Log("Start or end point not set");
            return;
        }
        if(!GetComponent<Grid>().Search(GetComponent<GameManager>().startNode, GetComponent<GameManager>().endNode)){
            Debug.Log("No valid path to start round");
            return;
        }
        enemySpawnCooldown = enemySpawnRate;
        switch(roundNumber){
            case 1:
                
                enemySpawnList.Add(enemy1);
                
                
                break;
            default:
                break;
            
            
        }
    }
    
    void spawnEnemy(GameObject enemy){
        Node spawnNode;
        if(GetComponent<GameManager>().startSet){
            spawnNode = GetComponent<GameManager>().startNode;
        }else{
            return;
        }
        
        GameObject newEnemy;
        newEnemy = Instantiate(enemy, new Vector3(spawnNode.getX(), spawnNode.getY(), -1), Quaternion.identity);
        
        newEnemy.GetComponent<Enemy>().gameManager = gameObject;
        newEnemy.GetComponent<Enemy>().Move(new Vector3(GetComponent<GameManager>().endNode.getX(), GetComponent<GameManager>().endNode.getY(), -1f));
        
    }
}
