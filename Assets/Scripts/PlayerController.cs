using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 15;
    private int count;

    public TMP_Text scoreDisplay;
    public TMP_Text timerDisplay;
    public TMP_Text textDisplay;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        ShowScore();
        rb = GetComponent<Rigidbody>();
      

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 0)
        {
            timerDisplay.text = "Ready in 3...";
            textDisplay.text = "Collect all 13 cubes to win! If you fall you restart!";
        }

        if (timer > 0 && timer < 10)
        {
            textDisplay.text = "";
        }

        if (timer >= 0)
        {
            timerDisplay.text = "Time: " + timer.ToString("F2");
        }

        if (this.transform.position.y < -10)
        {
            SceneManager.LoadScene(0);
        }

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
            ShowScore();

            //double == compares the numbers.
            if (count == 13)
            {
                textDisplay.text = "You win";

            }



        }
    }

    public void ShowScore()
    {
        scoreDisplay.text = "Score: " + count;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
