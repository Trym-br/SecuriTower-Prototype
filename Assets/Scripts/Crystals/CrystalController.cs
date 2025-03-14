using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;


public class CrystalController : MonoBehaviour
{
    [SerializeField] private Vector3[] InputPoints;
    [SerializeField] private Vector3[] OutputPoints;

    private void SendLaser()
    {
        
    }
    private void OnDrawGizmos()
    {
        foreach (var Point in InputPoints)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(transform.TransformPoint(Point), new Vector3(0.3f, 0.3f, 0.3f)); 
        }
        foreach (var Point in OutputPoints)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.TransformPoint(Point), new Vector3(0.3f, 0.3f, 0.3f)); 
        }
    }
}
