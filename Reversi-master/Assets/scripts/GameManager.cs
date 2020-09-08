using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public string CurrentPlayerColor;

    public int Length = 8;
    public bool IsBlackTurn = true;
    public GameObject FieldsParent;
    public GameObject[,] Field;
    public Transform ChipsParent;
    public List<Transform> Fields = new List<Transform>();
    public int NumOfBlack
    {
        get { return _numOfBlack; }
        set { _numOfBlack = value; }
    }
    public int NumOfWhite
    {
        get { return _numOfWhite; }
        set { _numOfWhite = value; }
    }
    public int Chips
    {
        get { return _chips; }
        set { _chips = value; }
    }

    public readonly string Player1 = "Player1";
    public readonly string Player2 = "Player2";
    public readonly string CanSpawn = "CanSpawn";
    public readonly string None = "None";

    [SerializeField] private GameObject _resultsPanel;
    [SerializeField] private GameObject _gameField;
    [SerializeField] private GameObject _eField;
    [SerializeField] private Text _winsText;
    [SerializeField] private Text _numbersOfWhiteChips;
    [SerializeField] private Text _numbersOfBlackChips;
    [SerializeField] private Text _currentColorChip;
    [SerializeField] private Button _exitButton;

    private int _numOfBlack;
    private int _numOfWhite;
    private int _chips;
    private int _numOfChips;

    private const int FirstField = 27;
    private const int SecondField = 28;
    private const int ThirdField = 35;
    private const int FourthField = 36;
    private const int StartPlayer1Count = 2;
    private const int StartPlayer2Count = 2;
    private const int NextPos = 1;
    private const string NothingWin = "Nothing";
    private const string MenuSceneName = "MainMenu";

    public override void Awake()
    {
        _numOfChips = Length * Length;
        CreatFields();
        NumOfBlack = StartPlayer1Count;
        NumOfWhite = StartPlayer2Count;
        if (_exitButton)
        {
            _exitButton.onClick.AddListener(ToExitButton);
        }
        PlayersScore();
    }

    private void Update()
    {
        if (NumOfWhite + NumOfBlack >= _numOfChips)
        {
            EndGame();
        }
    }
    private void CreatFields()
    {
        Vector3 startPosition = transform.position;
        float nextPosX = startPosition.x + NextPos;
        float nextPosZ = startPosition.z - NextPos;
        Field = new GameObject[Length, Length];
        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Length; x++)
            {
                CreatFieldTile(x, z, nextPosX, nextPosZ, startPosition);
                nextPosX++;
            }
            nextPosX = startPosition.x + NextPos;
            nextPosZ--;
        }
        FieldParamrters(FirstField, SecondField, ThirdField, FourthField);
    }

    private void CreatFieldTile(int x, int z, float nextPosX, float nextPosZ, Vector3 startPos)
    {
        Field[x, z] = Instantiate(_eField);
        Field[x, z].transform.parent = FieldsParent.transform;
        Field[x, z].transform.position = new Vector3(nextPosX, startPos.y, nextPosZ);
        Fields.Add(Field[x, z].transform);
    }

    private void FieldParamrters(int firstChip, int secondChip, int thirdChip, int fourthChip)
    {
        int player1Layer = LayerMask.NameToLayer(Player1);
        int player2Layer = LayerMask.NameToLayer(Player2);
        Fields[firstChip].gameObject.layer = player2Layer;
        Fields[secondChip].gameObject.layer = player1Layer;
        Fields[thirdChip].gameObject.layer = player1Layer;
        Fields[fourthChip].gameObject.layer = player2Layer;
    }

    private void ResultGame()
    {
        if (NumOfWhite > NumOfBlack)
        {
            _winsText.text = $"{Chip.Color2}Player2 Won";
        }
        else if (NumOfWhite < NumOfBlack)
        {
            _winsText.text = $"{Chip.Color1}Player1 Won";
        }
        else
        {
            _winsText.text = NothingWin;
        }
    }

    private void ToExitButton()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public void PlayersScore()
    {
        _currentColorChip.text = CurrentPlayerColor;
        _numbersOfBlackChips.text = $"{Chip.Color1}Player1: {NumOfBlack}";
        _numbersOfWhiteChips.text = $"{Chip.Color2}Player2: {NumOfWhite}";
    }

    public void EndGame()
    {
        _resultsPanel.SetActive(true);
        _gameField.SetActive(false);
        ResultGame();
    }
}