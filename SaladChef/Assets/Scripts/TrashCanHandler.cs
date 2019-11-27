using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanHandler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
        player.NearTrashCan = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
        player.NearTrashCan = false;
    }
}
