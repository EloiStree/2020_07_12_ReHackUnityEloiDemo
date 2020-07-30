using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ParticipantsVoteTextDisplay : MonoBehaviour
{
    public ChatVotePoll m_voteSource;
    public Text m_participantsCount;
    public Text m_didNotVote;
    public Text m_yes;
    public Text m_no;
    public Text m_dontCare;


    public void SetWith(ChatVotePoll poll) {
        m_voteSource = poll;
        Refresh();
    }

    public void Refresh() {

        int no, yes, dontcare, participantsCount, didNotVote;
        m_voteSource.GetVotesState(out participantsCount, out didNotVote, out dontcare, out yes, out no);

        m_participantsCount.text = "" + participantsCount;
        m_didNotVote.text = "" + didNotVote;
        m_yes.text = "" + yes;
        m_no.text = "" + no;
        m_dontCare.text = "" + dontcare;

    }
    
}
