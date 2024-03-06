using Battlefield.Abilities;

public class TargetingAi
{
    public (ID id, Card card) BestTarget(AiTargetType aiTargetType, string skill)
    {
        if (aiTargetType is null) return default;
        var possibleTargets = DuelManager.Instance.ValidTargets.ConvertToTuple();
        (ID id, Card card) bestTarget = default;
        var topScore = 0f;


        foreach (var target in possibleTargets)
        {
            var score = 0f;
            switch (aiTargetType.Targeting)
            {
                case TargetType.Smaller:
                    score = target.CalculateSmallerScore(aiTargetType.Estimate);
                    break;
                case TargetType.Immortals:
                    score = target.CalculateImmortalScore(aiTargetType.Estimate, aiTargetType.OnlyFriend,
                        aiTargetType.OnlyFoe);
                    break;
                case TargetType.CreatureHighAtk:
                    score = target.CalculateCreatureHighAtkScore(aiTargetType.Estimate);
                    break;
                case TargetType.CreatureLowAtk:
                    score = target.CalculateCreatureLowAtkScore(aiTargetType.Estimate, aiTargetType.DefineValue);
                    break;
                case TargetType.CreatureAndPlayer:
                    score = target.CalculateCreatureAndPlayerScore(aiTargetType.Estimate, aiTargetType.OnlyFriend,
                        aiTargetType.OnlyFoe);
                    break;
                case TargetType.Creature:
                    score = target.CalculateCreatureScore(aiTargetType.Estimate);
                    break;
                case TargetType.Permanent:
                    score = target.CalculatePermanentScore(aiTargetType.Estimate, skill);
                    break;
                case TargetType.Pillar:
                    score = target.CalculatePillarScore(aiTargetType.Estimate);
                    break;
                case TargetType.BetaCreature:
                    score = target.CalculateBetaCreatureScore(aiTargetType.Estimate, aiTargetType.OnlyFriend,
                        aiTargetType.OnlyFoe);
                    break;
                case TargetType.DefineDef:
                    score = target.CalculateDefineDefScore(aiTargetType.Estimate, aiTargetType.OnlyFriend,
                        aiTargetType.OnlyFoe, aiTargetType.DefineValue);
                    break;
                case TargetType.AlphaCreature:
                    score = target.CalculateAlphaCreatureScore(aiTargetType.OnlyFriend,
                        aiTargetType.OnlyFoe);
                    break;
                case TargetType.DefineAtk:
                    score = target.CalculateDefineAtk(aiTargetType.Estimate, aiTargetType.OnlyFoe,
                        aiTargetType.DefineValue, aiTargetType.DefTolerance);
                    break;
                case TargetType.Trebuchet:
                    score = target.CalculateTrebuchetScore(aiTargetType.Estimate);
                    break;
                case TargetType.SkillCreature:
                    score = target.CalculateSkillCreatureScore(aiTargetType.Estimate);
                    break;
                case TargetType.Weapon:
                    score = target.CalculateWeaponScore();
                    break;
                case TargetType.Fractal:
                    score = target.CalculateFractalScore(aiTargetType.Estimate);
                    break;
                case TargetType.Tears:
                    score = target.CalculateTearsScore(aiTargetType.Estimate);
                    break;
            }

            if (score <= topScore) continue;
            topScore = score;
            bestTarget = target;
        }
        return bestTarget;
    }
}