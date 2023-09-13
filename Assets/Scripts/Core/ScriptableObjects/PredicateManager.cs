public class PredicateManager
{
    private static readonly PredicateManager instance = new PredicateManager();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static PredicateManager()
    {
    }

    private PredicateManager()
    {
    }

    public static PredicateManager Instance
    {
        get
        {
            return instance;
        }
    }



    //static List<string> reverseTimePriority = new List<string> { "52t", "71d", "52m", "716" };
    //static List<string> mutationPriority = new List<string> { "0", "6ub", "4vr" };

    //public Predicate<ID> reverseTimePred = delegate (ID x) { return reverseTimePriority.Contains(DuelManager.GetCard(x).iD) && x.Owner.Equals(OwnerEnum.Opponent); };
    //public Predicate<ID> mutationsPred = delegate (ID x) { return mutationPriority.Contains(DuelManager.GetCard(x).iD) && x.Owner.Equals(OwnerEnum.Opponent); };
    //public Predicate<ID> endowPred = delegate (ID x) { return CardDatabase.Instance.weaponIdList.Contains(DuelManager.GetCard(x).iD) && x.Owner.Equals(OwnerEnum.Opponent); };
    //public Predicate<ID> petrifyPred = delegate (ID x) { return DuelManager.GetCard(x).passive.Contains("gravity pull") && x.Owner.Equals(OwnerEnum.Opponent); };
    //public Predicate<ID> rndOpponentPred = delegate (ID x) { return x.Owner.Equals(OwnerEnum.Player); };
    //public Predicate<ID> rndSelfPred = delegate (ID x) { return x.Owner.Equals(OwnerEnum.Opponent); };

    //public Predicate<ID> ragePred = delegate (ID x) { return ((DuelManager.GetCard(x).DefNow < 5) && x.Owner.Equals(OwnerEnum.Player)) 
    //                                                            || ((DuelManager.GetCard(x).DefNow > 5) && x.Owner.Equals(OwnerEnum.Opponent)); };

    //public Predicate<ID> beserkPred = delegate (ID x) { return ((DuelManager.GetCard(x).DefNow < 6) && x.Owner.Equals(OwnerEnum.Player)) 
    //                                                            || ((DuelManager.GetCard(x).DefNow > 6) && x.Owner.Equals(OwnerEnum.Opponent)); };

    //public Predicate<ID> purifyPred = delegate (ID x)
    //{
    //    return (DuelManager.GetCard(x).Poison > 0 || DuelManager.GetCard(x).IsAflatoxin) && x.Owner.Equals(OwnerEnum.Opponent);
    //};

    //public Predicate<ID> holyLightAtkPred = delegate (ID x) {
    //    return (DuelManager.GetCard(x).costElement.Equals(Element.Darkness) || DuelManager.GetCard(x).costElement.Equals(Element.Death)) && x.Owner.Equals(OwnerEnum.Player);
    //};

    //public Predicate<ID> holyLightDefPred = delegate (ID x) {
    //    return DuelManager.GetCard(x).DefDamage > 0 && !DuelManager.GetCard(x).costElement.Equals(Element.Darkness) && !DuelManager.GetCard(x).costElement.Equals(Element.Death) && x.Owner.Equals(OwnerEnum.Opponent);
    //};

    //public Predicate<ID> nightmarePred = delegate (ID x) {
    //    return (DuelManager.GetCard(x).costElement != DuelManager.Instance.player.playerPassiveManager.GetMark().costElement) 
    //    || DuelManager.GetCard(x).iD == "5ru" || DuelManager.GetCard(x).iD == "7qe" || DuelManager.GetCard(x).cost > 3;
    //};

    //public Predicate<ID> fractalPred = delegate (ID x) {
    //    return (DuelManager.GetCard(x).costElement == DuelManager.Instance.enemy.playerPassiveManager.GetMark().costElement) || DuelManager.GetCard(x).cost < 3;
    //};
}
