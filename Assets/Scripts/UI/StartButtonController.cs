using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class StartButtonController : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private GameObject buttonImage;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite disableSprite;


        private void OnEnable()
        {
            SetButtonSate(false);
        }

        public void SetButtonSate(bool isEnabled)
        {
            if (isEnabled)
            {
                startButton.interactable = true;
                startButton.image.sprite = normalSprite;
                buttonImage.SetActive(true);
            }
            else
            {
                startButton.interactable = false;
                startButton.image.sprite = disableSprite;
                buttonImage.SetActive(false);
            }
        }
    }
}