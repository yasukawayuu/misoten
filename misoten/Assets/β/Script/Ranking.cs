using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Ranking : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Text _point;
    List<Marimo> _players;
    // Start is called before the first frame update
    void Start()
    {
        _players = new List<Marimo>();
    }

    // Update is called once per frame
    void Update()
    {
        _name.text = "";
        _point.text = "";

        int maxRanking = 10; // 表示するランキングの上限
        for (int i = 0; i < Math.Min(_players.Count, maxRanking); i++)
        {
            // 一位の人
            if(i == 0) _players[i].King = true;
            // それ以外の人
            else _players[i].King = false;

            _name.text += (i + 1) + "." + _players[i].name + "\n";
            _point.text += _players[i].Point + "\n";
        }
    }

    private void FixedUpdate()
    {
        _players.Clear();
        GameObject[] playerCount = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerCount.Length; ++i)
        {
            if (playerCount[i].GetComponent<Player>() != null)
                _players.Add(playerCount[i].GetComponent<Player>());
            else
                _players.Add(playerCount[i].GetComponent<AI>());
        }
        _players.Sort((a,b) => Mathf.FloorToInt(b.Point) - Mathf.FloorToInt(a.Point));
    }
}
