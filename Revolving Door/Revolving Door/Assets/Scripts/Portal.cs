using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    [SerializeField] Transform tube;
    [SerializeField] Transform particleOrigin;

    bool teleporting = false;
    float particleTimer = 1;

    ParticleSystem[] emitters;
    int activeEmitterIndex = 0;

    Rigidbody playerRB;

    private void Update()
    {
        if (teleporting)
        {
            if (tube.position != transform.position)
            {
                tube.position = Vector3.Lerp(tube.position, transform.position, Time.deltaTime);
            }

            particleTimer += Time.deltaTime * 2;

            if (particleTimer > activeEmitterIndex)
            {
                if (activeEmitterIndex < emitters.Length)
                {
                    emitters[activeEmitterIndex].Play();
                    activeEmitterIndex++;
                }
            }

            playerRB.AddForce(Vector3.up * particleTimer);
        }
    }

    void beginTeleport()
    {
        teleporting = true;

        emitters = particleOrigin.GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!teleporting && other.tag == "Player")
        {
            other.GetComponent<Player>().setTeleporting(true);
            playerRB = other.GetComponent<Rigidbody>();
            beginTeleport();
        }
    }
}
