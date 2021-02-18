using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Piece puzzlePrefab;
    private List<Piece> puzzleList = new List<Piece>();
    private Vector3 startPosition = new Vector3(0.0f, 3.0f, 0.0f);
    public LayerMask collisionMask;
    Ray ray_up, ray_down, ray_left, ray_right;
    RaycastHit hit;
    private BoxCollider boxCollider;
    private Vector3 boxCollider_size;
    private Vector3 boxCollider_center;
    [HideInInspector]
    public static GameStatus gs = new GameStatus();
    public Text timerText;
    public static float startTime;
    // Start is called before the first frame update
    void Start() {
        startTime=Time.time;
        spawnPuzzle(14);
        setStartPosition();
        applyMaterial();
    }
    // Update is called once per frame
    void Update() {
        switch(gs.g) {
            case(GameStatus.gameStat.start_pressed):
                MixPuzzles();
                gs.g=GameStatus.gameStat.play;
                break;
            case(GameStatus.gameStat.play):
                float t=Time.time - startTime;
                timerText.text="Your time: \n" + t.ToString("f2");
                if (haveWeWon()) {
                    gs.g=GameStatus.gameStat.win;
                }
                MovePuzzle();
                break;
            case(GameStatus.gameStat.win):
                gs.g=GameStatus.gameStat.menu;
                SceneManager.LoadScene("Scenes/Exit Menu");
                break;
        }
    }
    private void spawnPuzzle(int number) {
        for(int i=0; i <= number; i++) {
            puzzleList.Add(Instantiate(puzzlePrefab, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 100.0f, 0.0f)) as Piece);
        }
    }
    private void setStartPosition() {
        puzzleList[0].transform.localPosition = startPosition;
        puzzleList[1].transform.localPosition = startPosition + 6*Vector3.right;
        puzzleList[2].transform.localPosition = startPosition + 12*Vector3.right;
        puzzleList[3].transform.localPosition = startPosition + 18*Vector3.right;
    
        puzzleList[4].transform.localPosition = startPosition + 6*Vector3.back;
        puzzleList[5].transform.localPosition = startPosition + 6*Vector3.back + 6*Vector3.right;
        puzzleList[6].transform.localPosition = startPosition + 6*Vector3.back + 12*Vector3.right;
        puzzleList[7].transform.localPosition = startPosition + 6*Vector3.back + 18*Vector3.right;
        
        puzzleList[8].transform.localPosition = startPosition + 12*Vector3.back;
        puzzleList[9].transform.localPosition = startPosition + 12*Vector3.back + 6*Vector3.right;
        puzzleList[10].transform.localPosition = startPosition + 12*Vector3.back + 12*Vector3.right;
        puzzleList[11].transform.localPosition = startPosition + 12*Vector3.back + 18*Vector3.right;
        
        puzzleList[12].transform.localPosition = startPosition + 18*Vector3.back;
        puzzleList[13].transform.localPosition = startPosition + 18*Vector3.back + 6*Vector3.right;
        puzzleList[14].transform.localPosition = startPosition + 18*Vector3.back + 12*Vector3.right;
    }
    void MovePuzzle() {
        foreach(Piece puzzle in puzzleList) {
            if (puzzle.clicked) {
                boxCollider=puzzle.GetComponent<BoxCollider>();
                boxCollider_size=boxCollider.size;
                boxCollider_center=boxCollider.center;
                
                float move_amount = 6;
                float direction=Mathf.Sign(move_amount);

                float x=(puzzle.transform.localPosition.x + boxCollider_center.x - boxCollider_size.x / 2) + boxCollider_size.x / 2;
                float y=(puzzle.transform.localPosition.y + boxCollider_center.y - boxCollider_size.y / 2) + boxCollider_size.y / 2;
                float z_up=puzzle.transform.localPosition.z + boxCollider_center.z + boxCollider_size.z / 2 * direction; 
                float z_down=puzzle.transform.localPosition.z + boxCollider_center.z + boxCollider_size.z / 2 * -direction; 
 
                ray_up=new Ray(new Vector3(x, y, z_up), new Vector3(0, 0, direction));
                ray_down=new Ray(new Vector3(x, y, z_down), new Vector3(0, 0, -direction));

                Debug.DrawRay(ray_up.origin, ray_up.direction);
                Debug.DrawRay(ray_down.origin, ray_down.direction);

                float z=(puzzle.transform.localPosition.z + boxCollider_center.z - boxCollider_size.z / 2) + boxCollider_size.z / 2;
                float x_left=puzzle.transform.localPosition.x + boxCollider_center.x + boxCollider_size.x / 2 * direction; 
                float x_right=puzzle.transform.localPosition.x + boxCollider_center.x + boxCollider_size.x / 2 * -direction; 
                
                ray_left=new Ray(new Vector3(x_left, y, z), new Vector3(-direction, 0, 0));
                ray_right=new Ray(new Vector3(x_right, y, z), new Vector3(direction, 0, 0));

                Debug.DrawRay(ray_left.origin, ray_left.direction);
                Debug.DrawRay(ray_right.origin, ray_right.direction);
                if (!puzzle.moved) {
                    if(!Physics.Raycast(ray_up, out hit, 7.0f, collisionMask) && puzzle.transform.localPosition.z < 0) {
                        puzzle.go_up=true;
                    }
                    if(!Physics.Raycast(ray_down, out hit, 7.0f, collisionMask) && puzzle.transform.localPosition.z > -18) {
                        puzzle.go_down=true;
                    }
                    if(!Physics.Raycast(ray_left, out hit, 7.0f, collisionMask) && puzzle.transform.localPosition.x > 0) {
                        puzzle.go_left=true;
                    }
                    if(!Physics.Raycast(ray_right, out hit, 7.0f, collisionMask) && puzzle.transform.localPosition.x < 18) {
                        puzzle.go_right=true;
                    }
                }
            }
        }
    }
    void applyMaterial() {
        string filePath;
        for(int i=1; i<= puzzleList.Count; i++) {
            filePath = "Puzzles/Cube" + i;
            Texture2D mat = Resources.Load(filePath, typeof(Texture2D)) as Texture2D;
            puzzleList[i-1].GetComponent<Renderer>().material.mainTexture=mat;
        }
    }
    bool isPositionEmpty(Vector3 pos) {
        foreach(Piece p in puzzleList) {
            Vector3 puzzlePos=p.transform.localPosition;
            if (puzzlePos.x == pos.x && puzzlePos.z == pos.z) return false;
        }
        return true;
    }
    Vector3 getValidMove(Vector3 pos) {
        Vector3 res=new Vector3();
        int n=Random.Range(0, 4);
        if (n==0) {
            res=6*Vector3.left;
        }
        else if (n==1) {
            res=6*Vector3.right;
        }
        else if (n==2) {
            res=6*Vector3.forward;
        }
        else {
            res=6*Vector3.back;
        }
        return res;
    }
    void swapPositions(Vector3 emptyPos, Vector3 move) {
        Vector3 exceptedPuzzlePos=emptyPos + move;
        foreach(Piece p in puzzleList) {
            Vector3 puzzlePos=p.transform.localPosition;
            if(puzzlePos.x == exceptedPuzzlePos.x && puzzlePos.z == exceptedPuzzlePos.z) {
                p.transform.localPosition=emptyPos;
                return;
            }
        }
    }
    void MixPuzzles() {
        List<Vector3> puzzlePosition = new List<Vector3>();
        foreach(Piece p in puzzleList) {
            puzzlePosition.Add(p.transform.localPosition);
        }
        puzzlePosition.Add(startPosition + 18*Vector3.back + 18*Vector3.right);
        for (int i=0; i<100; i++) {
            foreach(Vector3 pos in puzzlePosition) {
                if(isPositionEmpty(pos)) {
                    Vector3 move=getValidMove(pos);
                    swapPositions(pos, move);
                }
            }
        }
    }
    bool haveWeWon() {
        foreach(Piece p in puzzleList) {
            if (p.transform.localPosition != p.winPosition) {
                return false;
            }
        }
        return true;
    }
}