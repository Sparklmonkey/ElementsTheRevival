using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAiTurnComponent
{
    void PlayPillars(PlayerManager aiManager);
    void RestOfTurn(PlayerManager aiManager);
}