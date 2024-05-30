using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public UIManager UIManagerInstance;
    [SerializeField] private SpriteRenderer _bgPrefab;
    [SerializeField] private Cell _cellPrefab;
    public static List<Color> Colors;
    private bool hasGameFinished;
    [SerializeField] private float _gridSize = 2f;
    public bool Is_Tutorial;
    [SerializeField] private Level _level;
    [SerializeField] private Level[] Levels;
    private Vector2Int startGrid, endGrid;

    private int currentResetId;
    [SerializeField]
    private Cell[,] cells;
    [SerializeField] private List<Color> _cellColors;

    // [SerializeField] private float _ScaleValue = 0.5f;

    public SpriteRenderer CellSelectionBoarder;
    public SpriteRenderer CurrentCellSelectionBoarder;
    bool IsMatched;
    public int StarCounter = 0;
    public GameObject GameCompletePanel;
    public Image[] Stars;
   // public List<GameObject> Backgrounds = new List<GameObject>();

    public List<Cell> TotalValuesCell = new List<Cell>();

    public int HintCellNo = -1;
    public Vector2Int startUp, endUp;

    public bool IsGameStart;
    public bool IsEmpty = false;

//    public HapticManager Haptic_Manager;
    public Sprite[] bgcolors;
    public Image bg;
    public GameObject CellParent;

    public Text LevelText;
    public Camera _MainCamera;
    int CurrentLevel = 0;
    public GameObject[] TutorialHand;


    public GameObject[] levelsUI;
    public Sprite pinkImg, GrayImg;
    //    int current_Number = 0;
    public GameObject Hint_GrantedPage;
    public bool IsGet_Five_Hints = false;
    public GameObject AdsSticker;
    public Text Hints_Count;
    public Text StarsAmount;
    // Start is called before the first frame update
    void Start()
    {
        AD_Manager.instance.ShowInterstitialAd();
        UpdateHintUI();


        Instance = this;
        //bg.sprite = bgcolors[2];
        Colors = _cellColors;
        //  SpawnLevel();
//        Debug.Log("Level No :" + PlayerPrefs.GetInt(GameConstants.CurrentLevelNo));
        StarsAmount.text = PlayerPrefs.GetInt("TotalStars").ToString();

        LevelCount();



        if (!Is_Tutorial)
        {
            if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) >= Levels.Length)
            {
                CurrentLevel = Random.Range(0, Levels.Length);
                _level = Levels[CurrentLevel];
            }
            else
            {
                CurrentLevel = PlayerPrefs.GetInt(GameConstants.CurrentLevelNo);
                _level = Levels[CurrentLevel];
            }
            
        }
        else
        {
            CurrentLevel = 0;
            _level = Levels[0];
        }
        LevelText.text = "LEVEL " + (CurrentLevel + 1).ToString();
        
       
        StartButton();
    }

    



    void LevelCount()
    {
        //calculate current level number for ui only
        int OriginalNumber = PlayerPrefs.GetInt("ArrowUiLvlPos");
//        Debug.Log("OriginalNumber"+ OriginalNumber);
        for (int i = 0; i < 5; i++)
        {

            levelsUI[i].gameObject.GetComponent<Image>().sprite = GrayImg;

            if (i < OriginalNumber)
            {
                levelsUI[i].gameObject.GetComponent<Image>().sprite = pinkImg;

            }
            levelsUI[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = (CalculateFirstDigitLvlUI()+i).ToString();
            levelsUI[i].transform.GetChild(2).gameObject.SetActive(false);//arrow

        }
       
        levelsUI[OriginalNumber].transform.GetChild(2).gameObject.SetActive(true);//arrow
    }

    int CalculateFirstDigitLvlUI()
    {
        int x = 1;
        if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < 5) x = 1;
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < 10) x = 6;
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < 15) x = 11;
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < 20) x = 16;
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < 25) x = 21;
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < 30) x = 26;
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < 35) x = 31;
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < 40) x = 36;


        return x;
    }

    void ResetArrowIFLevelWin()
    {
        if (PlayerPrefs.GetInt("ArrowUiLvlPos") >=4) PlayerPrefs.SetInt("ArrowUiLvlPos", 0);
        PlayerPrefs.SetInt("ArrowUiLvlPos", PlayerPrefs.GetInt("ArrowUiLvlPos")+1);
    }


    // Update is called once per frame
    void Update()
    {
        if (IsGameStart)
        {

            DrawRectangle();
           
        }

    }

    private void SpawnLevel()
    {
        //SpawnBG
        SpriteRenderer bg = Instantiate(_bgPrefab, CellParent.transform);
        bg.size = new Vector2(_level.Columns + 0.05f, _level.Rows + 0.05f) * _gridSize;
        bg.transform.position = new Vector3(_level.Columns, _level.Rows, 0)
            * 0.5f * _gridSize;

        //SpawnCells
        cells = new Cell[_level.Rows, _level.Columns];
        for (int i = 0; i < _level.Rows; i++)
        {
            for (int j = 0; j < _level.Columns; j++)
            {
                cells[i, j] = Instantiate(_cellPrefab, CellParent.transform);
                bool isNumber = _level.NumberPositions.Contains(new Vector2Int(i, j));
                int colorIndex = _level.Data[i * _level.Columns + j];
                int numberValue = isNumber ? _level.NumberValue(new Vector2Int(i, j)) : 0;
                cells[i, j].Init(isNumber, colorIndex, numberValue);
                cells[i, j].transform.position = new Vector3(j + 0.5f, i + 0.5f, 0) * _gridSize;
            }
        }

        //SetUpCamera
        if (((CurrentLevel + 1) % 5) == 0)
        {
            if ((CurrentLevel + 1) != 5 && (CurrentLevel + 1) != 40 && (CurrentLevel + 1) != 45 && (CurrentLevel + 1) != 50 && (CurrentLevel + 1) != 75 && (CurrentLevel + 1) != 80 && (CurrentLevel + 1) != 85 && (CurrentLevel + 1) != 95 && (CurrentLevel + 1) != 100)
            {
                Camera.main.orthographicSize = Mathf.Max(_level.Rows, _level.Columns) * _gridSize + 0f;
            }
            else if ((CurrentLevel + 1) != 5)
            {
                Camera.main.orthographicSize = 25f;
            }
            if ((CurrentLevel + 1) == 40)
            {
                Camera.main.orthographicSize = 20f;
            }
            if ((CurrentLevel + 1) == 45)
            {
                Camera.main.orthographicSize = 22f;
            }
            if ((CurrentLevel + 1) == 85 || (CurrentLevel + 1) == 95 || (CurrentLevel + 1) == 100)
            {
                Camera.main.orthographicSize = 28f;
            }
        }
        else
        {
            Camera.main.orthographicSize = Mathf.Max(_level.Rows, _level.Columns) * _gridSize + 4f;
        }

        Vector3 camPos = Camera.main.transform.position;
        camPos.x = bg.transform.position.x;
        camPos.y = bg.transform.position.y;
        Camera.main.transform.position = camPos;
    }

    public void DrawRectangle()
    {
        if (hasGameFinished) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = Vector2Int.zero;
        gridPos.x = Mathf.FloorToInt(mousePos.y / _gridSize);
        gridPos.y = Mathf.FloorToInt(mousePos.x / _gridSize);

        if (Input.GetMouseButtonDown(0))
        {

            if (!IsValid(gridPos))
            {
                IsMouseClickedOnTable = false;
                return;
            }
            else
            {

                IsMouseClickedOnTable = true;
                MouseDown(gridPos);
            }



        }
        else if (Input.GetMouseButton(0))
        {

//            Haptic_Manager.TriggerHapticFeedback(0.1f, 0.017f, 0.001f);

            // Debug.Log("dragging");
            if (IsMouseClickedOnTable)
            {
                MousePressed(gridPos);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (IsMouseClickedOnTable)
            {
                MouseUP();
            }
            IsMouseClickedOnTable = false;
        }



    }

    public bool IsMouseClickedOnTable;
    private void MouseDown(Vector2Int gridposition)
    {
//        print("Mouse Down");
        IsEmpty = false;
        IsMatched = false;
        SpawnSelectionBoarder();
        startGrid = gridposition;
        endGrid = gridposition;


        if (!IsValid(startGrid)) return;
        HighLight();
    }
    private void MousePressed(Vector2Int gridposition)
    {
        // print("Mouse Pressed");

        // print("currentResetId : "+currentResetId);
        // print(startGrid + " : " + endGrid + " : reset id : " + currentResetId);
        CleanGrid(currentResetId);
        endGrid = gridposition;
        if (!IsValid(endGrid) || !IsValid(startGrid))
        {
            //  print("Out of Bound");

            return;
        }
        else
        {
            // print("In Bound");
        }

        HighLight();
    }
    private void MouseUP()
    {
//        print("Mouse UP");
        if (Is_Tutorial)
        {
            if (CellParent && CellParent.transform.GetChild(1).GetComponent<Cell>().FinalBoarder != null)
            {
                TutorialHand[0].SetActive(false);
                TutorialHand[1].SetActive(true);
            }
            if (CellParent && CellParent.transform.GetChild(3).GetComponent<Cell>().FinalBoarder != null)
            {
                TutorialHand[0].SetActive(false);
                TutorialHand[1].SetActive(false);
            }
        }
        
        startGrid = Vector2Int.zero;
        endGrid = Vector2Int.zero;
        if (IsMatched)
        {

            //  CurrentCellSelectionBoarder.enabled = true;
        }
        else
        {
            //  CurrentCellSelectionBoarder.enabled = false;


        }
        CurrentCellSelectionBoarder = null;
        //  print("check id : "+cells[4,4].Id);

        ActivateInnerCell(startUp, endUp);



        CounterEmptyCell(startUp, endUp);
        ActivateCellBorder();
        CleanGrid(-1);
        CheckWin();
    }
    public void CounterEmptyCell(Vector2Int Start, Vector2Int End)
    {
        int GridCellCounter = 0;
        int r = 0, c = 0;
        for (int i = Start.x; i <= End.x; i++)
        {
            for (int j = Start.y; j <= End.y; j++)
            {
                //|| cells[i, j].Id >= 0


                if (cells[i, j].Id == -1 || cells[i, j].Id >= 0)
                {
                    GridCellCounter += 1;
                    r = i;
                    c = j;

                }
                //print("GridCellCounter : " + GridCellCounter);
                if (cells[i, j].IsNumber)
                {
                    // numberPos = new Vector2Int(i, j);
                }
            }
        }
        if (GridCellCounter == 1)
        {
            //   print("1 cell selected");
            Destroy(cells[r, c].FinalBoarder);
            cells[r, c].FinalBoarder = null;
        }
        else
        {
            // print("More cell selected");


        }

        startUp = Vector2Int.zero;
        endUp = Vector2Int.zero;
    }


    public void ActivateInnerCell(Vector2Int Start, Vector2Int End)
    {
        int GridMatchCellCounter = 0;
        bool IsMatched = false;
        int value = -10;

        for (int i = Start.x; i <= End.x; i++)
        {
            for (int j = Start.y; j <= End.y; j++)
            {
                if (cells[i, j].IsNumber)
                {
                    value = cells[i, j].Value;
                }
                //  print(cells[i, j].Id);
                if (cells[Start.x, Start.y].Id == cells[i, j].Id)
                {

                    GridMatchCellCounter++;

                }
            }
        }

        if (value == GridMatchCellCounter)
        {

            IsMatched = true;
        }
        // print(IsMatched+": value : "+value +" : Matched cell : "+ GridMatchCellCounter);
        for (int i = Start.x; i <= End.x; i++)
        {
            for (int j = Start.y; j <= End.y; j++)
            {
                if (IsMatched)
                {
                    if (cells[i, j].Id >= 0 && !cells[i, j].IsNumber)
                    {
                        // GridCellCounter += 1;
                        cells[i, j].ActivateBorderSprite(true);
                        cells[i, j].ActivateInnerCell(false);
                    }
                    if (cells[i, j].IsNumber)
                    {
                        cells[i, j].CellBorder(false);
                        cells[i, j].ActivateInnerCell(false);
                    }
                }
                else
                {

                    if (cells[i, j].Id >= 0 && !cells[i, j].IsNumber)
                    {
                        // GridCellCounter += 1;
                        cells[i, j].ActivateBorderSprite(false);
                        cells[i, j].ActivateInnerCell(true);
                    }
                    if (cells[i, j].IsNumber)
                    {
                        cells[i, j].CellBorder(true);
                        cells[i, j].ActivateInnerCell(false);
                    }
                }
            }
        }
    }

    private bool IsValid(Vector2Int pos)
    {
        //return pos.x >= 0 && pos.y >= 0 && pos.x < _level.Columns && pos.y < _level.Rows;
        //
        // print(pos.x + " : " + pos.y + " : " + _level.Rows+" : "+ _level.Columns);
        return pos.x >= 0 && pos.y >= 0 && pos.x < _level.Rows && pos.y < _level.Columns;
    }
    private bool IsValidCell(int i, int j)
    {
        int numOfCells = 0;
        int id = cells[i, j].Id;
        for (int row = 0; row < _level.Rows; row++)
        {
            for (int col = 0; col < _level.Columns; col++)
            {
                numOfCells += cells[row, col].Id == id ? 1 : 0;
            }
        }

        return numOfCells == cells[i, j].Value;
    }
    private void CleanGrid(int id)
    {
        //  print("clean");
        for (int i = 0; i < _level.Rows; i++)
        {
            for (int j = 0; j < _level.Columns; j++)
            {
                cells[i, j].CleanGrid(id);
            }
        }
    }
    public Color Basecolor;
    private void Hint_CleanGrid(int id)
    {
        //  print("clean");
        for (int i = 0; i < _level.Rows; i++)
        {
            for (int j = 0; j < _level.Columns; j++)
            {
                if (cells[i, j].InnerCell.activeInHierarchy && cells[i, j]._cellRenderer.color != Basecolor)
                {
                    cells[i, j].CleanGrid(id);
                }
            }
        }
    }

    private void HighLight()
    {
        Vector2Int start, end;
        start = new Vector2Int(startGrid.x < endGrid.x ? startGrid.x : endGrid.x,
            startGrid.y < endGrid.y ? startGrid.y : endGrid.y);
        end = new Vector2Int(startGrid.x > endGrid.x ? startGrid.x : endGrid.x,
            startGrid.y > endGrid.y ? startGrid.y : endGrid.y);


        //  print(startGrid+" : "+endGrid);

        //  print(startGrid.x + " : " + startGrid.y + " : " + endGrid.x + " : " + endGrid.y);
        ScaleSelectionBoarder(startGrid, endGrid);


        startUp = start;
        endUp = end;

        int numberCells = 0;
        int GridCellCounter = 0;



        Vector2Int numberPos = Vector2Int.zero;
        for (int i = start.x; i <= end.x; i++)
        {
            for (int j = start.y; j <= end.y; j++)
            {
                numberCells += cells[i, j].IsNumber ? 1 : 0;
                // print(cells[i, j].name+ cells[i, j].Id);

                //  GridCellCounter+= cells[i, j].Id==-1 ? 1 : 0;
                if (cells[i, j].Id == -1)
                {
                    GridCellCounter += 1;
                }
                //    print("GridCellCounter : " + GridCellCounter);
                if (cells[i, j].IsNumber)
                {
                    numberPos = new Vector2Int(i, j);
                }
            }
        }


        if (numberCells == 0 || numberCells > 1) // when user grid contain 0 value's cell or greater than 1 value's cell
        {


            //    print("Number Cell : "+ numberCells);
            for (int i = start.x; i <= end.x; i++)
            {
                for (int j = start.y; j <= end.y; j++)
                {
                    if (cells[i, j].Id >= 0) // Clean whole Grid when grid contain any colorful cell
                    {
                        //  print("Cell Id > 0 : "+cells[i, j].name);
                        //  ClearBorderColorfullCell(cells[i, j].Id);
                        CleanGrid(cells[i, j].Id);
                    }
                    cells[i, j].ClearBorderEmptyCell(cells[i, j].Id);
                    if (numberCells > 1)
                    {
                        //   print("number cells > 1");
                        cells[i, j].ActivateBorderSprite(false);
                    }
                    else
                    {
                        //  print(start+" : "+ end);
                        //   print("number cells ==0");
                    }
                    cells[i, j].AddFinalBoarder(CurrentCellSelectionBoarder.gameObject);
                    cells[i, j].HighLightEmpty();
                }
            }
            currentResetId = -1;
        }
        if (numberCells == 1)  // when user grid contain exactly 1 value's Cell
        {
//            print("number cells 1");
            for (int i = start.x; i <= end.x; i++)
            {
                for (int j = start.y; j <= end.y; j++)
                {
                    if (cells[i, j].Id >= 0 &&
                        cells[i, j].Id != cells[numberPos.x, numberPos.y].Id)
                    {
                        // print("hello");
                        // ClearBorderColorfullCell(cells[i, j].Id);
                        CleanGrid(cells[i, j].Id);
                    }
                    if ((cells[i, j].Id >= 0 || cells[i, j].Id == -2) && cells[i, j].FinalBoarder != null)
                    {
                        if (cells[i, j].FinalBoarder != CurrentCellSelectionBoarder.gameObject)
                        {
                            //  print("bye");
                            //  GameObject obj = cells[i, j].FinalBoarder;
                            // ActivateCellBorder(cells[i, j].FinalBoarder.gameObject);
                            Destroy(cells[i, j].FinalBoarder);
                            Debug.Log("Destroy Matched Cell");
                            StarCounter = StarCounter + 1;
                            Debug.Log("Star Counter :" + StarCounter);


                            cells[i, j].FinalBoarder = null;
                            // print("bye : " + cells[i, j].name);
                        }

                    }



                    int id = cells[numberPos.x, numberPos.y].Id;

                    cells[i, j].HighLightFilled(id);
                    cells[i, j].AddFinalBoarder(CurrentCellSelectionBoarder.gameObject);
                    //  print("number cell 1");
                    //  ActivateInnerCell(false);
                }
            }
            currentResetId = cells[numberPos.x, numberPos.y].Id;


        }

        for (int i = 0; i < _level.Rows; i++)  // loop run for values' cells only
        {
            for (int j = 0; j < _level.Columns; j++)
            {
                if (cells[i, j].IsNumber)
                {
                    bool valid = IsValidCell(i, j);
                    //   print(valid);
                    if (valid)
                    {
                        // CurrentCellSelectionBoarder.enabled = true;
                    }
                    else
                    {
                        // CurrentCellSelectionBoarder.enabled = false;
                    }

                    cells[i, j].UpdateNumber(valid);

                }
            }
        }

        DeActivateCellBorder(startUp, endUp);

        ActivateCellBorder();


    }


    public void ActivateCellBorder()
    {


        for (int i = 0; i < _level.Rows; i++)  // loop run for values' cells only
        {
            for (int j = 0; j < _level.Columns; j++)
            {
                if (cells[i, j].IsNumber && cells[i, j].FinalBoarder == null && !cells[i, j].CellBoarder.gameObject.activeSelf)
                {
                    cells[i, j].CellBorder(true);
                }
            }
        }

    }

    public void DeActivateCellBorder(Vector2Int Start, Vector2Int End)
    {


        for (int i = Start.x; i <= End.x; i++)
        {
            for (int j = Start.y; j <= End.y; j++)
            {
                if (cells[i, j].IsNumber)
                {
                    cells[i, j].CellBorder(false);
                }
            }


        }

    }
    private void CheckWin()
    {
        for (int i = 0; i < _level.Rows; i++)
        {
            for (int j = 0; j < _level.Columns; j++)
            {
                if (cells[i, j].Id < 0)
                    return;

                if (cells[i, j].IsNumber && !IsValidCell(i, j))
                    return;
            }
        }

        hasGameFinished = true;
        //  print("Game is Finished");
        StartCoroutine(GameWin());
    }
    private IEnumerator GameWin()
    {
        int StarEarned = 3;
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.LevelWin);
        print("Game is Complete");
        yield return new WaitForSeconds(1f);
        CellParent.SetActive(false);
        GameCompletePanel.SetActive(true);
        if (StarCounter >= 15)
        {
            StarEarned = StarEarned - 2;
        }
        else if (StarCounter > 5 && StarCounter < 15)
        {
            StarEarned = StarEarned - 1;
        }
        // if (PlayerPrefs.GetInt(GameConstants.UnlockLevel) < PlayerPrefs.GetInt(GameConstants.CurrentLevelNo))
        //{
        //  PlayerPrefs.SetInt(GameConstants.UnlockLevel, PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) + 1);
        //}
        // PlayerPrefs.SetInt("StarsLevel" + PlayerPrefs.GetInt(GameConstants.CurrentLevelNo), StarEarned);
        // Debug.Log("Current Level No: " + PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) + "  Starn Earned :" + StarEarned);

        if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) == 9)
        {
            PlayerPrefs.SetInt(GameConstants.OpenModes, 2);
        }
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) == 19)
        {
            PlayerPrefs.SetInt(GameConstants.OpenModes, 3);
        }
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) == 29)
        {
            PlayerPrefs.SetInt(GameConstants.OpenModes, 4);
        }
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) == 39)
        {
            PlayerPrefs.SetInt(GameConstants.OpenModes, 5);
        }
        else if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) == 49)
        {
            PlayerPrefs.SetInt(GameConstants.OpenModes, 6);
        }


        string LvlUnlockString ="LvlUnlocked" + (PlayerPrefs.GetInt("CurrentLevelNo") +1).ToString();
        string StarOfThisLvl_pref= "LvlStars"+ PlayerPrefs.GetInt("CurrentLevelNo");

        if (PlayerPrefs.GetInt(StarOfThisLvl_pref) == 0)
        {
           // Debug.Log("Save Leaderboard Scores");
            PlayerPrefs.SetInt("TotalStars", PlayerPrefs.GetInt("TotalStars") + StarEarned);
            //Debug.Log("totalStars"+ PlayerPrefs.GetInt("TotalStars"));
            LeaderBoard_Manager.instance.SaveScores(PlayerPrefs.GetInt("TotalStars"));

        }

        PlayerPrefs.SetInt(StarOfThisLvl_pref, StarEarned);
        PlayerPrefs.SetInt(LvlUnlockString, 1);

