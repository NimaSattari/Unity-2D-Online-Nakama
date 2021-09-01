using DG.Tweening;
using UnityEngine;

public class DoTweenActions : MonoBehaviour
{
    [SerializeField] Vector3 targetLocation = Vector3.zero;
    [SerializeField] Vector3 targetSize = Vector3.zero;
    [SerializeField] Vector3 targetRotation = Vector3.zero;
    [SerializeField] float animationDuration = 1f;
    [SerializeField] Ease animationEase = Ease.Linear;
    [SerializeField] AnimationType animationType = AnimationType.Move;

    enum AnimationType
    {
        Move,
        Rotate,
        Scale,
        MoveAndScale,
        MoveAndRotate
    }

    public void DoAnimation()
    {
        DOTween.RewindAll();
        if (animationType == AnimationType.Move)
        {
            transform.DOLocalMove(targetLocation, animationDuration).SetEase(animationEase).SetAutoKill(false).SetRecyclable(true);
        }
        else if (animationType == AnimationType.Rotate)
        {
            transform.DORotate(targetRotation, animationDuration).SetEase(animationEase).SetAutoKill(false).SetRecyclable(true);
        }
        else if (animationType == AnimationType.Scale)
        {
            transform.DOScale(targetSize, animationDuration).SetEase(animationEase).SetAutoKill(false).SetRecyclable(true);
        }
        else if (animationType == AnimationType.MoveAndScale)
        {
            DOTween.Sequence().SetAutoKill(false).SetRecyclable(true)
                .Append(transform.DOLocalMove(targetLocation, animationDuration).SetEase(animationEase))
                .Join(transform.DOScale(targetSize, animationDuration).SetEase(animationEase));
        }
        else if (animationType == AnimationType.MoveAndRotate)
        {
            DOTween.Sequence().SetAutoKill(false).SetRecyclable(true)
                .Append(transform.DOLocalMove(targetLocation, animationDuration).SetEase(animationEase))
                .Join(transform.DORotate(targetRotation, animationDuration).SetEase(animationEase));
        }
    }
}