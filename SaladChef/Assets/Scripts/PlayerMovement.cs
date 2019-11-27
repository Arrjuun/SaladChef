using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles User Input and movement 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Key Mapping for Horizontal axis Created in Input
    /// </summary>
    public string Horizontal;

    /// <summary>
    /// Key Mapping for Vertical axis Created in Input
    /// </summary>
    public string Vertical;

    /// <summary>
    /// Key Mapping for Pickup Created in Input
    /// </summary>
    public string PickUpKey;

    /// <summary>
    /// Key Mapping for Pickup Created in Input
    /// </summary>
    public string DropKey;

    /// <summary>
    /// Movement Speed
    /// </summary>
    public float Speed = 5f;

    /// <summary>
    /// Active Vegetable
    /// </summary>
    public int NearVegetableIndex = -1;

    /// <summary>
    /// List to hold Vegetables picked up from the table
    /// </summary>
    public List<int> vegetableInventory = new List<int>();

    /// <summary>
    /// Player Score
    /// </summary>
    float score = 0;

    /// <summary>
    /// List to hold the salad
    /// </summary>
    public List<int> ProcessedSaladInventory = new List<int>();

    /// <summary>
    /// Indicator to check if the user is near the trash can
    /// </summary>
    public bool NearTrashCan;


    /// <summary>
    /// Vegetables Panel transform
    /// </summary>
    public Transform VegetablesInventoryHolder;

    /// <summary>
    /// Vegetables Panel transform
    /// </summary>
    public Transform SaladInventoryHolder;

    /// <summary>
    /// Inventory Vegetable prefab
    /// </summary>
    public GameObject VegetableInventoryPrefab;

    /// <summary>
    /// Inventory Salad prefab
    /// </summary>
    public GameObject SaladInventoryPrefab;

    public ChoppingBoard choppingBoard;

    public Plate plate;

    public Sprite[] VegetableSprites;

    public bool NearChoppingBoard;

    public bool NearPlate;

    public bool NearCustomer;

    public bool isChopping;

    public Customer customer;

    public Text TimeLeftText;

    public Text ScoreText;

    public float TimeLeft;

    void Update()
    {
        if(TimeLeft >= 0)
        {
            if (!isChopping)
            {
                if (Input.GetButtonDown(PickUpKey))
                {
                    if (NearVegetableIndex >= 0 && NearVegetableIndex <= 5)
                    {
                        AddVegetable(NearVegetableIndex);
                    }
                    if (NearChoppingBoard)
                    {
                        PickSalad();
                    }
                    if (NearPlate)
                    {
                        TakeFromPlate();
                    }
                }
                if (Input.GetButtonDown(DropKey))
                {
                    if (NearTrashCan)
                    {
                        FlushVegetableInventory();
                    }
                    if (NearChoppingBoard && vegetableInventory.Count > 0)
                    {
                        StartCoroutine(ChopVegetables(Constants.TimeToChop[vegetableInventory[0]]));
                    }
                    if (NearPlate && vegetableInventory.Count > 0)
                    {
                        AddToPlate();
                    }
                    if (NearCustomer && ProcessedSaladInventory.Count > 0)
                    {
                        customer.ReceiveSalad(this, ProcessedSaladInventory);
                        FlushVegetableInventory();
                    }
                }
                Move();
                TimeLeft -= Time.deltaTime;
                TimeLeftText.text = Mathf.Ceil(TimeLeft).ToString() + " s";
            }
        
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis(Horizontal);
        float moveVertical = Input.GetAxis(Vertical);
        transform.position += new Vector3(moveHorizontal, moveVertical, 0) * Speed * Time.deltaTime;
    }

    void AddVegetable(int vegIndex)
    {

        if (vegetableInventory.Count >= 2)
        {
            Debug.Log("Inventory Full");
        }
        else
        {
            vegetableInventory.Add(vegIndex);
            GameObject vegObject = Instantiate(VegetableInventoryPrefab, VegetablesInventoryHolder);
            vegObject.GetComponent<Image>().sprite = VegetableSprites[vegIndex];
        }

    }

    void FlushVegetableInventory()
    {
        ProcessedSaladInventory = new List<int>();
        foreach(Transform veg in SaladInventoryHolder)
        {
            Destroy(veg.gameObject);
        }
    }

    IEnumerator ChopVegetables(int waitTime)
    {
        isChopping = true;
        yield return new WaitForSeconds(waitTime);
        TimeLeft -= waitTime;
        choppingBoard.AddSaladVegetable(vegetableInventory[0], VegetableSprites[vegetableInventory[0]]);
        vegetableInventory.RemoveAt(0);
        Destroy(VegetablesInventoryHolder.GetChild(0).gameObject);
        isChopping = false;
    }

    void PickSalad()
    {
        if(choppingBoard.saladInventory.Count > 0 && ProcessedSaladInventory.Count == 0)
        {

            ProcessedSaladInventory = choppingBoard.GetSalad();
            PopulateSaladInventory();
        }

    }

    void PopulateSaladInventory()
    {
        foreach(int vegIndex in ProcessedSaladInventory)
        {
            GameObject vegObject = Instantiate(SaladInventoryPrefab, SaladInventoryHolder);
            vegObject.GetComponent<Image>().sprite = VegetableSprites[vegIndex];
        }
    }

    void AddToPlate()
    {
        if(plate.isPlateEmpty())
        {
            Destroy(VegetablesInventoryHolder.GetChild(0).gameObject);
            plate.AddVegToPlate(vegetableInventory[0], VegetableSprites[vegetableInventory[0]]);
            vegetableInventory.RemoveAt(0);
        }
    }

    void TakeFromPlate()
    {
        if (!plate.isPlateEmpty())
        {
            AddVegetable(plate.PickUpFromPlate());
        }
    }

    public void ChangeScore(float additionalScore)
    {
        score += additionalScore;
        ScoreText.text = score.ToString();
    }
}
