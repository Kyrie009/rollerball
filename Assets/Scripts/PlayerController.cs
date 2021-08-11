using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("UI Stuff")]
    public GameObject gameOverScreen;
    public GameObject winScreen;

    public Rigidbody rb;
    public float speed = 7;
    private int count;

    public TMP_Text scoreDisplay;
    public TMP_Text timerDisplay;
    public TMP_Text textDisplay;

    public float timer;
    public int currentLevel;

    GameObject resetPoint;
    bool resetting = false;
    Color originalColor;

    public TMP_Text lifeText;
    public int lifePoints = 3;

    private int pickupCount; //no. pickups in scene

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
        if (resetting)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
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

            if (lifePoints > 0)
            {
                StartCoroutine(ResetPlayer());
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
