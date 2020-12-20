using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnPromotion : MonoBehaviour
{

    public GameObject PromotionCanvas;
    public Button Queen, Rook, Bishop, Knight;
    public Board BoardRef;
    private Vector3 NormalPos, OffScreenPos, DesiredPos;
    public RectTransform PromotionTransform;

    // Start is called before the first frame update
    void Start()
    {
        Queen.onClick.AddListener(delegate { ButtonClicked(ButtonType.Queen); });
        Rook.onClick.AddListener(delegate { ButtonClicked(ButtonType.Rook); });
        Bishop.onClick.AddListener(delegate { ButtonClicked(ButtonType.Bishop); });
        Knight.onClick.AddListener(delegate { ButtonClicked(ButtonType.Knight); });

        NormalPos = PromotionTransform.localPosition;
        OffScreenPos = new Vector3(NormalPos.x, 800f, NormalPos.z);
        PromotionTransform.localPosition = OffScreenPos;

        ShowCanvas(false);
    }

    private void Update()
    {
        PromotionTransform.localPosition = Vector3.Lerp(PromotionTransform.localPosition, DesiredPos, 5.0f * Time.deltaTime);
    }

    public void ShowCanvas(bool b)
    {
        if (b)
        {
            DesiredPos = NormalPos;
            return;
        }
        DesiredPos = OffScreenPos;
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
