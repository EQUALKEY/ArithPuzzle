using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumButton : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    public GameObject Cover;

    private int thisNum;

    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
        thisNum = this.name[0]-'0';
    }

    private void OnMouseUpAsButton()
    {
        if (eccs.Stack_num[thisNum] > 0)
        {
            eccs.Clear_Num();
            eccs.Num = thisNum;
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
