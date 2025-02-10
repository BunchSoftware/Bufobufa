using Game.Environment.LMixTable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Environment.Aquarium
{
    public class Aquarium : MonoBehaviour
    {
        public string NameIngredient = "None";
        public string NameMaterial = "Classic";
        private GameObject DisplayCount;
        public bool NormalTemperature = false;
        public float NormalTimeCell = 3f;
        private float TimeCell = 666f;
        private float timerCell = 0f;
        public int CountCells = 0;
        public float TimeWaterSpend = -1f;
        public bool OnAquarium = false;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private Sprite NullFase;
        [SerializeField] private Sprite FirstFase;
        [SerializeField] private Sprite SecondFase;
        [SerializeField] private Sprite ThirdFase;
        [SerializeField] private Sprite NullFaseDirty;
        [SerializeField] private Sprite FirstFaseDirty;
        [SerializeField] private Sprite SecondFaseDirty;
        [SerializeField] private Sprite ThirdFaseDirty;

        [SerializeField] private List<GameObject> CellsList = new List<GameObject>();
        private int NumCell = 0;
        private SpriteRenderer ChoiceCellSprite;
        private ParticleSystem ParticleSystemm;

        public void ChangeCell(int ch)
        {
            GetAllCells();
            NumCell = (NumCell + ch + CellsList.Count) % CellsList.Count;
            //NameIngredient = CellsList[NumCell].GetComponent<IngradientItem>().typeIngradient;
            ChoiceCellSprite.sprite = CellsList[NumCell].GetComponent<SpriteRenderer>().sprite;
            //NormalTimeCell = CellsList[NumCell].GetComponent<IngradientItem>().TimeInAquarium;
            timerCell = 0f;
        }
        private void OnMouseDown()
        {
            GetAllCells();
        }

        private void GetAllCells()
        {
            if (CountCells != 0)
            {
                ParticleSystemm.Play();
                StartCoroutine(WaitParticleSystem(0.3f));
            }
            DisplayCount.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = CountCells.ToString();
            DisplayCount.GetComponent<Animator>().SetBool("On", true);
            StartCoroutine(waitDisplayCount());
            for (int i = 0; i < CountCells; i++)
            {
                //MixTable.Instance.AddIngridient(NameIngredient);
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
            ParticleSystemm = transform.Find("Particle System").GetComponent<ParticleSystem>();
            spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            ChoiceCellSprite = transform.Find("ChoiceCell").GetComponent<SpriteRenderer>();
            if (CellsList.Count > 0)
            {
                //NameIngredient = CellsList[NumCell].GetComponent<IngradientItem>().typeIngradient;
            }
            ChoiceCellSprite.sprite = CellsList[NumCell].GetComponent<SpriteRenderer>().sprite;
            //NormalTimeCell = CellsList[NumCell].GetComponent<IngradientItem>().TimeInAquarium;

            TimeCell = NormalTimeCell;
            DisplayCount = transform.Find("DisplayCount").gameObject;
        }
        private void Update()
        {
            if (TimeWaterSpend > 0f)
            {
                TimeWaterSpend -= Time.deltaTime;
            }
            if (CountCells == 0 && OnAquarium)
            {
                if (NameMaterial != "Classic" && TimeWaterSpend <= 0f)
                {
                    spriteRenderer.sprite = NullFaseDirty;
                }
                else
                {
                    spriteRenderer.sprite = NullFase;
                }
            }
            else if (CountCells < 4 && OnAquarium)
            {
                if (NameMaterial != "Classic" && TimeWaterSpend <= 0f)
                {
                    spriteRenderer.sprite = FirstFaseDirty;
                }
                else
                {
                    spriteRenderer.sprite = FirstFase;
                }
            }
            else if (CountCells < 9 && OnAquarium)
            {
                if (NameMaterial != "Classic" && TimeWaterSpend <= 0f)
                {
                    spriteRenderer.sprite = SecondFaseDirty;
                }
                else
                {
                    spriteRenderer.sprite = SecondFase;
                }
            }
            else if (CountCells < 15 && OnAquarium)
            {
                if (NameMaterial != "Classic" && TimeWaterSpend <= 0f)
                {
                    spriteRenderer.sprite = ThirdFaseDirty;
                }
                else
                {
                    spriteRenderer.sprite = ThirdFase;
                }
            }
            if (NormalTemperature) TimeCell = NormalTimeCell;
            else TimeCell = NormalTimeCell * 2;
            if (TimeWaterSpend > 0f || NameMaterial == "Classic") timerCell += Time.deltaTime;
            if (timerCell >= TimeCell)
            {
                if (CountCells < 15)
                    CountCells++;
                timerCell = 0;
            }
        }
        IEnumerator WaitParticleSystem(float f)
        {
            yield return new WaitForSeconds(f);
            ParticleSystemm.Stop();
        }
    }
}
