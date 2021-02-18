using System.Collections.Generic;
using System.Linq;
using Bolt;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityTemplateProjects.PlayerTDEW;


/// <summary>
/// Run in both Server and Player
/// States are in a SimpleIntState entity
/// </summary>
public class InLevelEndStateController : Bolt.EntityBehaviour<IInLevelEndState>
{
    [SerializeField] private List<PlayerTDEWController> _players;
    [SerializeField] private List<PlayerTDEWController> _playersAtEnd;

    public UnityEvent notifyEndLevel;

    public override void Attached()
    {
        _players = new List<PlayerTDEWController>();
        _playersAtEnd = new List<PlayerTDEWController>();
        
        state.AddCallback("PlayersAtEnd[]", (pState, pPath, pIndices) => { CheckPlayerAtEndAndNotify(); });
    }

    private void CheckPlayerAtEndAndNotify()
    {
        foreach (var playerAtEnd in state.PlayersAtEnd)
        {
            if (playerAtEnd == null)
                continue;

            var player = playerAtEnd.GetComponent<PlayerTDEWController>();

            if (!_playersAtEnd.Contains(player))
                _playersAtEnd.Add(player);
        }

        if (_playersAtEnd.Count == _players.Count)
        {
            notifyEndLevel?.Invoke();
        }
    }

    public void AddPlayer(PlayerTDEWController pPlayer)
    {
        if (!_players.Contains(pPlayer))
            _players.Add(pPlayer);
    }

    public void Remove(PlayerTDEWController pPlayer)
    {
        _players.Remove(pPlayer);
    }
}