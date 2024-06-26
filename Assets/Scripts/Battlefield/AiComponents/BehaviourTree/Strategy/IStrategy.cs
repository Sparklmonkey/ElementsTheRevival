public interface IStrategy
{
    Node.Status Process((Card card, ID id) cardId);
    void Reset();
}