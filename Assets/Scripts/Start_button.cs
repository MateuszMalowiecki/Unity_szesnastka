using UnityEngine;

public class Start_button : MonoBehaviour
{
    public GameObject go; 
    private void OnMouseDown() {
        Destroy(go);
        GameManager.gs.g = GameStatus.gameStat.start_pressed;
    }
}
