using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float startTime = 60f;
    [SerializeField] private bool countDown = true;
    [SerializeField] private bool startOnAwake = true;

    [Header("Display Settings")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private string timeFormat = "mm:ss";

    [Header("Events")]
    public UnityEvent onTimerComplete;

    private float currentTime;
    private bool isRunning;

    private void Awake()
    {
        currentTime = startTime;
        if (startOnAwake)
        {
            StartTimer();
        }
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (!isRunning) return;

        if (countDown)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                TimerComplete();
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime >= startTime)
            {
                TimerComplete();
            }
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        float timeToDisplay = Mathf.Max(0, currentTime);
        
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        switch (timeFormat.ToLower())
        {
            case "mm:ss":
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                break;
            case "m:ss":
                timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
                break;
            case "ss":
                timerText.text = string.Format("{0}", Mathf.FloorToInt(timeToDisplay));
                break;
            default:
                timerText.text = timeToDisplay.ToString("F2");
                break;
        }
    }

    private void TimerComplete()
    {
        isRunning = false;
        currentTime = countDown ? 0 : startTime;
        UpdateTimerDisplay();
        onTimerComplete?.Invoke();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
    }

    public void SetTime(float newTime)
    {
        startTime = newTime;
        currentTime = newTime;
        UpdateTimerDisplay();
    }
}