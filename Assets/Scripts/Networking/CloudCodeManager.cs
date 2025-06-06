using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;

namespace Networking.Networking
{
    public class CloudCodeManager : ICloudCodeManager
    {
        private readonly CloudCodeProcessor _cloudCodeProcessor = new CloudCodeProcessor(CloudCodeService.Instance);
        
        public async Task<ArenaResponse> GetT50Opponent()
        {
            return await _cloudCodeProcessor.CallCloudCodeWithResponse<ArenaResponse>("get-t50-opponent");
        }

        public async Task<DeckPresets> GetDeckPresets()
        {
            return await _cloudCodeProcessor.CallCloudCodeWithResponse<DeckPresets>("get-deck-presets");
        }

        public async Task<DeckPresets> SaveDeckPresets(string deckA, string deckB, string deckC)
        {
            var arguments = new Dictionary<string, object>
                { { "deckA", deckA }, { "deckB", deckB }, { "deckC", deckC } };
            return await _cloudCodeProcessor.CallCloudCodeWithResponse<DeckPresets>("save-deck-presets", arguments);
        }
    
        public async Task<CodeRedemptionResponse> GetCodeDetails(string redeemCode)
        {
            var arguments = new Dictionary<string, object> { { "CodeName", redeemCode } };
            return await _cloudCodeProcessor.CallCloudCodeWithResponse<CodeRedemptionResponse>("validate-redeem-code", arguments);
        }

        public async Task RedeemCode(string redeemCode)
        {
            var arguments = new Dictionary<string, object> { { "code", redeemCode } };
            await _cloudCodeProcessor.CallCloudCode("redeem-code", arguments);
        }

        public async Task<bool> CheckOraclePlay()
        {
            return await _cloudCodeProcessor.CallCloudCodeWithResponse<bool>("validate-oracle-usable");
        }

        public async Task<ScoreUpdateResponse> UpdateScore(int score)
        {
            var arguments = new Dictionary<string, object> { { "Score", score } };
            return await _cloudCodeProcessor.CallCloudCodeWithResponse<ScoreUpdateResponse>("update-player-score", arguments);
        }

        public async Task UpdateOraclePlayed()
        {
            await _cloudCodeProcessor.CallCloudCode("update-oracle-date");
        }
    }
}