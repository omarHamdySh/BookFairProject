using UnityEngine;

public interface IScrollable: ITraverseable
{
    int getObjectIndex();

    void setObjectIndex(int _objectIndex);

    float getScrollSpeed();

    bool getLandStatus();
    void move(Vector3 destination, float duration);
    void move(Vector3 destination, float duration, bool visibilty);

}