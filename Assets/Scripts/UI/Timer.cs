using Misc;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        public float timeLeft { get; set; }
        public TextMeshProUGUI timeValue; // used for showing countdown from 3, 2, 1 
        public static bool gamePaused;
        public GameObject pauseMenu;
        public float TimerLength { get; set; }
        public int RoundCounter { get; private set; }

        private void OnEnable()
        {
            GameEvents.GameOver.AddListener(GameOver);
        }

        private void OnDisable()
        {
            GameEvents.GameOver.RemoveListener(GameOver);
        }

        private void Start()
        {
            TimerLength = 60.0f;
            timeLeft = TimerLength;
        }

        void Update()
        {
            timeLeft -= Time.deltaTime;
            timeValue.text = (timeLeft).ToString("0");
            if (timeLeft < 0)
            {
                // Invokes TimerCompleted event, which other classes can listen to.
                // ? operator checks if the event is null  
                GameEvents.TimerCompleted?.Invoke();
                RoundCounter++;
            
                if (TimerLength < 5)
                {
                    TimerLength = 5;
                }
            
                timeLeft = TimerLength;
            }
        }

        private void GameOver()
        {
            GetComponent<Timer>().enabled = false;
        }

    }
}
