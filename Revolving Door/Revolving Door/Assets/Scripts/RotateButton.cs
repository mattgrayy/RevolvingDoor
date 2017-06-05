using UnityEngine;
using System.Collections;

public class RotateButton : InteractableObject {

    [SerializeField] Transform rotateObject;
    [SerializeField] float rotationValue;
    float targetRot;

    bool rotating = false;

    private void Start()
    {
        if (rotateObject != null)
        {
            targetRot = rotateObject.eulerAngles.y;
        }
    }

    private void Update()
    {
        if (rotating && rotateObject != null)
        {
            Vector3 currentRotation = rotateObject.eulerAngles;

            currentRotation.y = Mathf.LerpAngle(currentRotation.y, targetRot, Time.deltaTime/2);
            rotateObject.eulerAngles = currentRotation;

            if (Mathf.Abs(rotateObject.eulerAngles.y - targetRot) < 1)
            {
                rotating = false;
            }
        }
    }

    public override void Interact()
    {
        if (!rotating || Mathf.Abs(rotateObject.eulerAngles.y - targetRot) < 5)
        {
            rotating = true;
            targetRot += rotationValue;
        }
    }
}
