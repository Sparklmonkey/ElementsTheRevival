public class QuestManager
{
    private static readonly QuestManager instance = new();

    static QuestManager()
    {
    }

    private QuestManager()
    {
    }

    public static QuestManager Instance
    {
        get
        {
            return instance;
        }
    }



}