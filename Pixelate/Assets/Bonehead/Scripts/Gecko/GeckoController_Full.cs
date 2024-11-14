using System.Collections;
using UnityEngine;

public class GeckoController_Full : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] bool rootMotionEnabled;
    [SerializeField] bool idleBobbingEnabled;
    [SerializeField] bool headTrackingEnabled;
    [SerializeField] bool eyeTrackingEnabled;
    [SerializeField] bool tailSwayEnabled;
    [SerializeField] bool legSteppingEnabled;
    bool legIKEnabled;

    void Awake()
    {
        StartCoroutine(LegUpdateCoroutine());
    }

    [SerializeField] LegStepper_Full frontLeftLegStepper;
    [SerializeField] LegStepper_Full frontRightLegStepper;
    [SerializeField] LegStepper_Full backLeftLegStepper;
    [SerializeField] LegStepper_Full backRightLegStepper;

    // Only allow diagonal leg pairs to step together
    IEnumerator LegUpdateCoroutine()
    {
        while (true)
        {
            while (!legSteppingEnabled) yield return null;

            do
            {
                frontLeftLegStepper.TryMove();
                backRightLegStepper.TryMove();
                yield return null;
            } while (backRightLegStepper.Moving || frontLeftLegStepper.Moving);

            do
            {
                frontRightLegStepper.TryMove();
                backLeftLegStepper.TryMove();
                yield return null;
            } while (backLeftLegStepper.Moving || frontRightLegStepper.Moving);
        }
    }
}
