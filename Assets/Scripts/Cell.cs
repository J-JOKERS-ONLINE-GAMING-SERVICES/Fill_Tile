using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{

    public int Id;
    public bool IsNumber;
   
    public int Value;

    [SerializeField] public SpriteRenderer _cellRenderer;
    [SerializeField] private TMP_Text _numbersText;

    [SerializeField] public Image CellBoarder;
    [SerializeField] private Color _unsolvedColor;
    [SerializeField] private Color _solvedColor;
    [SerializeField] public GameObject InnerCell;
    public bool IsSolved;
  
    public GameObject FinalBoarder;

    private List<Color> cellColors;
    public void Init(bool isNumber, int colorValue, int numberValue)
    {
        name = name + Random.Range(0,1000);
        cellColors = GameManager.Colors;
        IsNumber = isNumber;
        if (IsNumber)
        {
            Id = colorValue;
            Value = numberValue;
            _numbersText.gameObject.SetActive(true);
            _numbersText.text = Value.ToString();
            _numbersText.color = _unsolvedColor;

            ActivateInnerCell(false);
           
            CellBoarder.gameObject.SetActive(true);
            GameManager.Instance.TotalValuesCell.Add(this);
          
        }
        else
        {
            Id = -2;
            Value = 0;
            _numbersText.gameObject.SetActive(false);
            ActivateInnerCell(true);
            CellBoarder.gameObject.SetActive(false);

    


        }
        _cellRenderer.color = cellColors[Id + 2];
    }

    public void CleanGrid(int id)
    {
       // print("Clean Grid");

        if (IsNumber || Id != id)
        {
            return;
        }

        Id = -2;
        _cellRenderer.color = cellColors[Id + 2];
        ActivateInnerCell(true);
     //   print("active inner cell");
        //print("clean grid");
      
    }
    
    public void ClearBorderColorfullCell(int id)
    {

        if (FinalBoarder != null)
        {
          
            if (Id != id)
            {
                return;
            }
            print("Destroy final border");
            Destroy(FinalBoarder);
            FinalBoarder = null;
          
        }
    }

    public void ClearBorderEmptyCell(int id)
    {

        if (Id == -2 && FinalBoarder != null)
        {
            if (FinalBoarder != GameManager.Instance.CurrentCellSelectionBoarder.gameObject)
            {
                Debug.Log("destroying empty board");
                GameManager.Instance.IsEmpty = false;
                InnerCell.SetActive(true);
                Destroy(FinalBoarder);
                FinalBoarder = null;
            }

        }
    }
    public void AddFinalBoarder(GameObject finalboarder)
    {
        FinalBoarder = finalboarder;

    }


    public void HighLightEmpty()
    {
       
        if (IsNumber) return;
        Id = -1;
        _cellRenderer.color = cellColors[Id + 2];

        //nida ActivateInnerCell(true);
        GameManager.Instance.IsEmpty = true;
        ActivateInnerCell(false);
        


        //  print("Higlight Empty");
    }
    public void FinalBoarderFunc(bool IsDestroy)
    {
        if(IsDestroy && FinalBoarder!=null)
        {
            Destroy(FinalBoarder);
            FinalBoarder = null;
        }


    }
    public void HighLightFilled(int id)
    {
       //   print("Higlight Filled");
      //  print("filled");

        Id = id;
        //  print(name);
        _cellRenderer.color = cellColors[Id + 2];
        ActivateInnerCell(false);
    
    }

    public void UpdateNumber(bool correct)
    {
        //  _numbersText.color = correct ? _solvedColor : _unsolvedColor;

       
        if (correct)
        {
            _numbersText.color = _solvedColor;

            ActivateBorderSprite(true);
            IsSolved = true;
            //print("solved color : "+name);

            // UnSolvedBG.SetActive(false);
        }
        else
        {
            _numbersText.color = _unsolvedColor;
            IsSolved = false;

            if (FinalBoarder)
            {
               // FinalBoarder.GetComponent<SpriteRenderer>().enabled = false;

            }

            //print("Unsolved color : " + name);
            //  UnSolvedBG.SetActive(true);


        }
    }

    public void ActivateBorderSprite(bool IsEnabled)
    {
        if (FinalBoarder)
        { 
        if (IsEnabled)
        {
            FinalBoarder.GetComponent<SpriteRenderer>().enabled = true;
                FinalBoarder.GetComponent<SpriteRenderer>().sortingOrder = 0;//nida
            FinalBoarder.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        }
        else
        {

                FinalBoarder.GetComponent<SpriteRenderer>().enabled = false;
            }
    }
    }

    public void ActivateInnerCell(bool IsActivate)
    {
        InnerCell.SetActive(IsActivate);


        //if (GameManager.Instance.IsEmpty)
        //{
        //    Debug.Log("here is empty");
        //    InnerCell.SetActive(false);
        //}


        //  print(name +" inner cell is : "+ IsActivate);

    }


    public void ScaleCell(float value)
    {
        //if(IsNumber)
        //{
        //    var temp = CellBoarder.rectTransform.sizeDelta;
        //    temp.y += value;

        //         CellBoarder.rectTransform.sizeDelta = temp;
        //}
    }

    public void CellBorder(bool IsActive)
    {
        if(IsNumber)
        {
            CellBoarder.gameObject.SetActive(IsActive);
          //  print("active cellborder");

        }
    }
}
