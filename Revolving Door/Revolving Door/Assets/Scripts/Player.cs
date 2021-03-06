﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform cubePrefab;
    [SerializeField] float baseBobTimer = 0;
    float bobTimer = 0;

    Rigidbody rb;
    Collider col;

    bool dead = false;

    bool exploding = false;
    float explodingTimer = 0;
    [SerializeField] Transform explosionPrefab;

    bool teleporting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        resetVariables();

        if (CameraMovement.m_instance != null)
            CameraMovement.m_instance.setPlayer(transform);
    }

    public void setSpawn(Transform _spawnPoint)
    {
        spawnPoint = _spawnPoint;
    }

    public Vector2 inputPos;
    public Vector2 dragPosOrigin;
    public Vector2 dragPos;
    public bool dragging = false;

    void takeInput()
    {
        inputPos = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(Input.GetMouseButtonDown(0))
        {
            dragPosOrigin = Input.mousePosition;
            dragPos = Input.mousePosition;
            dragging = true;
        }

        if (Input.GetMouseButton(0))
            dragPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
            dragging = false;
    }

        // Update is called once per frame
        void FixedUpdate ()
    {
        if (!dead)
        {
            if (!exploding)
            {
                takeInput();

                if (inputPos != Vector2.zero)
                    checkBob();
                else if (dragging && dragPos != dragPosOrigin)
                    checkBob(true);
            }

            if (!teleporting)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    exploding = true;
                    explodingTimer = 1;
                }
                if (Input.GetButtonUp("Fire1"))
                    exploding = false;
            }

            if (Input.GetButtonDown("Fire2"))
                makeSpawn();
        }

        if (exploding)
            checkExplode();
    }

    void checkBob(bool _drag = false)
    {
        if (bobTimer > 0)
            bobTimer -= Time.deltaTime;

        if (bobTimer <= 0)
        {
            bobTimer = baseBobTimer;

            if(_drag)
                rb.AddForce(((Vector3.up * 1.2f) + (Vector3.right * Mathf.Clamp((dragPos.x - dragPosOrigin.x), -1,1)) + (Vector3.forward * Mathf.Clamp((dragPos.y - dragPosOrigin.y), -1,1))) * 500);
            else
                rb.AddForce(((Vector3.up * 1.2f) + (Vector3.right * inputPos.x) + (Vector3.forward * inputPos.y)) * 500);
        }
    }

    void checkExplode()
    {
        if (explodingTimer > 0)
            explodingTimer -= Time.deltaTime;

        if (explodingTimer <= 0)
        {
            // explode and make new at spawn
            Destroy(rb);
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Transform newCube = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
            newCube.GetComponent<Player>().setSpawn(spawnPoint);
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
        rb.velocity = Vector3.zero;
        transform.position = _Position;
    }

    public void setTeleporting(bool _newValue)
    {
        if (_newValue)
            resetVariables();

        teleporting = _newValue;
    }

    void resetVariables()
    {
        bobTimer = baseBobTimer;
        dead = false;
        exploding = false;
        explodingTimer = 0;
        teleporting = false;
        rb.velocity = Vector3.zero;
        col.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Lethal" && !dead)
        {
            dead = true;
            exploding = true;
            explodingTimer = 0;
        }

        if (collision.transform.tag == "Interactable")
            collision.transform.GetComponent<InteractableObject>().Interact();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Lethal" && !dead)
        {
            dead = true;
            exploding = true;
            explodingTimer = 0;
        }

        if (collision.transform.tag == "Interactable")
            collision.transform.GetComponent<InteractableObject>().Interact();
    }
}
