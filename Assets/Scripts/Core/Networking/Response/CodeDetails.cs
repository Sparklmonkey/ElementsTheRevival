using Networking;

namespace Core.Networking.Response
{
    public class CodeDetails
    {
        public string CodeName;
        public string CardRewards;
        public int ElectrumRewards;
        public bool IsCardSelect;
        public bool IsSingleUse;

        public CodeDetails(CodeRedemptionResponse codeRedemptionResponse)
        {
            CodeName = codeRedemptionResponse.CodeName;
            CardRewards = codeRedemptionResponse.CardRewards;
            ElectrumRewards = codeRedemptionResponse.ElectrumRewards;
            IsSingleUse = codeRedemptionResponse.IsSingleUse;
            IsCardSelect = codeRedemptionResponse.IsCardSelect;
        }
    }
}