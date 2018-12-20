using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Cover : MonoBehaviour
{
    public GameObject EC;
    private MapEditorEC eccs;

    private void Awake()
    {
        eccs = EC.GetComponent<MapEditorEC>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initalize()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
    public void SetColor(int color)
    {

        Initalize();
        if (color != -1) transform.GetChild(color).gameObject.SetActive(true);
    }
}
