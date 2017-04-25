using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsManager : MonoBehaviour {

    public Text killText;
    public Text levelText;
    public Text tokenText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        killText.text = "Worlds Destroyed: " + GameManager.instance.killCount;
        levelText.text = "Combined World Level: " + GameManager.instance.levelCount;
        tokenText.text = "Total Worlds Gained: " + GameManager.instance.tokenCount;
	}

    public void restartGame(){
        SceneManager.LoadScene(0);
    }
}
