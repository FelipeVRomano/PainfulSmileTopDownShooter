using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Map Bounds")]
    [SerializeField] Vector2 _mapBoundsStart;
    [SerializeField] Vector2 _mapBoundsEnd;

    public Vector2 MapBoundsStart => _mapBoundsStart;
    public Vector2 MapBoundsEnd => _mapBoundsEnd;
}
