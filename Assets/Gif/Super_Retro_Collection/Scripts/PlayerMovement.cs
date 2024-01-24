// -----------------------------------------------------------------------------------------
// using classes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// -----------------------------------------------------------------------------------------
// player movement class
public class PlayerMovement : MonoBehaviour
{
    // static public members
    public static PlayerMovement instance;

    // -----------------------------------------------------------------------------------------
    // public members
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    // -----------------------------------------------------------------------------------------
    // private members
    private Vector2 movement;

    // -----------------------------------------------------------------------------------------
    // awake method to initialisation
    void Awake()
    {
        instance = this;
    }

    // -----------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        // update members
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    // -----------------------------------------------------------------------------------------
    // fixed update methode
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
