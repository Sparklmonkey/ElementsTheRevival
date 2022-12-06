using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuest
{
    public string QuestTitle { get; }
    public string QuestDescription { get; }
    public string QuestObjective { get; }
    public string QuestReward { get; }
    public bool IsComplete { get; }
    public void RewardPlayer();
}


public class WelcomeQuest : IQuest
{
    public string QuestTitle => "Quest 1 : Welcome!";

    public string QuestDescription => "This is Element's main menu. You just received 40 cards for your starter deck; are you ready for your first duel?";

    public string QuestObjective => "Objective: Defeat a level 0 elemental, click on the 'Quests' button AFTER your victory to get your reward. \n Click the 'Level 0' button to start your first duel.";

    public string QuestReward => "Reward: 10";

    public bool IsComplete => PlayerData.shared.hasDefeatedLevel0;

    public void RewardPlayer()
    {
        PlayerData.shared.electrum += 10;
    }
}

public class ImproveDeckQuest : IQuest
{
    public string QuestTitle => "Quest 2 : Improve Your Deck";

    public string QuestDescription => "There are 40 cards in your starter deck; a deck can have between 30 and 60 cards. Get rid of some of the cards you do not like/use, the cards removed from your deck are still yours and will remain in your storage.";

    public string QuestObjective => "Objective: Remove at least one card from your deck. Click 'Your Deck' and click on the card(s) you want to remove.";

    public string QuestReward => "Reward: 20";

    public bool IsComplete => PlayerData.shared.removedCardFromDeck;

    public void RewardPlayer()
    {
        PlayerData.shared.electrum += 20;
    }
}

public class BazaarQuest : IQuest
{
    public string QuestTitle => "Quest 3 : The Bazaar";

    public string QuestDescription => "Now that you have something in your storage you can sell it, get some money for it, and buy the cards you are looking for. You can buy almost any card at the bazaar, only the most rare cards are not for sale, you can get those only with a spin after winning against a level 1 or higher elemental.";

    public string QuestObjective => "Objective: Click on the 'Bazaar' button. Sell at least one card and buy at least one card.";

    public string QuestReward => "Reward: 30";

    public bool IsComplete => PlayerData.shared.hasSoldCardBazaar && PlayerData.shared.hasBoughtCardBazaar;

    public void RewardPlayer()
    {
        PlayerData.shared.electrum += 30;
    }
}
public class ElementalOneQuest : IQuest
{
    public string QuestTitle => "Quest 4 : Level 1 elementals";

    public string QuestDescription => "You are now ready for a more challenging fight. Level 1 elementals are not very well organized, their decks often lack a strategy, but sometimes they get lucky and they can fight hard. Winning against a level 1 elemental will grant you a spin at the wheel, get three cards of the same type and that card is yours.";

    public string QuestObjective => "Objective: Defeat a Level 1 elemental. Click the 'Level 1' button to start the duel.";

    public string QuestReward => "Reward: 35";

    public bool IsComplete => PlayerData.shared.hasDefeatedLevel1;

    public void RewardPlayer()
    {
        PlayerData.shared.electrum += 35;
    }
}
public class ElementalTwoQuest : IQuest
{
    public string QuestTitle => "Quest 5 : Level 2 elementals";

    public string QuestDescription => "It is time to try an even harder fight; Level 2 elementals have well organized decks, but if you defeat one you'll get even better rewards. Good Luck.";

    public string QuestObjective => "Objective: Defeat a Level 2 elemental. Click the 'Level 2' button to start the duel.";

    public string QuestReward => "Reward: 40";

    public bool IsComplete => PlayerData.shared.hasDefeatedLevel2;

    public void RewardPlayer()
    {
        PlayerData.shared.electrum += 40;
    }
}

public class ScoreOneQuest : IQuest
{
    public string QuestTitle => "Quest 6 : Your score";

    public string QuestDescription => "Every time you win a match you gain some score points, if you lose a game, you lose some. \n Your score determines the level of Arena Decks that you create. Increase your score and come back for a juicy reward.";

    public string QuestObjective => "Objective: Reach a score of at least 150";

    public string QuestReward => "Reward: 150";

    public bool IsComplete => PlayerData.shared.playerScore > 150;

    public void RewardPlayer()
    {
        PlayerData.shared.electrum += 150;
    }
}

public class ScoreTwoQuest : IQuest
{
    public string QuestTitle => "Quest 7 : Rare Cards";

    public string QuestDescription => "Rare cards are extremely hard to find, although they are not necessary for a winning strategy, they are often a nice addition for your deck. Get an even higher score and I will reward you with a rare card.";

    public string QuestObjective => "Objective: Reach a score of at least 500";

    public string QuestReward => "Reward: A rare card";

    public bool IsComplete => PlayerData.shared.playerScore > 500;

    public void RewardPlayer()
    {
        PlayerPrefs.SetFloat("ShouldShowRareCard", 1);
    }
}