using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RollDirection
{
    NONE,
    FORWARD,
    BACK,
    LEFT,
    RIGHT
}

public class CubeMovement : MonoBehaviour {

    [SerializeField] float m_rollSpeed;

    [SerializeField] Transform m_rollPivotParent, m_rollPivotF, m_rollPivotB, m_rollPivotR, m_rollPivotL;
    RollDirection m_rollDir;
    bool m_rolling;
    float m_rollAmmount = 0;

    private void FixedUpdate()
    {
        if (m_rolling)
        {
            roll();
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                setRollPoint(RollDirection.FORWARD);
            }
            if (Input.GetKey(KeyCode.S))
            {
                setRollPoint(RollDirection.BACK);
            }
            if (Input.GetKey(KeyCode.A))
            {
                setRollPoint(RollDirection.LEFT);
            }
            if (Input.GetKey(KeyCode.D))
            {
                setRollPoint(RollDirection.RIGHT);
            }
        }
    }

    void roll()
    {
        float rotValue = 90 * Time.deltaTime * m_rollSpeed;
        m_rollAmmount += rotValue;

        if (m_rollAmmount > 90)
            rotValue -= m_rollAmmount - 90;

        switch (m_rollDir)
        {
            case RollDirection.FORWARD:
                transform.RotateAround(m_rollPivotF.position, m_rollPivotF.right, rotValue);

                if (m_rollAmmount > 90)
                {
                    m_rollDir = RollDirection.NONE;
                    m_rolling = false;
                    m_rollAmmount = 0;

                    m_rollPivotParent.RotateAround(m_rollPivotParent.position, -m_rollPivotF.right, 90);
                }
                break;
            case RollDirection.BACK:
                transform.RotateAround(m_rollPivotB.position, -m_rollPivotF.right, rotValue);

                if (m_rollAmmount > 90)
                {
                    m_rollDir = RollDirection.NONE;
                    m_rolling = false;
                    m_rollAmmount = 0;

                    m_rollPivotParent.RotateAround(m_rollPivotParent.position, m_rollPivotF.right, 90);
                }
                break;
            case RollDirection.LEFT:
                transform.RotateAround(m_rollPivotL.position, m_rollPivotF.forward, rotValue);

                if (m_rollAmmount > 90)
                {
                    m_rollDir = RollDirection.NONE;
                    m_rolling = false;
                    m_rollAmmount = 0;

                    m_rollPivotParent.RotateAround(m_rollPivotParent.position, -m_rollPivotF.forward, 90);
                }
                break;
            case RollDirection.RIGHT:
                transform.RotateAround(m_rollPivotR.position, -m_rollPivotF.forward, rotValue);

                if (m_rollAmmount > 90)
                {
                    m_rollDir = RollDirection.NONE;
                    m_rolling = false;
                    m_rollAmmount = 0;

                    m_rollPivotParent.RotateAround(m_rollPivotParent.position, m_rollPivotF.forward, 90);
                }
                break;
            default:
                break;
        }
    }

    void setRollPoint(RollDirection _dir)
    {
        m_rollDir = _dir;
        m_rolling = true;
        m_rollAmmount = 0;
    }
}
