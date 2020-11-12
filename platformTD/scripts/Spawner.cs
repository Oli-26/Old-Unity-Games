using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Spawner : MonoBehaviour {

    public GameObject leftSpawner;
    public GameObject rightSpawner;
    public GameObject waveCounterText;
    public GameObject waveSymbol;
    public Sprite inProgress;
    public Sprite build;
    
    public GameObject waveProgress;
    Vector3 initialPosition;
    
    public int waveNumber = 1;
    
    float spawnTimer = 15f;
    float sTimer = 5f;
    int numberOfSpawns;
    
    float inWaveTimer = 0.4f;
    float iwTimer = 0f;
    float iwCount = 0f;
    
    public GameObject skeleton;
    public GameObject warlock;
    public GameObject ogre;
    public GameObject bomberChest;
    
    bool waveInProgress = false;
    
    
	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
        if(waveInProgress == false && GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
            waveCounterText.GetComponent<TextMesh>().color = new Color(50f/255f, 255f/255f, 50f/255f);
            waveSymbol.GetComponent<SpriteRenderer>().sprite = build;
        }
        
        waveCounterText.GetComponent<TextMesh>().text = "Wave " + waveNumber;
        if(waveInProgress){
            if(sTimer > 0){
                sTimer -= Time.deltaTime;
            }else{
                sTimer = spawnTimer;
                
                // Spawn enemy
                switch(waveNumber){
                    case 1:
                        if(numberOfSpawns % 2 == 0){
                            Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                            Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        }else{
                            Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                            Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        }
                        numberOfSpawns++;
                        break;
                    case 2:
                        if(numberOfSpawns % 2 == 0){
                            Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                            Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                            Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        }else{
                            Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                            Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                            Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        }
                        numberOfSpawns++;
                        break; 
                    case 5:
                       
                        Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                        Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                        
                   
                        numberOfSpawns++;
                        break; 
                    case 6:
                        Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                        Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                        
                   
                        
                        if(numberOfSpawns % 3 == 0){
                            Instantiate(bomberChest, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                            Instantiate(bomberChest, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                        }
                        numberOfSpawns++;
                        break;
                    case 7:
                        Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                        Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position- new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position+ new Vector3(0.2f,0f,0f), Quaternion.identity);

                        
                        if(numberOfSpawns % 3 == 0){
                            Instantiate(bomberChest, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                            Instantiate(bomberChest, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                        }
                        numberOfSpawns++;
                        break;    
                    case 8:
                        Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                        Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position- new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position+ new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position- new Vector3(0.3f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position+ new Vector3(0.3f,0f,0f), Quaternion.identity);
                   
                        
                        if(numberOfSpawns % 3 == 0){
                            Instantiate(bomberChest, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                            Instantiate(bomberChest, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                        }
                        numberOfSpawns++;
                        break;  
                    case 9:
                        Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                        Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position- new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position+ new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position- new Vector3(0.3f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position+ new Vector3(0.3f,0f,0f), Quaternion.identity);
                   
                        
                        if(numberOfSpawns % 2 == 0){
                            Instantiate(bomberChest, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                            Instantiate(bomberChest, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                        }
                        numberOfSpawns++;
                        break; 
                    case 10:
                        Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                        Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, rightSpawner.transform.position- new Vector3(0.2f,0f,0f), Quaternion.identity);
                        Instantiate(skeleton, leftSpawner.transform.position+ new Vector3(0.2f,0f,0f), Quaternion.identity);

                   
                        
                        if(numberOfSpawns % 3 == 0){
                            Instantiate(bomberChest, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                            Instantiate(bomberChest, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                            Instantiate(bomberChest, leftSpawner.transform.position + new Vector3(0.3f,0f,0f), Quaternion.identity);
                            Instantiate(bomberChest, rightSpawner.transform.position - new Vector3(0.3f,0f,0f), Quaternion.identity);
                        }
                        numberOfSpawns++;
                        break;
                    default:
                        if(numberOfSpawns % 2 == 0){
                            Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                            Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                            Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                            Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                        }else{
                            Instantiate(skeleton, leftSpawner.transform.position, Quaternion.identity);
                            Instantiate(warlock, rightSpawner.transform.position - new Vector3(0.1f,0f,0f), Quaternion.identity);
                            Instantiate(skeleton, rightSpawner.transform.position, Quaternion.identity);
                            Instantiate(warlock, leftSpawner.transform.position + new Vector3(0.1f,0f,0f), Quaternion.identity);
                        }
                        numberOfSpawns++;
                          
                        break;
                    
                    
                }
                
                
                waveProgress.GetComponent<Slider>().value = ((float)numberOfSpawns)/(Mathf.Round(1.2f*waveNumber)+1);
            }
            if(numberOfSpawns > Mathf.Round(1.2f*waveNumber) + 1){
                if(waveNumber == 5 ){
                    Instantiate(ogre, leftSpawner.transform.position, Quaternion.identity);
                    Instantiate(ogre, rightSpawner.transform.position, Quaternion.identity);
                    Instantiate(bomberChest, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                    Instantiate(bomberChest, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                }
                if(waveNumber == 10 ){
                    Instantiate(ogre, leftSpawner.transform.position, Quaternion.identity);
                    Instantiate(ogre, rightSpawner.transform.position, Quaternion.identity);
                    Instantiate(ogre, leftSpawner.transform.position+ new Vector3(0.1f,0f,0f), Quaternion.identity);
                    Instantiate(ogre, rightSpawner.transform.position- new Vector3(0.1f,0f,0f), Quaternion.identity);
                    Instantiate(bomberChest, rightSpawner.transform.position - new Vector3(0.2f,0f,0f), Quaternion.identity);
                    Instantiate(bomberChest, leftSpawner.transform.position + new Vector3(0.2f,0f,0f), Quaternion.identity);
                }
                waveInProgress = false;
                
            }
        }
	}
    
    public void startNextWave(){
        if(waveInProgress == false){
            numberOfSpawns = 0;
            waveProgress.GetComponent<Slider>().value = ((float)numberOfSpawns)/(Mathf.Round(1.2f*waveNumber)+1);
            sTimer = 5f;
            waveNumber++;
            waveInProgress = true;
            waveCounterText.GetComponent<TextMesh>().color = new Color(250f/255f, 50f/255f, 50f/255f);
            waveSymbol.GetComponent<SpriteRenderer>().sprite = inProgress;
        }
        
    }
}
