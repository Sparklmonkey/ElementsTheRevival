using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Achievements;
using Unity.Services.CloudCode;
using UnityEngine;

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
            var response = await _cloudCodeProcessor.CallCloudCodeWithResponse<CanOracle>("validate-oracle-usable");
            Debug.Log(response.canOpenOracle);
            return response.canOpenOracle;
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
        
        public async Task<List<PlayerAchievement>> GetPlayersAchievements()
        {
            var saveData = JsonUtility.ToJson(PlayerData.Shared);
            var response = await _cloudCodeProcessor.GetAchievementsByCategory("Collection", saveData);
            Debug.Log(response);
            return JsonHelper.FromJson<PlayerAchievement>(response.FixJson()).ToList();
        }
    }
}