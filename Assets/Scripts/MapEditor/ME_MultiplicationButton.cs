using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_MultiplicationButton : MonoBehaviour {

    public GameObject EC;
    private MapEditorEC eccs;

    public GameObject Cover;

    private void Awake()
    {
        eccs = EC.GetComponent<MapEditorEC>();
    }

    public void ClickUpAsButton()
    {

        eccs.Clear();
        eccs.Oper = 3;
        Cover.SetActive(true);

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
