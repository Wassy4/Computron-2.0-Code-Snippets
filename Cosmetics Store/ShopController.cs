using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI BalanceLabel;
    public static ShopController shopController;
    public GameObject HatContainerPrefab;
    public GameObject PaintContainerPrefab;
    public Transform Grid;
    public Transform Computron;
    public Transform Hat;

    public List<ShopItem> Hats = new List<ShopItem>();
    public List<ShopItem> Paints = new List<ShopItem>();
    private List<GameObject> HatContainerList = new List<GameObject>();
    private List<GameObject> PaintContainerList = new List<GameObject>();
    public List<GameObject> HatListButtons = new List<GameObject>();
    public List<GameObject> PaintListButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate player currency balance
        BalanceLabel.text = "x" + FindObjectOfType<PlayerState>().GetBalance();

        shopController = this;

        // Prevent duplicate items if this isn't the player's first time in the store
        if (FindObjectOfType<PlayerState>().ActiveSave.ShopHatsHashSet.Any())
        {
            Hats.Clear();
            Hats = FindObjectOfType<PlayerState>().ActiveSave.ShopHatsHashSet.ToList();
        }

        if (FindObjectOfType<PlayerState>().ActiveSave.ShopPaintsHashSet.Any())
        {
            Paints.Clear();
            Paints = FindObjectOfType<PlayerState>().ActiveSave.ShopPaintsHashSet.ToList();
        }

        // Start with the list of hats shown
        PopulateHatList();
        UpdateHatButtons();

        // Hide the paint list until button clicked
        PopulatePaintList();
        UpdatePaintButtons();
        HidePaintList();
    }

    // Update is called once per frame
    void Update()
    {
        BalanceLabel.text = "x" + FindObjectOfType<PlayerState>().GetBalance();
    }

    public void PopulateHatList()
    {
        for (int i = 0; i < Hats.Count; i++)
        {
            GameObject container = Instantiate(HatContainerPrefab, Grid, false);
            HatContainer containerScript = container.GetComponent<HatContainer>();

            containerScript.ItemName.text = Hats[i].ItemName;
            containerScript.ItemCost.text = "x" + Hats[i].ItemCost.ToString();
            containerScript.ItemID = Hats[i].ItemID;
            containerScript.BuyButton.GetComponent<NewHatBuyButton>().ItemID = Hats[i].ItemID;

            // Default doesn't have a sprite
            if (i == 0)
            {
                ;
            }
            else
            {
                containerScript.ItemSprite.GetComponent<Image>().sprite = SpriteRepository.spriteRepository.HatSprites[i];
                Color temp = containerScript.ItemSprite.GetComponent<Image>().color;
                temp.a = 1f;
                containerScript.ItemSprite.GetComponent<Image>().color = temp;
            }


            HatContainerList.Add(container);
            HatListButtons.Add(containerScript.BuyButton);

            if (!FindObjectOfType<PlayerState>().ActiveSave.ShopHatsHashSet.Contains(Hats[i]))
            {
                FindObjectOfType<PlayerState>().ActiveSave.ShopHatsHashSet.Add(Hats[i]);
            }
        }
    }

    public void PopulatePaintList()
    {
        for (int i = 0; i < Paints.Count; i++)
        {
            GameObject container = Instantiate(PaintContainerPrefab, Grid, false);
            PaintContainer containerScript = container.GetComponent<PaintContainer>();

            containerScript.ItemName.text = Paints[i].ItemName;
            containerScript.ItemCost.text = "x" + Paints[i].ItemCost.ToString();
            containerScript.ItemID = Paints[i].ItemID;
            containerScript.BuyButton.GetComponent<PaintBuyButton>().ItemID = Paints[i].ItemID;
            
            Color temp = containerScript.ItemSprite.GetComponent<Image>().color;
            temp.a = 1f;
            containerScript.ItemSprite.GetComponent<Image>().color = temp;
            containerScript.ItemSprite.GetComponent<Image>().sprite = SpriteRepository.spriteRepository.PaintSprites[i];

            PaintContainerList.Add(container);
            PaintListButtons.Add(containerScript.BuyButton);

            if (!FindObjectOfType<PlayerState>().ActiveSave.ShopPaintsHashSet.Contains(Paints[i]))
            {
                FindObjectOfType<PlayerState>().ActiveSave.ShopPaintsHashSet.Add(Paints[i]);
            }
        }
    }

    public void UpdateHatButtons()
    {
        int currentItemID = FindObjectOfType<PlayerState>().ActiveSave.PlayerHatID;

        for (int i = 0; i < HatListButtons.Count; i++)
        {
            NewHatBuyButton BuyButtonScript = HatListButtons[i].GetComponent<NewHatBuyButton>();

            for (int j = 0; j < Hats.Count; j++)
            {
                if (Hats[j].ItemID == BuyButtonScript.ItemID && Hats[j].Purchased && Hats[j].ItemID != FindObjectOfType<PlayerState>().ActiveSave.PlayerHatID)
                {
                    BuyButtonScript.ButtonText.text = "Equip";
                }
                else if (Hats[j].ItemID == BuyButtonScript.ItemID && Hats[j].Purchased && Hats[j].ItemID == FindObjectOfType<PlayerState>().ActiveSave.PlayerHatID)
                {
                    BuyButtonScript.ButtonText.text = "Equipped";
                    Hat.GetComponent<SpriteRenderer>().sprite = SpriteRepository.spriteRepository.HatSprites[j];
                }
            }
        }
    }

    public void UpdatePaintButtons()
    {
        int currentItemID = FindObjectOfType<PlayerState>().ActiveSave.PlayerPaintID;

        for (int i = 0; i < PaintListButtons.Count; i++)
        {
            PaintBuyButton BuyButtonScript = PaintListButtons[i].GetComponent<PaintBuyButton>();

            for (int j = 0; j < Paints.Count; j++)
            {
                if (Paints[j].ItemID == BuyButtonScript.ItemID && Paints[j].Purchased && Paints[j].ItemID != FindObjectOfType<PlayerState>().ActiveSave.PlayerPaintID)
                {
                    BuyButtonScript.ButtonText.text = "Equip";
                }
                else if (Paints[j].ItemID == BuyButtonScript.ItemID && Paints[j].Purchased && Paints[j].ItemID == FindObjectOfType<PlayerState>().ActiveSave.PlayerPaintID)
                {
                    BuyButtonScript.ButtonText.text = "Equipped";
                    Computron.GetComponent<SpriteRenderer>().sprite = SpriteRepository.spriteRepository.PaintSprites[j];
                }
            }
        }
    }

    public void HideHatList()
    {
        for (int i = 0; i < HatContainerList.Count; i++)
        {
            HatContainerList[i].SetActive(false);
        }
    }

    public void HidePaintList()
    {
        for (int i = 0; i < PaintContainerList.Count; i++)
        {
            PaintContainerList[i].SetActive(false);
        }
    }

    public void ShowHatList()
    {
        for (int i = 0; i < HatContainerList.Count; i++)
        {
            HatContainerList[i].SetActive(true);
        }
    }

    public void ShowPaintList()
    {
        for (int i = 0; i < PaintContainerList.Count; i++)
        {
            PaintContainerList[i].SetActive(true);
        }
    }
}

