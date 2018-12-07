using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplicationButton : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    public GameObject Cover;

    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
    }

    private void OnMouseUpAsButton()
    {
        if (eccs.Stack_Oper[3] > 0)
        {
            eccs.Clear();
            eccs.Oper = 3;
            Cover.SetActive(true);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Initialize()
    {
        Cover.SetActive(false);
    }
}
