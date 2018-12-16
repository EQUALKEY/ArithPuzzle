using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    public GameObject Palette;
    public GameObject Cover;

    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
    }

    public void ClickDown()
    {
        if (this.name == "block_red")
            eccs.color = 1;
        else if (this.name == "block_yellow")
            eccs.color = 2;
        else if (this.name == "block_blue")
            eccs.color = 3;
        else if (this.name == "block_green")
            eccs.color = 4;
        else if (this.name == "block_black")
            eccs.color = 5;

        Cover.GetComponent<Cover>().SetColor(eccs.color - 1);
        Palette.SetActive(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
