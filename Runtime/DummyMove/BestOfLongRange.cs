using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BestOfLongRange : MonoBehaviour
{
    public RandomAddToDummy m_randomPusher;
    public DummyCharacter_AngleMs m_random;
    public DummyCharacter_AngleMs m_bestOf;
    public Transform m_endPoint;
    public bool m_isActive;
    public long m_interaction;

    float m_distance = float.MaxValue;
    float m_currentDistance = float.MaxValue;
    float m_pathDistance = float.MaxValue;
    float m_currentPathDistance = float.MaxValue;
    void Update()
    {
        if (m_isActive) { 
            m_randomPusher.PushRandom();
            Quaternion rot;
            Vector3 pos;
            m_random.GetCurrentPosition(out pos,out  rot);
            m_currentDistance = Vector3.Distance(m_endPoint.position, pos);

            if (m_currentDistance < 2)
            {
                    m_distance = m_currentDistance;
                m_currentPathDistance = m_random.m_totalDistance;
                if (m_currentPathDistance < m_pathDistance)
                {
                    m_pathDistance = m_currentPathDistance;
                    m_bestOf.m_moves = m_random.m_moves.ToList();
                
                }


            }
            
            m_interaction++;
        }
    }
}
