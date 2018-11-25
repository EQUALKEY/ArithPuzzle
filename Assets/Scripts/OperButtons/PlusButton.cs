using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusButton : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    public GameObject Palette;
    public GameObject[] Cover = new GameObject[5];

    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
    }

    private void OnMouseUpAsButton()
    {
        eccs.Clear_Oper();
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
