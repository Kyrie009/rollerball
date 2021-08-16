using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger2 : MonoBehaviour
{
    //2part door trigger
    public Animator DoorAnim;
    public Animator BridgeAnim;
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            StartCoroutine(OpenThatDoor());

        }
    }

    IEnumerator OpenThatDoor()
    {
        DoorAnim.SetTrigger("OpenDoor");
        yield return new WaitForSeconds(1f);
        BridgeAnim.SetTrigger("OpenBridge");
    }
}
