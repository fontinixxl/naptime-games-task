using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fontinixxl.NaptimeGames.UI
{
    public class HighlightButton : MonoBehaviour, ISelectable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite highlightedSprite;
        [SerializeField] private Vector3 highlightedScale = new(1.1f, 1.1f, 1.1f);
        [Header("Data")]
        [SerializeField] private int value;

        public int Value => value;

        private bool _isSelected;
        private Vector3 _normalScale;

        private void OnValidate()
        {
            if (displayText != null)
            {
                displayText.text = value.ToString();
            }
        }
        
        private void Awake()
        {
            displayText.text = value.ToString();
            _normalScale = transform.localScale;
            UpdateAppearance(); // Update appearance at Awake to handle initial selected state
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;
            buttonImage.sprite = highlightedSprite;
            transform.localScale = highlightedScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;
            buttonImage.sprite = normalSprite;
            transform.localScale = _normalScale;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            if (_isSelected)
            {
                buttonImage.sprite = highlightedSprite;
                transform.localScale = highlightedScale;
            }
            else
            {
                buttonImage.sprite = normalSprite;
                transform.localScale = _normalScale;
            }
        }
    }
}