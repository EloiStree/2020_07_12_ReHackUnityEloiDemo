using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_ParticipantsVoteYesNoDontCare : MonoBehaviour
{

    public GameObject m_prefab;
    public RectTransform m_whereToAdd;
    public InputField m_questionAsk;

    public List<UI_ParticipantVoteYesNo> m_currentlyAvailable = new List<UI_ParticipantVoteYesNo>();

    public UnityEvent m_voteDetected;

    public void ClearAllPrefab()
    {
        Clear(m_whereToAdd);
    }

    internal void GetVoteState(out int participantsCount, out int yes, out int no, out int dontcare, out int didNotVote)
    {
        participantsCount=no=yes=dontcare= didNotVote = 0;
        for (int i = 0; i < m_currentlyAvailable.Count; i++)
        {
            if (m_currentlyAvailable[i] != null) {
                participantsCount++;
                if (m_currentlyAvailable[i].m_voteChoosed == UI_ParticipantVoteYesNo.Vote.DontCare)
                    dontcare++;
                else if (m_currentlyAvailable[i].m_voteChoosed == UI_ParticipantVoteYesNo.Vote.No)
                    no++;
                else if (m_currentlyAvailable[i].m_voteChoosed == UI_ParticipantVoteYesNo.Vote.Yes)
                    yes++;
                else if (m_currentlyAvailable[i].m_voteChoosed == UI_ParticipantVoteYesNo.Vote.NotVoted)
                    didNotVote++;

            }

        }
    }

    public void UnvoteForAll()
    {
        for (int i = 0; i < m_currentlyAvailable.Count; i++)
        {
            if (m_currentlyAvailable[i] != null)
                m_currentlyAvailable[i].SetVote(UI_ParticipantVoteYesNo.Vote.NotVoted);

        }
    }



    public void PushAsNotVotedMessage(RestreamChatMessage msg)
    {
        PushAsVoted(msg, UI_ParticipantVoteYesNo.Vote.NotVoted);

    }
    public void PushAsYesMessage(RestreamChatMessage msg)
    {

        PushAsVoted(msg, UI_ParticipantVoteYesNo.Vote.Yes);
    }
    public void PushAsNoMessage(RestreamChatMessage msg)
    {
        PushAsVoted(msg, UI_ParticipantVoteYesNo.Vote.No);

    }
    public void PushAsDontCareMessage(RestreamChatMessage msg)
    {
        PushAsVoted(msg, UI_ParticipantVoteYesNo.Vote.DontCare);
    }
    public void PushAsVoted(RestreamChatMessage msg, UI_ParticipantVoteYesNo.Vote vote)
    {
        UI_ParticipantVoteYesNo participant = GetParticipant(ref msg);
        if (participant == null) {
            participant= AddParticipant(msg);
        }
         participant.SetVote(vote);
        m_voteDetected.Invoke();

    }

    private UI_ParticipantVoteYesNo GetParticipant(ref RestreamChatMessage msg) { 
        string id = msg.UserID.GetID();
        for (int i = 0; i < m_currentlyAvailable.Count; i++)
        {
            if (m_currentlyAvailable[i]!=null
                && m_currentlyAvailable[i].GetAssociatedID() ==id) {
                return m_currentlyAvailable[i];
            }

        }
        return null;
    }

    private UI_ParticipantVoteYesNo AddParticipant(RestreamChatMessage msg)
    {
        GameObject created = Instantiate(m_prefab);
        created.transform.SetParent(m_whereToAdd);
        created.transform.localScale = Vector3.one;
        UI_ParticipantVoteYesNo participant = created.GetComponent<UI_ParticipantVoteYesNo>();

        participant.SetVote(UI_ParticipantVoteYesNo.Vote.NotVoted);
        participant.SetAssociatedID(msg.UserID.GetID());
        participant.SetDisplayName(msg.UserName);
        m_currentlyAvailable.Add(participant);


        return participant;
    }


    public static Transform Clear( Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }

    public string GetResumeOfTheVote() {
        string result = "Question:" + m_questionAsk.text;
        UI_ParticipantVoteYesNo p=null;
        for (int i = 0; i < m_currentlyAvailable.Count; i++)
        {
            p = m_currentlyAvailable[i];

            if (p!= null)
                result += string.Format("\n{0}|{1}|{2}", p.m_participantId, p.m_participantName, p.m_voteChoosed);

        }
        return result;
    }
    public void PushResultInClipboard() {
        UnityClipboard.Set(GetResumeOfTheVote());   
    }
}
