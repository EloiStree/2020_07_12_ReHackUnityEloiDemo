using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class UI_ParticipantVoteYesNo : MonoBehaviour
{

    public Color m_notVotedColor = Color.grey;
    public Color m_hasVotedColor = Color.green;
    public Color m_noColor = Color.red;
    public Color m_dontCareColor = Color.yellow;
    public ChoosedColorEvent m_votedColorChange;
    public NameChangedEvent m_nameChanged;
    public ChatVote m_voteChoosed;

    public void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        SetChatVote(GetChatVote());
    }

    public void SetChatVote(ChatVote vote)
    {
        m_voteChoosed = vote;
        m_votedColorChange.Invoke(GetCurrentVoteColor());
        m_nameChanged.Invoke(GetDisplayName());

    }

    public ChatVote GetChatVote()
    {
        return m_voteChoosed;
    }

    private ChatVoteType GetVoteState()
    {
        return m_voteChoosed.GetVote();
    }


    public string GetDisplayName()
    {
        return m_voteChoosed.GetUserInfo().UserName;
    }
    public string GetAssociatedID()
    {
        return m_voteChoosed.GetUserInfo().UserID.GetID();
    }
   
    private Color GetCurrentVoteColor()
    {
        return GetVoteColor( m_voteChoosed.GetVote() );
    }
    private Color GetVoteColor(ChatVoteType voteValue)
    {
        Color colorChoosed;
        switch (voteValue)
        {
            case ChatVoteType.Yes:
                colorChoosed = (m_hasVotedColor);
                break;
            case ChatVoteType.No:
                colorChoosed = (m_noColor);
                break;
            case ChatVoteType.DontCare:
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

    public RestreamChatUser GetUserInfo()
    {
       return m_voteChoosed.GetUserInfo();
    }
    public ChatVoteType GetVoteChoosed() { return m_voteChoosed.GetVote(); }
}
