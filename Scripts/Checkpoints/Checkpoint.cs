//using UnityEngine;

//public class Checkpoint : MonoBehaviour
//{
//    private TrackCheckpoints trackCheckpoints;
//    private MeshRenderer meshRenderer;
//    public bool isCorrect = false;

//    public void Awake()
//    {
//        meshRenderer = GetComponent<MeshRenderer>();
//    }

//    //public void Start()
//    //{
//    //    this.isCorrect = false;
//    //}

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.TryGetComponent<CarController>(out CarController car))
//        {
//            trackCheckpoints.AgentThroughCheckpoint(this.transform, other.transform);
//        }
//    }

//    // Метод для установки ссылки на граф чекпоинтов
//    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
//    {
//        this.trackCheckpoints = trackCheckpoints;
//    }

//    // Метод для отображения чекпоинта
//    public void Show()
//    {
//        this.isCorrect = true;
//        Debug.Log($"I am burn! {this.isCorrect}");
//        Debug.Log($"I am burn2! {this.gameObject.name}");

//        meshRenderer.enabled = true;
//    }

//    // Метод для скрытия чекпоинта
//    public void Hide()
//    {
//        Debug.Log($"NOT burn! {meshRenderer}");
//        this.isCorrect = false;
//        meshRenderer.enabled = false;
//    }

//    // Метод для получения Transform объекта чекпоинта
//    public Transform GetTransform()
//    {
//        return transform;
//    }
//}


using System.Collections.Generic;
using UnityEngine;

public class CheckpointNode : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    public MeshRenderer meshRenderer;
    public bool isCorrect = false;
    public List<CheckpointNode> connectedNodes;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        connectedNodes = new List<CheckpointNode>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarController>(out CarController car))
        {
            trackCheckpoints.AgentThroughCheckpoint(this.transform, other.transform);
        }
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }

    public void Show()
    {
        this.isCorrect = true;
        //Debug.Log($"I am burn! {this.isCorrect}");
        Debug.Log($"I am burn! {this.gameObject.name}");

        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        //Debug.Log($"NOT burn! {meshRenderer}");
        this.isCorrect = false;
        meshRenderer.enabled = false;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
