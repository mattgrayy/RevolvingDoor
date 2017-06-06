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

            currentRotation.y = Mathf.MoveTowardsAngle(rotateObject.eulerAngles.y, targetRot, rotationValue * Time.deltaTime);
            rotateObject.eulerAngles = currentRotation;

            if (Mathf.Abs(currentRotation.y - targetRot) < 1)
            {
                rotating = false;
            }
        }
    }

    public override void Interact()
    {
        if (!rotating)
        {
            rotating = true;
            targetRot += rotationValue;
            if (targetRot > 360)
            {
                targetRot -= 360;
            }
        }
    }
}
