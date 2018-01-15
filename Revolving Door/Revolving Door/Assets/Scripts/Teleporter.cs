using UnityEngine;
using System.Collections;

public class Teleporter : InteractableObject {

    [SerializeField]Level targetLevel;

    public override void Interact()
    {
        if (LevelManager.m_instance != null)
            LevelManager.m_instance.loadLevel(targetLevel);
    }
}
