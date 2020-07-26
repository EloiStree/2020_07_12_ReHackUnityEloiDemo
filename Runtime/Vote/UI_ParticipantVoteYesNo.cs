using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class UI_ParticipantVoteYesNo : MonoBehaviour
{

    public string m_participantId;
    public string m_participantName;
    public Vote m_voteChoosed= Vote.NotVoted;
    public enum Vote {NotVoted,Yes,No,DontCare }
    public Color m_notVotedColor = Color.grey;
    public Color m_hasVotedColor = Color.green;
    public Color m_noColor = Color.red;
    public Color m_dontCareColor = Color.yellow;
    public ChoosedColorEvent m_votedColorChange;
    public NameChangedEvent m_nameChanged;


    public void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        SetDisplayName(GetDisplayName());
        SetColorFromVote(GetVoteState());
    }

    private Vote GetVoteState()
    {
        return m_voteChoosed;
    }

    private void SetColorFromVote(Vote vote)
    {
        SetVote(vote);
    }

 

    public void SetDisplayName(string name)
    {
        m_participantName = name;
        m_nameChanged.Invoke(m_participantName);
    }
    public void SetAssociatedID(string id)
    {
        m_participantId = id;
    }
    public string GetDisplayName()
    {
        return m_participantName;

    }
    public string GetAssociatedID()
    {
        return m_participantId;
    }
    public void SetVote(Vote voteValue)
    {
        m_voteChoosed = voteValue;
        m_votedColorChange.Invoke(GetCurrentVoteColor());

    }
    private Color GetCurrentVoteColor()
    {
        return GetVoteColor(m_voteChoosed);
    }
    private Color GetVoteColor(Vote voteValue)
    {
        Color colorChoosed;
        switch (voteValue)
        {
            case Vote.Yes:
                colorChoosed = (m_hasVotedColor);
                break;
            case Vote.No:
                colorChoosed = (m_noColor);
                break;
            case Vote.DontCare:
                colorChoosed = (m_dontCareColor);
                break;
            default:
                colorChoosed = (m_notVotedColor);
                break;
        }

        return colorChoosed;
    }

    [System.Serializable]
    public class ChoosedColorEvent : UnityEvent<Color>
    {

    }
    [System.Serializable]
    public class NameChangedEvent : UnityEvent<string>
    {

    }
}
