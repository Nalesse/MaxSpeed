using System.Collections;
using Misc;
using Player;
using TMPro;
using UI;
using UnityEngine;

public class Score : MonoBehaviour
    {
        // Start is called before the first frame update
        [field: SerializeField] public float PlayerScore { get; set; }
        [SerializeField] private float tempScore;
        private float highScore;
        [SerializeField] private TextMeshProUGUI scoreValue;
        [SerializeField] private TextMeshProUGUI finalScoreValue;
        [SerializeField] private TextMeshProUGUI tempScoreValue;
        [SerializeField] private TextMeshProUGUI highScoreValue;
        [SerializeField] private TextMeshProUGUI pointMultValue;
        [SerializeField] private AudioSource trickBufferFilled;
        private float pointsIncreasing;
        private int pointMultiplier;
        private TrickSystem TrickSystem;
        public float trickBuffer;
        private bool isScoreMultiplying = true;
        private bool soundPlayed;

        [Header("Score Requirement")]
        [SerializeField] private float scoreRequirement;
        [SerializeField] private float increaseAmount;
        [SerializeField] private int timerDecreaseRound;
        [SerializeField] private int timerDecreaseAmount;
        [SerializeField] private Timer timer;

        [SerializeField] private TextMeshProUGUI goalValue;

        // for debug only
        [Header("Debug")]
        [SerializeField] private bool enableGameOver;

        private void OnEnable()
        {
            GameEvents.TimerCompleted.AddListener(ScoreRequirement);
            GameEvents.GameOver.AddListener(GameOver);
        }

        private void OnDisable() => GameEvents.TimerCompleted.RemoveListener(ScoreRequirement);

        private void Start()
        {
            PlayerScore = 0f;
            goalValue.text = "" + scoreRequirement;
            pointsIncreasing = 1f;
            TrickSystem = GameObject.Find("Player").GetComponent<TrickSystem>();

            StartCoroutine(Setup());
        }

        IEnumerator Setup()
        {
            yield return LeaderboardController.Instance.LoginRoutine();
            yield return LeaderboardController.Instance.GetHighScoreRoutine();
            highScore = LeaderboardController.Instance.Score;
            highScoreValue.text = $"{(int)highScore:00000}";
        }

        // Update is called once per frame
        private void Update()
        {
            scoreValue.text = $"{(int)PlayerScore:00000}";
            finalScoreValue.text = $"{(int)PlayerScore}";
            tempScoreValue.text = $"{(int)tempScore:00000}";

            if (Player.Player.Instance.gameOver)
            {
                tempScore = 0;
                return;
            }

            if (TrickSystem.isDoingTrick)
            {
                pointsIncreasing = 100 * Time.deltaTime;
                tempScore += pointsIncreasing;
            }
            if (TrickSystem.isDoingTrickSmaller)
            {
                pointMultValue.text = "x" + $"{pointMultiplier}";
            }

            if (Player.Player.Instance.isDamage)
            {
                tempScore = 0;
                pointMultiplier = 0;
                trickBuffer = 0;
                trickBufferFilled.pitch = 1;
            }

            if (Player.Player.Instance.isGrounded)
            {

                PlayerScore += tempScore * pointMultiplier;
                tempScore = 0;
                pointMultiplier = 0;
                trickBuffer = 0;
                pointMultValue.text = "x" + $"{pointMultiplier}";
                trickBufferFilled.pitch = 1;
            }
            
        }

        public void IncreaseMultiplier()
        {
            // if (!TrickSystem.isDoingTrickSmaller && tempScore > 0)
            // {
            //     if (!isScoreMultiplying || !(trickBuffer >= 10)) return;
            //
            //     trickBuffer = 0;
            //     pointMultiplier += 1;
            //     isScoreMultiplying = false;
            //     soundPlayed = false;
            // }
            // else
            // {
            //     isScoreMultiplying = true;
            // }
            Debug.Log("Increase multiplier called");
            trickBuffer = 0;
            pointMultiplier += 1;
            isScoreMultiplying = false;
            trickBufferFilled.Play();
            trickBufferFilled.pitch += 0.5f;
            soundPlayed = false;
        }

        private void FixedUpdate()
        {
            if (!TrickSystem.isDoingTrick) return;
            
            trickBuffer += 0.1f;
            if (trickBuffer >= 10 && !soundPlayed)
            {
                soundPlayed = true;
            }
        }

        private void ScoreRequirement()
        {
            if (PlayerScore < (int)scoreRequirement)
            {
                if (enableGameOver)
                {
                    GameEvents.GameOver?.Invoke();
                }
            
                Debug.Log("Game Over");
            }
            else
            {
                scoreRequirement += increaseAmount;
                increaseAmount += 500;
                scoreRequirement = Mathf.CeilToInt(scoreRequirement);
                goalValue.text = $"{scoreRequirement}";

                if (timer.RoundCounter >= timerDecreaseRound)
                {
                    timer.TimerLength -= timerDecreaseAmount;
                    timerDecreaseAmount += 5;
                }
                
                Debug.Log("Score Requirement: " + (int)scoreRequirement);
            }
        }
        private void GameOver()
        {
            var scoreToSubmit = (int)PlayerScore;
            
            if (PlayerScore > highScore)
            {
                highScore = (int)PlayerScore;
                highScoreValue.text = $"{(int)highScore:00000}";
                scoreToSubmit = (int)highScore;
            }
            LeaderboardController.Instance.SubmitScore(scoreToSubmit);
            
        } 
    }