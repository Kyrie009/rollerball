using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HelloWorld : MonoBehaviour
{
    int maxHealth = 100;
    public int playerHealth;
    public string playerName = "Kyrie";



    // Start is called before the first frame update
    void Start()
    {   
        playerHealth = maxHealth;

        Debug.Log("Hello World!");
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) 
        {

            Debug.Log("Hello Again!");

        }
          

        if (Input.GetKeyDown(KeyCode.Return))
        {

            //Take 10 of the players health
            //if the players is less than or equal to zero
            //print that the player has died else.
            //print the players name and new health
            playerHealth -= 10;
            if (playerHealth <= 0) 
            {

                Debug.Log(playerName + " has died");
            
            }

            else 
            {

                Debug.Log(playerName + " has shot himself for 10 Damage");
            
            }
            
            
        }



    }
}
