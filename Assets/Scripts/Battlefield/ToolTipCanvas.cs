using UnityEngine;

public class ToolTipCanvas : MonoBehaviour
{
    public static ToolTipCanvas Instance { get; private set; }
    [SerializeField]
    private RectTransform canvasRectTransform;
    private RectTransform _bottomFieldRectTransform, _topFieldRectTransform, _detailRectTransform;

    private float _toolTipStart = 0f;
    public float detailThreshold;

    [SerializeField]
    private GameObject topFieldToolTip, bottomFieldToolTip, creatureDetailToollTip;

    private Card _cardOnDisplay;
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void Awake()
    {
        Instance = this;
        _bottomFieldRectTransform = transform.Find("BottomFieldCardTT").GetComponent<RectTransform>();
        _topFieldRectTransform = transform.Find("TopFieldCardTT").GetComponent<RectTransform>();
        _detailRectTransform = transform.Find("Details").GetComponent<RectTransform>();
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

    private Vector2 _objectPosition;
    private Vector2 _detailObjectPosition;
    private int _fieldIndex;
    private bool _isCreatureField;
    public void SetupToolTip(Vector2 objectPosition, Vector2 objectSize, Card cardToDisplay, int fieldIndex, bool isCreatureField)
    {
        this._isCreatureField = isCreatureField;
        this._fieldIndex = fieldIndex;
        _cardOnDisplay = cardToDisplay;
        _detailObjectPosition = objectPosition - objectSize;
        _detailObjectPosition.x += _detailRectTransform.rect.width / 2;
        _detailObjectPosition.y += _detailRectTransform.rect.height;
        //isCreature = cardToDisplay.cardType.Equals(CardType.Creature);
        this._objectPosition = objectPosition;
        if (objectPosition.y > canvasRectTransform.rect.height / 2)
        {
            this._objectPosition.x += objectSize.x;
            topFieldToolTip.SetActive(true);
            topFieldToolTip.GetComponent<CardDisplayDetail>().SetupCardView(_cardOnDisplay, true, true);
        }
        else
        {
            this._objectPosition.x -= objectSize.x;
            this._objectPosition.y -= objectSize.y;

            bottomFieldToolTip.SetActive(true);
            bottomFieldToolTip.GetComponent<CardDisplayDetail>().SetupCardView(_cardOnDisplay, false, true);
        }


        gameObject.SetActive(true);
        SetToolPosition();
    }

    public void HideToolTip()
    {
        _toolTipStart = 0f;
        _objectPosition = Vector2.zero;
        topFieldToolTip.SetActive(false);
        bottomFieldToolTip.SetActive(false);
        creatureDetailToollTip.SetActive(false);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    private void Update()
    {
        if (!_isCreatureField) { return; }
        if (_toolTipStart > detailThreshold && !creatureDetailToollTip.activeSelf)
        {
            creatureDetailToollTip.SetActive(true);
            creatureDetailToollTip.GetComponent<CreatureDetailToolTip>().SetupDetailView(_cardOnDisplay, _fieldIndex);
            _detailRectTransform.anchoredPosition = _detailObjectPosition;
            return;
        }
        _toolTipStart += Time.deltaTime;
    }

    private void SetToolPosition()
    {
        var anchoredPosition = _objectPosition;// / canvasRectTransform.localScale.x;
        if (bottomFieldToolTip.activeSelf)
        {
            //anchoredPosition.y += objectPosition.y;
            if (anchoredPosition.x - _bottomFieldRectTransform.rect.width < 0)
            {
                anchoredPosition.x = _bottomFieldRectTransform.rect.width / 2;
            }
            if (anchoredPosition.y - _bottomFieldRectTransform.rect.height < 0)
            {
                anchoredPosition.y = _bottomFieldRectTransform.rect.height;
            }
            _bottomFieldRectTransform.anchoredPosition = anchoredPosition;
            return;
        }

        //anchoredPosition.x += objectPosition.x / 2;
        if (anchoredPosition.x + _topFieldRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - _topFieldRectTransform.rect.width / 2;
        }
        if (anchoredPosition.y + _topFieldRectTransform.rect.height + 50f > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - _topFieldRectTransform.rect.height;
        }
        _topFieldRectTransform.anchoredPosition = anchoredPosition;
        return;
    }

}
