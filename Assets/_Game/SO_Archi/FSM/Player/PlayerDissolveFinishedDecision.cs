using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDissolveFinished", menuName = "SO/FSM/Player/Decisions/DissolveFinished", order = -1)]
public class PlayerDissolveFinishedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return((Player)controller).IsDissolveFinished;
    }
}