public class PvPCommand_ReceiveAction : PvPCommand
{
    private PvP_Action action;

    public PvPCommand_ReceiveAction(PvP_Action action)
    {
        this.action = action;
    }

    public override void StartCommandExecution()
    {
        DuelManager.ReceivePvpAction(action);
        CommandExecutionComplete();
    }
}
