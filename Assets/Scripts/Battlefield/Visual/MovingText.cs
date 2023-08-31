using TMPro;
using UnityEngine;

namespace Elements.Duel.Visual
{

    public class MovingText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        public Vector3 SetupObject(string text, TextDirection textDirection)
        {
            this.text.text = text;
            return GetNewDirection(transform.position, textDirection);
        }

        public Vector3 GetNewDirection(Vector3 currentPosition, TextDirection textDirection)
        {
            switch (textDirection)
            {
                case TextDirection.Up:
                    return currentPosition + new Vector3(0f, 100f, 0f);
                case TextDirection.Down:
                    return currentPosition - new Vector3(0f, 100f, 0f);
                case TextDirection.Left:
                    return currentPosition - new Vector3(100f, 0f, 0f);
                case TextDirection.Right:
                    return currentPosition + new Vector3(100f, 0f, 0f);
                default:
                    return currentPosition;
            }
        }
    }
}