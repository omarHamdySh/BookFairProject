public interface IScrollable
{
    void move();

    void onMoving();

    void onLand();

    void onDeparture();

    float getScrollSpeed();
}
