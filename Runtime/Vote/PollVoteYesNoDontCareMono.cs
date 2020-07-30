using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using UnityEngine.Events;

public class PollVoteYesNoDontCareMono : MonoBehaviour
{
    
    public ChatVotePoll m_poll= new ChatVotePoll();
    public ChatVotePollEvent m_onChanged;
    public UnityEvent m_resetPoolVotes;
    public UnityEvent m_resetPoolParticipants;

    public void PushInPoll_Yes(RestreamChatMessage message)
    {
        m_poll.SetVote(ChatVoteType.Yes, message);
        NotifyChange();
    }
    public void PushInPoll_No(RestreamChatMessage message)
    {
        m_poll.SetVote(ChatVoteType.No, message);
        NotifyChange();
    }
    public void PushInPoll_DontCare(RestreamChatMessage message)
    {
        m_poll.SetVote(ChatVoteType.DontCare, message);
        NotifyChange();
    }
    public void PushInPoll_NotVoted(RestreamChatMessage message)
    {
        m_poll.SetVote(ChatVoteType.NotVoted, message);
        NotifyChange();
    }

    public void RemoveVoters()
    {
        m_poll.RemoveAll();
        NotifyChange();
        m_resetPoolParticipants.Invoke();

    }
    public void SetAllAsUnvoted()
    {
        m_poll.UnvoteAll();
        NotifyChange();
        m_resetPoolVotes.Invoke();



    }

    private void NotifyChange()
    {
        m_onChanged.Invoke(m_poll);
    }



    public void PushResultInClipboard(string topic)
    {
        UnityClipboard.Set(m_poll.SaveAsMD(topic));
    }
   

}

public class QuestionChatVotePoll : ChatVotePoll {

    [SerializeField]
    public string m_question;
    [SerializeField]
    public string m_id;
    [SerializeField]
    public ChatVotePoll m_votes;
}

[System.Serializable]
public class ChatVotePollEvent : UnityEvent<ChatVotePoll>
{ }


    [System.Serializable]
public class ChatVotePoll {

    
    [SerializeField] List<ChatVote> m_votesAsList = new List<ChatVote>();
    private Dictionary<string, ChatVote> m_voteChecker = new Dictionary<string, ChatVote>();

    public IEnumerable<ChatVote> GetVotes() { return m_votesAsList; }
    public void GetVotesState(out int voter, out int notvoted, out int dontcare, out int yes, out int no) {
        IEnumerable<ChatVote> vote = GetVotes();
        IEnumerable<ChatVote> tmp=null;
        voter = vote.Count();
        ChatVote.KeepYes(vote, out tmp);
        yes = tmp.Count();
        ChatVote.KeepNotVoted(vote, out tmp);
        notvoted = tmp.Count();
        ChatVote.KeepDontCare(vote, out tmp);
        dontcare = tmp.Count();
        ChatVote.KeepNo(vote, out tmp);
        no = tmp.Count();

    }

    public void AddAsUnvote(RestreamChatMessage message)
    {
        SetVote(ChatVoteType.NotVoted, message.GetAsUserInfo());
    }

    public void SetVote(ChatVoteType vote, RestreamChatMessage message)
    {
        SetVote(vote, message.GetAsUserInfo());
    }


    public void AddAsUnvote(RestreamChatUser user) {
        SetVote(ChatVoteType.NotVoted, user);
    }

    public void SetVote(ChatVoteType vote, RestreamChatUser user) {
        if (user == null)
            return;
        string id= user.UserID.GetID();
       // Debug.Log("ID:" + id);
        if (!m_voteChecker.ContainsKey(id)) {
            m_voteChecker.Add(id, new ChatVote(user,vote));
        }
        m_voteChecker[id].SetVote(vote);
        SaveAsList();
    }

    private void SaveAsList()
    {
        m_votesAsList = m_voteChecker.Values.ToList();
    }

    public void UnvoteAll()
    {
        foreach (var item in m_voteChecker.Keys)
        {
            m_voteChecker[item].SetVote(ChatVoteType.NotVoted);
        }
        SaveAsList();
    }

    public void RemoveAll()
    {
        m_voteChecker.Clear();
        SaveAsList();
    }

