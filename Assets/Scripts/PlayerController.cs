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

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        count = 0;
        ShowScore();
        rb = GetComponent<Rigidbody>();

        //Set draw order
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);

        //lives and resets
        lifeText.text = "Life: " + lifePoints.ToString();
        resetPoint = GameObject.Find("ResetPoint");
        originalColor = GetComponent<Renderer>().material.color;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 0)
        {
            timerDisplay.text = "Ready in 3...";
            textDisplay.text = "Collect all 13 cubes to win! If you fall you lose";
        }

        if (timer > 0 && timer < 10)
        {
            textDisplay.text = "";
        }

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
    //when hit something tagged as respawn you lose and restart.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn")) 
        {
            lifePoints -= 1;
            lifeText.text = "Life: " + lifePoints.ToString();
            if (lifePoints == 0)
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
                WinGame();
            }
        }
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
    public void WinGame()
    {
        textDisplay.text = "You win";
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1f);
        winScreen.SetActive(true);

    }

    public void ShowScore()
    {
        scoreDisplay.text = "Score: " + count;
    }

   
}
