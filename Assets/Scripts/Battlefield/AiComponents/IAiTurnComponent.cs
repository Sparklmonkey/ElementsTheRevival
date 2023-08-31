using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAiTurnComponent
{
    IEnumerator PlayPillars(PlayerManager aiManager);
    IEnumerator RestOfTurn(PlayerManager aiManager);
}