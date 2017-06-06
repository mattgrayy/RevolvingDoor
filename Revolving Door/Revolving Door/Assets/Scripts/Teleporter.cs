using UnityEngine;
using System.Collections;

public class Teleporter : InteractableObject {

    public enum Level
    {
        LEVEL_LOBBY = 0,
        LEVEL_1 = 1
    }

    [SerializeField]Level targetLevel;

    public override void Interact()
    {
    }
}
