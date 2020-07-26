using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class DummyCharacter_AngleMs : MonoBehaviour
{
    public Transform m_startPoint;
    public List<AngleDistanceMove> m_moves = new List<AngleDistanceMove>();
    public List<RaycastMoveStep> m_moveSteps;
    public LayerMask m_raycastMask = 1;
    public Color m_generalColor;

    [Header("Debug")]
    public float m_totalDistance;

    public void GetCurrentPosition(out Vector3 position, out Quaternion rotation)
    {
        position = m_startPoint.position;
        rotation = m_startPoint.rotation;
        RaycastMoveStep last = GetLast();
        if (last != null)
        { 
            position = last.newPosition;
            rotation = last.newRotation;
        }

    }

    public Vector3[] GetPathPoints() {
        List<Vector3> pos = m_moveSteps.Select(k => k.startPosition).ToList();
        if (m_moveSteps.Count > 0)
            pos.Add(m_moveSteps[m_moveSteps.Count-1].newPosition);
        return pos.ToArray(); 
    }

    public RaycastMoveStep GetLast() {
        if (m_moveSteps.Count <= 0)
            return null;
        return m_moveSteps[m_moveSteps.Count - 1]; 
    }

    public void Start()
    {
        if (m_startPoint == null)
            return;
        RefreshMove();
    }

    public void Update()
    {
        if (m_startPoint == null)
            return;
        RefreshMove();
        DrawPath(Time.deltaTime);

    }

    public void RefreshMove() {
        if (m_startPoint == null)
            return;
        Vector3 currentPosition = m_startPoint.position;
        Quaternion currentRotation = m_startPoint.rotation;
        RaycastMoveStep lastComputed;
        m_moveSteps.Clear();
        for (int i = 0; i < m_moves.Count; i++)
        {
            lastComputed = new RaycastMoveStep(
               currentPosition,
               currentRotation,
               m_moves[i].m_angle,
               m_moves[i].m_distanceToMove,
               m_raycastMask);
            m_moveSteps.Add(lastComputed);
            currentPosition = lastComputed.newPosition;
            currentRotation = lastComputed.newRotation;
        }
        m_totalDistance = 0;
        for (int i = 0; i < m_moveSteps.Count-1; i++)
        {
            m_totalDistance += m_moveSteps[i].GetDistance();
        }
    }

    public void DrawPath(float time)
    {
        if (m_startPoint == null)
            return;
        for (int i = 0; i < m_moveSteps.Count; i++)
        {
            Debug.DrawLine(m_moveSteps[i].startPosition, m_moveSteps[i].newPosition, m_generalColor, time);
        }
    }

    private void OnValidate()
    {
        if (m_startPoint == null)
            return;
        if (!Application.isPlaying)
        { 
            RefreshMove();
            DrawPath(5);
        }
    }
}

[System.Serializable]
public class RaycastMoveStep
{
    public Vector3 startPosition;
    public Quaternion startRotation;
    public Vector3 newPosition;
    public Quaternion newRotation;

    public RaycastMoveStep(Vector3 position, Quaternion rotation, float angle, float distance, LayerMask mask) {
        startPosition = position;
        startRotation = rotation;
        newRotation = startRotation*Quaternion.Euler(0, angle, 0);

        //if (Physics.OverlapSphere(position, 0.001f, mask, QueryTriggerInteraction.Ignore).Length > 0)
        //{
        //    newPosition = startPosition;
        //}
        //else
        { 

            RaycastHit hit;
            if (Physics.Raycast(position, newRotation * Vector3.forward, out hit, distance, mask, QueryTriggerInteraction.Ignore))
            {
                newPosition = position+ (hit.point- position )* 0.9f;
            }
            else newPosition = startPosition + (newRotation*Vector3.forward) * distance;
        }
    }
    public float GetDistance()
    {
        return Vector3.Distance(startPosition, newPosition);
    }
}

[System.Serializable]
public class AngleDistanceMoveList
{
    public List<AngleDistanceMove> m_values = new List<AngleDistanceMove>();
    public void Add(AngleDistanceMove move)
    {
        m_values.Add(move);
    }

 
    public void Set(int index, AngleDistanceMove move, bool allowExtending)
    {
        if (index < 0) return;
        if (index+1 >= m_values.Count) {
            if (!allowExtending) return;
            for (int i =m_values.Count - 1; i < index; i++)
            {
                m_values.Add(new AngleDistanceMove());
            }

        }
        m_values[index]=move;
    }
    public void Add(params AngleDistanceMove[] move)
    {
        m_values.AddRange(move);
    }
    public void Set(params AngleDistanceMove [] move)
    {
        m_values.Clear();
        m_values.AddRange(move);
    }

    public void RemoveLastAdd() {
        if(m_values.Count>0)
        m_values.RemoveAt(m_values.Count - 1);
    }
    public void Clear() { m_values.Clear(); }
}

[System.Serializable]
public class AngleDistanceMove {
    public float m_angle;
    public float m_distanceToMove;
    public AngleDistanceMove(): this(0,0)
    {
    }

    public AngleDistanceMove(float angle, float distance) {
        m_angle = angle;
        m_distanceToMove = distance;
    }

    public AngleDistanceMove(float angle, float speed, float timeInMs) {
        m_angle = angle;
        m_distanceToMove = speed * timeInMs/1000;
    }

 
}
