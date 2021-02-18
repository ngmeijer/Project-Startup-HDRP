using System.Collections.Generic;
using System.Linq;
using Bolt;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects.PlayerTDEW;


/// <summary>
/// Run in both Server and Player
/// States are in a SimpleIntState entity
/// </summary>
public class InLevelEndStateController : Bolt.EntityBehaviour<IInLevelEndState>
{
    private List<PlayerTDEWController> _players;
    private List<PlayerTDEWController> _playersAtEnd;
    
    public override void Attached() {
        
        state.AddCallback("PlayersAtEnd[]", (pState, pPath, pIndices) => { CheckPlayerAtEndAndNotify(); });
    }

    private void CheckPlayerAtEndAndNotify()
    {
        
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