using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButtonController : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void OnEnable()
    {
        startButton.interactable = false;
    }

    public void EnableStartButton()
    {
        startButton.interactable = true;
    }
}
