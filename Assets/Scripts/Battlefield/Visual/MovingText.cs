using TMPro;
using UnityEngine;

namespace Elements.Duel.Visual
{

    public class MovingText : MonoBehaviour
    {
        private bool isSetup;
        [SerializeField]
        private TextMeshProUGUI text;
        private Vector3 newPosition;
        private float timer = 0f;
        public void SetupObject(string text, TextDirection textDirection)
        {
            newPosition = GetNewDirection(transform.position, textDirection);
            this.text.text = text;
            isSetup = true;
        }

        public Vector3 GetNewDirection(Vector3 currentPosition, TextDirection textDirection)
        {
            timer += Time.deltaTime;
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

        private void Update()
        {
            if (isSetup && !GameOverVisual.isGameOver)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * 50f);
                if (transform.position == newPosition || timer >= 1f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}