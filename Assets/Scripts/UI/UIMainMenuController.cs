using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fontinixxl.NaptimeGames.UI
{
    public class UIMainMenuController : MonoBehaviour
    {
        public event Action<int> OnOptionSelected;

        [SerializeField] private Button[] buttons;
        private ISelectable _selectedButton;

        private void OnDisable()
        {
            if (_selectedButton == null) return;
            _selectedButton?.SetSelected(false);
            _selectedButton = null;
        }

        private void Start()
        {
            foreach (var button in buttons)
            {
                var selectable = button.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    button.onClick.AddListener(() => OnButtonClicked(selectable));
                }
            }
        }

        private void OnButtonClicked(ISelectable clickedButton)
        {
            if (_selectedButton == clickedButton) return;

            _selectedButton?.SetSelected(false);

            OnOptionSelected?.Invoke(clickedButton.Value);

            _selectedButton = clickedButton;
            _selectedButton.SetSelected(true);
        }
    }
}