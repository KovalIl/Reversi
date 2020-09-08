using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Chip : MonoBehaviour
{
    public static int Player1Color;
    public static int Player2Color;
    public static string Color1;
    public static string Color2;
    public static string Currcolor1;
    public static string Currcolor2;

    [SerializeField] private Material _player1Matereal;
    [SerializeField] private Material _player2Matereal;

    private const string ColorTagString = "<color=#{0}>||</color>";

    private GameManager _gameManager;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        Player1Color = MainMenuController.Chip1Value;
        Player2Color = MainMenuController.Chip2Value;
        _gameManager = GameManager.Instance;
        SetOriginalColor(Player1Color, _player1Matereal);
        SetOriginalColor(Player2Color, _player2Matereal);
        string color1 = GanaratePlayerColor(_player1Matereal.color);
        string color2 = GanaratePlayerColor(_player2Matereal.color);
        Color1 = color1;
        Color2 = color2;
        Currcolor1 = color1 + "Player1";
        Currcolor2 = color2 + "Player2";
    }

    private string GanaratePlayerColor(Color materealColor)
    {
        return string.Format(ColorTagString, ColorUtility.ToHtmlStringRGB(materealColor));
    }

    private void OnTriggerStay(Collider collider)
    {
        GameObject parent = collider.gameObject.transform.parent.gameObject;
        if (!parent)
        {
            return;
        }
        if (parent.layer == LayerMask.NameToLayer(_gameManager.Player1))
        {
            meshRenderer.material = _player1Matereal;
        }
        else if (parent.layer == LayerMask.NameToLayer(_gameManager.Player2))
        {
            meshRenderer.material = _player2Matereal;
        }
    }

    private void SetOriginalColor(int colorNumber, Material chipMaterial)
    {
        switch (colorNumber)
        {
            case 0:
                chipMaterial.color = Color.green;
                break;
            case 1:
                chipMaterial.color = Color.red;
                break;
            case 2:
                chipMaterial.color = Color.blue;
                break;
            case 3:
                chipMaterial.color = Color.white;
                break;
            case 4:
                chipMaterial.color = Color.black;
                break;
            case 5:
                chipMaterial.color = Color.gray;
                break;
        }
    }
}