using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour {



    public void SetStack(int num)
    {
        for(int i = 0; i < 10; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(num).gameObject.SetActive(true);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
