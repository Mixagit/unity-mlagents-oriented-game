using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public List<CheckpointNode> checkpointGraph;
    public List<CheckpointNode> path;
    private Dictionary<Transform, int> nextCheckpointIndexDict;

    public class CarCheckPointEventArgs : EventArgs
    {
        public Transform carTransform { get; set; }
    }

    public event EventHandler<CarCheckPointEventArgs> OnPlayerCorrectCheckpoint;
    public event EventHandler<CarCheckPointEventArgs> OnPlayerWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList;
    [SerializeField] private Transform checkpointA;
    [SerializeField] private Transform checkpointB;

    private void Awake()
    {
        nextCheckpointIndexDict = new Dictionary<Transform, int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointIndexDict[carTransform] = 0;
        }

        Transform checkpointsTransform = transform.Find("Checkpoints");
        if (checkpointsTransform == null)
        {
            Debug.LogError("Checkpoints not found!");
            return;
        }

        CreateCheckpointGraph();

        int nextCheckpointIndex = nextCheckpointIndexDict[carTransformList[0]];

        if (checkpointGraph.Count > 0)
        {
            MeshRenderer firstCheckpointRenderer = checkpointGraph[0].meshRenderer;
            if (firstCheckpointRenderer != null)
            {
                Debug.Log($"First checkpoint: {checkpointGraph[nextCheckpointIndex].transform.gameObject.name}");
                checkpointGraph[nextCheckpointIndex].Show();
            }
            else
            {
                Debug.LogError("No checkpoints found!");
            }
        }

        path = FindPath(checkpointA, checkpointB);

        if (path.Count > 0)
        {
            Debug.Log("Found path:");
            foreach (var checkpoint in path)
            {
                Debug.Log(checkpoint.name);
                Debug.Log(checkpoint.isCorrect);

            }
        }
        else
        {
            Debug.LogError("No path found!");
        }
    }

    private void CreateCheckpointGraph()
    {
        checkpointGraph = new List<CheckpointNode>();

        var checkpoints = transform.Find("Checkpoints");
        foreach (Transform checkpointTransform in checkpoints)
        {
            CheckpointNode checkpointNode = checkpointTransform.GetComponent<CheckpointNode>();
            if (checkpointNode != null)
            {
                checkpointNode.SetTrackCheckpoints(this);

                foreach (Transform otherCheckpointTransform in checkpoints)
                {
                    if (otherCheckpointTransform != checkpointTransform && Vector3.Distance(checkpointTransform.position, otherCheckpointTransform.position) <= 15)
                    {
                        CheckpointNode otherCheckpointNode = otherCheckpointTransform.GetComponent<CheckpointNode>();
                        if (otherCheckpointNode != null)
                        {
                            checkpointNode.connectedNodes.Add(otherCheckpointNode);
                        }
                    }
                }

                checkpointGraph.Add(checkpointNode);
            }
        }
    }

    public void AgentThroughCheckpoint(Transform checkpointTransform, Transform carTransform)
    {
        // Получаем индекс текущего чекпоинта для автомобиля
        int currentIndex = nextCheckpointIndexDict[carTransform];
        CheckpointNode currentCheckpoint = path[currentIndex];

        Debug.Log($"currentCheckpoint: {currentCheckpoint.gameObject.name}");

        // Проверяем, является ли текущий чекпоинт правильным, сравнивая его с переданным чекпоинтом
        bool isCorrectCheckpoint = checkpointTransform == currentCheckpoint.transform;

        if (isCorrectCheckpoint)
        {
            path[currentIndex].Hide();

            // Логика для перехода к следующему чекпоинту
            if (currentIndex + 1 < path.Count) // Проверяем, не достигли ли мы конца списка
            {
                currentIndex++;
                nextCheckpointIndexDict[carTransform] = currentIndex; // Обновляем индекс следующего чекпоинта в словаре
                CheckpointNode nextCheckpoint = path[currentIndex];
                if (nextCheckpoint != null)
                    nextCheckpoint.Show();

                OnPlayerCorrectCheckpoint?.Invoke(this, new CarCheckPointEventArgs { carTransform = carTransform });
            }
            else
            {
                // Логика для обработки достижения последнего чекпоинта
                // Например, можно сбросить индекс чекпоинта и начать заново
                ResetCheckpoint(carTransform);
            }
        }
        else
        {
            // Если текущий чекпоинт - неправильный
            OnPlayerWrongCheckpoint?.Invoke(this, new CarCheckPointEventArgs { carTransform = carTransform });
        }
    }

    public CheckpointNode GetNextCheckpointPosition(Transform carTransform)
    {
        int nextIndex = nextCheckpointIndexDict[carTransform];
        return checkpointGraph[nextIndex];
    }

    public void ResetCheckpoint(Transform carTransform)
    {
        nextCheckpointIndexDict[carTransform] = 0;
        checkpointGraph[0].Show();
        for (int i = 1; i < checkpointGraph.Count; i++)
            checkpointGraph[i].Hide();
    }

    private List<CheckpointNode> FindPath(Transform checkpointA, Transform checkpointB)
    {
        List<CheckpointNode> path = new List<CheckpointNode>();

        // Находим узел чекпоинта A в графе
        CheckpointNode startNode = checkpointGraph.Find(node => node.transform == checkpointA);
        // Находим узел чекпоинта B в графе
        CheckpointNode endNode = checkpointGraph.Find(node => node.transform == checkpointB);

        if (startNode == null || endNode == null)
        {
            Debug.LogError("Start or end checkpoint not found!");
            return path;
        }

        // Инициализируем очередь для обхода
        Queue<CheckpointNode> queue = new Queue<CheckpointNode>();
        // Инициализируем словарь для отслеживания предков каждого узла
        Dictionary<CheckpointNode, CheckpointNode> parentMap = new Dictionary<CheckpointNode, CheckpointNode>();

        // Добавляем начальный узел в очередь
        queue.Enqueue(startNode);

        // Итеративный поиск в ширину
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            //Debug.Log($"Current node: {currentNode.transform.gameObject.name}");

            // Если достигнут конечный узел, строим путь
            if (currentNode == endNode)
            {
                //Debug.Log("End node reached. Building path...");
                // Построение пути, начиная с конечного узла и двигаясь к начальному узлу
                while (currentNode != startNode)
                {
                    path.Insert(0, currentNode);
                    //Debug.Log($"Adding {currentNode.transform.gameObject.name} to path");
                    currentNode = parentMap[currentNode];
                }
                path.Insert(0, startNode);
                //Debug.Log($"Adding start node {startNode.transform.gameObject.name} to path");
                break;
            }

            // Перебираем всех смежных узлов текущего узла
            foreach (var neighborNode in currentNode.connectedNodes)
            {
                //Debug.Log($"Neighbor node: {neighborNode.transform.gameObject.name}");
                // Если смежный узел еще не был посещен
                if (!parentMap.ContainsKey(neighborNode))
                {
                    //Debug.Log($"Enqueueing neighbor node: {neighborNode.transform.gameObject.name}");
                    // Добавляем смежный узел в очередь для дальнейшего обхода
                    queue.Enqueue(neighborNode);
                    // Устанавливаем текущий узел как предка смежного узла
                    parentMap[neighborNode] = currentNode;
                }
            }
        }

        if (path.Count == 0)
        {
            Debug.LogWarning("No path found!");
        }
        else
        {
            Debug.Log("Path found:");
            foreach (var checkpointNode in path)
            {
                Debug.Log(checkpointNode.transform.gameObject.name);
            }
        }

        return path;
    }
}