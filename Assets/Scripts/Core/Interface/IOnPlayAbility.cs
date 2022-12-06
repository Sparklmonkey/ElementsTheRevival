using System.Collections;

public interface IOnPlayAbility
{
    IEnumerator ActiveActionWhenPlayed(ID owner);

    void ActiveActionWhenDestroyed(ID owner);
}