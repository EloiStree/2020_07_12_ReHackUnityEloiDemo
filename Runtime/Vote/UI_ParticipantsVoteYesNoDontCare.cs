using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public ChatVotePoll m_poll;

    public void SetPoll(ChatVotePoll poll) {
        m_poll = poll;
        RefreshUI(false);
    }

    private void RefreshUI(bool deleteAndAddPrefab)
    {
        if(deleteAndAddPrefab && m_whereToAdd)
            ClearAllPrefab();
       IEnumerable<ChatVote> votes =  m_poll.GetVotes();
        foreach (var item in votes)
        {
            SetOrAdd(item);
        }
        //for (int i = 0; i < m_currentlyAvailable.Count; i++)
        //{
        //    if(m_currentlyAvailable[i]!=null)
        //        m_currentlyAvailable[i].Refresh();

        //}
    }

    public void ClearAllPrefab()
    {
        Clear(m_whereToAdd);
    }

    public void GetVoteState(out int participantsCount, out int yes, out int no, out int dontcare, out int didNotVote)
    {
        m_poll.GetVotesState(out participantsCount, out didNotVote, out  dontcare, out  yes, out  no);
        //participantsCount=no=yes=dontcare= didNotVote = 0;
        //for (int i = 0; i < m_currentlyAvailable.Count; i++)
        //{
        //    if (m_currentlyAvailable[i] != null) {
        //        participantsCount++;
        //        if (m_currentlyAvailable[i].m_voteChoosed == UI_ParticipantVoteYesNo.Vote.DontCare)
        //            dontcare++;
        //        else if (m_currentlyAvailable[i].m_voteChoosed == UI_ParticipantVoteYesNo.Vote.No)
        //            no++;
        //        else if (m_currentlyAvailable[i].m_voteChoosed == UI_ParticipantVoteYesNo.Vote.Yes)
        //            yes++;
        //        else if (m_currentlyAvailable[i].m_voteChoosed == UI_ParticipantVoteYesNo.Vote.NotVoted)
        //            didNotVote++;

        //    }

        //}
    }

    public void UnvoteForAll()
    {
        m_poll.UnvoteAll();
        RefreshUI(false);
    }
    public void RemoveAll()
    {
        m_poll.RemoveAll();
        
        RefreshUI(true);
    }



    public void SetOrAdd(ChatVote vote)
    {
        UserIdentifier userId = vote.GetUserInfo().UserID;
        UI_ParticipantVoteYesNo participant = GetParticipant(ref userId);
        if (participant == null) {
            participant= AddParticipant(vote.GetUserInfo());
        }
         participant.SetChatVote(vote);
        m_voteDetected.Invoke();

    }

    private UI_ParticipantVoteYesNo GetParticipant(ref UserIdentifier user) { 
        string id = user.GetID();
        for (int i = 0; i < m_currentlyAvailable.Count; i++)
        {
            if (m_currentlyAvailable[i]!=null
                && m_currentlyAvailable[i].GetAssociatedID() ==id) {
                return m_currentlyAvailable[i];
            }

        }
        return null;
    }

    private UI_ParticipantVoteYesNo AddParticipant(RestreamChatUser user)
    {
        GameObject created = Instantiate(m_prefab);
        created.transform.SetParent(m_whereToAdd);
        created.transform.localScale = Vector3.one;
        UI_ParticipantVoteYesNo participant = created.GetComponent<UI_ParticipantVoteYesNo>();

        participant.SetChatVote(new ChatVote(user, ChatVoteType.NotVoted));
        m_currentlyAvailable.Add(participant);


        return participant;
    }


    public static Transform Clear( Transform transform)
    {
        if (transform == null)
            return null;
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
    public  void PushInClipboardAsMarkdown() {
        UnityClipboard.Set(m_poll.SaveAsMD(m_questionAsk.text));
    }

   
}
