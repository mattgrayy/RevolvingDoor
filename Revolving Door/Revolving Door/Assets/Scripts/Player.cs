using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform cubePrefab;
    [SerializeField] float baseBobTimer = 0;
    float bobTimer = 0;

    Rigidbody rb;

    bool dead = false;

    bool exploding = false;
    float explodingTimer = 0;
    [SerializeField] Transform explosionPrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (CameraMovement.m_instance != null)
        {
            CameraMovement.m_instance.player = transform;
        }
    }

    public void setSpawn(Transform _spawnPoint)
    {
        spawnPoint = _spawnPoint;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!dead)
        {
            if (!exploding)
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    checkBob();
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                exploding = true;
                explodingTimer = 1;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                exploding = false;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                makeSpawn();
            }
        }
        if (exploding)
        {
            checkExplode();
        }
    }

    void checkBob()
    {
        if (bobTimer > 0)
        {
            bobTimer -= Time.deltaTime;
        }
        if (bobTimer <= 0)
        {
            bobTimer = baseBobTimer;
            rb.AddForce(((Vector3.up * 1.2f) + (Vector3.right * Input.GetAxis("Horizontal")) + (Vector3.forward * Input.GetAxis("Vertical"))) * 500);
        }
    }

    void checkExplode()
    {
        if (explodingTimer > 0)
        {
            explodingTimer -= Time.deltaTime;
        }
        if (explodingTimer <= 0)
        {
            // explode
            Destroy(rb);
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Transform newCube = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
            newCube.GetComponent<Player>().setSpawn(spawnPoint);
            newCube.GetComponent<BoxCollider>().enabled = true;
            newCube.GetComponent<Player>().exploding = false;
            Destroy(gameObject);
        }

     // shake based on explodingtimer
        // random position
        transform.position = new Vector3(transform.position.x + Random.Range(-0.02f,0.02f), transform.position.y + Random.Range(0, 0.02f), transform.position.z + Random.Range(-0.02f, 0.02f));
        // random rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + Random.Range(-3f, 3f), transform.eulerAngles.y + Random.Range(-3f, 3f), transform.eulerAngles.z + Random.Range(-3f, 3f));
    }

    void makeSpawn()
    {
        spawnPoint.position = transform.position;
    }

    public void setPosition(Vector3 _Position)
    {
        transform.position = _Position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Lethal" && !dead)
        {
            dead = true;
            exploding = true;
        }
    }
}
