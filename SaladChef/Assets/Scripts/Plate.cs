using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    [SerializeField]
    string PlayerName;

    public int vegetableIndex = -1;

    public GameObject VegetablePrefab;

    public Transform VegetableHolder;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (PlayerName.Equals(col.name))
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            player.NearPlate = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (PlayerName.Equals(col.name))
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            player.NearPlate = false;
        }
    }

    public void AddVegToPlate(int vegIndex, Sprite sprite)
    {
        vegetableIndex = vegIndex;
        GameObject vegObject = Instantiate(VegetablePrefab, VegetableHolder);
        vegObject.GetComponent<Image>().sprite = sprite;
    }

    public int PickUpFromPlate()
    {
        var temp = vegetableIndex;

        foreach (Transform veg in VegetableHolder)
        {
            Destroy(veg.gameObject);
        }
        vegetableIndex = -1;
        return temp;
    }

    public bool isPlateEmpty()
    {
        if(vegetableIndex == -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
