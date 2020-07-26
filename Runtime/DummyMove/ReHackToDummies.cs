using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using UnityEngine;

public class ReHackToDummies : MonoBehaviour
{
    public GameObject m_prefabDummy;
    public Transform m_startPoint;
    ReHackUsersCollection<AngleDistanceMoveList> m_userDummiesPath= 
        new ReHackUsersCollection<AngleDistanceMoveList>();
    ReHackUsersCollection<DummyCharacter_AngleMs> m_userDummiesObj= 
        new ReHackUsersCollection<DummyCharacter_AngleMs>();

    //   public const string number = "[0-9]+\\.?[0-9]*";
    public const string numberInt = "-?[0-9]+";
    public const string numberuInt = "[0-9]+";
    public Regex set = new Regex("^set " + numberuInt + " " + numberInt + " " + numberuInt + "$");
    public Regex clear = new Regex("^clear$");
    public Regex add = new Regex("^add " + numberInt + " " + numberuInt + "$");
    //public Regex add = new Regex("^add.*");
    public void TryToInterpertMessage(RestreamChatMessage userMsg) {
        AddNewUserIfNew(userMsg);
        string msg = userMsg.Message.ToLower().Trim() ;
        bool hasChanged = false;
        if (set.IsMatch(msg))
        {
            string[] tokens = msg.Split(' ');
            int distance;
            int angle;
            int index;
            if (int.TryParse(tokens[1], out index) && int.TryParse(tokens[2], out angle) && int.TryParse(tokens[3], out distance))
            {
                GetPath(userMsg).Set(index, new AngleDistanceMove(angle, distance / 100f),true);

                hasChanged = true;
            }

        }
        else if (clear.IsMatch(msg))
        {
            GetPath(userMsg).Clear();

            hasChanged = true;
        }
        else if (add.IsMatch(msg))
        {
            string[] tokens = msg.Split(' ');
            int distance;
            int angle;
            if(int.TryParse(tokens[1], out angle) && int.TryParse(tokens[2], out distance))
            {
                GetPath(userMsg).Add(new AngleDistanceMove(angle, distance / 100f));
                hasChanged = true;
            }

        }
        if (hasChanged)
            RefreshUser(userMsg);


    }

    public void RefreshUser(RestreamChatMessage user) {
        
        SetDummyWith(GetPath( user), GetDummy( user));
    }

    public void SetDummyWith(AngleDistanceMoveList path, DummyCharacter_AngleMs dummy) {
        dummy.m_moves = path.m_values;
        dummy.RefreshMove();
    }

    public void AddNewUserIfNew(RestreamChatMessage user) {
        if (!m_userDummiesObj.IsRegistered(ref user)) {
            GameObject created = GameObject.Instantiate(m_prefabDummy);
            DummyCharacter_AngleMs script = created.GetComponent<DummyCharacter_AngleMs>();
            m_userDummiesObj.Store(ref user, script);
            AngleDistanceMoveList moves = new AngleDistanceMoveList();
            m_userDummiesPath.Store(ref user, moves);
            script.m_startPoint = m_startPoint;
            script.m_generalColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
        }
    }

    public DummyCharacter_AngleMs GetDummy(RestreamChatMessage user)
    {

        return m_userDummiesObj.GetFromMessage(ref user);
    }
    public AngleDistanceMoveList GetPath(RestreamChatMessage user)
    {
        return m_userDummiesPath.GetFromMessage(ref user);
    }

}

