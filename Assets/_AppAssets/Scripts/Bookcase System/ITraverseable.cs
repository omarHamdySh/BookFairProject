using UnityEngine;

public interface ITraverseable
{
    void move(Vector3 destination, float duration);

    void onMoving();

    void onLand();

    void onDeparture();

}
