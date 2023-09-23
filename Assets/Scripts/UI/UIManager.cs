using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        private int _objectsSelected;

        private void Awake()
        {
            _objectsSelected = 0;
        }

        public void SetSelectedObjects(int selectedObjects)
        {
            _objectsSelected = selectedObjects;
        }

        public void StartGame()
        {
            if (_objectsSelected == 0)
            {
                Debug.LogError("Start game has been pressed without selecting an option!");
                return;
            }
            
            mainMenuPanel.SetActive(false);
            GameManager.Instance.StartGame(_objectsSelected);
        }
    }
}