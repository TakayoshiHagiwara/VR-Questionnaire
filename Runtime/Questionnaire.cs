// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (Toyohashi University of Technology)
// Created: 2020/11/22
// Summary: Questionnaire class. Contains information of the questionnaire.
// -----------------------------------------------------------------------

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questionnaire
{
    public GameObject questionnaireObj;
    public QType qType;
    public float score;
    public Component[] components;
    public List<Image> handleImages;

    /// <summary>
    /// Constructor for Questionnaire
    /// </summary>
    /// <param name="obj">GameObject of the questionnaire</param>
    /// <param name="questionnaireType">Select the questionnaire type: Slider (Visual analog scale) or Toggle (Likert scale)</param>
    public Questionnaire(GameObject obj, QType questionnaireType)
    {
        questionnaireObj = obj;
        qType = questionnaireType;

        switch (questionnaireType)
        {
            case QType.Slider:
                components = obj.GetComponentsInChildren(typeof(Slider));
                this.score = components[0].GetComponent<Slider>().value;
                break;
            case QType.Toggle:
                components = obj.GetComponentsInChildren(typeof(Toggle));
                this.score = Mathf.Ceil(components.Length / 2f);
                break;
        }

        handleImages = obj.GetComponentsInChildren<Image>().ToList();
        handleImages = handleImages.Where(img => img.name == "Handle" || img.name == "Checkmark").ToList();
    }

    /// <summary>
    /// Questionnaire type
    /// Slider (Visual analog scale) or Toggle (Likert scale)
    /// </summary>
    public enum QType
    {
        Slider = 0,
        Toggle = 1
    };
}