    public string SaveAsCSV()
    {
        string result = "";
        foreach (ChatVote cv in m_votesAsList)
        {
            if (cv != null)
                result += string.Format("{0},{1},{2}\n", cv.GetUserInfo().UserID, cv.GetDisplayName(), cv.GetVote().ToString());
        }
        return result;
    }
    public string SaveAsMD(string subjectOfPoll)
    {
        int all, dontcare, yes, not, unvoted;
        GetVotesState(out all, out unvoted, out dontcare, out yes,out  not);
        string result = 
            "# Vote Poll\n  " +
            "Topic: " + subjectOfPoll + "\n  " +
            string.Format("All|Blank |Dont Care|Yes|No\n  ") +
            string.Format("-|-|-|-|-\n  ") +
            string.Format("{0}|{1}|{2}|{3}|{4}\n  ", all, unvoted, dontcare, yes, not) +
            "----------------\n  \n  " +
            "Platform|ID|Given Name|Vote\n  "+
        "-|-|-|-:\n  ";
        foreach (ChatVote cv in m_votesAsList)
        {
            if (cv != null)
                result += string.Format("{0}|{1}|{2}|{3}\n ", cv.GetUserInfo().UserID.Platform,
                   cv.GetUserInfo().UserID.UserName,
                   cv.GetDisplayName(),
                   GetVoteAsShort(cv.GetVote()));
        }
        return result;
    }

    private string GetVoteAsShort(ChatVoteType chatVoteType)
    {
        switch (chatVoteType)
        {
            case ChatVoteType.Yes:return "+1";
            case ChatVoteType.No:return "-1";
            case ChatVoteType.DontCare:return "0";
                
            case ChatVoteType.NotVoted:
            default: return "?";
        }
    }

    public string LoadAsCSV(string csv)
    {
        throw new NotImplementedException();
    }
}


[System.Serializable]
public class ChatVote {
    #region INSTANCE
    public string m_displayName;
    public RestreamChatUser m_user;
    public ChatVoteType m_voteChoosed = ChatVoteType.NotVoted;

    public ChatVote(RestreamChatUser user, ChatVoteType vote)
    {
        this.m_user = user;
        this.m_voteChoosed = vote;
    }

    public ChatVoteType GetVote()
    {
        return m_voteChoosed;
    }

    public void SetVote(ChatVoteType voteValue)
    {
        m_voteChoosed = voteValue;
    }

    public RestreamChatUser GetUserInfo()
    {
        return m_user;
    }

    public void SetUserInfo(RestreamChatUser user)
    {
        m_user = user;
    }

    public void SetWantedDisplayName(string name)
    {
        m_displayName = name;
    }
    public string GetDisplayName() {
        if(m_displayName!=null && m_displayName.Length>0)
            return m_displayName;
        return m_user.UserName;
    }
    #endregion
    #region STATIC
    public static void KeepYes(IEnumerable<ChatVote> poll, out IEnumerable<ChatVote> result)
    {
        KeepVote(poll, out result, ChatVoteType.Yes);
    }
    public static void KeepNo(IEnumerable<ChatVote> poll, out IEnumerable<ChatVote> result)
    {
        KeepVote(poll, out result, ChatVoteType.No);
    }
    public static void KeepNotVoted(IEnumerable<ChatVote> poll, out IEnumerable<ChatVote> result)
    {
        KeepVote(poll, out result, ChatVoteType.NotVoted);
    }
    public static void KeepDontCare(IEnumerable<ChatVote> poll, out IEnumerable<ChatVote> result)
    {
        KeepVote(poll, out result, ChatVoteType.DontCare);
    }

    public static void KeepVote(IEnumerable<ChatVote> poll, out IEnumerable<ChatVote> result, ChatVoteType vote)
    {
        result = poll.Where(k => k.m_voteChoosed == vote);
    }
    public static void KeepPlatform(IEnumerable<ChatVote> poll, out IEnumerable<ChatVote> result, ChatPlatform platform)
    {
        result = poll.Where(k => k.m_user.Platform == platform);
    }
    public static void GetDisplayName(IEnumerable<ChatVote> poll, out IEnumerable<string> result)
    {
        result = poll.Select(k => k.GetDisplayName());
    }
    public static void GetUsers(IEnumerable<ChatVote> poll, out IEnumerable<string> result)
    {
        result = poll.Select(k => k.m_user.UserName);
    }
    public static void GetUsersId(IEnumerable<ChatVote> poll, out IEnumerable<string> result)
    {
        result = poll.Select(k => k.m_user.UserID.GetID());
    }

    #endregion
}
public enum ChatVoteType { NotVoted, Yes, No, DontCare }


