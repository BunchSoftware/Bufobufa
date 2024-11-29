
using System;
using UnityEngine;
using UnityEngine.UI;

public enum TypeOfExerciseCompletion
{
    NotDone = 2,
    Run = 3,
    Done = 1
}

public class ExerciseGUI : MonoBehaviour
{
    [SerializeField] private RectTransform description;
    [SerializeField] private Button exerciseButton;
    [SerializeField] private Image checkMark;

    [Header("������ ���������� �������")]
    [SerializeField] private Button runButton;
    [SerializeField] private Button executionButton;
    [SerializeField] private Button doneButton;

    [Header("�������� �������� �������")]
    [SerializeField] private Text headerText;
    [SerializeField] private Text rewardText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image avatar;

    [Header("�������� ������ � �������")]
    [SerializeField] private Image background;
    [SerializeField] private Color colorNotDoneExerciseBackground;
    [SerializeField] private Color colorDoneExerciseBackground;
    [SerializeField] private Color colorRunExerciseBackground;

    private TypeOfExerciseCompletion currentExerciseCompletion = TypeOfExerciseCompletion.NotDone;
    private bool isExpandExercise = false;
    private Exercise exercise;

    public void Init(Action<ExerciseGUI, bool> ActionExercise, Exercise exercise)
    {
        executionButton.onClick.RemoveAllListeners();

        exerciseButton.onClick.AddListener(() =>
        {
            ActionExercise.Invoke(this, true);

            if (isExpandExercise)
                ExpandExercise(false);
            else
                ExpandExercise(true);
        });

        runButton.onClick.RemoveAllListeners();

        runButton.onClick.AddListener(() => 
        {
            SetExerciseCompletion(TypeOfExerciseCompletion.Run);
            ActionExercise.Invoke(this, false);
        });

        this.exercise = exercise;   

        headerText.text = exercise.header;
        rewardText.text = exercise.rewardText;
        descriptionText.text = exercise.description;
        avatar.sprite = exercise.avatar;
    }

    public void ExpandExercise(bool isExpandExercise)
    {
        this.isExpandExercise = isExpandExercise;
        if (description != null)
        {
            description.gameObject.SetActive(isExpandExercise);
        }
        else
            throw new System.Exception("������ ! �������� ������ Description");
    }

    public void SetExerciseCompletion(TypeOfExerciseCompletion exerciseCompletion)
    {
        currentExerciseCompletion = exerciseCompletion;

        switch(exerciseCompletion)
        {
            case TypeOfExerciseCompletion.NotDone:
                {
                    background.color = colorNotDoneExerciseBackground;
                    runButton.gameObject.SetActive(true);
                    doneButton.gameObject.SetActive(false);
                    executionButton.gameObject.SetActive(false);
                    checkMark.gameObject.SetActive(false);
                    break;
                }
            case TypeOfExerciseCompletion.Run:
                {
                    background.color = colorRunExerciseBackground;
                    runButton.gameObject.SetActive(false);
                    doneButton.gameObject.SetActive(false);
                    executionButton.gameObject.SetActive(true);
                    checkMark.gameObject.SetActive(false);
                    break;
                }
            case TypeOfExerciseCompletion.Done:
                {
                    background.color = colorDoneExerciseBackground;
                    runButton.gameObject.SetActive(false);
                    doneButton.gameObject.SetActive(true);
                    executionButton.gameObject.SetActive(false);
                    checkMark.gameObject.SetActive(true);
                    break;
                }
        }
    }

    public Exercise GetExercise()
    {
        return exercise;
    }

    public ExerciseReward DoneExercise(string messageCondition)
    {
        ExerciseReward exerciseReward = exercise.DoneExercise(messageCondition);
        SetExerciseCompletion(TypeOfExerciseCompletion.Done);
        return exerciseReward;
    }

    public TypeOfExerciseCompletion GetExerciseCompletion()
    {
        return currentExerciseCompletion;
    }
}
