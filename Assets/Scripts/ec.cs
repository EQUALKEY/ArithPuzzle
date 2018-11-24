using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ec : MonoBehaviour {


    public Vector2[,] gridArr = new Vector2 [11, 11];

    public int[,] gridInit = new int[11, 11];
    public int[,] gridNow = new int[11, 11];
    public int[,] gridAnswer = new int[11, 11];

    // grid
    public GameObject gridGO;
    private grid gridcs;
    private int gridSize;

    // oper
    public int Oper; // 0 : 아무것도, 1 : + , 2 : - , 3: ×, 4 : ÷
    public int color; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색

    public int[] OperStack = new int[6]; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색


    struct xy // 그냥 x,y를 표시하기 위함. pair랑 같음
    {
        int x;
        int y;

        public xy(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    xy startPos, endPos;


    private void Awake()
    {
        gridcs = gridGO.GetComponent<grid>();

        gridSize = 10;  // grid 사이즈 설정. 후에 stage별로 받아와야함.

        gridcs.MakeGrid(gridSize);

        SetOperStack();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startPos = NowPos();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            endPos = NowPos();
            Operate();
        }
	}

    xy NowPos() // 현재 마우스 커서가  grid 속 어느 부분에 있는지 xy형태로 반환
    {
        xy result = new xy(gridSize,gridSize);
        Vector2 NowCurPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x , Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if(NowCurPos.x > gridArr[0,0].x && NowCurPos.x < gridArr[gridSize,gridSize].x && NowCurPos.y < gridArr[0, 0].y && NowCurPos.y > gridArr[gridSize, gridSize].y)
        {
            for (int i = 0; i < gridSize; i++)
            {
                if (NowCurPos.x >= gridArr[i, 0].x && NowCurPos.x < gridArr[i + 1, 0].x)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (NowCurPos.y <= gridArr[0, j].y && NowCurPos.y > gridArr[j + 1, 0].y)
                        {
                            result = new xy(i, j);
                        }
                    }
                }
            }
        }
        return result;
                
    }

    public void StageEnd()
    {

    }
    void SetOperStack()
    {
        //Operstack = Load 해오기

        //숫자들 set해주기
    }
    void Operate()
    {

    }
}
