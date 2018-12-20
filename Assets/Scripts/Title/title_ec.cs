using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title_ec : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LevelStage(string str) // level : 1-easy 2-normal 3-hard 4-hell
    {
        string[] newstr = str.Split('-');
        PlayerPrefs.SetInt("level", int.Parse(newstr[0]));
        PlayerPrefs.SetInt("stage", int.Parse(newstr[1]));
        SceneManager.LoadScene("play");
    }
    public void gotoMapEditor()
    {
        SceneManager.LoadScene("MapEditor");
    }
}
