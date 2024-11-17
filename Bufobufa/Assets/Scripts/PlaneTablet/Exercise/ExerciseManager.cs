using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class JSONExercise
{
    public string header;
    public string reward;
    public string description;
    public string pathToAvatar;
}

public class ExerciseManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private List<Exercise> exercises = new List<Exercise>();
    private List<JSONExercise> JSONExercises = new List<JSONExercise>();

    private void Start()
    {
        exercises.Clear();

        string path = Application.streamingAssetsPath + "/" + "exercises.json";
        JSONExercises = JsonConvert.DeserializeObject<List<JSONExercise>>(File.ReadAllText(path));

        for (int i = 0; i < JSONExercises.Count; i++)
        {
            Instantiate(prefab, transform);
        }

        for (int i = 0; i < JSONExercises.Count; i++)
        {
            Exercise exercise;
            if (gameObject.transform.GetChild(i).TryGetComponent<Exercise>(out exercise))
            {
                exercise.Init((exercise) =>
                {
                    for (int j = 0; j < exercises.Count; j++)
                    {
                        if (exercises[j] != exercise)
                            exercises[j].ExpandExercise(false);
                    }
                }, JSONExercises[i]);
                exercise.ExpandExercise(false);

                exercises.Add(exercise);
            };
        }
    }
}
