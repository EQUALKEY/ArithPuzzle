using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridAnswer : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    public GameObject LeftTop, RightBottom;
    public GameObject[] BlockPref = new GameObject[3]; // 0:4*4 1:6*6 2:8*8
    public GameObject Blocks; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색

    private Vector2[,] gridArrAnswer = new Vector2[11, 11];

    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
    }

    public void MakeGrid(int num)
    {
        int index;
        if (eccs.gridSize == 4)
            index = 0;
        else if (eccs.gridSize == 6)
            index = 1;
        else
            index = 2;
        float dis = (RightBottom.transform.position.x - LeftTop.transform.position.x) / (float)(num);
        for (int i = 0; i <= num; i++)
        {
            for (int j = 0; j <= num; j++)
            {

                gridArrAnswer[i, j] = new Vector2(LeftTop.transform.position.x + (dis * i), LeftTop.transform.position.y - (dis * j));

                if (i != num && j != num)
                {
                    GameObject newblock = Instantiate(BlockPref[index], gridArrAnswer[i, j], new Quaternion(0f, 0f, 0f, 1f));
                    newblock.name = "BlockAnswer(" + i + "," + j + ")";
                    newblock.transform.SetParent(Blocks.transform,false);
                    newblock.transform.position = gridArrAnswer[i, j];
                    ChangeBlockColor(i, j, eccs.gridAnswer[i, j]);
                }
            }
        }
    }
    public void ChangeBlockColor(int i, int j, int color)
    {
        GameObject GO = GameObject.Find("BlockAnswer(" + i + "," + j + ")");
        for (int k = 0; k < GO.transform.childCount; k++)
            GO.transform.GetChild(k).gameObject.SetActive(false);
        if (color != 0)
            GO.transform.GetChild(color).gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
