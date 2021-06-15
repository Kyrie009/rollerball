using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 15;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
      
    }

    // This type of update good for physics. Independent and runs in the background despite frame drops.
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pick Up")) 
        {
            Destroy(collision.gameObject);
        }
     
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            Destroy(other.gameObject);
            
            count += 1;
            Debug.Log("Pick Up Count:" + count);

            //double == compares the numbers.
            if (count == 13)
            {
                Debug.Log("You Win");

            }



        }
    }
}
