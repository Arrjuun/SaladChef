using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool[] CustomersPresent = new bool[5];
    float[] XTransforms = { -1f, -0.5f, 0f, 0.5f, 1f };

    public Transform CustomerHolder;
    public GameObject CustomerPrefab;

    public PlayerMovement Player1;
    public PlayerMovement Player2;

    bool isGameOver;

    void Update()
    {
        if(!isGameOver)
        {
            PopulateCustomers();
            isTimeOver();
        }
    }

    void PopulateCustomers()
    {
        for (int i = 0; i < CustomersPresent.Length; i++)
        {
            if (!CustomersPresent[i])
            {
                GameObject customerObject = Instantiate(CustomerPrefab, CustomerHolder);
                customerObject.name = $"Customer{i}";
                customerObject.transform.localPosition = new Vector3(XTransforms[i], customerObject.transform.localPosition.y);
                Customer customer = customerObject.GetComponent<Customer>();
                customer.CustomerIndex = i;
                customer.gameManager = this;
                CustomersPresent[i] = true;
            }
        }
    }

    void isTimeOver()
    {
        if(Player1.TimeLeft <= 0 && Player2.TimeLeft <= 0)
        {
            isGameOver = true;
        }
    }

    public void SpawnBooster(string playerName)
    {
        Debug.Log($"Spawning booster for {playerName}");
    }
}
