using UnityEngine;
using System.Collections;

public class SpineFloor : MonoBehaviour
{

    enum SpineFloorMovement
    {
        SFM_UP,
        SFM_DOWN
    }

    float MAX_Y_HEIGHT = 1.5f;
    float MIN_Y_HEIGHT = -2;
    SpineFloorMovement movementState = SpineFloorMovement.SFM_UP;
    Vector3 direction;
    float speed = 5;
    // Use this for initialization
    void Start()
    {
        int randomState = Random.Range(0, 2);
        switch(randomState)
        {
            case 0:
                movementState = SpineFloorMovement.SFM_UP;
                break;
            case 1:
                movementState = SpineFloorMovement.SFM_DOWN;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(transform.position.y >= MAX_Y_HEIGHT)
        {
            movementState = SpineFloorMovement.SFM_DOWN;
        }
        else if(transform.position.y <= MIN_Y_HEIGHT)
        {
            movementState = SpineFloorMovement.SFM_UP;
        }

        switch (movementState)
        {
            case SpineFloorMovement.SFM_UP:
                {
                    direction = Vector3.up;
                }
                break;
            case SpineFloorMovement.SFM_DOWN:
                {
                    direction = Vector3.down;
                }
                break;
        }

        Vector3 velocity = direction* speed * Time.deltaTime;
        transform.Translate(velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerControl>();
            collision.gameObject.SendMessage("OnCollsionEnterDeathObject");
        }
    }
}
