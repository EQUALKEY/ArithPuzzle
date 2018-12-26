﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ec : MonoBehaviour {

    // 상수값들
    const float width = 2208;
    const float height = 1242;

    float ratio;
    // 시작할 때 Load할 정보들
    public DB DataBasecs;
    public int[,] gridInit = new int[11, 11];
    public int[,] gridAnswer = new int[11, 11];
    public int[] Stack_Oper_Init = new int[7];
    public int[] Stack_Num_Init = new int[10];
    int level;
    int stage;

    public Vector2[,] gridArr;
    public Vector2[,] gridViewArr;
    public int[,] gridNow = new int[11, 11];

    // grid
    public GameObject gridGO;
    private grid gridcs;
    public int gridSize;
    public GameObject gridAnswerGO;
    private gridAnswer grAcs;

    // Pre
    public Button Undo;
    public int[,] gridPre = new int[11, 11];
    public int[] Stack_Oper_Pre;
    public int[] Stack_num_Pre;

    // oper
    public int Oper; // 0 : 아무것도, 1 : + , 2 : - , 3: ×, 4 : ÷, 5: change_color, 6: erase_line
    public int color; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색
    public int Num;

    public int color_from;
    public int color_to;
    public int is_Vertical; //0: null , 1:vertical 2: horizental

    //?   public int[] OperStack = new int[6]; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색

    public int[] Stack_Oper; // 0: null, 1 : + , 2 : - , 3: ×, 4 : ÷, 5: change_color, 6: erase_line
    public GameObject[] StackOfOper = new GameObject[7];
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
    public GameObject ChangeColorButtonGO;
    public GameObject EraseLineButtonGO;


    public GameObject[] NumGO = new GameObject[10];
    public NumButton[] Numcs = new NumButton[10];

    public GameObject Overlay, Overlay_Top, Overlay_Bottom, Overlay_Left, Overlay_Right,Overlay_field;
    public GameObject Direction_Button, UpButton, DownButton, LeftButton, RightButton;
    public GameObject Direction_Button_Division, UpButton_Division, DownButton_Division, LeftButton_Division, RightButton_Division;

    public GameObject CleraBoard;


    // UI 연동
    public Text Stage_text;
    public Text remain_text;
    public int remain;
    public Text Clear_text;

    bool isOverlay = false;

    public Text imsiText;

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
    xy OverlayStartPos, OverlayEndPos;
    xy OverlayLT, OverlayRB;
    private void Awake()
    {
        ratio = Screen.width / width;

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
        DataBasecs = GetComponent<DB>();
        GetData();





        gridcs.MakeGrid(gridSize);
        grAcs.MakeGrid(gridSize);

        SetOperStack();
        SetNumStack();
        OffInteractable_Num();
        SetOperButton();

        gridcs.isfinish_setting = true;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 NowCurPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        

        if (Input.GetKeyDown(KeyCode.Mouse0) )
        {
            if (!isOverlay)
            {
                startPos = NowPos();
                if (NowCurPos.x > gridViewArr[0, 0].x && NowCurPos.x < gridViewArr[gridSize, gridSize].x && NowCurPos.y < gridViewArr[0, 0].y && NowCurPos.y > gridViewArr[gridSize, gridSize].y)
                {
                    Overlay.SetActive(true);
                    OverlayStartPos = NowPos();
                }
            }
            else
            {
                if(NowCurPos.x > gridViewArr[0, 0].x && NowCurPos.x < gridViewArr[gridSize, gridSize].x && NowCurPos.y < gridViewArr[0, 0].y && NowCurPos.y > gridViewArr[gridSize, gridSize].y)
                {

                }
                else
                {
                    isOverlay = false;
                    Direction_Button.SetActive(false);
                    Direction_Button_Division.SetActive(false);
                    Overlay.SetActive(false);
                    OverlayStartPos = new xy();
                    OverlayEndPos = new xy();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && !isOverlay)
        {
            endPos = NowPos();
            if (NowCurPos.x > gridViewArr[0, 0].x && NowCurPos.x < gridViewArr[gridSize, gridSize].x && NowCurPos.y < gridViewArr[0, 0].y && NowCurPos.y > gridViewArr[gridSize, gridSize].y)
            {
                Overlay.SetActive(false);
                Operate();

            }
        }

        else if (Input.GetKey(KeyCode.Mouse0) && NowCurPos.x > gridViewArr[0, 0].x && NowCurPos.x < gridViewArr[gridSize, gridSize].x && NowCurPos.y < gridViewArr[0, 0].y && NowCurPos.y > gridViewArr[gridSize, gridSize].y && !isOverlay)
        {
            
            OverlayEndPos = NowPos();
            Overlay.SetActive(true);
            Set_Overlay();
        }
        else if (Input.GetKey(KeyCode.Mouse0) & !isOverlay)
        {
            
            Overlay.SetActive(false);
        }

    }

    xy NowPos() // 현재 마우스 커서가  grid 속 어느 부분에 있는지 xy형태로 반환
    {
        xy result = new xy(gridSize, gridSize);
        Vector2 NowCurPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (NowCurPos.x > gridViewArr[0, 0].x && NowCurPos.x < gridViewArr[gridSize, gridSize].x && NowCurPos.y < gridViewArr[0, 0].y && NowCurPos.y > gridViewArr[gridSize, gridSize].y)
        {
            for (int i = 0; i < gridSize; i++)
            {
                if (NowCurPos.x >= gridViewArr[i, 0].x && NowCurPos.x < gridViewArr[i + 1, 0].x)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (NowCurPos.y <= gridViewArr[0, j].y && NowCurPos.y > gridViewArr[0, j + 1].y)
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
        DataBasecs.loadDB();
        level = PlayerPrefs.GetInt("level");
        stage = PlayerPrefs.GetInt("stage");
        gridSize = DataBasecs.stageDB[level, stage].size;
        gridInit = (int[,])DataBasecs.stageDB[level, stage].init.Clone();
        gridAnswer = (int[,])DataBasecs.stageDB[level, stage].answer.Clone();
        Stack_Oper_Init = (int[])DataBasecs.stageDB[level, stage].Oper.Clone();
        Stack_Num_Init = (int[])DataBasecs.stageDB[level, stage].Num.Clone();
        remain = DataBasecs.stageDB[level, stage].remain;

        Stage_text.text = "STAGE " + stage.ToString();
        remain_text.text = (remain).ToString() + "번";
    }

    public void Clear()
    {
        OffInteractable_Num();
        Clear_Num();
        Clear_Oper();
        Overlay.SetActive(false);
        Direction_Button.SetActive(false);
        Direction_Button_Division.SetActive(false);
        Clear_text.text = level + " - " + stage;
        isOverlay = false;
    }
    public void Clear_Oper()
    {
        Oper = 0;
        color = 0;
        color_from = 0;
        color_to = 0;

        PBcs.Initialize();
        MBcs.Initialize();
        MPBcs.Initialize();
        DBcs.Initialize();
        ChangeColorButtonGO.GetComponent<ChangeColor>().Initialize();
        EraseLineButtonGO.GetComponent<EraseLine>().Initialize();


    }
    public void Clear_Num()
    {
        Num = 999;
        for(int i = 0; i < 10; i++)
        {
            Numcs[i].Initialize();
        }

    }
    public void FinishOperate()
    {
        remain_text.text = (--remain).ToString() + "번";
        Stack_Oper[Oper]--;
        StackOfOper[Oper].GetComponent<StackManager>().SetStack(Stack_Oper[Oper]);
        if (Oper != 5 && Oper != 6)
        {
            Stack_num[Num]--;
            StackOfNum[Num].GetComponent<StackManager>().SetStack(Stack_num[Num]);
        }
        Clear_Oper();
        Clear_Num();
        Overlay.SetActive(false);
        Direction_Button.SetActive(false);
        Direction_Button_Division.SetActive(false);
        OffInteractable_Num();
        SetOperButton();
        CheckAnswer();
    }
    public void StageEnd()
    {

    }
    void SetOperStack()
    {
        Stack_Oper = new int[7];
        //Load 해오기
        for(int i = 1; i <= 6; i++)
        {
            Stack_Oper[i] = Stack_Oper_Init[i];
        }

        //Stack_Oper[] 값넣어주기
        for (int i = 1; i <= 6; i++)
        {
            StackOfOper[i].GetComponent<StackManager>().SetStack(Stack_Oper[i]);
        }
    }
    void SetNumStack()
    {
        Stack_num = new int[10];
        //Load
        for(int i = 0; i <= 9; i++)
        {
            Stack_num[i] = Stack_Num_Init[i];
        }

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
        imsiText.text = "Oper : "+Oper +"  Num : " + Num +"\nLT = ( " + LT.x.ToString() + " , " + LT.y.ToString() + " )\nRB = ( " + RB.x.ToString() + " , " + RB.y.ToString() + " )";
        OverlayLT = LT;
        OverlayRB = RB;
        if (Oper == 1) // +
        {
            bool isable = true;
            int cnt = 0;
            for (int i = LT.x; i <= RB.x; i++)
            {
                for (int j = LT.y; j <= RB.y; j++)
                {
                    cnt++;
                    if (gridNow[i, j] != 0)
                        isable = false;
                }
            }
            if (isable && cnt == Num && remain>0)
            {
                SavePreData();
                for (int i = LT.x; i <= RB.x; i++)
                {
                    for (int j = LT.y; j <= RB.y; j++)
                    {
                        gridcs.ChangeBlockColor(i, j, color);
                    }
                }

                FinishOperate();
            }
            else
            {
                PopDisable();
            }
        }
        else if (Oper == 2) // -
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
            if (isable && cnt == Num && remain > 0)
            {
                SavePreData();
                for (int i = LT.x; i <= RB.x; i++)
                {
                    for (int j = LT.y; j <= RB.y; j++)
                    {
                        gridcs.ChangeBlockColor(i, j, 0);
                    }
                }
                FinishOperate();
            }
            else
            {
                PopDisable();
            }
        }
        else if (Oper == 3) // ×
        {
            Overlay.SetActive(true);
            Direction_Button.SetActive(true);
            bool isable =true;
            for (int i = LT.x; i <= RB.x; i++)
            {
                for (int j = LT.y; j <= RB.y; j++)
                {
                    if (gridNow[i, j] == 0)
                        isable = false;
                }
            }
            if (!isable)
            {
                Overlay.SetActive(false);
                Direction_Button.SetActive(false);
                goto end;
            }

            bool isableUp,isableDown,isableLeft,isableRight;
            isableUp = isableDown = isableLeft = isableRight = true;
          
            //위로 가능한지 확인
            if (LT.y - (Num - 1) * (RB.y - LT.y+1) < 0)
            {
                isableUp = false;;
            }
            else
            {
                for(int i = LT.x; i <= RB.x; i++)
                {
                    for(int j= LT.y - 1; j >= LT.y - (Num - 1) * (RB.y - LT.y + 1); j--)
                    {
                        if (gridNow[i, j] != 0)
                            isableUp = false;
                    }
                }
            }

            //아래로 가능한지 확인
            if (RB.y + (Num - 1) * (RB.y - LT.y+1) >= gridSize)
            {
                isableDown = false; ;
            }
            else
            {
                for (int i = LT.x; i <= RB.x; i++)
                {
                    for (int j = RB.y + 1; j <= RB.y + (Num - 1) * (RB.y - LT.y + 1); j++)
                    {
                        if (gridNow[i, j] != 0)
                            isableDown = false;
                    }
                }
            }
            //왼쪽로 가능한지 확인
            if(LT.x - (Num - 1) * (RB.x - LT.x +1) <0)
            {
                isableLeft = false;
            }
            else
            {
                for (int i = LT.x-1; i >= LT.x - (Num - 1) * (RB.x - LT.x + 1); i--)
                {
                    for (int j = LT.y ; j <= RB.y; j++)
                    {
                        if (gridNow[i, j] != 0)
                            isableLeft = false;
                    }
                }
            }

            //오른쪽으로 가능한지 확인

            if (RB.x + (Num - 1) * (RB.x - LT.x + 1) >= gridSize)
            {
                isableRight = false;
            }
            else
            {
                for (int i = RB.x+1; i <= RB.x + (Num-1) * (RB.x - LT.x + 1); i++)
                {
                    for (int j = LT.y; j <= RB.y; j++)
                    {
                        if (gridNow[i, j] != 0)
                            isableRight = false;
                    }
                }
            }

          
            UpButton.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[RB.x + 1, LT.y]) / 2f + new Vector2(0f,50f) * ratio;
            DownButton.transform.position = (gridViewArr[LT.x, RB.y+1] + gridViewArr[RB.x + 1, RB.y+1]) / 2f - new Vector2(0f, 50f) * ratio;
            LeftButton.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[LT.x, RB.y+1]) / 2f - new Vector2(50f, 0f) * ratio;
            RightButton.transform.position = (gridViewArr[RB.x+1, LT.y] + gridViewArr[RB.x + 1, RB.y+1]) / 2f + new Vector2(50f, 0f) * ratio;
            if (isableUp || isableDown || isableLeft || isableRight)
                isOverlay = true;

            UpButton.SetActive(isableUp& (remain > 0));
            DownButton.SetActive(isableDown & (remain > 0));
            LeftButton.SetActive(isableLeft & (remain > 0));
            RightButton.SetActive(isableRight & (remain > 0));

            end:;
        }
        else if(Oper ==4) // ÷
        {
            Overlay.SetActive(true);
            Direction_Button_Division.SetActive(true);
            bool isable = true;
            for (int i = LT.x; i <= RB.x; i++)
            {
                for (int j = LT.y; j <= RB.y; j++)
                {
                    if (gridNow[i, j] == 0)
                        isable = false;
                }
            }
            if (!isable)
            {
                Overlay.SetActive(false);
                Direction_Button_Division.SetActive(false);
                goto end;
            }
            
            bool isVertical, isHorizental;
            isVertical = isHorizental = true;

            if ((RB.x - LT.x + 1) % Num != 0)
            {
                isHorizental = false;
            }
            else {
                for (int j = LT.y; j <= RB.y; j++)
                {
                    for (int k = 0; k < ((RB.x - LT.x + 1) / Num); k++)
                    {
                        for (int i = LT.x + ((RB.x - LT.x + 1) / Num) + k; i <= RB.x; i += ((RB.x - LT.x + 1) / Num))
                        {
                            if (gridNow[i, j] != gridNow[LT.x + k, j])
                            {
                                isHorizental = false;
                            }

                        }
                    }
                }
            }
            if ((RB.y - LT.y + 1) % Num != 0)
            {
                isVertical = false;
            }
            else
            {
                for (int i = LT.x; i <= RB.x; i++)
                {
                    for (int k = 0; k < ((RB.y - LT.y + 1) / Num); k++)
                    {
                        for (int j = LT.y + ((RB.y - LT.y + 1) / Num) + k; j <= RB.y; j += ((RB.y - LT.y + 1) / Num))
                        {
                            if (gridNow[i, j] != gridNow[i, LT.y+k])
                            {
                                isVertical = false;
                            }

                        }
                    }
                }
            }

            DownButton_Division.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[RB.x + 1, LT.y]) / 2f - new Vector2(0f, 50f)*ratio;
            UpButton_Division.transform.position = (gridViewArr[LT.x, RB.y + 1] + gridViewArr[RB.x + 1, RB.y + 1]) / 2f + new Vector2(0f, 50f)*ratio;
            RightButton_Division.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[LT.x, RB.y + 1]) / 2f + new Vector2(50f, 0f)*ratio;
            LeftButton_Division.transform.position = (gridViewArr[RB.x + 1, LT.y] + gridViewArr[RB.x + 1, RB.y + 1]) / 2f - new Vector2(50f, 0f)*ratio;
            if (isVertical || isHorizental)
                isOverlay = true;

            UpButton_Division.SetActive(isVertical & (remain > 0));
            DownButton_Division.SetActive(isVertical & (remain > 0));
            LeftButton_Division.SetActive(isHorizental & (remain > 0));
            RightButton_Division.SetActive(isHorizental & (remain > 0));

            end:;
        }
        else if(Oper == 5) // change_color
        {
            if (color_from != 0 && color_to != 0 && remain > 0)
            {

                SavePreData();
                for (int i = LT.x; i <= RB.x; i++)
                {
                    for (int j = LT.y; j <= RB.y; j++)
                    {
                        if (gridNow[i, j] == color_from)
                            gridcs.ChangeBlockColor(i, j, color_to);
                    }
                }

                FinishOperate();
            }
        }
        else if(Oper == 6) // erase_line
        {
            if (remain > 0)
            {
                if (is_Vertical == 1)
                {
                    SavePreData();
                    int i = LT.x;
                    for (int j = 0; j < gridSize; j++)
                    {
                        gridcs.ChangeBlockColor(i, j, 0);
                    }

                    FinishOperate();
                }
                else if (is_Vertical == 2)
                {
                    SavePreData();
                    int j = LT.y;
                    for (int i = 0; i < gridSize; i++)
                    {
                        gridcs.ChangeBlockColor(i, j, 0);
                    }

                    FinishOperate();
                }
            }
        }
        else // ?
        {

        }
    }
    public void Multiple_Up()
    {
        SavePreData();
        for (int i = OverlayLT.x; i <= OverlayRB.x; i++)
        {
            for (int k = 0; k < (OverlayRB.y - OverlayLT.y + 1); k++)
            {
                for (int a = 0; a < Num-1; a++)
                {

                    int j = OverlayLT.y - 1 - k -(OverlayRB.y-OverlayLT.y+1) * a;
                    gridcs.ChangeBlockColor(i, j, gridNow[i, OverlayRB.y-k]);
                }
            }
        }

        FinishOperate();
    }
    public void Multiple_Down()
    {
        SavePreData();
        for (int i = OverlayLT.x; i <= OverlayRB.x; i++)
        {
            for (int k = 0; k < (OverlayRB.y - OverlayLT.y + 1); k++)
            {
                for (int a = 0; a < Num-1; a++)
                {

                    int j = OverlayRB.y + 1 + k + (OverlayRB.y - OverlayLT.y + 1) * a;
                    gridcs.ChangeBlockColor(i, j, gridNow[i, OverlayLT.y + k]);
                }
            }
        }
        FinishOperate();
    }
    public void Multiple_Left()
    {
        SavePreData();
        for (int j = OverlayLT.y; j <= OverlayRB.y; j++)
        {
            for (int k = 0; k < (OverlayRB.x - OverlayLT.x + 1); k++)
            {
                for (int a = 0; a < Num-1; a++)
                {

                    int i = OverlayLT.x - 1 - k - (OverlayRB.x - OverlayLT.x + 1) * a;
                    gridcs.ChangeBlockColor(i, j, gridNow[OverlayRB.x-k, j]);
                }
            }
        }
        FinishOperate();
    }
    public void Multiple_Right()
    {
        SavePreData();
        for (int j = OverlayLT.y; j <= OverlayRB.y; j++)
        {
            for (int k = 0; k < (OverlayRB.x - OverlayLT.x + 1); k++)
            {
                for (int a = 0; a < Num-1; a++)
                {

                    int i = OverlayRB.x + 1 + k + (OverlayRB.x - OverlayLT.x + 1) * a;
                    gridcs.ChangeBlockColor(i, j, gridNow[OverlayLT.x + k, j]);
                }
            }
        }
        FinishOperate();
    }
    public void Division_Up()
    {
        SavePreData();
        for (int i = OverlayLT.x; i <= OverlayRB.x ; i++)
        {
            for (int j = OverlayLT.y+(OverlayRB.y - OverlayLT.y+1)/Num; j <= OverlayRB.y; j++)
            {
                gridcs.ChangeBlockColor(i, j, 0);
            }
        }
        FinishOperate();
    }
    public void Division_Down()
    {
        SavePreData();
        for (int i = OverlayLT.x; i <= OverlayRB.x; i++)
        {
            for (int j = OverlayLT.y ; j <= OverlayRB.y- (OverlayRB.y - OverlayLT.y + 1) / Num; j++)
            {
                gridcs.ChangeBlockColor(i, j, 0);
            }
        }
        FinishOperate();
    }
    public void Division_Left()
    {
        SavePreData();
        for (int i = OverlayLT.x + (OverlayRB.x - OverlayLT.x + 1) / Num; i <= OverlayRB.x; i++)
        {
            for (int j = OverlayLT.y ; j <= OverlayRB.y; j++)
            {
                gridcs.ChangeBlockColor(i, j, 0);
            }
        }
        FinishOperate();
    }
    public void Division_Right()
    {
        SavePreData();
        for (int i = OverlayLT.x ; i <= OverlayRB.x - (OverlayRB.x - OverlayLT.x + 1) / Num; i++)
        {
            for (int j = OverlayLT.y; j <= OverlayRB.y; j++)
            {
                gridcs.ChangeBlockColor(i, j, 0);
            }
        }
        FinishOperate();
    }

    void PopDisable()
    {

    }
    void Set_Overlay()
    {
        xy LT, RB = new xy();
        LT.x = OverlayStartPos.x < OverlayEndPos.x ? OverlayStartPos.x : OverlayEndPos.x;
        LT.y = OverlayStartPos.y < OverlayEndPos.y ? OverlayStartPos.y : OverlayEndPos.y;
        RB.x = OverlayStartPos.x > OverlayEndPos.x ? OverlayStartPos.x : OverlayEndPos.x;
        RB.y = OverlayStartPos.y > OverlayEndPos.y ? OverlayStartPos.y : OverlayEndPos.y;


        Overlay_Top.transform.position = (gridViewArr[LT.x, LT.y]+ gridViewArr[RB.x+1, LT.y])/2;
        Overlay_Top.transform.localScale = new Vector3(RB.x - LT.x+1f , 1f, 1f);
        Overlay_Bottom.transform.position = (gridViewArr[LT.x, RB.y+1] + gridViewArr[RB.x+1, RB.y+1])/2;
        Overlay_Bottom.transform.localScale = new Vector3(RB.x - LT.x+1f , 1f, 1f);
        Overlay_Left.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[LT.x,RB.y+1])/2;
        Overlay_Left.transform.localScale = new Vector3(1f, RB.y - LT.y +1f, 1f);
        Overlay_Right.transform.position = (gridViewArr[RB.x+1, LT.y]+gridViewArr[RB.x+1,RB.y+1])/2;
        Overlay_Right.transform.localScale = new Vector3(1f, RB.y - LT.y +1f, 1f);
        Overlay_field.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[RB.x + 1, RB.y + 1]) / 2;
        Overlay_field.transform.localScale = new Vector3(RB.x - LT.x + 1f, RB.y - LT.y + 1f, 1f);
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
    public void SetOperButton()
    {
        if (Stack_Oper[1] == 0)
            PlusButtonGO.GetComponent<Button>().interactable = false;
        else
            PlusButtonGO.GetComponent<Button>().interactable = true;
        if (Stack_Oper[2] == 0)
            MinusButtonGO.GetComponent<Button>().interactable = false;
        else
            MinusButtonGO.GetComponent<Button>().interactable = true;
        if (Stack_Oper[3] == 0)
            MultiplicationButtonGO.GetComponent<Button>().interactable = false;
        else
            MultiplicationButtonGO.GetComponent<Button>().interactable = true;
        if (Stack_Oper[4] == 0)
            DivisionButtonGO.GetComponent<Button>().interactable = false;
        else
            DivisionButtonGO.GetComponent<Button>().interactable = true;
        if (Stack_Oper[5] == 0)
            ChangeColorButtonGO.GetComponent<Button>().interactable = false;
        else
            ChangeColorButtonGO.GetComponent<Button>().interactable = true;
        if (Stack_Oper[6] == 0)
            EraseLineButtonGO.GetComponent<Button>().interactable = false;
        else
            EraseLineButtonGO.GetComponent<Button>().interactable = true;
    }
    public void OffInteractable_Num()
    {
        for(int i = 1; i < 10; i++)
        {
            NumGO[i].GetComponent<Button>().interactable = false;
        }
    }
    public void OnInteractable_Num()
    {
        for (int i = 1; i < 10; i++)
        {
            if(Stack_num[i]!=0)
                NumGO[i].GetComponent<Button>().interactable = true;
        }
    }
    public void SavePreData()
    {
        Undo.interactable = true;
        gridPre = (int[,])(gridNow.Clone());
        Stack_num_Pre = (int[])(Stack_num.Clone());
        Stack_Oper_Pre = (int[])(Stack_Oper.Clone());
    }
    public void LoadPreData()
    {
        Undo.interactable = false;
        for(int i = 0; i < gridSize; i++)
        {
            for(int j = 0; j < gridSize; j++)
            {
                gridcs.ChangeBlockColor(i, j, gridPre[i, j]);
            }
        }
        Stack_num = (int[])(Stack_num_Pre.Clone());
        for(int i=1;i<10;i++)
            StackOfNum[i].GetComponent<StackManager>().SetStack(Stack_num[i]);
        Stack_Oper = (int[])(Stack_Oper_Pre.Clone());
        for (int i = 1; i < 7; i++)
            StackOfOper[i].GetComponent<StackManager>().SetStack(Stack_Oper[i]);
        SetOperButton();
        remain_text.text = (++remain).ToString()+"번";
    }
    public void GotoTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void Reset()
    {
        SceneManager.LoadScene("Play");
    }
    public void GotoNext()
    {
        PlayerPrefs.SetInt("stage", stage + 1);
        SceneManager.LoadScene("Play");
    }
}
