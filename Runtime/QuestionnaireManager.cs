// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (Toyohashi University of Technology)
// Created: 2020/11/22
// Summary: Manager for questionnaire.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireManager : MonoBehaviour
{
    // Set manager
    public ControllerInputManager controllerInputManager;
    private IControllerInput _controllerInput;
    public AnimationManager animationManager;

    // Set questionnaire object and info
    [SerializeField, Tooltip("You can attach a GameObject that has a Toggle or Slider as a child")]
    private List<GameObject> _questionnaireObjList = default;
    private List<Questionnaire> _questionnaires = new List<Questionnaire>();
    private Questionnaire _currentQuestionnaire;
    private int _questionnaireNumMax;

    private List<int> _questionnaireOrder = new List<int>();
    public List<int> QuestionnaireOrder { get { return _questionnaireOrder; } }
    private int _currentQuestionnaireNum;
    public int CurrentQuestionnaireNum { get { return _currentQuestionnaireNum; } }

    // Toggle settings
    private Toggle _activeToggle;
    private int _selectToggleNum;

    // Slider setting
    private float updateSliderValue;

    // Handle image color
    private Color selectedColor = new Color(255, 0, 0);
    private Color defaultColor = new Color(255, 255, 255);

    [SerializeField, Tooltip("If True, randomize the order of the questionnaires")]
    private bool _isRandomizeOrder;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Initialize questionnaire settings
    /// </summary>
    void Init()
    {
        // Set controller input interface and initialize animation manager
        _controllerInput = controllerInputManager.controllerInput;
        animationManager.Init(_questionnaireObjList);

        // Init and set questionnaire
        _questionnaireNumMax = _questionnaireObjList.Count;
        _currentQuestionnaireNum = 0;
        _selectToggleNum = 3;

        InitQuestionnaire();
        if (_questionnaires.Count == 0)
            return;

        // Generate the order in which questionnaires are presented. Default is to randomize the order.
        _questionnaireOrder = MakeQuestionnaireOrder(_questionnaireNumMax, _isRandomizeOrder);

        // Set first questionnaire
        animationManager.SetFirstQuestionnaire(_questionnaireOrder[_currentQuestionnaireNum]);
        _currentQuestionnaire = _questionnaires[_questionnaireOrder[_currentQuestionnaireNum]];

        // Set handle color active
        if(_currentQuestionnaire.qType == Questionnaire.QType.Toggle)
            SetHandleColor(_currentQuestionnaire.handleImages[_selectToggleNum], true);
        else
            SetHandleColor(_currentQuestionnaire.handleImages[0], true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateQuestionnaire();
    }

    /// <summary>
    /// Automatically set the questionnaire format from the GameObject set in the inspector
    /// </summary>
    void InitQuestionnaire()
    {
        foreach (GameObject obj in _questionnaireObjList)
        {
            if (obj.GetComponentsInChildren(typeof(Slider)).Length != 0)
            {
                _questionnaires.Add(new Questionnaire(obj, Questionnaire.QType.Slider));
            }
            else if (obj.GetComponentsInChildren(typeof(Toggle)).Length != 0)
            {
                _questionnaires.Add(new Questionnaire(obj, Questionnaire.QType.Toggle));
            }
        }
    }

    /// <summary>
    /// Generate the order of presentation of the questionnaire
    /// </summary>
    /// <param name="max">Maximum number of questionnaires</param>
    /// <param name="isShuffle">Optional: Whether to randamize or not</param>
    /// <returns>Questionnaire order</returns>
    List<int> MakeQuestionnaireOrder(int max, bool isShuffle = true)
    {
        List<int> questionnaireOrder = new List<int>();
        for (int i = 0; i < max; i++)
            questionnaireOrder.Add(i);

        if (isShuffle)
            questionnaireOrder = questionnaireOrder.OrderBy(order => Guid.NewGuid()).ToList();
        
        return questionnaireOrder;
    }

    /// <summary>
    /// Update questionnaire score and state
    /// </summary>
    void UpdateQuestionnaire()
    {
        if (_questionnaires.Count == 0)
            return;

        switch(_currentQuestionnaire.qType)
        {
            case Questionnaire.QType.Slider:
                foreach (Slider s in _currentQuestionnaire.components)
                {
                    if (_controllerInput.IsPressSelectButton())
                        UpdateSlider(s);
                    if (_controllerInput.IsLongPressSelectButton())
                        UpdateSlider(s, isFast: true);
                }
                break;

            case Questionnaire.QType.Toggle:
                if (_controllerInput.IsPressSelectButton())
                    UpdateToggle(_currentQuestionnaire);
                break;
        }

        // Process when the Next button is pressed
        // WARNING: If a button is pressed several times in short periods in quick, the animation may not be executed correctly.
        if (_controllerInput.IsPressNext() && _currentQuestionnaireNum < _questionnaireNumMax - 1)
        {
            _currentQuestionnaireNum++;
            ChangeQuestionnaireState(_currentQuestionnaireNum - 1, _currentQuestionnaireNum);
        }
        // Process when all questionnaires are completed
        else if (_controllerInput.IsPressNext() && _currentQuestionnaireNum == _questionnaireNumMax - 1)
        {
            // You can write the data output process and scene switching process, etc. here.
            Debug.Log("Complete!");
            foreach (Questionnaire result in _questionnaires)
                Debug.Log(result.score);
        }

        // Process when the "Back to previous survey" button is pressed (deprecated)
        // WARNING: If a button is pressed several times in short periods in quick, the animation may not be executed correctly.
        if (_controllerInput.IsPressPrevious() && _currentQuestionnaireNum > 0)
        {
            _currentQuestionnaireNum--;
            ChangeQuestionnaireState(_currentQuestionnaireNum + 1, _currentQuestionnaireNum);
        }
    }

    /// <summary>
    /// Change questionnaire state (Handle color, current questionnaire, animation)
    /// </summary>
    /// <param name="prevNum">Previous questionnaire num</param>
    /// <param name="nextNum">Next questionnaire num</param>
    void ChangeQuestionnaireState(int prevNum, int nextNum)
    {
        SetHandleColor(_currentQuestionnaire.handleImages, false);
        _currentQuestionnaire = _questionnaires[_questionnaireOrder[nextNum]];

        if (_currentQuestionnaire.qType == Questionnaire.QType.Toggle)
        {
            _selectToggleNum = (int)Mathf.Floor(_currentQuestionnaire.components.Length / 2);
            SetHandleColor(_currentQuestionnaire.handleImages[_selectToggleNum], true);
        }
        else
            SetHandleColor(_currentQuestionnaire.handleImages[0], true);

        animationManager.ChangeAnimationState(_questionnaireOrder[prevNum], _questionnaireOrder[nextNum]);
    }

    /// <summary>
    /// Update slider (visual analog scale) value
    /// </summary>
    /// <param name="s">Slider object</param>
    /// <param name="isFast">If True, changes the value of the slider quickly</param>
    void UpdateSlider(Slider s, bool isFast = false)
    {
        updateSliderValue = 0;

        if (s.wholeNumbers)
            updateSliderValue = 1;
        else
            updateSliderValue = 0.1f;

        if (isFast)
            updateSliderValue = updateSliderValue * 2;

        if (_controllerInput.Axis2d().x > 0 && s.value < s.maxValue)
            s.value += updateSliderValue;
        else if (_controllerInput.Axis2d().x < 0 && s.value > s.minValue)
            s.value -= updateSliderValue;

        _currentQuestionnaire.score = s.value;
    }

    /// <summary>
    /// Set current select handle color
    /// </summary>
    /// <param name="img">Target image to change color</param>
    /// <param name="isActive">If True, set to red; if False, set to white</param>
    void SetHandleColor(Image img, bool isActive)
    {
        if (isActive)
            img.color = selectedColor;
        else
            img.color = defaultColor;
    }

    /// <summary>
    /// Set handles color
    /// </summary>
    /// <param name="imgs">Target images to change color</param>
    /// <param name="isActive">If True, set to red; if False, set to white</param>
    void SetHandleColor(List<Image> imgs, bool isActive)
    {
        foreach (Image img in imgs)
            SetHandleColor(img, isActive);
    }

    /// <summary>
    /// Update toggle (Likert scale) value
    /// </summary>
    /// <param name="q">Current questionnaire</param>
    void UpdateToggle(Questionnaire q)
    {
        SetHandleColor(q.handleImages[_selectToggleNum], false);

        if (_controllerInput.Axis2d().x > 0 && _selectToggleNum < q.components.Length - 1)
            _selectToggleNum++;
        else if (_controllerInput.Axis2d().x < 0 && _selectToggleNum > 0)
            _selectToggleNum--;

        _activeToggle = (Toggle)q.components[_selectToggleNum];
        _activeToggle.isOn = true;
        SetHandleColor(q.handleImages[_selectToggleNum], true);

        q.score = _selectToggleNum + 1;
    }
}
