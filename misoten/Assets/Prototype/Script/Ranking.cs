using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Ranking : MonoBehaviour
{
    Text _rangking;
    List<Player> _players;
    // Start is called before the first frame update
    void Start()
    {
        _rangking = GetComponent<Text>();
        _players = new List<Player>();
    }

    // Update is called once per frame
    void Update()
    { 
        _rangking.text = 
            "ƒ‰ƒ“ƒLƒ“ƒO\n" +
            _players[0].Name.PadRight(20) +  _players[0].Point.ToString().PadLeft(10) + "\n" +
            _players[1].Name.PadRight(20) +  _players[1].Point.ToString().PadLeft(10) + "\n";
    }

    private void FixedUpdate()
    {
        _players.Clear();
        GameObject[] playerCount = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerCount.Length; ++i)
        {
            _players.Add(playerCount[i].GetComponent<Player>());
        }
        _players.Sort((a,b) => b.Point - a.Point);
    }
}
