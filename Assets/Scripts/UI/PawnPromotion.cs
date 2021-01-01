using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class PawnPromotion : UISlider
{

    public GameObject PromotionCanvas;
    public Button Queen, Rook, Bishop, Knight;
    public Board BoardRef;
    public RectTransform PromotionTransform;

    // Start is called before the first frame update
    void Start()
    {
        Queen.onClick.AddListener(delegate { ButtonClicked(ButtonType.Queen); });
        Rook.onClick.AddListener(delegate { ButtonClicked(ButtonType.Rook); });
        Bishop.onClick.AddListener(delegate { ButtonClicked(ButtonType.Bishop); });
        Knight.onClick.AddListener(delegate { ButtonClicked(ButtonType.Knight); });

        SetTransformObject(PromotionTransform);
        ShowCanvas(false);
        PromotionTransform.localPosition = OffScreenPos;
    }

    private void ButtonClicked(ButtonType type)
    {
        BoardRef.PromotePawn(type);
        ShowCanvas(false);
    }
}

public enum ButtonType
{
    Queen,
    Rook, 
    Bishop,
    Knight
}
