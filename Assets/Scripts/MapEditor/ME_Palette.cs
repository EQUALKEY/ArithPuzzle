using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Palette : MonoBehaviour {

    public GameObject EC;
    private MapEditorEC eccs;

    


    private void Awake()
    {
        eccs = EC.GetComponent<MapEditorEC>();
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
