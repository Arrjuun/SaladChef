using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public int numVegetables;
    public int[] vegetables;
    public int CustomerIndex;

    public float TimeLeft;

    public float TotalTime;

    public Transform VegetableHolder;
    public GameObject SaladVegetablePrefab;
    public GameManager gameManager;

    public Sprite[] sprites;

    public bool isAngry;

    public Slider slider;

    public int AwardPoints;

    HashSet<PlayerMovement> WrongedPlayers = new HashSet<PlayerMovement>();

    public SpriteRenderer customerImage;

    void Awake()
    {
        numVegetables = Random.Range(1, 6);
        vegetables = new int[numVegetables];

        for(int i=0; i<numVegetables; i++)
        {
            vegetables[i] = Random.Range(0, 6);
            TimeLeft += Constants.TimeToChop[vegetables[i]];
            AwardPoints += Constants.ScoreForVegetables[vegetables[i]];

        }
        TimeLeft *= 24;
        TotalTime = TimeLeft;
        PopulatePlate();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
        player.customer = this;
        player.NearCustomer = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
        player.customer = null;
        player.NearCustomer = false;
    }

    public void ReceiveSalad(PlayerMovement player, List<int> processedSalad)
    {
        if(!areVegetablesSame(processedSalad))
        {
            GetAngry(player);
            //NoteWrongDelivery(player);
        }
        else
        {
            if(TimeLeft / TotalTime > 0.7)
            {
                AwardPlayer(player, true);
                gameManager.SpawnBooster(player.name);
            }
            else
            {
                AwardPlayer(player, false);
            }
            RemoveCustomer();
        }
    }

    void PopulatePlate()
    {
        for(int i=0;i<numVegetables;i++)
        {
            GameObject vegObject = Instantiate(SaladVegetablePrefab, VegetableHolder);
            vegObject.GetComponent<Image>().sprite = sprites[vegetables[i]];
        }
    }

    bool areVegetablesSame(List<int> processedVegetables)
    {
        if(vegetables.Length != processedVegetables.Count)
        {
            return false;
        }
        for(int i=0;i> vegetables.Length; i++)
        {
            if(vegetables[i] != processedVegetables[i])
            {
                return false;
            }
        }
        return true;
    }

    void GetAngry(PlayerMovement player)
    {
        WrongedPlayers.Add(player);
        customerImage.color = new Color(200, 0, 0);
        isAngry = true;
    }

    //void NoteWrongDelivery(PlayerMovement player)
    //{

    //    Debug.Log($"Got wrong delivery from {player.name}");
    //}

    void Update()
    {
        if(Constants.GameOver)
        {
            Destroy(this.gameObject);
        }
        if(TimeLeft <= 0)
        {
            Punish();
            RemoveCustomer();
        }
        else
        {
            if (!isAngry)
            {
                TimeLeft -= Time.deltaTime;
            }
            else
            {
                TimeLeft -= 2 * Time.deltaTime;
            }
            slider.value = TimeLeft/TotalTime;
        }
    }

    void Punish()
    {
        if(isAngry)
        {
            if(WrongedPlayers.Count > 0)
            {
                foreach(PlayerMovement player in WrongedPlayers)
                {
                    player.ChangeScore(-1 * AwardPoints);
                }
            }
        }
        else
        {
            gameManager.Player1.ChangeScore(AwardPoints / -2);
            gameManager.Player2.ChangeScore(AwardPoints / -2);
        }
    }

    void RemoveCustomer()
    {
        gameManager.CustomersPresent[CustomerIndex] = false;
        Destroy(this.gameObject);
    }

    void AwardPlayer(PlayerMovement player, bool fastDelivery)
    {
        if(fastDelivery)
        {
            player.ChangeScore(2 * AwardPoints);
        }
        else
        {
            player.ChangeScore(AwardPoints);
        }
    }

}
