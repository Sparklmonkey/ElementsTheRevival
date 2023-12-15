public class QuestManager
{
    static QuestManager()
    {
    }

    private QuestManager()
    {
    }

    public static QuestManager Instance { get; } = new();
}