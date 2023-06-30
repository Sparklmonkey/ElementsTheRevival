using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{
    public class PlayerDisplayer : Identifiable
    {
        [SerializeField]
        private Image validTargetGlow;

        [SerializeField]
        private Sprite poisonSprite, puritySprite, neurotoxinSprite;
        public Image poisonImg, sanctImage, silenceImage;
        public TextMeshProUGUI poisonLabel, boneShieldLabel, purityLabel;
        [SerializeField]
        private GameObject cloakVisual;

        public void ShouldShowTarget(bool shouldShow)
        {
            validTargetGlow.color = shouldShow ? new Color(255, 255, 255, 255) : new Color(0, 0, 0, 0);
        }

        public void UpdatePlayerIndicators(Counters playerCounters)
        {
            if (playerCounters.poison != 0)
            {
                poisonImg.gameObject.SetActive(true);
                if (playerCounters.poison < 0)
                {
                    poisonImg.sprite = puritySprite;
                    poisonLabel.text = "+" + Mathf.Abs(playerCounters.poison).ToString();
                }
                else if (playerCounters.neurotoxin > 0)
                {
                    poisonImg.sprite = neurotoxinSprite;
                    poisonLabel.text = playerCounters.poison.ToString();
                }
                else if (playerCounters.poison == 0)
                {
                    poisonImg.gameObject.SetActive(false);
                }
                else
                {
                    poisonImg.sprite = poisonSprite;
                    poisonLabel.text = playerCounters.poison.ToString();
                }
            }

            if (playerCounters.invisibility != 0)
            {
                if (GetObjectID().Owner.Equals(OwnerEnum.Opponent) && playerCounters.invisibility > 0)
                {
                    cloakVisual.SetActive(true);
                }
                if (playerCounters.invisibility <= 0)
                {
                    playerCounters.invisibility = 0;
                    if (GetObjectID().Owner.Equals(OwnerEnum.Opponent))
                    {
                        cloakVisual.SetActive(false);
                    }
                }
            }
            silenceImage.gameObject.SetActive(playerCounters.silence > 0);
            sanctImage.gameObject.SetActive(playerCounters.sanctuary > 0);
        }
    }
}
