using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DummyMeshRepresentation : MonoBehaviour
{
    public DummyCharacter_AngleMs m_target;
    public Transform m_meshCurrentPosition;
    public LineRenderer m_meshPath;
    public Renderer[] m_renderers;
    public Vector3 m_lineRendererOffset = Vector3.up*0.1f;

    void Update()
    {
        for (int i = 0; i < m_renderers.Length; i++)
        {
            m_renderers[i].material.color = m_target.m_generalColor;
        }
       Vector3 position;
       Quaternion rotation;
        m_target.GetCurrentPosition(out position, out rotation );
        m_meshCurrentPosition.position = position;
        m_meshCurrentPosition.rotation = rotation;
        Vector3[] positions = m_target.GetPathPoints();
        m_meshPath.positionCount = (positions.Length);
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] += m_lineRendererOffset;
        }
        m_meshPath.SetPositions(m_target.GetPathPoints());


    }
}
