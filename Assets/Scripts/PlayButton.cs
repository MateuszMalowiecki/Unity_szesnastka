using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
    private void OnMouseDown() {
        SceneManager.LoadScene("Scenes/Szesnastka");
    } 
}
