using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles User Input and movement 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;

    /// <summary>
    /// Key Mapping Created in Input
    /// </summary>
    [SerializeField]
    string horizontal;

    /// <summary>
    /// Key Mapping Created in Input
    /// </summary>
    [SerializeField]
    string vertical;

    /// <summary>
    /// Movement Speed
    /// </summary>
    [SerializeField]
    float speed = 5f;

    void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {

        float moveHorizontal = Input.GetAxis (horizontal);
        float moveVertical = Input.GetAxis (vertical);

        transform.position += new Vector3(moveHorizontal, moveVertical, 0) * speed * Time.deltaTime;
    }


}
