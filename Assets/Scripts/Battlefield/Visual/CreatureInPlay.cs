using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{

    public class CreatureInPlay : CardDisplayer
    {
        [SerializeField]
        private Image cardImage, upgradeShine, rareIndicator, cardHeadBack, activeAElement;
        [SerializeField]
        private TextMeshProUGUI creatureValue, cardName, activeAName, activeACost;
        [SerializeField]
        private GameObject upMovingText, activeAHolder;

        private EventBinding<ClearCardDisplayEvent> _clearCardDisplayBinding;
        private EventBinding<UpdateCardDisplayEvent> _updateCardDisplayBinding;
        private EventBinding<ShouldShowTargetableEvent> _shouldShowTargetableBinding;
        private EventBinding<ShouldShowUsableEvent> _shouldShowUsableBinding;
    
        private void OnDisable() {
            EventBus<ClearCardDisplayEvent>.Unregister(_clearCardDisplayBinding);
            EventBus<UpdateCardDisplayEvent>.Unregister(_updateCardDisplayBinding);
            EventBus<ShouldShowTargetableEvent>.Unregister(_shouldShowTargetableBinding);
            EventBus<ShouldShowUsableEvent>.Unregister(_shouldShowUsableBinding);
        }
        private void OnEnable()
        {
            _clearCardDisplayBinding = new EventBinding<ClearCardDisplayEvent>(HideCard);
            EventBus<ClearCardDisplayEvent>.Register(_clearCardDisplayBinding);
            _updateCardDisplayBinding = new EventBinding<UpdateCardDisplayEvent>(DisplayCard);
            EventBus<UpdateCardDisplayEvent>.Register(_updateCardDisplayBinding);
        
            _shouldShowTargetableBinding = new EventBinding<ShouldShowTargetableEvent>(ShouldShowTarget);
            EventBus<ShouldShowTargetableEvent>.Register(_shouldShowTargetableBinding);
            _shouldShowUsableBinding = new EventBinding<ShouldShowUsableEvent>(ShouldShowUsableGlow);
            EventBus<ShouldShowUsableEvent>.Register(_shouldShowUsableBinding);
        }
        public void DisplayCard(UpdateCardDisplayEvent updateCardDisplayEvent)
        {
            if (!updateCardDisplayEvent.Id.Equals(displayerId))
            {
                return;
            }
            maskImage.gameObject.SetActive(true);
            cardImage.sprite = ImageHelper.GetCardImage(updateCardDisplayEvent.Card.imageID);
            var creatureAtk = updateCardDisplayEvent.Card.passiveSkills.Antimatter ? -updateCardDisplayEvent.Card.AtkNow : updateCardDisplayEvent.Card.AtkNow;
            creatureValue.text = $"{creatureAtk}|{updateCardDisplayEvent.Card.DefNow}";
            cardName.text = updateCardDisplayEvent.Card.cardName;
            cardHeadBack.sprite = ImageHelper.GetCardHeadBackground(updateCardDisplayEvent.Card.costElement.FastElementString());
            if (updateCardDisplayEvent.Card.IsRare())
            {
                upgradeShine.gameObject.SetActive(true);
                cardName.font = underlayWhite;
                cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
            }
            else
            {
                upgradeShine.gameObject.SetActive(false);
                cardName.font = underlayBlack;
                cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
            rareIndicator.gameObject.SetActive(updateCardDisplayEvent.Card.IsRare());

            // PlayMaterializeAnimation(cardToDisplay.costElement);
            activeAHolder.SetActive(false);
            if (updateCardDisplayEvent.Card.skill != "")
            {
                activeAHolder.SetActive(true);
                activeAName.text = updateCardDisplayEvent.Card.skill;
                if (updateCardDisplayEvent.Card.skillCost > 0)
                {
                    activeACost.text = updateCardDisplayEvent.Card.skillCost.ToString();
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                    activeAElement.sprite = ImageHelper.GetElementImage(updateCardDisplayEvent.Card.skillElement.FastElementString());
                }
                else
                {
                    activeACost.text = "";
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
                }
            }
            else
            {
                if (updateCardDisplayEvent.Card.passiveSkills.Vampire)
                {
                    activeAName.text = "Vamprire";
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
                    activeACost.text = "";
                }
                else
                {
                    activeAHolder.SetActive(false);
                }
            }
        }

        public IEnumerator ShowDamage(int damage)
        {
            if (damage <= 0) { yield break; }
            var sText = Instantiate(upMovingText, transform);
            var destination = sText.GetComponent<MovingText>().SetupObject(damage.ToString(), TextDirection.Up);
            for (var i = 0; i < 15; i++)
            {
                sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 50f);
                yield return null;
            }
            Destroy(sText);
        }

        public void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
        {
            if (!clearCardDisplayEvent.Id.Equals(displayerId))
            {
                return;
            }
            ClearDisplay();
        }

    }
}
