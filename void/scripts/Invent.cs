using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invent : MonoBehaviour {
    public Item[] invent = new Item[5];
    public bool[] inventOccupied = new bool[5];
	// Use this for initialization
	void Start () {
		inventOccupied[0] = true;
        inventOccupied[1] = false;
        inventOccupied[2] = false;
        inventOccupied[3] = false;
        inventOccupied[4] = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public bool checkSlot(int slot){
        if(slot<0 || slot > 4){
            return false;
        }
        return inventOccupied[slot];
    }
    public Item getSlot(int slot){
        if(slot<0 || slot > 4 || inventOccupied[slot] == false){
            return null;
        }
        return (invent[slot]);
    }
    
    public void emptySlot(int slot){
        if(slot<0 || slot > 4){
            return;
        }
        invent[slot] = null;
        inventOccupied[slot] = false;
    }
    
    public void setSlot(int slot, Item item){
        if(slot < 0 || slot > 4){
            return;
        }
        invent[slot] = item;
        inventOccupied[slot] = true;
    }
    
    public void exchangeSlot(int slot1, int slot2){
        Item temp = invent[slot1];
        setSlot(slot1, invent[slot2]);
        setSlot(slot2, temp);
    }
}
