//using System.Collections.Generic;
//using UnityEngine;

//public class AStarPathfinding
//{
//    public static List<Transform> FindShortestPath(Transform startCheckpoint, Transform targetCheckpoint)
//    {
//        // Создаем список для хранения открытых и закрытых узлов
//        List<Transform> openList = new List<Transform>();
//        HashSet<Transform> closedSet = new HashSet<Transform>();

//        // Создаем словарь для хранения предыдущего узла для каждого узла
//        Dictionary<Transform, Transform> cameFrom = new Dictionary<Transform, Transform>();

//        // Создаем словарь для хранения стоимости пути от начального узла до текущего
//        Dictionary<Transform, float> gScore = new Dictionary<Transform, float>();

//        // Инициализируем начальные значения
//        gScore[startCheckpoint] = 0f;

//        // Добавляем начальный узел в открытый список
//        openList.Add(startCheckpoint);

//        while (openList.Count > 0)
//        {
//            // Находим узел с наименьшей стоимостью пути из начального узла
//            Transform currentCheckpoint = null;
//            float lowestFScore = Mathf.Infinity;
//            foreach (Transform checkpoint in openList)
//            {
//                float fScore = gScore.ContainsKey(checkpoint) ? gScore[checkpoint] + HeuristicCostEstimate(checkpoint, targetCheckpoint) : Mathf.Infinity;
//                if (fScore < lowestFScore)
//                {
//                    lowestFScore = fScore;
//                    currentCheckpoint = checkpoint;
//                }
//            }

//            // Если достигли целевого узла, возвращаем кратчайший путь
//            if (currentCheckpoint == targetCheckpoint)
//            {
//                return ReconstructPath(cameFrom, currentCheckpoint);
//            }

//            // Перемещаем текущий узел из открытого списка в закрытый
//            openList.Remove(currentCheckpoint);
//            closedSet.Add(currentCheckpoint);

//            // Просматриваем всех соседей текущего узла
//            foreach (Transform neighbor in GetNeighbors(currentCheckpoint))
//            {
//                if (closedSet.Contains(neighbor))
//                {
//                    continue; // Пропускаем уже закрытые узлы
//                }

//                float tentativeGScore = gScore.ContainsKey(currentCheckpoint) ? gScore[currentCheckpoint] + DistanceBetween(currentCheckpoint, neighbor) : Mathf.Infinity;

//                if (!openList.Contains(neighbor))
//                {
//                    openList.Add(neighbor); // Добавляем соседа в открытый список, если его там нет
//                }
//                else if (tentativeGScore >= gScore[neighbor])
//                {
//                    continue; // Это не лучший путь
//                }

//                // Этот путь лучше. Запоминаем его
//                cameFrom[neighbor] = currentCheckpoint;
//                gScore[neighbor] = tentativeGScore;
//            }
//        }

//        // Если не удалось найти путь
//        return null;
//    }

//    // Функция для оценки стоимости оставшегося пути (эвристика)
//    private static float HeuristicCostEstimate(Transform current, Transform target)
//    {
//        return Vector3.Distance(current.position, target.position);
//    }

//    // Функция для вычисления расстояния между двумя узлами
//    private static float DistanceBetween(Transform nodeA, Transform nodeB)
//    {
//        return Vector3.Distance(nodeA.position, nodeB.position);
//    }

//    // Функция для восстановления пути из словаря cameFrom
//    private static List<Transform> ReconstructPath(Dictionary<Transform, Transform> cameFrom, Transform currentCheckpoint)
//    {
//        List<Transform> path = new List<Transform>();
//        while (cameFrom.ContainsKey(currentCheckpoint))
//        {
//            path.Add(currentCheckpoint);
//            currentCheckpoint = cameFrom[currentCheckpoint];
//        }
//        path.Reverse();
//        return path;
//    }

//    // Функция для получения соседей узла
//    private static List<Transform> GetNeighbors(Transform checkpoint)
//    {
//        List<Transform> neighbors = new List<Transform>();
//        CheckpointNode checkpointNode = checkpoint.GetComponent<CheckpointNode>();
//        if (checkpointNode != null)
//        {
//            foreach (var connectedCheckpoint in checkpointNode.connectedCheckpoints)
//            {
//                neighbors.Add(connectedCheckpoint.checkpointTransform);
//            }
//        }
//        return neighbors;
//    }
//}
