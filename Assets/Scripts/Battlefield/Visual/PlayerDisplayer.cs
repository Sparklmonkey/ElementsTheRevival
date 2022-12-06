using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{
    public class PlayerDisplayer : Identifiable
    {
        [SerializeField]
        private Image validTargetGlow;

        public void ShouldShowTarget(bool shouldShow)
        {
            validTargetGlow.color = shouldShow ? new Color(255, 0, 0, 255) : new Color(0, 0, 0, 0);
        }
    }
}
