using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button startButton;
        
        private int _objectsSelected;

        private void Awake()
        {
            _objectsSelected = 0;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGameOver += ShowGameOverUI;
        }
        private void OnDisable()
        {
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

        public void SetSelectedObjects(int selectedObjects)
        {
            _objectsSelected = selectedObjects;
            startButton.interactable = true;
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
            startButton.interactable = false;
        }
        
        #endregion
    }
}