using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MapEditorEC : MonoBehaviour
{
    // 상수값들
    const float width = 2208;
    const float height = 1242;
    float ratio;

    public Dropdown MapSizeDropdown;
    public GameObject[] MapGrid = new GameObject[3]; // 4*4,6*6,8*8
    public GameObject[] InitMapGrid = new GameObject[3];
    public GameObject[] AnswerMapGrid = new GameObject[3];
    public int[,] Map_4 = new int[4, 4];
    public int[,] Map_6 = new int[6, 6];
    public int[,] Map_8 = new int[8 ,8];


    public Vector2[,] gridArr;
    public Vector2[,] gridViewArr;
    public int[,] gridNow = new int[11, 11];
    public GameObject Blocks;
    public Vector2[,] gridArrInit;
    public Vector2[,] gridArrAnswer;

    public GameObject gridGO;
    private ME_grid gridcs;
    public int gridSize;
    public GameObject gridAnswerGO;
    //private gridAnswer grAcs;


    public InputField SaveInputField,LoadInputField;
    public Toggle SaveToggle;
    public InputField[] StackOperIF = new InputField[7];
    public InputField[] StackNumIF = new InputField[10];
    public InputField RemainIF;

    public int[] Stack_Oper = new int[7]; // 0: null, 1 : + , 2 : - , 3: ×, 4 : ÷
    public GameObject[] StackOfOper = new GameObject[7];
    public int[] Stack_num = new int[7];
    public GameObject[] StackOfNum = new GameObject[10];

    public int Oper; // 0 : 아무것도, 1 : + , 2 : - , 3: ×, 4 : ÷
    public int color; // 0: 흰색 1: 빨간색 2: 노란색 3: 파란색 4: 초록색 5: 검은색
    public int Num;

    public int color_from;
    public int color_to;
    public int is_Vertical; //0: null , 1:vertical 2: horizental

    public GameObject PlusButtonGO;
    private ME_PlusButton PBcs;
    public GameObject PalleteGO;
    private ME_Palette Pcs;
    public GameObject MinusButtonGO;
    private ME_MinusButton MBcs;
    public GameObject MultiplicationButtonGO;
    private ME_MultiplicationButton MPBcs;
    public GameObject DivisionButtonGO;
    private ME_DivisionButton DBcs;
    public GameObject ChangeColorButtonGO;
    public GameObject EraseLineButtonGO;

    public GameObject[] NumGO = new GameObject[10];
    public ME_NumButton[] Numcs = new ME_NumButton[10];

    public Button Undo;
    public int[,] gridPre = new int[11, 11];
    public int Oper_Pre;
    public int Num_Pre;

    public struct stageSRC
    {
        public int size;
        public int[,] init;
        public int[,] answer;
        public int[] Oper;
        public int[] Num;
        public int remain;

        public stageSRC(int size, int[,] init, int[,] answer, int[] Oper, int[] Num, int remain)
        {
            this.size = size;
            this.init = init;
            this.answer = answer;
            this.Oper = Oper;
            this.Num = Num;
            this.remain = remain;
        }
    }
    public stageSRC newstageSRC;

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

    public GameObject Overlay, Overlay_Top, Overlay_Bottom, Overlay_Left, Overlay_Right, Overlay_field;
    public GameObject Direction_Button, UpButton, DownButton, LeftButton, RightButton;
    public GameObject Direction_Button_Division, UpButton_Division, DownButton_Division, LeftButton_Division, RightButton_Division;

    bool isOverlay = false;

    // Start is called before the first frame update
    private void Awake()
    {
        MapGrid[MapSizeDropdown.value].SetActive(true);
        InitMapGrid[MapSizeDropdown.value].SetActive(true);
        AnswerMapGrid[MapSizeDropdown.value].SetActive(true);

        ratio = Screen.width / width;

        gridArr = new Vector2[11, 11];
        gridViewArr = new Vector2[11, 11];
        gridArrInit = new Vector2[11, 11];
        gridArrAnswer = new Vector2[11, 11];

        gridcs = gridGO.GetComponent<ME_grid>();
        //grAcs = gridAnswerGO.GetComponent<gridAnswer>();
        PBcs = PlusButtonGO.GetComponent<ME_PlusButton>();
        Pcs = PalleteGO.GetComponent<ME_Palette>();
        MBcs = MinusButtonGO.GetComponent<ME_MinusButton>();
        MPBcs = MultiplicationButtonGO.GetComponent<ME_MultiplicationButton>();
        DBcs = DivisionButtonGO.GetComponent<ME_DivisionButton>();
        for (int i = 0; i < 10; i++)
        {
            Numcs[i] = NumGO[i].GetComponent<ME_NumButton>();
        }
        gridSize = 4;
        gridcs.MakeGrid(gridSize);
        //grAcs.MakeGrid(4);
        
        OffInteractable_Num();
        SetOperButton();
        Initialize();

        newstageSRC = new stageSRC(gridSize, new int[gridSize, gridSize], new int[gridSize, gridSize], new int[7], new int[10], 0);

    }
    void Start()
    {
        StreamWriter SW = new StreamWriter(new FileStream("Assets\\" + "imsi.txt", FileMode.OpenOrCreate));
        SW.Flush();
        for (int k = 1; k <= 11; k++)
        {
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    if (j < 10)
                        SW.WriteLine(k.ToString()+"0" + i.ToString());
                    else
                        SW.WriteLine(k.ToString() + "0" + i.ToString() );
                }
            }
            for (int i = 10; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    if (j < 10)
                        SW.WriteLine(k.ToString() + i.ToString() );
                    else
                        SW.WriteLine(k.ToString() + i.ToString());
                }
            }
        }
        SW.WriteLine("");
        SW.Close();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 NowCurPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);;

        if (Input.GetKeyDown(KeyCode.Mouse0))
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
                if (NowCurPos.x > gridViewArr[0, 0].x && NowCurPos.x < gridViewArr[gridSize, gridSize].x && NowCurPos.y < gridViewArr[0, 0].y && NowCurPos.y > gridViewArr[gridSize, gridSize].y)
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

    void Operate()
    {
        xy LT, RB = new xy();
        LT.x = startPos.x < endPos.x ? startPos.x : endPos.x;
        LT.y = startPos.y < endPos.y ? startPos.y : endPos.y;
        RB.x = startPos.x > endPos.x ? startPos.x : endPos.x;
        RB.y = startPos.y > endPos.y ? startPos.y : endPos.y;
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
            if (isable && cnt == Num)
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
            if (isable && cnt == Num)
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
                Direction_Button.SetActive(false);
                goto end;
            }

            bool isableUp, isableDown, isableLeft, isableRight;
            isableUp = isableDown = isableLeft = isableRight = true;

            //위로 가능한지 확인
            if (LT.y - (Num - 1) * (RB.y - LT.y + 1) < 0)
            {
                isableUp = false; ;
            }
            else
            {
                for (int i = LT.x; i <= RB.x; i++)
                {
                    for (int j = LT.y - 1; j >= LT.y - (Num - 1) * (RB.y - LT.y + 1); j--)
                    {
                        if (gridNow[i, j] != 0)
                            isableUp = false;
                    }
                }
            }

            //아래로 가능한지 확인
            if (RB.y + (Num - 1) * (RB.y - LT.y + 1) >= gridSize)
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
            if (LT.x - (Num - 1) * (RB.x - LT.x + 1) < 0)
            {
                isableLeft = false;
            }
            else
            {
                for (int i = LT.x - 1; i >= LT.x - (Num - 1) * (RB.x - LT.x + 1); i--)
                {
                    for (int j = LT.y; j <= RB.y; j++)
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
                for (int i = RB.x + 1; i <= RB.x + (Num - 1) * (RB.x - LT.x + 1); i++)
                {
                    for (int j = LT.y; j <= RB.y; j++)
                    {
                        if (gridNow[i, j] != 0)
                            isableRight = false;
                    }
                }
            }


            UpButton.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[RB.x + 1, LT.y]) / 2f + new Vector2(0f, 50f) * ratio;
            DownButton.transform.position = (gridViewArr[LT.x, RB.y + 1] + gridViewArr[RB.x + 1, RB.y + 1]) / 2f - new Vector2(0f, 50f) * ratio;
            LeftButton.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[LT.x, RB.y + 1]) / 2f - new Vector2(50f, 0f) * ratio;
            RightButton.transform.position = (gridViewArr[RB.x + 1, LT.y] + gridViewArr[RB.x + 1, RB.y + 1]) / 2f + new Vector2(50f, 0f) * ratio;
            if (isableUp || isableDown || isableLeft || isableRight)
                isOverlay = true;

            UpButton.SetActive(isableUp);
            DownButton.SetActive(isableDown);
            LeftButton.SetActive(isableLeft);
            RightButton.SetActive(isableRight);

            end:;
        }
        else if (Oper == 4) // ÷
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
            else
            {
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
                            if (gridNow[i, j] != gridNow[i, LT.y + k])
                            {
                                isVertical = false;
                            }

                        }
                    }
                }
            }

            DownButton_Division.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[RB.x + 1, LT.y]) / 2f - new Vector2(0f, 50f) * ratio;
            UpButton_Division.transform.position = (gridViewArr[LT.x, RB.y + 1] + gridViewArr[RB.x + 1, RB.y + 1]) / 2f + new Vector2(0f, 50f) * ratio;
            RightButton_Division.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[LT.x, RB.y + 1]) / 2f + new Vector2(50f, 0f) * ratio;
            LeftButton_Division.transform.position = (gridViewArr[RB.x + 1, LT.y] + gridViewArr[RB.x + 1, RB.y + 1]) / 2f - new Vector2(50f, 0f) * ratio;
            if (isVertical || isHorizental)
                isOverlay = true;

            UpButton_Division.SetActive(isVertical);
            DownButton_Division.SetActive(isVertical);
            LeftButton_Division.SetActive(isHorizental);
            RightButton_Division.SetActive(isHorizental);

            end:;
        }
        else if (Oper == 5) // change_color
        {
            if (color_from != 0 && color_to != 0)
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
        else if (Oper == 6) // erase_line
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
                for (int a = 0; a < Num - 1; a++)
                {

                    int j = OverlayLT.y - 1 - k - (OverlayRB.y - OverlayLT.y + 1) * a;
                    gridcs.ChangeBlockColor(i, j, gridNow[i, OverlayRB.y - k]);
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
                for (int a = 0; a < Num - 1; a++)
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
                for (int a = 0; a < Num - 1; a++)
                {

                    int i = OverlayLT.x - 1 - k - (OverlayRB.x - OverlayLT.x + 1) * a;
                    gridcs.ChangeBlockColor(i, j, gridNow[OverlayRB.x - k, j]);
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
                for (int a = 0; a < Num - 1; a++)
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
        for (int i = OverlayLT.x; i <= OverlayRB.x; i++)
        {
            for (int j = OverlayLT.y + (OverlayRB.y - OverlayLT.y + 1) / Num; j <= OverlayRB.y; j++)
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
            for (int j = OverlayLT.y; j <= OverlayRB.y - (OverlayRB.y - OverlayLT.y + 1) / Num; j++)
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
            for (int j = OverlayLT.y; j <= OverlayRB.y; j++)
            {
                gridcs.ChangeBlockColor(i, j, 0);
            }
        }
        FinishOperate();
    }
    public void Division_Right()
    {
        SavePreData();
        for (int i = OverlayLT.x; i <= OverlayRB.x - (OverlayRB.x - OverlayLT.x + 1) / Num; i++)
        {
            for (int j = OverlayLT.y; j <= OverlayRB.y; j++)
            {
                gridcs.ChangeBlockColor(i, j, 0);
            }
        }
        FinishOperate();
    }

    public void SavePreData()
    {
        Undo.interactable = true;
        gridPre = (int[,])(gridNow.Clone());
        Num_Pre = Num;
        Oper_Pre = Oper;
    }
    public void LoadPreData()
    {
        Undo.interactable = false;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                gridcs.ChangeBlockColor(i, j, gridPre[i, j]);
            }
        }
        StackNumIF[Num_Pre].text = (int.Parse(StackNumIF[Num_Pre].text) - 1).ToString();
        StackOperIF[Oper_Pre].text = (int.Parse(StackOperIF[Oper_Pre].text) - 1).ToString();
        RemainIF.text = (int.Parse(RemainIF.text) - 1).ToString();
        /*  Stack_num = (int[])(Stack_num_Pre.Clone());
          for (int i = 1; i < 10; i++)
              StackOfNum[i].GetComponent<StackManager>().SetStack(Stack_num[i]);
          Stack_Oper = (int[])(Stack_Oper_Pre.Clone());
          for (int i = 1; i < 5; i++)
              StackOfOper[i].GetComponent<StackManager>().SetStack(Stack_Oper[i]);*/
    }
    public void SetMapSize()
    {
        foreach (GameObject GO in MapGrid)
            GO.SetActive(false);
        foreach (GameObject GO in InitMapGrid)
            GO.SetActive(false);
        foreach (GameObject GO in AnswerMapGrid)
            GO.SetActive(false);
        MapGrid[MapSizeDropdown.value].SetActive(true);
        InitMapGrid[MapSizeDropdown.value].SetActive(true);
        AnswerMapGrid[MapSizeDropdown.value].SetActive(true);
        if(MapSizeDropdown.value==0)
            gridSize = 4;
        else if (MapSizeDropdown.value == 1)
            gridSize = 6;
        else if (MapSizeDropdown.value == 2)
            gridSize = 8;
        for (int i = Blocks.transform.childCount - 1; i >= 0; i--)
            Destroy(Blocks.transform.GetChild(i).gameObject);
        
        newstageSRC = new stageSRC(gridSize,new int[gridSize,gridSize],new int[gridSize,gridSize],new int[7],new int[10],0);

        newstageSRC.size = gridSize;
        Debug.Log(newstageSRC.size);
        Initialize();
        gridNow = new int[11, 11];
        gridcs.MakeGrid(gridSize);
    }

    public void Initialize()
    {
        for (int i = gridcs.Blocks_Init.transform.childCount - 1; i >= 0; i--)
            Destroy(gridcs.Blocks_Init.transform.GetChild(i).gameObject);

        for (int i = gridcs.Blocks_Answer.transform.childCount - 1; i >= 0; i--)
            Destroy(gridcs.Blocks_Answer.transform.GetChild(i).gameObject);

        for (int i = 1; i <= 6; i++)
            StackOperIF[i].text = "0";
        for (int i = 1; i <= 9; i++)
            StackNumIF[i].text = "0";
        RemainIF.text = "0";
    }

    public void SaveData()
    {
        StreamWriter SW = new StreamWriter(new FileStream("Assets\\"+SaveInputField.text+".txt", FileMode.OpenOrCreate));
        if (SaveToggle.isOn)
        {
            SW.Flush();
            string initstr = "";
            string answerstr = "";
            string Operstr = "";
            string Numstr = "";
            for (int i = 0; i < newstageSRC.size; i++)
                for (int j = 0; j < newstageSRC.size; j++)
                    initstr += newstageSRC.init[i, j];
            for (int i = 0; i < newstageSRC.size; i++)
                for (int j = 0; j < newstageSRC.size; j++)
                    answerstr += newstageSRC.answer[i, j];
            for (int i = 0; i <= 6; i++)
                Operstr += newstageSRC.Oper[i];
            for (int i = 0; i <= 9; i++)
                Numstr += newstageSRC.Num[i];
            SW.WriteLine(newstageSRC.size + "\n" + initstr + "\n" + answerstr + "\n" + Operstr + "\n" + Numstr + "\n" + newstageSRC.remain + "\n");
            SW.Close();
            Debug.Log(SaveInputField.text + ".txt is successfully \"Press\"saved");
        }
        else
        {
            SW.Flush();
            string initstr = "";
            string answerstr = "";
            string Operstr = "";
            string Numstr = "";
            for (int i = 0; i < newstageSRC.size; i++)
            {
                initstr += "{";
                for (int j = 0; j < newstageSRC.size; j++)
                {
                    initstr += newstageSRC.init[i, j];
                    if (j != newstageSRC.size - 1)
                        initstr += ",";
                }
                initstr += "}";
                 if (i == newstageSRC.size - 1)
                    initstr += "\n";
                else
                    initstr += ",\n";
            }
            for (int i = 0; i < newstageSRC.size; i++)
            {
                answerstr += "{";
                for (int j = 0; j < newstageSRC.size; j++)
                {
                    answerstr += newstageSRC.answer[i, j];
                    if (j != newstageSRC.size - 1)
                        answerstr += ",";
                }
                answerstr += "}";
                if (i == newstageSRC.size - 1)
                    answerstr += "\n";
                else
                    answerstr += ",\n";
            }
            Operstr += "{";
            for (int i = 0; i <= 6; i++)
            {
                Operstr += newstageSRC.Oper[i];
                if (i != 6)
                    Operstr += ",";
            }
            Operstr += "}";
            Numstr += "{";
            for (int i = 0; i <= 9; i++)
            {
                Numstr += newstageSRC.Num[i];
                if (i != 9)
                    Numstr += ",";
            }
            Numstr += "}";
            SW.WriteLine("new stageSRC(" + newstageSRC.size + ",\nnew int[,] {\n" + initstr + "},\nnew int[,] {\n" + answerstr + "},\nnew int[] " + Operstr + ",\nnew int[] " + Numstr + ",\n" + newstageSRC.remain + "\n"  + ")\n");
            SW.Close();
            Debug.Log(SaveInputField.text + ".txt is successfully saved");
        }
    }
    public void LoadData()
    {
        StreamReader SR = new StreamReader(new FileStream("Assets\\" + LoadInputField.text + ".txt", FileMode.Open));
        string initstr = "";
        string answerstr = "";
        string Operstr = "";
        string Numstr = "";
        newstageSRC.size = int.Parse(SR.ReadLine());
        newstageSRC.init = new int[newstageSRC.size, newstageSRC.size];
        newstageSRC.answer = new int[newstageSRC.size, newstageSRC.size];
        newstageSRC.Oper = new int[7];
        newstageSRC.Num = new int[10];
        initstr = SR.ReadLine();
        for (int i = 0; i < newstageSRC.size; i++)
            for (int j = 0; j < newstageSRC.size; j++)
                newstageSRC.init[i, j]=initstr[i*newstageSRC.size+j]-'0';
        answerstr = SR.ReadLine();
        for (int i = 0; i < newstageSRC.size; i++)
            for (int j = 0; j < newstageSRC.size; j++)
                newstageSRC.answer[i, j] = answerstr[i * newstageSRC.size + j] - '0';
        Operstr = SR.ReadLine();
        for (int i = 0; i <= 6; i++)
            newstageSRC.Oper[i] = Operstr[i] - '0'; 
        Numstr = SR.ReadLine();
        for (int i = 0; i <= 9; i++)
            newstageSRC.Num[i] = Numstr[i] - '0';
        newstageSRC.remain = int.Parse(SR.ReadLine());

        gridSize = newstageSRC.size;
        
        if (gridSize == 4)
            MapSizeDropdown.value = 0;
        else if (gridSize == 6)
            MapSizeDropdown.value = 1;
        else if (gridSize == 8)
            MapSizeDropdown.value = 2;
        
        for (int i = Blocks.transform.childCount - 1; i >= 0; i--)
            Destroy(Blocks.transform.GetChild(i).gameObject);
        for (int i = gridcs.Blocks_Init.transform.childCount - 1; i >= 0; i--)
            Destroy(gridcs.Blocks_Init.transform.GetChild(i).gameObject);
        for (int i = gridcs.Blocks_Answer.transform.childCount - 1; i >= 0; i--)
            Destroy(gridcs.Blocks_Answer.transform.GetChild(i).gameObject);
        for (int i = 1; i <= 6; i++)
            StackOperIF[i].text = newstageSRC.Oper[i].ToString();
        for (int i = 1; i <= 9; i++)
            StackNumIF[i].text = newstageSRC.Num[i].ToString();
        RemainIF.text = newstageSRC.remain.ToString();

        gridNow = (int[,])newstageSRC.answer.Clone();
        gridcs.MakeGridAnswer(gridSize);
        gridNow = (int[,])newstageSRC.init.Clone();
        gridcs.MakeGridInit(gridSize);
        gridcs.MakeGrid(gridSize);

        SR.Close();
        Debug.Log(LoadInputField.text + ".txt is successfully loaded");
    }
    public void Clear()
    {
        OffInteractable_Num();
        Clear_Num();
        Clear_Oper();
        Overlay.SetActive(false);
        Direction_Button.SetActive(false);
        Direction_Button_Division.SetActive(false);
        isOverlay = false;
    }
    public void Clear_Oper()
    {
        Oper = 0;
        color = 0;
        PBcs.Initialize();
        MBcs.Initialize();
        MPBcs.Initialize();
        DBcs.Initialize();
        ChangeColorButtonGO.GetComponent<ME_ChangeColor>().Initialize();
        EraseLineButtonGO.GetComponent<ME_EraseLine>().Initialize();
    }
    public void Clear_Num()
    {
        Num = 999;
        for (int i = 1; i < 10; i++)
        {
            Numcs[i].Initialize();
        }

    }
    void Set_Overlay()
    {
        xy LT, RB = new xy();
        LT.x = OverlayStartPos.x < OverlayEndPos.x ? OverlayStartPos.x : OverlayEndPos.x;
        LT.y = OverlayStartPos.y < OverlayEndPos.y ? OverlayStartPos.y : OverlayEndPos.y;
        RB.x = OverlayStartPos.x > OverlayEndPos.x ? OverlayStartPos.x : OverlayEndPos.x;
        RB.y = OverlayStartPos.y > OverlayEndPos.y ? OverlayStartPos.y : OverlayEndPos.y;


        Overlay_Top.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[RB.x + 1, LT.y]) / 2;
        Overlay_Top.transform.localScale = new Vector3(RB.x - LT.x + 1f, 1f, 1f)*(4f/gridSize);
        Overlay_Bottom.transform.position = (gridViewArr[LT.x, RB.y + 1] + gridViewArr[RB.x + 1, RB.y + 1]) / 2;
        Overlay_Bottom.transform.localScale = new Vector3(RB.x - LT.x + 1f, 1f, 1f) * (4f / gridSize); ;
        Overlay_Left.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[LT.x, RB.y + 1]) / 2;
        Overlay_Left.transform.localScale = new Vector3(1f, RB.y - LT.y + 1f, 1f) * (4f / gridSize); ;
        Overlay_Right.transform.position = (gridViewArr[RB.x + 1, LT.y] + gridViewArr[RB.x + 1, RB.y + 1]) / 2;
        Overlay_Right.transform.localScale = new Vector3(1f, RB.y - LT.y + 1f, 1f) * (4f / gridSize); ;
        Overlay_field.transform.position = (gridViewArr[LT.x, LT.y] + gridViewArr[RB.x + 1, RB.y + 1]) / 2;
        Overlay_field.transform.localScale = new Vector3(RB.x - LT.x + 1f, RB.y - LT.y + 1f, 1f) * (4f / gridSize); ;
    }
    public void FinishOperate()
    {/*
        Stack_Oper[Oper]--;
        StackOfOper[Oper].GetComponent<StackManager>().SetStack(Stack_Oper[Oper]);
        Stack_num[Num]--;
        StackOfNum[Num].GetComponent<StackManager>().SetStack(Stack_num[Num]);*/
        if(Oper !=5 && Oper!=6)
            StackNumIF[Num].text = (int.Parse(StackNumIF[Num].text) + 1).ToString();
        StackOperIF[Oper].text = (int.Parse(StackOperIF[Oper].text) + 1).ToString();
        for (int i = 1; i <= 6; i++)
            newstageSRC.Oper[i] = int.Parse(StackOperIF[i].text);
        for (int i = 1; i <= 9; i++)
            newstageSRC.Num[i] = int.Parse(StackNumIF[i].text);
        RemainIF.text = (int.Parse(RemainIF.text) + 1).ToString();
        newstageSRC.remain = int.Parse(RemainIF.text);
        Clear_Oper();
        Clear_Num();
        Overlay.SetActive(false);
        Direction_Button.SetActive(false);
        Direction_Button_Division.SetActive(false);
        OffInteractable_Num();
        SetOperButton();
    }
    public void SetOperButton()
    {
    }
    public void OffInteractable_Num()
    {
        for (int i = 1; i < 10; i++)
        {
            NumGO[i].GetComponent<Button>().interactable = false;
        }
    }
    public void OnInteractable_Num()
    {
        for (int i = 1; i < 10; i++)
        {
                NumGO[i].GetComponent<Button>().interactable = true;
        }
    }
    public void SetInit()
    {
        Initialize();
        for (int i = gridcs.Blocks_Init.transform.childCount - 1; i >= 0; i--)
            Destroy(gridcs.Blocks_Init.transform.GetChild(i).gameObject);
        gridcs.MakeGridInit(gridSize);
    }
    public void SetAnswer()
    {
        for (int i = gridcs.Blocks_Answer.transform.childCount - 1; i >= 0; i--)
            Destroy(gridcs.Blocks_Answer.transform.GetChild(i).gameObject);
        gridcs.MakeGridAnswer(gridSize);
    }
    void PopDisable()
    {

    }
    public void gotoTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
