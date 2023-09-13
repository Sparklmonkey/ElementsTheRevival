using UnityEngine;

public class ToolTipCanvas : MonoBehaviour
{
    public static ToolTipCanvas Instance { get; private set; }
    [SerializeField]
    private RectTransform canvasRectTransform;
    private RectTransform bottomFieldRectTransform, topFieldRectTransform, detailRectTransform;

    private float toolTipStart = 0f;
    public float detailThreshold;

    [SerializeField]
    private GameObject topFieldToolTip, bottomFieldToolTip, creatureDetailToollTip;

    private Card cardOnDisplay;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        Instance = this;
        bottomFieldRectTransform = transform.Find("BottomFieldCardTT").GetComponent<RectTransform>();
        topFieldRectTransform = transform.Find("TopFieldCardTT").GetComponent<RectTransform>();
        detailRectTransform = transform.Find("Details").GetComponent<RectTransform>();
        //textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();

        if (canvasRectTransform == null)
        {
            Debug.LogError("Need to set Canvas Rect Transform!");
        }
        bottomFieldToolTip.SetActive(false);
        topFieldToolTip.SetActive(false);
        creatureDetailToollTip.SetActive(false);
        gameObject.SetActive(false);
    }

    private Vector2 objectPosition;
    private Vector2 detailObjectPosition;
    private int fieldIndex;
    private bool isCreatureField;
    public void SetupToolTip(Vector2 objectPosition, Vector2 objectSize, Card cardToDisplay, int fieldIndex, bool isCreatureField)
    {
        this.isCreatureField = isCreatureField;
        this.fieldIndex = fieldIndex;
        cardOnDisplay = cardToDisplay;
        detailObjectPosition = objectPosition - objectSize;
        detailObjectPosition.x += detailRectTransform.rect.width / 2;
        detailObjectPosition.y += detailRectTransform.rect.height;
        //isCreature = cardToDisplay.cardType.Equals(CardType.Creature);
        this.objectPosition = objectPosition;
        if (objectPosition.y > canvasRectTransform.rect.height / 2)
        {
            this.objectPosition.x += objectSize.x;
            topFieldToolTip.SetActive(true);
            topFieldToolTip.GetComponent<CardDisplayDetail>().SetupCardView(cardOnDisplay, true, true);
        }
        else
        {
            this.objectPosition.x -= objectSize.x;
            this.objectPosition.y -= objectSize.y;

            bottomFieldToolTip.SetActive(true);
            bottomFieldToolTip.GetComponent<CardDisplayDetail>().SetupCardView(cardOnDisplay, false, true);
        }


        gameObject.SetActive(true);
        SetToolPosition();
    }

    public void HideToolTip()
    {
        toolTipStart = 0f;
        objectPosition = Vector2.zero;
        topFieldToolTip.SetActive(false);
        bottomFieldToolTip.SetActive(false);
        creatureDetailToollTip.SetActive(false);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isCreatureField) { return; }
        if (toolTipStart > detailThreshold && !creatureDetailToollTip.activeSelf)
        {
            creatureDetailToollTip.SetActive(true);
            creatureDetailToollTip.GetComponent<CreatureDetailToolTip>().SetupDetailView(cardOnDisplay, fieldIndex);
            detailRectTransform.anchoredPosition = detailObjectPosition;
            return;
        }
        toolTipStart += Time.deltaTime;
    }

    private void SetToolPosition()
    {
        Vector2 anchoredPosition = objectPosition;// / canvasRectTransform.localScale.x;
        if (bottomFieldToolTip.activeSelf)
        {
            //anchoredPosition.y += objectPosition.y;
            if (anchoredPosition.x - bottomFieldRectTransform.rect.width < 0)
            {
                anchoredPosition.x = bottomFieldRectTransform.rect.width / 2;
            }
            if (anchoredPosition.y - bottomFieldRectTransform.rect.height < 0)
            {
                anchoredPosition.y = bottomFieldRectTransform.rect.height;
            }
            bottomFieldRectTransform.anchoredPosition = anchoredPosition;
            return;
        }

        //anchoredPosition.x += objectPosition.x / 2;
        if (anchoredPosition.x + topFieldRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - topFieldRectTransform.rect.width / 2;
        }
        if (anchoredPosition.y + topFieldRectTransform.rect.height + 50f > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - topFieldRectTransform.rect.height;
        }
        topFieldRectTransform.anchoredPosition = anchoredPosition;
        return;
    }

}
