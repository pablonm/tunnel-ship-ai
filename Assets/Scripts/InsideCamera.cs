using UnityEngine;

public class InsideCamera : MonoBehaviour
{
    private Transform target;
    private int targetScore = 0;

    public float smoothSpeed = 0.125f;

    private void GetShip()
    {
        SpaceShip[] ships = FindObjectsOfType<SpaceShip>();
        if (ships.Length > 0)
        {
            SpaceShip maxShip = ships[0];
            foreach (SpaceShip ship in ships)
                if (ship.active && ship.score > maxShip.score)
                    maxShip = ship;
            if (maxShip.score < targetScore || maxShip.score > targetScore + 1)
            {
                target = maxShip.transform;
                targetScore = maxShip.score;
            }
        }
    }

    private void FixedUpdate()
    {
        GetShip();
        if (target)
        {
            Vector3 desiredPosition = target.position - target.transform.forward;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(target);
        }
    }
}
