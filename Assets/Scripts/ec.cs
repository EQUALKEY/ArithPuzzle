using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ec : MonoBehaviour {


    // 시작할 때 Load할 정보들
    public int[,] gridInit = new int[11, 11];
    public int[,] gridAnswer = new int[11, 11];
    public int[] Stack_Oper_Init = new int[5];
    public int[] Stack_Num_Init = new int[10];


    public Vector2[,] gridArr;
    public Vector2[,] gridViewArr;
    public int[,] gridNow = new int[11, 11];

    // grid
    public GameObject gridGO;
    private grid gridcs;
    private int gridSize;
    public GameObject gridAnswerGO;
    private gridAnswer grAcs;

    // oper
    public int Oper; // 0 : 아무것도, 1 : + , 2 : - , 3: ×, 4 : ÷
    public int color; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색
    public int Num;

    //?   public int[] OperStack = new int[6]; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색

    public int[] Stack_Oper; // 0: null, 1 : + , 2 : - , 3: ×, 4 : ÷
    public GameObject[] StackOfOper = new GameObject[5];
    public int[] Stack_num;
    public GameObject[] StackOfNum = new GameObject[10];


    public GameObject PlusButtonGO;
    private PlusButton PBcs;
    public GameObject PalleteGO;
    private Palette Pcs;
    public GameObject MinusButtonGO;
    private MinusButton MBcs;
    public GameObject MultiplicationButtonGO;
    private MultiplicationButton MPBcs;
    public GameObject DivisionButtonGO;
    private DivisionButton DBcs;

    public GameObject[] NumGO = new GameObject[10];
    public NumButton[] Numcs = new NumButton[10];

    public GameObject CleraBoard;

    struct xy // 그냥 x,y를 표시하기 위함. pair랑 같음
    {
        public int x;
        public int y;

        public xy(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    xy startPos, endPos;


    private void Awake()
    {
        gridArr = new Vector2[11, 11];
        gridViewArr = new Vector2[11, 11];

        gridcs = gridGO.GetComponent<grid>();
        grAcs = gridAnswerGO.GetComponent<gridAnswer>();
        PBcs = PlusButtonGO.GetComponent<PlusButton>();
        Pcs = PalleteGO.GetComponent<Palette>();
        MBcs = MinusButtonGO.GetComponent<MinusButton>();
        MPBcs = MultiplicationButtonGO.GetComponent<MultiplicationButton>();
        DBcs = DivisionButtonGO.GetComponent<DivisionButton>();
        for (int i = 0; i < 10; i++)
        {
            Numcs[i] = NumGO[i].GetComponent<NumButton>();
        }


        //////////////////////////// 정의

        GetData();



        gridSize = 10;  // grid 사이즈 설정. 후에 stage별로 받아와야함.

        gridcs.MakeGrid(gridSize);
        grAcs.MakeGrid(gridSize);

        SetOperStack();
        SetNumStack();

        for(int i = 0; i <= 10; i++)
        {
            string str ="";
            for(int j=0; j <= 10; j++)
            {
                str += gridViewArr[i, j].ToString();
                str += " ";
            }
            Debug.Log(str);
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        Vector2 NowCurPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            startPos = NowPos();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            endPos = NowPos();
            if (NowCurPos.x > gridViewArr[0, 0].x && NowCurPos.x < gridViewArr[gridSize, gridSize].x && NowCurPos.y < gridViewArr[0, 0].y && NowCurPos.y > gridViewArr[gridSize, gridSize].y)
            {
                Operate();
            }
        }
    }

    xy NowPos() // 현재 마우스 커서가  grid 속 어느 부분에 있는지 xy형태로 반환
    {
        xy result = new xy(gridSize, gridSize);
        Vector2 NowCurPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (NowCurPos.x > gridViewArr[0, 0].x && NowCurPos.x < gridViewArr[gridSize, gridSize].x && NowCurPos.y < gridViewArr[0, 0].y && NowCurPos.y > gridViewArr[gridSize, gridSize].y)
        {
            for (int i = 0; i < gridSize; i++)
            {
                if (NowCurPos.x >= gridViewArr[i, 0].x && NowCurPos.x < gridViewArr[i + 1, 0].x)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (NowCurPos.y <= gridViewArr[0, j].y && NowCurPos.y > gridViewArr[0, j+1].y)
                        {
                            result = new xy(i, j);
                        }
                    }
                }
            }
        }
        return result;

    }
    void GetData()
    {
        gridInit = new int[,] {
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,5,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 }
        };
        gridAnswer = new int[,] {
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,5,2,2,0,0,0,0 },
            {0,0,0,1,1,1,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0 }
        };
        Stack_Oper_Init = new int[] { 0, 5, 1, 2, 3 };
        Stack_Num_Init = new int[] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
    }


    public void Clear_Oper()
    {
        Oper = 0;
        color = 0;
        PBcs.Initialize();
        MBcs.Initialize();
        MPBcs.Initialize();
        DBcs.Initialize();
    }
    public void Clear_Num()
    {
        Num = -1;
        for(int i = 0; i < 10; i++)
        {
            Numcs[i].Initialize();
        }

    }
    public void StageEnd()
    {

    }
    void SetOperStack()
    {
        //Load 해오기

        Stack_Oper = (int[])Stack_Oper_Init.Clone() ;

        //Stack_Oper[] 값넣어주기
        for (int i = 1; i <= 4; i++)
        {
            StackOfOper[i].GetComponent<StackManager>().SetStack(Stack_Oper[i]);
        }
    }
    void SetNumStack()
    {
        //Load
        Stack_num = (int[])Stack_Num_Init.Clone();

        //Stack_Num[] 값넣어주기
        for (int i = 0; i <= 9; i++)
        {
            StackOfNum[i].GetComponent<StackManager>().SetStack(Stack_num[i]);
        }
    }
    void Operate()
    {
        xy LT, RB = new xy();
        LT.x = startPos.x < endPos.x ? startPos.x : endPos.x;
        LT.y = startPos.y < endPos.y ? startPos.y : endPos.y;
        RB.x = startPos.x > endPos.x ? startPos.x : endPos.x;
        RB.y = startPos.y > endPos.y ? startPos.y : endPos.y;
        if (Oper == 1) // +
        {
            bool isable = true;
            int cnt=0;
            for (int i = LT.x; i <= RB.x; i++)
            {
                for (int j = LT.y; j <= RB.y; j++)
                {
                    cnt++;
                    if (gridNow[i, j] != 0)
                        isable = false;
                }
            }
            if (isable && cnt==Num)
            {
                for (int i = LT.x; i <= RB.x; i++)
                {
                    for (int j = LT.y; j <= RB.y; j++)
                    {
                        gridcs.ChangeBlockColor(i, j, color);
                    }
                }
                Stack_Oper[Oper]--;
                StackOfOper[Oper].GetComponent<StackManager>().SetStack(Stack_Oper[Oper]);
                Stack_num[Num]--;
                StackOfNum[Num].GetComponent<StackManager>().SetStack(Stack_num[Num]);
                Clear_Oper();
                Clear_Num();
            }
            else
            {
                PopDisable();
            }
        }
        else if(Oper == 2) // -
        {
            bool isable = true;
            int cnt = 0;
            for (int i = LT.x; i <= RB.x; i++)
            {
                for (int j = LT.y; j <= RB.y; j++)
                {
                    cnt++;
                    if (gridNow[i, j] == 0)
                        isable = false;
                }
            }
            if (isable && cnt == Num)
            {
                for (int i = LT.x; i <= RB.x; i++)
                {
                    for (int j = LT.y; j <= RB.y; j++)
                    {
                        gridcs.ChangeBlockColor(i, j, 0);
                    }
                }
                Stack_Oper[Oper]--;
                StackOfOper[Oper].GetComponent<StackManager>().SetStack(Stack_Oper[Oper]);
                Stack_num[Num]--;
                StackOfNum[Num].GetComponent<StackManager>().SetStack(Stack_num[Num]);
                Clear_Oper();
                Clear_Num();
            }
            else
            {
                PopDisable();
            }
        }
        else if(Oper ==3) // ×
        {

        }
        else if(Oper ==4) // ÷
        {

        }
        else // ?
        {

        }
        CheckAnswer();
    }
    void PopDisable()
    {

    }
    void CheckAnswer()
    {
        bool chk = true;
        for(int i = 0; i < gridSize; i++)
        {
            for(int j = 0; j < gridSize; j++)
            {
                if (gridNow[i, j] != gridAnswer[i, j])
                    chk = false;
            }
        }
        if(chk)
        {
            CleraBoard.SetActive(true);
        }
    }
}
