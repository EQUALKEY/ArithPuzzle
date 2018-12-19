using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ME_grid : MonoBehaviour {

    // 상수값들
    const float width = 2208;
    const float height = 1242;

    public GameObject EC;
    private MapEditorEC eccs;

    public GameObject[] LeftTop = new GameObject[3];  // 0 : 4*4 1: 6*6 2: 8*8
    public GameObject[] RightBottom = new GameObject[3];
    public GameObject[] LeftTopView = new GameObject[3];
    public GameObject[] RightBottomView= new GameObject[3];

    public GameObject[] LeftTop_Init = new GameObject[3];  // 0 : 4*4 1: 6*6 2: 8*8
    public GameObject[] RightBottom_Init = new GameObject[3];
    public GameObject[] LeftTop_Answer = new GameObject[3];  // 0 : 4*4 1: 6*6 2: 8*8
    public GameObject[] RightBottom_Answer = new GameObject[3];

    public GameObject[] BlockPref = new GameObject[3];
    public GameObject Blocks; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색
    public GameObject Blocks_Init;
    public GameObject Blocks_Answer;

    private void Awake()
    {
        eccs = EC.GetComponent<MapEditorEC>();
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
        if (num == 4)
            index = 0;
        else if (num == 6)
            index = 1;
        else
            index = 2;
        float dis_x = (RightBottom[index].transform.position.x - LeftTop[index].transform.position.x) / (num - 1);
        float dis_y = (LeftTop[index].transform.position.y - RightBottom[index].transform.position.y) / (num - 1);
        float disView_x = (RightBottomView[index].transform.position.x - LeftTopView[index].transform.position.x) / (num - 1);
        float disView_y = (LeftTopView[index].transform.position.y - RightBottomView[index].transform.position.y) / (num - 1);
        
        for (int i = 0; i <= num; i++)
        {
            for(int j = 0; j <= num; j++)
            {
                eccs = EC.GetComponent<MapEditorEC>();
                
                eccs.gridArr[i, j] = new Vector2(LeftTop[index].transform.position.x + (dis_x * i), LeftTop[index].transform.position.y - (dis_y * j));
                eccs.gridViewArr[i,j]= new Vector2(LeftTopView[index].transform.position.x + (disView_x * i), LeftTopView[index].transform.position.y - (disView_y * j));
                eccs.gridViewArr[i, j].x *= Screen.width / width;
                eccs.gridViewArr[i, j].y = (eccs.gridViewArr[i, j].y - Screen.height / 2f + height/2f ) * (Screen.height / height);
                if (i != num && j != num)
                {
                    GameObject newblock = Instantiate(BlockPref[index], eccs.gridArr[i, j], new Quaternion(0f, 0f, 0f, 1f));
                    newblock.name = "Block(" + i + "," + j + ")";
                    newblock.transform.SetParent(Blocks.transform,false);
                    newblock.transform.position = eccs.gridArr[i, j];
                    ChangeBlockColor(newblock, i, j, 0);
                    eccs.gridNow[i, j] = 0;
                }
            }
        }
    }
    public void MakeGridInit(int num)
    {
        int index;
        if (num == 4)
            index = 0;
        else if (num == 6)
            index = 1;
        else
            index = 2;
        float dis_x = (RightBottom_Init[index].transform.position.x - LeftTop_Init[index].transform.position.x) / (num - 1);
        float dis_y = (LeftTop_Init[index].transform.position.y - RightBottom_Init[index].transform.position.y) / (num - 1);

        for (int i = 0; i <= num; i++)
        {
            for (int j = 0; j <= num; j++)
            {
                eccs = EC.GetComponent<MapEditorEC>();

                eccs.gridArrInit[i, j] = new Vector2(LeftTop_Init[index].transform.position.x + (dis_x * i), LeftTop_Init[index].transform.position.y - (dis_y * j));
                if (i != num && j != num)
                {
                    GameObject newblock = Instantiate(BlockPref[index], eccs.gridArrInit[i, j], new Quaternion(0f, 0f, 0f, 1f));
                    newblock.name = "Block_Init(" + i + "," + j + ")";
                    newblock.transform.SetParent(Blocks_Init.transform, false);
                    newblock.transform.position = eccs.gridArrInit[i, j];
                    newblock.transform.localScale *= 0.5f;
                    ChangeBlockColor(newblock, i, j, eccs.gridNow[i,j]);
                    eccs.newstageSRC.init[i,j] = eccs.gridNow[i,j];
                }
            }
        }
    }
    public void MakeGridAnswer(int num)
    {
        int index;
        if (num == 4)
            index = 0;
        else if (num == 6)
            index = 1;
        else
            index = 2;
        float dis_x = (RightBottom_Answer[index].transform.position.x - LeftTop_Answer[index].transform.position.x) / (num - 1);
        float dis_y = (LeftTop_Answer[index].transform.position.y - RightBottom_Answer[index].transform.position.y) / (num - 1);

        for (int i = 0; i <= num; i++)
        {
            for (int j = 0; j <= num; j++)
            {
                eccs = EC.GetComponent<MapEditorEC>();

                eccs.gridArrAnswer[i, j] = new Vector2(LeftTop_Answer[index].transform.position.x + (dis_x * i), LeftTop_Answer[index].transform.position.y - (dis_y * j));
                if (i != num && j != num)
                {
                    GameObject newblock = Instantiate(BlockPref[index], eccs.gridArrAnswer[i, j], new Quaternion(0f, 0f, 0f, 1f));
                    newblock.name = "Block_Answer(" + i + "," + j + ")";
                    newblock.transform.SetParent(Blocks_Answer.transform, false);
                    newblock.transform.position = eccs.gridArrAnswer[i, j];
                    newblock.transform.localScale *= 0.5f;
                    ChangeBlockColor(newblock, i, j, eccs.gridNow[i,j]);
                    eccs.newstageSRC.answer[i, j] = eccs.gridNow[i,j];
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
    public void ChangeBlockColor(GameObject GO, int i, int j, int color)
    {
        for (int k = 0; k < GO.transform.childCount; k++)
            GO.transform.GetChild(k).gameObject.SetActive(false);
        if (color != 0)
            GO.transform.GetChild(color).gameObject.SetActive(true);
        eccs.gridNow[i, j] = color;
    }

        public void GridInit()
    {

    }

    
}
