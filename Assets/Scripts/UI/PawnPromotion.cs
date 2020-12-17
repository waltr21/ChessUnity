using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnPromotion : MonoBehaviour
{

    public GameObject PromotionCanvas;
    public Button Queen, Rook, Bishop, Knight;
    public Board BoardRef;
    // Start is called before the first frame update
    void Start()
    {
        Queen.onClick.AddListener(delegate { ButtonClicked(ButtonType.Queen); });
        Rook.onClick.AddListener(delegate { ButtonClicked(ButtonType.Rook); });
        Bishop.onClick.AddListener(delegate { ButtonClicked(ButtonType.Bishop); });
        Knight.onClick.AddListener(delegate { ButtonClicked(ButtonType.Knight); });
        ShowCanvas(false);
    }

    public void ShowCanvas(bool b)
    {
        PromotionCanvas.SetActive(b);
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
