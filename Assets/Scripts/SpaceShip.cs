using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;

public class SpaceShip : MonoBehaviour
{
    public RaycastDirection[] rays;
    public float turningStep = 3f;
    public float speed = 0.01f;
    public int score = 0;
    public bool active = false;

    public bool callbackCalled = false;
    private NeuralNetwork brain;
    private Action<int> onFinish;

    public Vector<double> GetChromosome()
    {
        return brain.GetWeightsAndBias();
    }

    public void InitializeBrain(Vector<double> weightsAndBias)
    {
        brain = new NeuralNetwork(rays.Length, 8, 4);
        if (weightsAndBias != null)
            brain.SetWeightsAndBias(weightsAndBias);
    }

    public void StartEngine(Action<int> onFinishCallback)
    {
        onFinish = onFinishCallback;
        active = true;
        score = 0;
    }

    private Vector<double> _GetInputs()
    {
        return DenseVector.Build.Dense(rays.Length, i => rays[i].GetDistance());
    }

    private void _TurnLeft()
    {
        transform.Rotate(new Vector3(0, -turningStep, 0));
    }

    private void _TurnRight()
    {
        transform.Rotate(new Vector3(0, turningStep, 0));
    }

    private void _GoUp()
    {
        transform.Rotate(new Vector3(-turningStep, 0, 0));
    }

    private void _GoDown()
    {
        transform.Rotate(new Vector3(turningStep, 0, 0));
    }

    private void _Engine()
    {
        transform.position = transform.position + transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active) {
            if (other.gameObject.CompareTag("TunnelWalls") || other.gameObject.CompareTag("WinMark"))
            {
                active = false;
            }
            if (other.gameObject.CompareTag("TunnelPoint"))
            {
                score++;
            }
            if (other.gameObject.CompareTag("WinMark"))
            {
                score+= 10;
            }
        }
    }

    void Update()
    {
        if (active)
        {
            Vector<double> decision = brain.Think(_GetInputs());
            switch (decision.MaximumIndex())
            {
                case 0:
                    _GoDown();
                    break;
                case 1:
                    _GoUp();
                    break;
                case 2:
                    _TurnRight();
                    break;
                case 3:
                    _TurnLeft();
                    break;
                default:
                    break;
            }
            _Engine();
        }
        else
        {
            if (!callbackCalled)
            {
                callbackCalled = true;
                onFinish(score);
            }
        }
    }
}
