using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRaycaster : MonoBehaviour
{
    public Transform m_direction;
    public LayerMask m_mask = 1;
    public float m_computedDistance;
    public bool  m_didHitSomething;
    public bool m_useDebugDraw;
    public float m_debugDrawMaxDistance=1;
    public Color m_debugDrawFarColor= Color.red;
    public Color m_debugDrawNearColor =Color.green;

    void Update()
    {
        RaycastHit hit;
        m_didHitSomething = Physics.Raycast(m_direction.position, m_direction.forward, out hit, float.MaxValue, m_mask);
        if (m_didHitSomething)
            m_computedDistance = Vector3.Distance(m_direction.position, hit.point);
        else m_computedDistance = float.MaxValue;

        if (m_useDebugDraw) {
            float distance = m_debugDrawMaxDistance;
            if (m_didHitSomething)
                distance = m_computedDistance;
            Color c = Color.Lerp(m_debugDrawNearColor, m_debugDrawFarColor, distance / m_debugDrawMaxDistance);

            Debug.DrawLine(m_direction.position,m_direction.position+ m_direction.forward * distance, c);
        }
    }
    private void Reset()
    {
        m_direction = transform;
    }
}
