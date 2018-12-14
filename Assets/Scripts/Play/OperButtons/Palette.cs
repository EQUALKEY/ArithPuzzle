using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    


    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //this.gameObject.SetActive(false);
        }
	}
    
}
