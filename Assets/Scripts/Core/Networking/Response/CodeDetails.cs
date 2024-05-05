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
            CodeName = codeRedemptionResponse.codeName;
            CardRewards = codeRedemptionResponse.cardRewards;
            ElectrumRewards = codeRedemptionResponse.electrumRewards;
            IsSingleUse = codeRedemptionResponse.isSingleUse;
            IsCardSelect = codeRedemptionResponse.isCardSelect;
        }
    }
}