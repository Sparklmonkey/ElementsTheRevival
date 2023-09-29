using Networking;

namespace Core.Networking.Response
{
    public class CodeDetails
    {
        public string codeName;
        public string cardRewards;
        public int electrumRewards;
        public bool isCardSelect;
        public bool isSingleUse;

        public CodeDetails(CodeRedemptionResponse codeRedemptionResponse)
        {
            codeName = codeRedemptionResponse.codeName;
            cardRewards = codeRedemptionResponse.cardRewards;
            electrumRewards = codeRedemptionResponse.electrumRewards;
            isSingleUse = codeRedemptionResponse.isSingleUse;
            isCardSelect = codeRedemptionResponse.isCardSelect;
        }
    }
}