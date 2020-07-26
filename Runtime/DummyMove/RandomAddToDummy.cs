using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAddToDummy : MonoBehaviour
{
    public DummyCharacter_AngleMs m_dummy;
    public float m_supposedSpeed;
    public float m_minTimeInMs=2, m_maxTimeInMs=10;
    public float m_minAngle=-90, m_maxAngle=90;
    public int m_interation=6;
    public void PushRandom()
    {
        m_dummy.m_moves.Clear();
        for (int i = 0; i < m_interation; i++)
        {
            m_dummy.m_moves.Add(new AngleDistanceMove(GetRandomAngle(), m_supposedSpeed, GetRandomTime()));

        }
            m_dummy.RefreshMove();
    }

    private float GetRandomTime()
    {
        return UnityEngine.Random.Range( m_minTimeInMs, m_maxTimeInMs);
    }

    private float GetRandomAngle()
    {
        return UnityEngine.Random.Range(m_minAngle, m_maxAngle);
    }
}
