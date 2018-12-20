using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ME_EraseLine : MonoBehaviour
{
    public GameObject EC;
    private MapEditorEC eccs;

    public GameObject erase_line_x;
    public GameObject horizental, vertical;
    private void Awake()
    {
        eccs = EC.GetComponent<MapEditorEC>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetVertical()
    {
        eccs.is_Vertical = 1;
        eccs.Oper = 6;
    }
    public void SetHorizental()
    {
        eccs.is_Vertical = 2;
        eccs.Oper = 6;
    }

    public void Initialize()
    {
        erase_line_x.SetActive(false);
        horizental.GetComponent<Button>().interactable = true;
        vertical.GetComponent<Button>().interactable = true;
        vertical.SetActive(true);
        eccs.is_Vertical = 0;
    }
}
