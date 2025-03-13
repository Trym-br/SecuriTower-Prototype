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
    [SerializeField] private GameObject[] OutputLastHit;
    // [SerializeField] private Func<bool>[] crystalConditionals;
    public Func<bool>[] crystalConditionals;
    [SerializeField] private bool[] ActiveInputs;
    [SerializeField] private bool[] ActiveOutputs;
    // [SerializeField] private Dictionary<TKey, TValue>[] ActiveInputs;
    // [SerializeField] private Dictionary<Index, Bool>[] ActiveOutputs;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        ActiveInputs = new bool[InputPoints.Length];
        ActiveOutputs = new bool[OutputPoints.Length];
        Array.Fill(ActiveInputs, false);
        Array.Fill(ActiveOutputs, false);
        crystalConditionals = new Func<bool>[]
        {
            // () => if(Direction is in InputPoints) ActiveOutputs[i] = true;
            // () => if(InputPoints.Contains(InputPoints[0]) && InputPoints[1] == InputPoints[2])
            // () =>
            // {
            //     if (ActiveInputs[0])
            //     {
            //         ActiveOutputs[0] = true;
            //         return true;
            //     }
            //     else
            //     {
            //         return false;
            //     }
            // }
        };
    }

    private void Update()
    {
        
    }

    public void OnLaserInteract(Vector3 Direction, bool Attach)
    {
        // Handle Inputs
        int Index = Array.IndexOf(InputPoints, Direction/2);
        // print("Direction: " + Direction/2 + " / " + InputPoints[0] + " | " + Index);
        if (Index != -1)
        {
            ActiveInputs[Index] = Attach;
        }
    }

    private void CrystalLogic()
    {
        bool clearFlag = true;
        foreach (var input in ActiveInputs)
        {
            if (!input)
            {
                clearFlag = false;
            }
        }

        if (clearFlag)
        {
            /*Array.Fill(ActiveOutputs, true);*/
            foreach (var outputPoint in OutputPoints)
            {
                SendLaser(outputPoint);
            }
        }
    }

    private void SendLaser(Vector3 OutputPoint)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position + OutputPoint,
            OutputPoint,
            Mathf.Infinity
        );
        // print("Found an object - distance: " + hit.distance);
        if (hit.distance > 0)
        {
            Debug.DrawLine(transform.position + OutputPoint, hit.point, Color.green);
            OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)] = hit.rigidbody.gameObject;
        }
        else
        {
            OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)].GetComponent<CrystalController>().OnLaserInteract(OutputPoint, false);
        }
        Debug.DrawRay(transform.position + OutputPoint, OutputPoint, Color.red);
        // hit.rigidbody.gameObject.GetComponent<CrystalController>().OnLaserInteract(Vector2.left, isON);
    }

    private void FixedUpdate()
    {
       CrystalLogic(); 
    }

    // Debug Draw input and output points
    private void OnDrawGizmos()
    {
        foreach (var Point in InputPoints)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(transform.position + Point, new Vector3(0.3f, 0.3f, 0.3f)); 
        }
        foreach (var Point in OutputPoints)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.position + Point, new Vector3(0.3f, 0.3f, 0.3f)); 
        }
    }
}
