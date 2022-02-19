public class Command_PlayCardOnField : Command
{
    private PlayerManager playerManager;
    private Card card;
    private ID newLocationId;

    public Command_PlayCardOnField(PlayerManager playerManager, Card card, ID newLocationId)
    {
        this.playerManager = playerManager;
        this.card = card;
        this.newLocationId = newLocationId;
    }

    public override void StartCommandExecution()
    {
        playerManager.PlayCardOnFieldVisual(newLocationId, card);
    }
}