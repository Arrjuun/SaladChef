using UnityEngine;

public class VegHandler : MonoBehaviour
{
    public int veg;
    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
        player.NearVegetableIndex = veg;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
        player.NearVegetableIndex = -1;
    }
}
