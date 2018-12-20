using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton_To : MonoBehaviour
{
    public GameObject EC;
    private ec eccs;

    public GameObject Palette;
    public GameObject Cover;

    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
    }

    public void ClickDown()
    {
        if (this.name == "block_red")
            eccs.color_to = 1;
        else if (this.name == "block_yellow")
            eccs.color_to = 2;
        else if (this.name == "block_blue")
            eccs.color_to = 3;
        else if (this.name == "block_green")
            eccs.color_to = 4;
        else if (this.name == "block_black")
            eccs.color_to = 5;

        eccs.Oper = 5;
        Cover.GetComponent<Cover>().SetColor(eccs.color_to - 1);
        Palette.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
