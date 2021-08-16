using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : Singleton<LockManager>
{
    public GameObject lock1;
    public GameObject lock2;
    bool lock1Open;
    bool lock2Open;
    public Animator[] stairWayToHeaven;
    public int animCount;
    
    // Start is called before the first frame update
    void Start()
    {
        lock1Open = false;
        lock2Open = false;
    }

    public void Lock1()
    {
        lock1Open = true;
        lock1.GetComponent<Renderer>().material.color = Color.magenta;
        DualLock();
    }
    public void Lock2()
    {
        lock2Open = true;
        lock2.GetComponent<Renderer>().material.color = Color.green;
        DualLock();
    }

    public void DualLock()
    {
        if (lock1Open & lock2Open)
        {  
            if (animCount < stairWayToHeaven.Length)
            {
                StartCoroutine(OpenDualLock());            
            }
        }
    }

    IEnumerator OpenDualLock()
    {
        stairWayToHeaven[animCount].SetTrigger("OpenStairs");
        yield return new WaitForSeconds(0.5f);
        animCount++;
        DualLock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Lock1();
            Lock2();
        }
    }
}
