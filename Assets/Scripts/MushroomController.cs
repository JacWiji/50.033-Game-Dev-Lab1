using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private Rigidbody2D consummableMushroom;
    
    private Vector2 currentPosition;
    private Vector2 currentDirection; 
    private float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        consummableMushroom = GetComponent<Rigidbody2D>();
        consummableMushroom.AddForce(Vector2.up * 20, ForceMode2D.Impulse);

        currentDirection = new Vector2(1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
         Vector2 nextPosition = consummableMushroom.position + speed * currentDirection.normalized * Time.fixedDeltaTime;
        consummableMushroom.MovePosition(nextPosition);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            currentDirection *= -1; //change mushroom direction
        }

        if (col.gameObject.CompareTag("Player"))
        {
            currentDirection *= 0; //stop
        }

    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
