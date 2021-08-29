using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public GameObject player;

    private Vector3 offset;
    private Vector3 pivotOffset;

    public Transform pivot;

    [Range(0.01f, 1.0f)]
    public bool lookAtPivot = false;
    public bool rotateAroundPivot = true;
    public float rotationSpeed;
    public bool camMoveEnabled;

    // Start is called before the first frame update
    void Start()
    {
        //pivot offset to player
        pivotOffset = pivot.position - player.transform.position;
        //camera offset to pivot
        offset = transform.position - pivot.position;
        //camera setup
        CameraEnabled();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (camMoveEnabled == true)
        {
            //pivot follows the player
            pivot.transform.position = player.transform.position + pivotOffset;

            //third person camera
            if (rotateAroundPivot)
            {
                Quaternion camTurnAngle =
                    Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
                offset = camTurnAngle * offset;
            }
            //camera follows the pivot
            Vector3 newPos = pivot.transform.position + offset;
            transform.position = newPos;
            //camera looks at the pivot
            if (lookAtPivot || rotateAroundPivot)
            {
                transform.LookAt(pivot);
            }
        }
    }
    //Camera movement toggle
    public void CameraEnabled()
    {
        camMoveEnabled = true;
    }

    public void CameraDisable()
    {
        camMoveEnabled = false;
    }
}
