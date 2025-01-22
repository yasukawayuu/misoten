using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLogList : SingletonMonoBehaviour<KillLogList>
{

    [SerializeField] private KillLog[] _killLogList;
    
    public void SetKillLogList(string perpetratorText, string victimText)
    {
        for(int i = 1; i < _killLogList.Length; i++) 
        {
            if (_killLogList[i - 1].IsActive())
            {
                _killLogList[i].SetKillLog(_killLogList[i - 1].PerpetratorText, _killLogList[i - 1].VictimText);
            }
        }

        _killLogList[0].SetKillLog(perpetratorText, victimText);
    }
}
