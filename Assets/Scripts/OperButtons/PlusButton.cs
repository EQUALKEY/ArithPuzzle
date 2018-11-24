using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusButton : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    public GameObject Pallete;

    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
    }

    private void OnMouseUpAsButton()
    {
        Pallete.SetActive(true);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
