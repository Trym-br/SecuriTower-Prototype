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
        OutputLastHit = new GameObject[OutputPoints.Length];
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

    // public void move()
    // {
    //     foreach (var point in OutputPoints)
    //     {
    //         SendLaser(point, false);
    //     }
    // }

    private void Update()
    {
        
    }

    public void OnLaserInteract(Vector3 Direction, bool Attach)
    {
        // Handle Inputs
        int Index = Array.IndexOf(InputPoints, -Direction);
        print("Direction: " + Direction/2 + " / " + InputPoints[0] + " | " + Index);
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
            ActiveOutputs[0] = true;
            for (int i = 0; i < OutputPoints.Length; i++)
            {
                print(i + " outputPoint: " + OutputPoints[i]);
                SendLaser(OutputPoints[i], true);
            }
        }
        else if(ActiveOutputs[0])
        {
            for (int i = 0; i < OutputPoints.Length; i++)
            {
                print(i + " outputPoint: " + OutputPoints[i]);
                SendLaser(OutputPoints[i], false);
                OnLaserInteract(OutputPoints[i],false);
                ActiveOutputs[i] = false; 
            }
        }
    }

    private void SendLaser(Vector3 OutputPoint, bool isON)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position + OutputPoint,
            OutputPoint,
            Mathf.Infinity
        );
        // print("Found an object - distance: " + hit.distance);
        if (hit.distance > 0)
        {
            // print("Laser blocked?: " + OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)] != null + " / " + hit.rigidbody.gameObject != OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)]);
            if (OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)] != null && hit.rigidbody.gameObject != OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)])
            {
                print("Laser blocked: ");
                print("saved object: " + OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)].name);
                OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)].GetComponent<CrystalController>().OnLaserInteract(OutputPoint, false);
            }
            Debug.DrawLine(transform.position + OutputPoint, hit.point, Color.green);
            // print("Index of OutputPoint " + Array.IndexOf(OutputPoints, OutputPoint));
            // print("current val: " + OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)]);
            // print("current hit: " + hit.rigidbody.gameObject);
            // print("current hit: " + hit.rigidbody.gameObject.name);
            if (hit.rigidbody.gameObject.CompareTag("Crystal"))
            {
                print("Activating laser again");
                hit.rigidbody.gameObject.GetComponent<CrystalController>().OnLaserInteract(OutputPoint, isON);
                OutputLastHit[Array.IndexOf(OutputPoints, OutputPoint)] = hit.rigidbody.gameObject;
            }
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
