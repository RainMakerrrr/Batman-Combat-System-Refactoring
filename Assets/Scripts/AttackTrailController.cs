using UnityEngine;

public class AttackTrailController : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightFoot;
    [SerializeField] private Transform _leftFoot;

    private void SetTrailParent(int type)
    {
        switch (type)
        {
            case 0:
                _trail.transform.parent = _rightHand;
                break;
            case 1:
                _trail.transform.parent = _leftHand;
                break;
            case 2:
                _trail.transform.parent = _rightFoot;
                break;
            case 3:
                _trail.transform.parent = _rightFoot;
                break;
            default:
                _trail.transform.parent = _leftFoot;
                break;
        }
    }
}
