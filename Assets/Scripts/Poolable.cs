using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public string Pool { get => pool; set { pool = value; } }
    public int Id { get => id; set { id = value; } }
    public int LocalPoolId { get => localPoolId; set { localPoolId = value; } }
    public int GlobalPoolId { get => globalPoolId; set { globalPoolId = value; } }

    [SerializeField] private string pool;
    [SerializeField] private int id = -1;
    [SerializeField] private int localPoolId = -1;
    [SerializeField] private int globalPoolId = -1;
}
