using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Variables

    private             Data                data                    = new Data();

    [SerializeField]    GameEvents          events                  = null;

    [SerializeField]    Animator            timerAnimtor            = null;
    [SerializeField]    TextMeshProUGUI     timerText               = null;
    [SerializeField]    Color               timerHalfWayOutColor    = Color.yellow;
    [SerializeField]    Color               timerAlmostOutColor     = Color.red;
    private             Color               timerDefaultColor       = Color.white;

    private             List<AnswerData>    PickedAnswers           = new List<AnswerData>();
    private             List<int>           FinishedQuestions       = new List<int>();
    private             int                 currentQuestion         = 0;

    private             int                 timerStateParaHash      = 0;

    private             IEnumerator         IE_WaitTillNextRound    = null;
    private             IEnumerator         IE_StartTimer           = null;


    private             bool                IsFinished
       
    {
        get
        {
            return (FinishedQuestions.Count < 10) ? false : true;
            //return (FinishedQuestions.Count < data.Questions.Length) ? false : true;
        }
    }

    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called when the object becomes enabled and active
    /// </summary>
    void OnEnable()
    {

        events.UpdateQuestionAnswer += UpdateAnswers;
    }
    /// <summary>
    /// Function that is called when the behaviour becomes disabled
    /// </summary>
    void OnDisable()
    {
        
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    /// <summary>
    /// Function that is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// </summary>
    void Awake()
    {

        events.CurrentFinalScore = 0;
    }
    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        
        timerDefaultColor = timerText.color;
        LoadQuestions();

        timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }
    

    #endregion

    /// <summary>
    /// Function that is called to update new selected answer.
    /// </summary>
    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (data.Questions[currentQuestion].Type == AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    /// <summary>
    /// Function that is called to clear PickedAnswers list.
    /// </summary>
    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }

    /// <summary>
    /// Function that is called to display new question.
    /// </summary>
    void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        } else { Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method."); }

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    /// <summary>
    /// Function that is called to accept picked answers and check/display the result.
    /// </summary>
    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);

        UpdateScore((isCorrect) ? data.Questions[currentQuestion].AddScore : -data.Questions[currentQuestion].AddScore);

        if (IsFinished)
        {
            AudioManager.Instance.StopSound("GameMusic");
            SetHighscore();
        }

        var type 
            = (IsFinished) 
            ? UIManager.ResolutionScreenType.Finish 
            : (isCorrect) ? UIManager.ResolutionScreenType.Correct 
            : UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen(type, data.Questions[currentQuestion].AddScore);
        }

        AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if (type != UIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    public void Back()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        var score = PlayerPrefs.GetInt(GameUtility.GemScore);
        Debug.Log(score);
        var highscore = events.CurrentFinalScore;
        Debug.Log(highscore);
    }

    #region Timer Methods

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);

                timerAnimtor.SetInteger(timerStateParaHash, 2);
                break;
            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimtor.SetInteger(timerStateParaHash, 1);
                break;
        }
    }
    IEnumerator StartTimer()
    {
        var totalTime = data.Questions[currentQuestion].Timer;
        var timeLeft = totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0)
        {
            timeLeft--;

            AudioManager.Instance.PlaySound("CountdownSFX");

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }
    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    #endregion

    /// <summary>
    /// Function that is called to check currently picked answers and return the result.
    /// </summary>
    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Function that is called to compare picked answers with question correct answers.
    /// </summary>
    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = data.Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    /// <summary>
    /// Function that is called to load data from the xml file.
    /// </summary>
    void LoadQuestions()
    {
        //Object[] objs = Resources.LoadAll("Questions", typeof(Question));
        //_questions = new Question[objs.Length];
        //for (int i = 0; i < objs.Length; i++)
        //{
        //    _questions[i] = (Question)objs[i];
        //}
        var path = Path.Combine(GameUtility.FileDir, GameUtility.FileName + events.level + ".xml");
        Debug.Log(PlayerPrefs.GetString("difficult"));
        int hardLevel = events.level + 1;
        if (PlayerPrefs.GetString("difficult") == "hard") 
            path = Path.Combine(GameUtility.FileDir, GameUtility.FileName + hardLevel + ".xml");
        Debug.Log(path);
        data = Data.Fetch(path);
    }

    /// <summary>
    /// Function that is called restart the game.
    /// </summary>
    public void RestartGame()
    {
        AudioManager.Instance.PlaySound("GameMusic");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Function that is called to quit the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Function that is called to set new highscore if game score is higher.
    /// </summary>
    private void SetHighscore()
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        var score = PlayerPrefs.GetInt(GameUtility.GemScore);
        int final = score + events.CurrentFinalScore;
        PlayerPrefs.SetInt(GameUtility.Final, final);
        if (highscore < final)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, final);
        }
        string path = Directory.GetCurrentDirectory() + "/HighScore.txt";
        if (System.IO.File.Exists(path))
        {
            string[] a = File.ReadAllLines(path);
            int[] Hscore = new int[3];
                for (int i = 0; i < a.Length; ++i)
                {
                    Hscore[i] = int.Parse(a[i]);
                }
                if (Hscore[2] < final) Hscore[2] = final;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = i + 1; j < 3; j++)
                    {
                        if (Hscore[i] < Hscore[j])
                        {
                            // Nếu arr[i] > arr[j] thì hoán đổi giá trị của arr[i] và arr[j]
                            int temp = Hscore[i];
                            Hscore[i] = Hscore[j];
                            Hscore[j] = temp;
                        }
                    }
                }
                string[] b = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    b[i] = Hscore[i].ToString();
                }

                System.IO.File.WriteAllLines(path, b);
        }
        
    }
    /// <summary>
    /// Function that is called update the score and update the UI.
    /// </summary>
    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;
        if (events.CurrentFinalScore < 0) events.CurrentFinalScore = 0;

        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }

    #region Getters

    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;
        //Debug.Log(randomIndex);
        return data.Questions[currentQuestion];
    }
    int GetRandomQuestionIndex()
    {
        var random = 0;
        //Debug.Log(data.Questions.Length);
        if (FinishedQuestions.Count < data.Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, data.Questions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    #endregion
}