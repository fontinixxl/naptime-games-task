using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIMainMenuController mainMenuController;
        [SerializeField] private StartButtonController startButtonController;

        [Header("UI References")] 
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameOverPanel;

        private int _objectsSelected;
        private Button _startButton;

        private void Awake()
        {
            _objectsSelected = 0;
            _startButton = startButtonController.GetComponent<Button>();
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            mainMenuController.OnOptionSelected += SetSelectedObjects;
            GameManager.Instance.OnGameOver += ShowGameOverUI;
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            mainMenuController.OnOptionSelected -= SetSelectedObjects;
            GameManager.Instance.OnGameOver -= ShowGameOverUI;
        }

        private void Start()
        {
            ShowGameUI();
        }

        public void ShowGameUI()
        {
            mainMenuPanel.SetActive(true);
            gameOverPanel.SetActive(false);
        }

        private void ShowGameOverUI()
        {
            gameOverPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
        }

        private void HideAllUIPanels()
        {
            gameOverPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
        }

        #region Unity OnClick Event Handlers

        private void SetSelectedObjects(int selectedObjects)
        {
            _objectsSelected = selectedObjects;
            startButtonController.SetButtonSate(true);
        }

        public void OnStartButtonClicked()
        {
            if (_objectsSelected == 0)
            {
                Debug.LogError("Start game has been pressed without selecting an option!");
                return;
            }

            GameManager.Instance.StartGame(_objectsSelected);
            HideAllUIPanels();
            startButtonController.SetButtonSate(true);
        }

        #endregion
    }
}