using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public Node[,] grid;
    const int size = 20;
    
    
    List<Node> openList;
    List<Node> closedList;
    public List<Node> finalSearchPath;
    
    public GameObject path;
    
    float finalSearchValue;
	// Use this for initialization
	void Start () {
		grid = new Node[size, size];
        for(int i = 0; i < size; i ++){
            for(int j = 0; j < size; j ++){
                grid[i,j] = new Node();
                grid[i,j].setPos(i,j);
            }
        }
        grid[10,10].addPlatform(Instantiate(path, new Vector3(10, 10, 0), Quaternion.identity));
        grid[11,10].addPlatform(Instantiate(path, new Vector3(9, 10, 0), Quaternion.identity));
        grid[10,11].addPlatform(Instantiate(path, new Vector3(10, 9, 0), Quaternion.identity));
        grid[11,11].addPlatform(Instantiate(path, new Vector3(9, 9, 0), Quaternion.identity));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void createTiles(){
        for(int i = 1; i < finalSearchPath.Count-1; i++){
            Node n = finalSearchPath[i];
            n.getPlatform().GetComponent<SpriteRenderer>().color = Color.blue;

            
        }
    }
    
    public bool Search(Node a, Node b){
        
        finalSearchValue = 10000f;
        finalSearchPath = null;
        for(int i = 0; i < size; i ++){
            for(int j = 0; j < size; j ++){
                grid[i,j].reset();
            }
        }
        openList = new List<Node>();
        closedList = new List<Node>();
        
        a.g = 0;
        a.h = distanceBetweenNodes(a, b);
        a.shortestPath = new List<Node>();
        a.shortestPathValue = 0;
        openList.Add(a);
        
        searchAux(b);
        
        if(finalSearchPath != null){
            return true;
        }else{
            return false;
        }
        
    }
    
    void searchAux(Node targetNode){
        Node currentNode;
        
        openList.Sort((n1,n2) => n1.getValue().CompareTo(n2.getValue()));
      
        currentNode = openList[0];
        
    
        
        List<Node> searchPath = currentNode.shortestPath;
        searchPath.Add(currentNode);
        
        openList.Remove(currentNode);
        
        
        List<Node> neighbors = findNeighbors(currentNode);
        
        for(int i =0; i < neighbors.Count; i++){
           
            if(!inList(openList, neighbors[i]) && !inList(closedList, neighbors[i])){
                
                neighbors[i].g = currentNode.g+1f;
                neighbors[i].h = distanceBetweenNodes(neighbors[i], targetNode);
                if(neighbors[i].shortestPathValue == 1000f){
                    neighbors[i].shortestPath = new List<Node>(searchPath);
                    neighbors[i].shortestPathValue = neighbors[i].getValue();
                    
                }
                
                if(neighbors[i].shortestPathValue > neighbors[i].getValue()){
                    neighbors[i].shortestPath = searchPath;
                    neighbors[i].shortestPathValue = neighbors[i].getValue();
                    
                }
                
                
                if(neighbors[i].getX() == targetNode.getX() && neighbors[i].getY() == targetNode.getY()){
                    if(targetNode.getValue() < finalSearchValue){
                        finalSearchPath = neighbors[i].shortestPath;
                        finalSearchPath.Add(neighbors[i]);
                        finalSearchValue = targetNode.getValue();
                        return;
                    }
                }
                
                openList.Add(neighbors[i]); 
                
            }
        }
        closedList.Add(currentNode);
        
        if(openList.Count == 0){
            return;
        }
        searchAux(targetNode);
        
    }
    
    float distanceBetweenNodes(Node n1, Node n2){
        return Mathf.Sqrt( Mathf.Pow(n1.getX() - n2.getX(), 2) +  Mathf.Pow(n1.getY() - n2.getY(), 2));
    }
    
    List<Node> findNeighbors(Node n){
           int nodeX = n.getX();
           int nodeY = n.getY();
           List<Node> list = new List<Node>();

           
           for(int i = -1; i <= 1; i++){
               for(int j = -1; j <= 1; j++){
                   int x = nodeX + i;
                   int y = nodeY + j;
                   if(i == 0 && j == 0){
                       
                   }else if(Mathf.Abs(i) != 1 || Mathf.Abs(j) != 1){
                       if(x < size && y < size && x >= 0 && y >= 0 && grid[x,y].active){
                           list.Add(grid[x,y]);
                       }
                       
                   }
               }
           }
           
           return list;
    }
    
    
    bool inList(List<Node> list, Node a){
        for(int i = 0; i< list.Count; i++){
            Node b = list[i];
            if(b.getX() == a.getX() && b.getY() == a.getY()){
                return true;
            }
        }
        return false;
    }
    
    public Node findClosestNode(Vector3 pos, ref bool pass){
        float clickedX = Mathf.Round(pos.x);
        float clickedY = Mathf.Round(pos.y);
        if(clickedX >= 0 && clickedX < size && clickedY >= 0 && clickedY < size){
                Node n  = GetComponent<Grid>().grid[(int)clickedX, (int)clickedY];
                pass = true;
                return n;
        }
        pass = false;

        return null;
    }
}
