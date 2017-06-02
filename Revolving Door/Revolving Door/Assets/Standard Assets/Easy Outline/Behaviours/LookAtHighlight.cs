using UnityEngine;
using System.Collections;

public class LookAtHighlight : MonoBehaviour {

    private GameObject lastLookedAt;
	
	void Update () {
        if(lastLookedAt != null)
        {
            lastLookedAt.layer = LayerMask.NameToLayer("Default");
        }

        RaycastHit rch;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rch);
        if (rch.collider != null)
        {
            rch.collider.gameObject.layer = LayerMask.NameToLayer("TransparentFX");
            lastLookedAt = rch.collider.gameObject;
        }
	}
}
