using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private TextAsset JsonFile;
    private List<ExerciseGUI> exercisesGUI = new List<ExerciseGUI>();

    public Action<Exercise> GetCurrentExercise;
    public Action<ExerciseReward> GetExerciseReward;

    private ExerciseGUI currentExerciseGUI;

    private void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }

    private void Start()
    {
        List<Exercise> exercises = JsonConvert.DeserializeObject<List<Exercise>>(File.ReadAllText(AssetDatabase.GetAssetPath(JsonFile)));

        for (int i = 0; i < exercises.Count; i++)
        {
            if (exercises[i].pathToAvatar != null)
            {
                string imageName = exercises[i].pathToAvatar;
                string[] sub = exercises[i].pathToAvatar.Split("#");

                if (sub.Length == 2)
                {
                    imageName = sub[0];
                    string spriteName = sub[1];
                    exercises[i].avatar = Load(imageName, spriteName);
                }
            }
        }

        for (int i = 0; i < exercises.Count; i++)
        {
            prefab.name = $"Exercise {i}";
            Instantiate(prefab, transform);
        }

        for (int i = 0; i < exercises.Count; i++)
        {
            ExerciseGUI exercise;

            if (gameObject.transform.GetChild(i).TryGetComponent<ExerciseGUI>(out exercise))
            {
                exercise.Init((exercise, isExpandExercise) =>
                {
                    currentExerciseGUI = exercise;
                    GetCurrentExercise?.Invoke(currentExerciseGUI.GetExercise());

                    if (isExpandExercise)
                    {
                        for (int j = 0; j < exercises.Count; j++)
                        {
                            if (exercisesGUI[j] != exercise)
                                exercisesGUI[j].ExpandExercise(false);
                        }
                    }
                    else
                        Sort(exercise);
                }, exercises[i]);
                exercise.ExpandExercise(false);

                exercisesGUI.Add(exercise);
            };
        }
    }

    public void DoneCurrentExercise(string messageExercise)
    {
        GetExerciseReward?.Invoke(currentExerciseGUI.DoneExercise(messageExercise));
        Sort(currentExerciseGUI);
    }

    private void Sort(ExerciseGUI exercise)
    {
        for (int j = 0; j < exercisesGUI.Count; j++)
        {
            if (exercisesGUI[j] != exercise && exercisesGUI[j].GetExerciseCompletion() != TypeOfExerciseCompletion.Done)
                exercisesGUI[j].SetExerciseCompletion(TypeOfExerciseCompletion.NotDone);
            else if (exercisesGUI[j].GetExerciseCompletion() == TypeOfExerciseCompletion.Run)
                transform.GetChild(j).SetAsFirstSibling();
            else if (exercisesGUI[j].GetExerciseCompletion() == TypeOfExerciseCompletion.Done)
                transform.GetChild(j).SetAsLastSibling();
        }

        for (var i = 1; i < exercisesGUI.Count; i++)
        {
            for (var j = 0; j < exercisesGUI.Count - i; j++)
            {
                if (exercisesGUI[j].GetExerciseCompletion() < exercisesGUI[j + 1].GetExerciseCompletion())
                {
                    var temp = exercisesGUI[j];
                    exercisesGUI[j] = exercisesGUI[j + 1];
                    exercisesGUI[j + 1] = temp;
                }
            }
        }
    }

    private Sprite Load(string imageName, string spriteName)
    {
        UnityEngine.Object[] all = AssetDatabase.LoadAllAssetsAtPath(imageName);

        foreach (var s in all)
        {
            if (s.name == spriteName)
            {
                return s as Sprite;
            }
        }
        return null;
    }
}
