using System.Threading.Tasks;

namespace Networking.Networking
{
    public interface ICloudCodeManager
    {
        Task<ArenaResponse> GetT50Opponent();
        Task<DeckPresets> GetDeckPresets();
        Task<DeckPresets> SaveDeckPresets(string deckA, string deckB, string deckC);
        Task<CodeRedemptionResponse> GetCodeDetails(string redeemCode);
        Task RedeemCode(string redeemCode);
        Task<bool> CheckOraclePlay();
        Task<ScoreUpdateResponse> UpdateScore(int score);
        Task UpdateOraclePlayed();
    }
}