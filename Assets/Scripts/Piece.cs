using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour {
    [HideInInspector]
    public bool clicked=false;
    [HideInInspector]
    public bool go_left;
    [HideInInspector]
    public bool go_right;
    [HideInInspector]
    public bool go_up;
    [HideInInspector]
    public bool go_down;
    [HideInInspector]
    public bool moved;
    [HideInInspector]
    public Vector3 winPosition;
    [HideInInspector]
    private Vector3 currentPosition, endPosition;
    private float lerpTime=0.1f;
    // Start is called before the first frame update
    void Start() {
        winPosition=transform.localPosition;
    }
    // Update is called once per frame
    void Update() {
        movePuzzle();
    }
    private void OnMouseDown() {
        clicked = true;
    } 
    private void OnMouseUp() {
        clicked = false;
        moved=false;
    }
    void movePuzzle() {
        if (go_right) {
            currentPosition=transform.localPosition;
            endPosition= currentPosition + 6*Vector3.right;
            StartCoroutine(Move());
            go_right = false;
            moved = true;
        }
        if (go_left) {
            currentPosition=transform.localPosition;
            endPosition= currentPosition + 6*Vector3.left;
            StartCoroutine(Move());
            go_left = false;
            moved = true;
        }
        if (go_up) {
            currentPosition=transform.localPosition;
            endPosition= currentPosition + 6*Vector3.forward;
            StartCoroutine(Move());
            go_up = false;
            moved = true;
        }
        if (go_down) {
            currentPosition=transform.localPosition;
            endPosition= currentPosition + 6*Vector3.back;
            StartCoroutine(Move());
            go_down = false;
            moved = true;
        }
    }
    IEnumerator Move() {
        float elapsedTime=0f;
        while(elapsedTime < lerpTime) {
            transform.localPosition=Vector3.Lerp(currentPosition, endPosition, elapsedTime/lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition=endPosition;
    }
}