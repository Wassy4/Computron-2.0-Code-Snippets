using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintsController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI BalanceLabel;
    public TMPro.TextMeshProUGUI HintDescription;
    public static HintsController hintsController;
    public List<HintItem> HintList = new List<HintItem>();
    public GameObject CostGroup1, CostGroup2, CostGroup3;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < HintList.Count; i++)
        {
            HintList[i].HintText = FindObjectOfType<GameState>().SelectedPuzzle.Hints[i];
            HintList[i].HintCost = FindObjectOfType<GameState>().SelectedPuzzle.HintCosts[i];
        }

        BalanceLabel.text = "x" + FindObjectOfType<PlayerState>().GetBalance();
        HintDescription.text = "Click on a hint to purchase and reveal it.";
        if (CostGroup1 == null)
            CostGroup1 = GameObject.Find("CostGroup1");
        if (CostGroup2 == null)
            CostGroup2 = GameObject.Find("CostGroup2");
        if (CostGroup3 == null)
            CostGroup3 = GameObject.Find("CostGroup3");
        hintsController = this;
        
        //UpdateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        BalanceLabel.text = "x" + FindObjectOfType<PlayerState>().GetBalance();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < HintList.Count; i++)
        {
            if (HintList[i].HintType == "LEAST_HELPFUL")
            {
                if (FindObjectOfType<PlayerState>().GetHintBought(FindObjectOfType<GameState>().SelectedPuzzle.PuzzleName, (HintType)Enum.Parse(typeof(HintType), HintList[i].HintType, true)) == false)
                {
                    HintList[i].HintButton.GetComponentInChildren<TMP_Text>().text = "?";
                    HintList[i].HintButton.interactable = true;
                    CostGroup1.GetComponentInChildren<TMP_Text>().text = "x" + HintList[i].HintCost.ToString();
                    CostGroup1.SetActive(true);
                }
                else
                {
                    HintList[i].HintButton.GetComponentInChildren<TMP_Text>().text = "1";
                    HintList[i].HintButton.interactable = true;
                    CostGroup1.SetActive(false);
                }
            }
            else if (HintList[i].HintType == "HELPFUL")
            {
                if (FindObjectOfType<PlayerState>().GetHintBought(FindObjectOfType<GameState>().SelectedPuzzle.PuzzleName, (HintType)Enum.Parse(typeof(HintType), HintList[i].HintType, true)) == false)
                {
                    // Must have purchased least helpful hint first
                    if (FindObjectOfType<PlayerState>().GetHintBought(FindObjectOfType<GameState>().SelectedPuzzle.PuzzleName, HintType.LEAST_HELPFUL) == true)
                    {
                        HintList[i].HintButton.GetComponentInChildren<TMP_Text>().text = "?";
                        HintList[i].HintButton.interactable = true;
                        CostGroup2.GetComponentInChildren<TMP_Text>().text = "x" + HintList[i].HintCost.ToString();
                        CostGroup2.SetActive(true);
                    }
                    else
                    {
                        HintList[i].HintButton.GetComponentInChildren<TMP_Text>().text = "X";
                        HintList[i].HintButton.interactable = false;
                        CostGroup2.SetActive(false);
                    }
                }
                else
                {
                    HintList[i].HintButton.GetComponentInChildren<TMP_Text>().text = "2";
                    HintList[i].HintButton.interactable = true;
                    CostGroup2.SetActive(false);
                }
            }
            else if (HintList[i].HintType == "MOST_HELPFUL")
            {
                if (FindObjectOfType<PlayerState>().GetHintBought(FindObjectOfType<GameState>().SelectedPuzzle.PuzzleName, (HintType)Enum.Parse(typeof(HintType), HintList[i].HintType, true)) == false)
                {
                    // Must have purchased helpful hint first
                    if (FindObjectOfType<PlayerState>().GetHintBought(FindObjectOfType<GameState>().SelectedPuzzle.PuzzleName, HintType.HELPFUL) == true)
                    {
                        HintList[i].HintButton.GetComponentInChildren<TMP_Text>().text = "?";
                        HintList[i].HintButton.interactable = true;
                        CostGroup3.GetComponentInChildren<TMP_Text>().text = "x" + HintList[i].HintCost.ToString();
                        CostGroup3.SetActive(true);
                    }
                    else
                    {
                        HintList[i].HintButton.GetComponentInChildren<TMP_Text>().text = "X";
                        HintList[i].HintButton.interactable = false;
                        CostGroup3.SetActive(false);
                    }
                }
                else
                {
                    HintList[i].HintButton.GetComponentInChildren<TMP_Text>().text = "3";
                    HintList[i].HintButton.interactable = true;
                    CostGroup3.SetActive(false);
                }
            }
        }
    }
}
