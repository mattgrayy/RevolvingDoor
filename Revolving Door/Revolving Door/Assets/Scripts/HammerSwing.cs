using UnityEngine;
using System.Collections;

public class HammerSwing : MonoBehaviour {

    float baseTargetRot = 40;
    float targetRot = 0;
    bool positive = true;

    float swingTimer = 0.1f;

    private void Start()
    {
        targetRot = baseTargetRot;
    }

    void Update ()
    {
        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, targetRot, Time.deltaTime * swingTimer), transform.eulerAngles.y, transform.eulerAngles.z);
        swingTimer += Time.deltaTime;

        // ghets the absolute value of the 'angle distance' between current x and target x
        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.x, targetRot)) < 2)
        {
            swingTimer = 0.1f;
            if (positive)
            {
                positive = false;
                targetRot = -baseTargetRot;
            }
            else
            {
                positive = true;
                targetRot = baseTargetRot;
            }
        }
	}
}
