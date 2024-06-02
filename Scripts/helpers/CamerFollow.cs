using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // Цель, за которой будет следить камера
    public Vector3 offset = new Vector3(0f, 2f, 5f); // Смещение камеры относительно цели

    // Метод LateUpdate вызывается после выполнения всех обновлений кадра
    void LateUpdate()
    {
        if (target != null)
        {
            // Вычисляем позицию, куда должна переместиться камера (позади цели)
            Vector3 targetPosition = target.position + target.forward * offset.z + target.up * offset.y;
            // Плавно перемещаем камеру к новой позиции
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
            // Направляем камеру в противоположном направлении относительно цели
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
        }
    }
}
