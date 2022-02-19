public interface IOnPlayAbility
{
    void ActiveActionWhenPlayed(ID owner);

    void ActiveActionWhenDestroyed(ID owner);
}