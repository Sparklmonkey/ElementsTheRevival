public class Leaf : Node
{
    private readonly IStrategy _strategy;

    public Leaf(IStrategy strategy, string nodeName = "Node") : base(nodeName)
    {
        _strategy = strategy;
    }

    // public override Status Process() => _strategy.Process();
    public override void Reset() => _strategy.Reset();
}

public class BehaviourTree : Node
{
    public BehaviourTree(string nodeName = "Node") : base(nodeName) { }

    public override Status Process()
    {
        while (CurrentChild < ChildNodes.Count)
        {
            var status = ChildNodes[CurrentChild].Process();
            if (status != Status.Success)
            {
                return status;
            }

            CurrentChild++;
        }

        return Status.Success;
    }
}