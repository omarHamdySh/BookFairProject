public interface IScrollable
{

    void moveRight();
    void moveLeft();
    void moveUp();
    void moveDown();

    void onMove();

    void onLand();
    void onDepart();
}
