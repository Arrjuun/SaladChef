using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingBoard : MonoBehaviour
{
    [SerializeField]
    string PlayerName;

    public List<int> saladInventory = new List<int>();

    public GameObject SaladVegetablePrefab;

    public Transform SaladHolder;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (PlayerName.Equals(col.name))
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            player.NearChoppingBoard = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (PlayerName.Equals(col.name))
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            player.NearChoppingBoard = false;
        }
    }

    public void AddSaladVegetable(int vegetableIndex, Sprite sprite)
    {
        saladInventory.Add(vegetableIndex);
        GameObject vegObject = Instantiate(SaladVegetablePrefab, SaladHolder);
        vegObject.GetComponent<Image>().sprite = sprite;
    }

    public List<int> GetSalad()
    {
        var temp = saladInventory;
        foreach(Transform veg in SaladHolder)
        {
            Destroy(veg.gameObject);
        }
        saladInventory = new List<int>();
        return temp;
    }

    void Update()
    {
        if (Constants.GameOver)
        {
            saladInventory = new List<int>();
            foreach (Transform veggies in SaladHolder)
            {
                Destroy(veggies.gameObject);
            }
        }
    }
}
