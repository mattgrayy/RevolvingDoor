using UnityEngine;
using System.Collections;

public class ExplodingChunk : MonoBehaviour {

    float freezeTimer = 20;

	// Use this for initialization
	void Start ()
    {
        float upForce = Random.Range(100, 200);
        float rightForce = Input.GetAxis("Horizontal");
        float forwardForce = Input.GetAxis("Vertical");

        if (rightForce != 0)
        {
            rightForce *= Random.Range(0, 200);
        }
        else
        {
            rightForce = Random.Range(-200, 200);
        }

        if (forwardForce != 0)
        {
            forwardForce *= Random.Range(0, 200);
        }
        else
        {
            forwardForce = Random.Range(-200, 200);
        }

        GetComponent<Rigidbody>().AddForce((Vector3.up * upForce) + (Vector3.right * rightForce) + (Vector3.forward * forwardForce));
	}

    private void Update()
    {
        if (GetComponent<Rigidbody>() && GetComponent<Rigidbody>().velocity.magnitude < 0.3f)
        {
            freezeTimer -= Time.deltaTime;

            Color curCol = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(curCol, Color.white, Time.deltaTime / (20 - (20 - freezeTimer)));
            GetComponent<Rigidbody>().mass = Mathf.Lerp(GetComponent<Rigidbody>().mass, 1, Time.deltaTime / (20 - (20 - freezeTimer)));

            if (freezeTimer <= 0)
            {
                freeze();
            }
        }
    }

    void freeze()
    {
        if (GetComponent<Rigidbody>())
        {
            Destroy(GetComponent<Rigidbody>());
        }

        GetComponent<MeshRenderer>().material.color = Color.white;
        Destroy(this);
    }
}
