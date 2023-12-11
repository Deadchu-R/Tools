using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Text;


public class SimpleTypeWriterEffect : MonoBehaviour
{
    #region Required Components

    [Header("Required Components:")] [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    #endregion

    #region TypeWriter Effect Variables

    [Header("TypeWriter Effect Variables:")] [SerializeField]
    private float typingSpeed = 0.05f;

    #endregion

    #region Events

    [Header("Events:")] [SerializeField] UnityEvent onFinishedText = new UnityEvent();
    [SerializeField] UnityEvent onLetterTyped = new UnityEvent();
    [SerializeField] UnityEvent onWordTyped = new UnityEvent();

    #endregion

    private Coroutine _typeTextCoroutine;
    private bool _isCoroutineRunning = false;
    private string _fullText;
    private StringBuilder _textBuilder = new StringBuilder();
    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();




    /// <summary>
    ///  will set the text to the textMeshProUGUI component
    /// </summary>
    /// <param name="page">taking info from page.Text</param>
    public void SetText(Page page)
    {
        if (page == null)
        {
            Debug.LogError("Page is null, seems you forgot to set the page in the inspector or to the Page ScriptableObject");
            return;
        }
        
        textMeshProUGUI.text = "";
        _fullText = page.Text;
       if (!_isCoroutineRunning)  _typeTextCoroutine = StartCoroutine(TypeText()); // in order to prevent multiple coroutines running at the same time, will check if any coroutine is running
 
    }

    /// <summary>
    ///  will check if finished typing the text then invoke the OnFinishedText event
    /// </summary>
    /// <returns></returns>
    private bool IsFinishedText() // will check if type-writer finished to write the text
    {
        if (string.Equals(textMeshProUGUI.text, _fullText))
        {
            _isCoroutineRunning = false;
            onFinishedText.Invoke();
            return true;
        }
        else return false;
    }

    /// <summary>
    /// will stop the typing Coroutine immediately
    /// </summary>
    public void StopText()
    {
        if (!_isCoroutineRunning || _typeTextCoroutine == null) return;
        _isCoroutineRunning = false;
        StopCoroutine(_typeTextCoroutine);
        _typeTextCoroutine = null;
    }

    /// <summary>
    /// Skipping the setting Finale Text
    /// </summary>
    public void SetFinaleText()
    {
        textMeshProUGUI.text = _fullText;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    ///  will start the typing Coroutine
    /// Invoking the OnLetterTyped event each letter typed
    /// Invoking the OnWordTyped event each word typed
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypeText()
    {
        if (_isCoroutineRunning)
        {
            Debug.LogWarning("Coroutine is already running.");
            yield break;
        }
        
        _isCoroutineRunning = true;
        _textBuilder.Length = 0;
        int characterCount = _fullText.Length;

        for (int i = 0; i < characterCount; i++)
        {
            onLetterTyped?.Invoke();
            if (_fullText[i] == ' ')
            {
                onWordTyped?.Invoke();
            }
            _textBuilder.Append(_fullText[i]);
            textMeshProUGUI.text = _textBuilder.ToString();
            if (IsFinishedText()) yield break;
            for (int j = 0; j < (int)(typingSpeed * 60); j++)
            {
                yield return _waitForEndOfFrame;
            }
        }

    }
}