//        Debug.Log(LvlUnlockString + ":"+ 1 );
  //      Debug.Log(StarOfThisLvl_pref +":"+ StarEarned );


       

        for (int i = 0; i < StarEarned; i++)
        {
            Stars[i].enabled = true;
        }
        //  UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void NextLevel()
    {

        Debug.Log("Current Level :" + PlayerPrefs.GetInt(GameConstants.CurrentLevelNo));
        //        Haptic_Manager.TriggerHapticFeedback(0.1f, 0.017f, 0.001f);
        //      if (Sound_Manager.instance.ad_manager == null) Sound_Manager.instance.ad_manager = GameObject.Find("AdsManager");
        AD_Manager.instance.ShowInterstitialAd();

        ResetArrowIFLevelWin();
        HapticManager.instance.click();

        if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) < Levels.Length && PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) != 99)
        {
            PlayerPrefs.SetInt(GameConstants.UnlockLevel, PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) + 1);
            PlayerPrefs.SetInt(GameConstants.CurrentLevelNo, PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) + 1);
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(1);
        }


    }
    public void SkipLevelOnRewardedAd()
    {
        AD_Manager.instance.ShowInterstitialAd();

    }
    public void SkipLevel()
    {
        Debug.Log("Skipped Level :" + PlayerPrefs.GetInt(GameConstants.CurrentLevelNo));
//        Haptic_Manager.TriggerHapticFeedback(0.1f, 0.017f, 0.001f);
        if (PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) <= Levels.Length && PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) != 99)
        {
            PlayerPrefs.SetInt(GameConstants.UnlockLevel, PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) + 1);
            PlayerPrefs.SetInt(GameConstants.CurrentLevelNo, PlayerPrefs.GetInt(GameConstants.CurrentLevelNo) + 1);
        }
        SceneManager.LoadScene(2);
    }
    public void SpawnSelectionBoarder()
    {
        // print("spawn boarder");
        CurrentCellSelectionBoarder = Instantiate(CellSelectionBoarder, CellParent.transform);

        EmptyGrayBoarders.Add(CurrentCellSelectionBoarder.gameObject);
    }
    public List<GameObject> EmptyGrayBoarders = new List<GameObject>(0);

    public void DestroyGrayBoardersOnHint()
    {
        for (int i = 0; i < EmptyGrayBoarders.Count; i++)
        {
            if (EmptyGrayBoarders[i] == null)
            {
                EmptyGrayBoarders.RemoveAt(i);
            }
            else if (EmptyGrayBoarders[i].GetComponent<SpriteRenderer>().color == Color.white)
            {
                Destroy(EmptyGrayBoarders[i]);
                EmptyGrayBoarders.RemoveAt(i);
            }
        }
    }
    public void ScaleSelectionBoarder(Vector2 StartCell, Vector2 EndCell)
    {

        int row = ((int)Mathf.Abs(StartCell.x - EndCell.x)) + 1;
        int column = ((int)Mathf.Abs(StartCell.y - EndCell.y)) + 1;


        int midrow = ((int)Mathf.Abs(StartCell.x + EndCell.x) / 2);
        int midcolumn = ((int)Mathf.Abs(StartCell.y + EndCell.y) / 2);



        int Max_row = (int)Mathf.Max(StartCell.x, EndCell.x);
        int Min_row = (int)Mathf.Min(StartCell.x, EndCell.x);

        int Max_column = (int)Mathf.Max(StartCell.y, EndCell.y);
        int Min_column = (int)Mathf.Min(StartCell.y, EndCell.y);

        // Vector2 pos = new Vector2((Max_row + Min_row) / 2, (Max_column + Min_column) / 2);
        float midpoint = Vector3.Distance(cells[Max_row, Max_column].transform.position, cells[Min_row, Min_column].transform.position) / 2;
        //  print("Mid point : "+ midpoint);

        //  print(Vector3.Distance(cells[Max_row, Max_column].transform.position, cells[Min_row, Min_column].transform.position));

        //     print(" : Max RowColumn : " + Max_row + " : " + Max_column + " : Min RowColumn : " + Min_row + " : " + Min_column);
        //  print("Rows : "+row+" and column : "+column);

        var temp = cells[Min_row, Min_column].transform.position;
        if (row == 1 || column == 1)
        {
            //  print("Row or column is 1");
            if (Min_row != Max_row)
            {
                temp.y += midpoint;

            }
            if (Min_column != Max_column)
            {
                temp.x += midpoint;
            }


        }
        else
        {

            temp.x += column - 1;
            temp.y += row - 1;
        }


        temp.z = -0.2f;


        //   print(midrow + " : " + midcolumn +" at " + cells[midrow, midcolumn].name);


        //  print("rows : "+row+" and column : "+column);

        CurrentCellSelectionBoarder.size = new Vector2(column + 0.05f, row + 0.05f) * _gridSize;
        CurrentCellSelectionBoarder.transform.position = temp;


    }

    public void ClearBorderColorfullCell(int id)
    {
        // print("Clear Border");
        for (int i = 0; i < _level.Rows; i++)
        {
            for (int j = 0; j < _level.Columns; j++)
            {
                cells[i, j].ClearBorderColorfullCell(id);
            }
        }
    }
    public void HintOnRewardedAd()
    {
        HapticManager.instance.click();
       /// Debug.Log("HintOnRewardedAd");
        if (PlayerPrefs.GetInt(GameConstants.SFX) == 0) return;
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
        if (PlayerPrefs.GetInt(GameConstants.HintCount)<1)
        {
           // Debug.Log("showing reward ad");
            AD_Manager.instance.ShowRewardedAd();
        }
        else
        {
            Hint();
        }


    }

    public void OnRewarded()
    {
        if (IsGet_Five_Hints)
        {
            IsGet_Five_Hints = false;
            PlayerPrefs.SetInt(GameConstants.HintCount,PlayerPrefs.GetInt(GameConstants.HintCount) + 5);
            Hint_GrantedPage.SetActive(false);

        }
        else
        {
            PlayerPrefs.SetInt(GameConstants.HintCount, PlayerPrefs.GetInt(GameConstants.HintCount) + 1);
            Hint_GrantedPage.SetActive(true);


        }
        UpdateHintUI();
    }

    public void UpdateHintUI()
    {
        if(PlayerPrefs.GetInt(GameConstants.HintCount)<1)
        AdsSticker.SetActive(true);
        else
        AdsSticker.SetActive(false);

        Hints_Count.text = PlayerPrefs.GetInt(GameConstants.HintCount).ToString();
    }

    public void OnClickGet5Hints()
    {
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);

        IsGet_Five_Hints = true;
        AD_Manager.instance.ShowRewardedAd();

    }

    public void OnClickNoThanks()
    {
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
        Hint_GrantedPage.SetActive(false);

    }



    public void Hint()
    {
        DestroyGrayBoardersOnHint();

        for (int i = 0; i < TotalValuesCell.Count; i++)
        {
           // Debug.Log("Hint Button Check" + i + " Is Sloved : " + TotalValuesCell[i].IsSolved);
            if (TotalValuesCell[i].IsSolved == false)
            {
             //   Debug.Log("Hint Button in the Solved" + HintCellNo);
                if (SiblingCellsFree(i))
                {
                    HintCellNo = i;
                 //   Debug.Log("Hint Button Clicked" + HintCellNo);
                    // CleanGrid(HintCellNo);
                    Hint_CleanGrid(HintCellNo);
                    break;
                }

            }
        }
        PlayerPrefs.SetInt((GameConstants.HintCount), PlayerPrefs.GetInt(GameConstants.HintCount) - 1);
        UpdateHintUI();
        if (HintCellNo == -1)
        {
            return;
        }

        DetectHintCells();
        //  TotalHints++;
    }

    public List<Cell> AutoSelectedCells = new List<Cell>();
    bool SiblingCellsFree(int cellNo)
    {
        bool IsSiblingCellsFree = true;




        int k = 0;
        for (int i = 0; i < _level.Rows; i++)
        {
            for (int j = 0; j < _level.Columns; j++)
            {

                if (TotalValuesCell[cellNo].Id == _level.Data[k])
                {
                    Debug.Log("Sibling Id : " + cells[i, j].Id);
                    Debug.Log("Sibling Number Bool : " + cells[i, j].IsNumber);
                    print(_level.Data[k]);
                    if (cells[i, j].Id != -2 && !cells[i, j].IsNumber)
                    {
                        if (cells[i, j].InnerCell.activeInHierarchy)
                        {
                            Debug.Log("Color Changing :" + cells[i, j]);
                            cells[i, j].Id = -2;
                            cells[i, j]._cellRenderer.color = Basecolor;

                        }
                        else
                        {
                            IsSiblingCellsFree = false;
                            break;
                        }
                    }
                }
                k++;
            }



        }






        return IsSiblingCellsFree;
    }
    public void DetectHintCells()
    {

        //if()
        //{

        //}




        if (hasGameFinished) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = Vector2Int.zero;
        gridPos.x = Mathf.FloorToInt(mousePos.y / _gridSize);
        gridPos.y = Mathf.FloorToInt(mousePos.x / _gridSize);


        MouseDown(gridPos);


        int k = 0;
        for (int i = 0; i < _level.Rows; i++)
        {
            for (int j = 0; j < _level.Columns; j++)
            {
                //  print(_level.Data[k]);
                if (TotalValuesCell[HintCellNo].Id == _level.Data[k])
                {
                    AutoSelectedCells.Add(cells[i, j]);

                    cells[i, j].AddFinalBoarder(CurrentCellSelectionBoarder.gameObject);
                    cells[i, j].HighLightFilled(TotalValuesCell[HintCellNo].Id);
                    cells[i, j].ActivateInnerCell(false);
                    if (cells[i, j].IsNumber)
                    {
                        cells[i, j].CellBorder(false);
                        cells[i, j].ActivateBorderSprite(true);
                        cells[i, j].UpdateNumber(true);
                    }


                    if (AutoSelectedCells.Count == 1)
                    {
                        startUp = new Vector2Int(i, j);
                    }
                    endUp = new Vector2Int(i, j);
                }
                k++;

            }
        }


        //    print(startUp+" : "+ endUp);
        ScaleSelectionBoarder(startUp, endUp);












        //  Start = startUp;
        // end = endUp;
        // startUp = Vector2Int.zero;
        // endUp = Vector2Int.zero;




        AutoSelectedCells.Clear();  //ok

        //startGrid = Vector2Int.zero;
        //endGrid = Vector2Int.zero;



        if (IsMatched)
        {

            //  CurrentCellSelectionBoarder.enabled = true;
        }
        else
        {
            //  CurrentCellSelectionBoarder.enabled = false;


        }
        CurrentCellSelectionBoarder = null;  //ok


        //   ActivateInnerCell(startUp, endUp);



        // CounterEmptyCell(startUp, endUp);
        ActivateCellBorder();  //ok
                               // CleanGrid(-1);
        CheckWin();

        HintCellNo = -1;
    }


    #region UI Button Actions

    public void StartButton()
    {
       // Haptic_Manager.TriggerHapticFeedback(0.3f, 0.02f, 0.03f);//for click

        IsGameStart = true;
        UIManagerInstance.UserInterface.GamePlayPanel.SetActive(true);
        //UIManagerInstance.UserInterface.MainMenuPanel.SetActive(false);

        SpawnLevel();

    }
    public void SettingsButton()
    {
      //  Haptic_Manager.TriggerHapticFeedback(0.3f, 0.02f, 0.03f);//for click

        if (UIManagerInstance.UserInterface.SettingsPanel.activeSelf)
        {

            UIManagerInstance.UserInterface.SettingsPanel.SetActive(false);

        }
        else
        {
            UIManagerInstance.UserInterface.SettingsPanel.SetActive(true);
        }



    }
    public void RemoveAdsButton()
    {
        //Haptic_Manager.TriggerHapticFeedback(0.3f, 0.02f, 0.03f);//for click

        print("Remove Ads Button");

    }
    public void VibrationButton()
    {
       // Haptic_Manager.TriggerHapticFeedback(0.3f, 0.02f, 0.03f);//for click

        print("Vibration Button");

    }
    public void MusicButton()
    {
        //Haptic_Manager.TriggerHapticFeedback(0.3f, 0.02f, 0.03f);//for click

        print("Music Button");

    }
 
    int bgIndex = 1;

    public void ChangeBackgoundColor()
    {
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
        HapticManager.instance.click();

        if (bgIndex >= bgcolors.Length) bgIndex = 0;

        bg.sprite = bgcolors[bgIndex];
        bgIndex++;
    }

    public void HomeButtonGamePlay()
    {
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
        HapticManager.instance.click();

        SceneManager.LoadScene(1);
        

    }
    public void Restart()
    {
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
        HapticManager.instance.click();

        SceneManager.LoadScene(2);
        print("Restart Button");
    }
    #endregion



}
