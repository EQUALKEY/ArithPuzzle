using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class grid : MonoBehaviour {

    public GameObject EC;
    private ec eccs;

    public GameObject[] LeftTop = new GameObject[3];  // 0 : 4*4 1: 6*6 2: 8*8
    public GameObject[] RightBottom = new GameObject[3];
    public GameObject[] LeftTopView = new GameObject[3];
    public GameObject[] RightBottomView= new GameObject[3];

    public GameObject[] book = new GameObject[3];

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
        int index;
        if (eccs.gridSize == 4)
            index = 0;
        else if (eccs.gridSize == 6)
            index = 1;
        else
            index = 2;

        book[index].SetActive(true);
        float dis_x = (RightBottom[index].transform.position.x - LeftTop[index].transform.position.x) / (num - 1);
        float dis_y = (LeftTop[index].transform.position.y - RightBottom[index].transform.position.y) / (num - 1);
        float disView_x = (RightBottomView[index].transform.position.x - LeftTopView[index].transform.position.x) / (num - 1);
        float disView_y = (LeftTopView[index].transform.position.y - RightBottomView[index].transform.position.y) / (num - 1);
        
        for (int i = 0; i <= num; i++)
        {
            for(int j = 0; j <= num; j++)
            {

                eccs.gridArr[i, j] = new Vector2(LeftTop[index].transform.position.x + (dis_x * i), LeftTop[index].transform.position.y - (dis_y * j));
                eccs.gridViewArr[i,j]= new Vector2(LeftTopView[index].transform.position.x + (disView_x * i), LeftTopView[index].transform.position.y - (disView_y * j));


                if (i != num && j != num)
                {
                    GameObject newblock = Instantiate(BlockPref, eccs.gridArr[i, j], new Quaternion(0f, 0f, 0f, 1f));
                    newblock.name = "Block(" + i + "," + j + ")";
                    newblock.transform.SetParent(Blocks.transform);
                    ChangeBlockColor(i, j, eccs.gridInit[i, j]);
                    eccs.gridNow[i, j] = eccs.gridInit[i, j];
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
