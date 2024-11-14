using UnityEngine;
using UnityEngine.UI;

namespace GameControl
{
    public class StatusController : MonoBehaviour
    {
        [SerializeField] private Slider currentHealthImage, currentStaminaImage;

        private Status _playerStatus;
        
        private void Awake()
        {
            _playerStatus = FindObjectOfType<Status>();
        }

        private void Update()
        {
            currentHealthImage.value = _playerStatus.currentHealthValue;
            currentStaminaImage.value = _playerStatus.currentStaminaValue;
        }
    }
}