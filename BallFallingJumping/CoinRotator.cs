using DG.Tweening;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    public int bonusNum;


    void OnTriggerEnter(Collider collision)
    {

        switch (bonusNum)
        {
            case 1://монетка
                GameController.Instance.bonus+=1 * GameController.Instance.x2bonus;
                GameController.Instance.AddStars(1 * GameController.Instance.x2bonus);
                break;
            case 2://х2
                GameController.Instance.x2Timer = 10.0f;
                break;
            case 3://снежинка-замедление
                //GameController.Instance.isFreezed=
                GameController.Instance.freezeTimer = 15.0f;
                break;
            default:
                break;

        }

        SoundMng.Instance.PlaySound(SoundMng.SoundType.StarTouch);

        Destroy(gameObject);
    }

    private void Start()
    {
        transform.DOLocalMove(Vector3.up * 0.25f, 0.25f).SetRelative().SetLoops(-1, LoopType.Yoyo);
        transform.DOLocalRotate(Vector3.up * 360f, 0.5f, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}