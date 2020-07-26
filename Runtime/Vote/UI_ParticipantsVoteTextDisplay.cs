using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ParticipantsVoteTextDisplay : MonoBehaviour
{
    public UI_ParticipantsVoteYesNoDontCare m_voteSource;
    public Text m_participantsCount;
    public Text m_didNotVote;
    public Text m_yes;
    public Text m_no;
    public Text m_dontCare;

  

    public void Refresh() {

        int no, yes, dontcare, participantsCount, didNotVote;
        m_voteSource.GetVoteState(out participantsCount, out yes,out no, out dontcare, out didNotVote);

        m_participantsCount.text = "" + participantsCount;
        m_didNotVote.text = "" + didNotVote;
        m_yes.text = "" + yes;
        m_no.text = "" + no;
        m_dontCare.text = "" + dontcare;

    }
    
}
