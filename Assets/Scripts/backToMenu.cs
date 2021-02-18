using UnityEngine;
using UnityEngine.SceneManagement;

public class backToMenu : MonoBehaviour
{
    private void OnMouseDown() {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
