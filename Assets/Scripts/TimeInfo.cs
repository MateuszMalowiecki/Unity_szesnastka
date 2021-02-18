using UnityEngine;
using UnityEngine.UI;

public class TimeInfo : MonoBehaviour {
    public Text timerText;
    // Start is called before the first frame update
    void Start() {
        timerText.text = "Congratulations, You won in " + (Time.time - GameManager.startTime).ToString("f2") + " seconds";
    }
}
