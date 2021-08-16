using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController :Singleton<PlayerController>
{
    [Header("UI Stuff")]
    public GameObject gameOverScreen;
    public GameObject winScreen;
    //physics
    public Rigidbody rb;
    public float speed = 15;
    private int count;
    //UI references
    public TMP_Text scoreDisplay;
    public TMP_Text timerDisplay;
    public TMP_Text textDisplay;
    public TMP_Text lifeText;
    //Counters
    public int lifePoints = 3;
    public float timer;
    public int currentLevel;
    private int pickupCount; //no. pickups in scene
    //respawn 
    GameObject resetPoint;
    bool resetting = false;
    Color originalColor;
    //keys
    bool havekey1;
    bool havekey2;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        count = 0;
        ShowScore();
        rb = GetComponent<Rigidbody>();
        //Find all pick ups in the current scene
        pickupCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;

        //Set draw order
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);
        CountDown();
        havekey1 = false;
        havekey2 = false;

        //lives and resets
        lifeText.text = "Life: " + lifePoints.ToString();
        resetPoint = GameObject.Find("ResetPoint");
        originalColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0)
        {
            timerDisplay.text = "Time: " + timer.ToString("F2");
        }    
    }

    // This type of update good for physics. Independent and runs in the background despite frame drops.
    void FixedUpdate()
    {
        //return no input during respawn
        if (resetting)
            return;
        //controller input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //rotates the player to the direction of the camera
        this.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        //translates the input vectors into coordinates
        movement = this.transform.TransformDirection(movement);
        rb.AddForce(movement * speed);
    }
    //when hit something tagged as respawn you lose and restart. triggered when player collides into something with a collider.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn")) 
        {
            lifePoints -= 1;
            lifeText.text = "Life: " + lifePoints.ToString();
            if (lifePoints == 0) //Gameover Condition
            {
                gameOverScreen.SetActive(true);
            }

            if (lifePoints > 0) //If still have lives respawn
            {
                StartCoroutine(ResetPlayer());
            } 
        }
        //When touch BouncePad
        if (collision.gameObject.CompareTag("BouncePad"))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
        }
        //Open Lock if have key
        if (collision.gameObject.CompareTag("Lock1"))
        {
            if (havekey1)
            {
                _LM.Lock1();
                havekey1 = false;
            }   
        }
        if (collision.gameObject.CompareTag("Lock2"))
        {
            if (havekey2)
            {
                _LM.Lock2();
                havekey2 = false;
            }         
        }
    }

    public IEnumerator ResetPlayer()
    {
        resetting = true;
        GetComponent<Renderer>().material.color = Color.black;
        rb.velocity = Vector3.zero;
        Vector3 startPos = transform.position;
        float resetSpeed = 2f;
        var i = 0.0f;
        var rate = 1.0f / resetSpeed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, resetPoint.transform.position, i);
            yield return null;
        }
        GetComponent<Renderer>().material.color = originalColor;
        resetting = false;
    }
    // triggered when player passes through an object with an 'isTrigger' check.
    private void OnTriggerEnter(Collider other)
    {
        //Score count and victory condition
        if (other.gameObject.CompareTag("Pick Up")) 
        {
            Destroy(other.gameObject);
            
            count += 1;
            ShowScore();
            if (count >= pickupCount)
            {
                WinGame();
            }
        }
        //Key
        if (other.gameObject.CompareTag("Key1"))
        {
            Destroy(other.gameObject);
            havekey1 = true;
        }

        if (other.gameObject.CompareTag("Key2"))
        {
            Destroy(other.gameObject);
            havekey2 = true;
        }
    }

  
    public void ShowScore()
    {
        scoreDisplay.text = "Score: " + count.ToString();
    }

    public void CountDown()
    {
        textDisplay.text = "Collect all 13 cubes to win! If you fall you lose";
        StartCoroutine(StartCountD());
    }

    IEnumerator StartCountD()
    {
        timerDisplay.text = "Ready in 3...";
        yield return new WaitForSeconds(1f);
        timerDisplay.text = "Ready in 2...";
        yield return new WaitForSeconds(1f);
        timerDisplay.text = "Ready in 1...";
        yield return new WaitForSeconds(1f);
        textDisplay.text = "";
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
    public void WinGame()
    {
        textDisplay.text = "Level cleared";
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 1;
        winScreen.SetActive(true);

    } 
}
