using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public static CameraMovement m_instance;

    public Transform player;

    Vector3 targetPos;
    float yOffset;

    private void Start()
    {
        if (m_instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }

        targetPos = transform.position;
        yOffset = transform.position.y - player.position.y;
    }

    // Update is called once per frame
    void Update ()
    {
        if (player != null)
        {
            Plane[] planesInner;

            GetComponent<Camera>().fieldOfView -= 20f;
            planesInner = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>());
            GetComponent<Camera>().fieldOfView += 20f;

            if (!GeometryUtility.TestPlanesAABB(planesInner, player.GetComponent<BoxCollider>().bounds))
            {
                targetPos = new Vector3(player.position.x, player.position.y + yOffset, transform.position.z);
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }
}
