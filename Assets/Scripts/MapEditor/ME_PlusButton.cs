using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_PlusButton : MonoBehaviour {

    public GameObject EC;
    private MapEditorEC eccs;

    public GameObject Palette;
    public GameObject[] Cover = new GameObject[5];

    private void Awake()
    {
        eccs = EC.GetComponent<MapEditorEC>();
    }

    public void ClickUpAsButton()
    {
        eccs.Clear();
        eccs.Oper = 1;
        Palette.SetActive(true);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Initialize()
    {
        Palette.SetActive(false);
        for (int i = 0; i < 5; i++)
            Cover[i].SetActive(false);
    }
}
