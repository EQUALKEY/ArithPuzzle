using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    public GameObject LeftTop,RightBottom;
    public GameObject BlockPref;
    public GameObject Blocks; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색

    private void Awake()
    {
        eccs = EC.GetComponent<ec>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void MakeGrid(int num)
    {
        float dis = (RightBottom.transform.position.x - LeftTop.transform.position.x)/(float)(num);
        for(int i = 0; i <= num; i++)
        {
            for(int j = 0; j <= num; j++)
            {

                eccs.gridArr[i, j] = new Vector2(LeftTop.transform.position.x + (dis * i), LeftTop.transform.position.y - (dis * j));
           
                if (i != num && j != num)
                {
                    GameObject newblock = Instantiate(BlockPref, eccs.gridArr[i, j], new Quaternion(0f, 0f, 0f, 1f));
                    newblock.name = "Block(" + i + "," + j + ")";
                    newblock.transform.SetParent(Blocks.transform);
                    ChangeBlockColor(i, j, eccs.gridInit[i, j]);
                }
            }
        }
    }
    public void ChangeBlockColor(int i,int j,int color)
    {
        GameObject GO = GameObject.Find("Block(" + i + "," + j + ")");
        for (int k = 0; k < GO.transform.childCount; k++)
            GO.transform.GetChild(k).gameObject.SetActive(false);
        if(color!=0)
            GO.transform.GetChild(color).gameObject.SetActive(true);
        eccs.gridNow[i, j] = color;
    }

    public void GridInit()
    {

    }

    public void CheckAnswer()
    {
        if (eccs.gridNow == eccs.gridAnswer)
            eccs.StageEnd();
    }
}
