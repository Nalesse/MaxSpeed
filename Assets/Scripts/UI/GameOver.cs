using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOver : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject gameOverScreen;
        public Button gameOverFirstButton;
        private void OnEnable()
        {
            GameEvents.GameOver.AddListener(enableGameOverScreen);
        }

        private void enableGameOverScreen()
        {
            gameOverScreen.SetActive(true);
            gameOverFirstButton.Select();
        
        }
    }
}
