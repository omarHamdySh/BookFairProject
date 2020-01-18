using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathDataSO", menuName = "DataManagers/PathData")]
public class PathDataSO : ScriptableObject
{
    #region Private Variables

    [Space, SerializeField, Header("Floors Scroll Speed"), Tooltip("Scrolling speed for Floors")]
    private float floorScrollSpeed = 0;

    [Space, SerializeField, Header("Bookcases Scroll Speed"), Tooltip("Scrolling speed for Bookcases")]
    private float bookcaseScrollSpeed = 0;

    [Space, SerializeField, Header("Shelves Scroll Speed"), Tooltip("Scrolling speed for Shelves")]
    private float shelfScrollSpeed = 0;

    [Space, SerializeField, Header("Books Scroll Speed"), Tooltip("Scrolling speed for Books")]
    private float bookScrollSpeed = 0;

    #endregion

    #region Getters/Setter

    public float FloorScrollSpeed { get => floorScrollSpeed; set => floorScrollSpeed = value; }
    public float BookcaseScrollSpeed { get => bookcaseScrollSpeed; set => bookcaseScrollSpeed = value; }
    public float BookScrollSpeed { get => bookScrollSpeed; set => bookScrollSpeed = value; }
    public float ShelfScrollSpeed { get => shelfScrollSpeed; set => shelfScrollSpeed = value; }

    #endregion
}
