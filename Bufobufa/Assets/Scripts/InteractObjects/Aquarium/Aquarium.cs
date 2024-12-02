using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Aquarium : MonoBehaviour
{
    public string NameIngredient = "None";
    private GameObject DisplayCount;
    public bool NormalTemperature = false;
    public bool NormalGround = false;
    public bool OnAquarium = false;
    public float NormalTimeCell = 3f;
    private float TimeCell = 666f;
    private float timerCell = 0f;
    public int CountCells = 0;
    public float TimeWaterSpend = -1f;
    private float timerWater = 0f;

    [SerializeField] private Sprite NullFase;
    [SerializeField] private Sprite FirstFase;
    [SerializeField] private Sprite SecondFase;
    [SerializeField] private Sprite ThirdFase;

    [SerializeField] private List<GameObject> CellsList = new List<GameObject>();
    private int NumCell = 0;
    private SpriteRenderer ChoiceCellSprite;

    public void ChangeCell(int ch)
    {
        GetAllCells();
        NumCell = (NumCell + ch + CellsList.Count) % CellsList.Count;
        NameIngredient = CellsList[NumCell].GetComponent<Ingredient>().IngredientName;
        ChoiceCellSprite.sprite = CellsList[NumCell].GetComponent<SpriteRenderer>().sprite;
        NormalTimeCell = CellsList[NumCell].GetComponent<Ingredient>().TimeInAquarium;
        timerCell = 0f;
    }

    private void OnMouseDown()
    {
        GetAllCells();
    }

    private void GetAllCells()
    {
        DisplayCount.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = CountCells.ToString();
        DisplayCount.GetComponent<Animator>().SetBool("On", true);
        StartCoroutine(waitDisplayCount());
        for (int i = 0; i < CountCells; i++)
        {
            StoreManager.Instance.AddIngridient(NameIngredient);
        }
        CountCells = 0;
    }

    IEnumerator waitDisplayCount()
    {
        yield return new WaitForSeconds(2);
        DisplayCount.GetComponent<Animator>().SetBool("On", false);
    }
    private void Start()
    {
        ChoiceCellSprite = transform.Find("ChoiceCell").GetComponent<SpriteRenderer>();
        if (CellsList.Count > 0)
        {
            NameIngredient = CellsList[NumCell].GetComponent<Ingredient>().IngredientName;
        }
        ChoiceCellSprite.sprite = CellsList[NumCell].GetComponent<SpriteRenderer>().sprite;
        NormalTimeCell = CellsList[NumCell].GetComponent<Ingredient>().TimeInAquarium;

        TimeCell = NormalTimeCell;
        DisplayCount = transform.Find("DisplayCount").gameObject;
    }
    private void Update()
    {
        if (OnAquarium && TimeWaterSpend != -1f)
        {
            timerWater += Time.deltaTime;
            if (timerWater >= TimeWaterSpend)
            {

            }
        }
        if (NormalTemperature || NormalGround) OnAquarium = true;
        else OnAquarium = false;
        if (OnAquarium && TimeCell != -1f) timerCell += Time.deltaTime;
        if (timerCell >= TimeCell)
        {
            CountCells++;
            timerCell = 0;
        }
        if (NormalGround)
        {
            if (NormalTemperature) TimeCell = NormalTimeCell;
            else TimeCell = NormalTimeCell * 2;
        }
        else
        {
            TimeCell = -1f;
            timerCell = 0f;
        }

        if (CountCells == 0)
        {
            GetComponent<SpriteRenderer>().sprite = NullFase;
        }
        else if (CountCells < 4)
        {
            GetComponent<SpriteRenderer>().sprite = FirstFase;
        }
        else if (CountCells < 9)
        {
            GetComponent<SpriteRenderer>().sprite = SecondFase;
        }
        else if (CountCells < 15)
        {
            GetComponent<SpriteRenderer>().sprite = ThirdFase;
        }
    }
}
