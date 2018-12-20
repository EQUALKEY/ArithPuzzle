using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_ChangeColor : MonoBehaviour
{
    public GameObject EC;
    private MapEditorEC eccs;

    public GameObject change_color_x;
    public GameObject Pallete_From, Pallete_To;
    public GameObject block_From, block_To;
    // Start is called before the first frame update
    private void Awake()
    {
        eccs = EC.GetComponent<MapEditorEC>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        change_color_x.SetActive(false);
        Pallete_From.SetActive(true);
        Pallete_To.SetActive(true);
        for (int i = 0; i < block_From.transform.childCount; i++)
            block_From.transform.GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < block_To.transform.childCount; i++)
            block_To.transform.GetChild(i).gameObject.SetActive(false);
        eccs.color_from = 0;
        eccs.color_to = 0;
    }
}
