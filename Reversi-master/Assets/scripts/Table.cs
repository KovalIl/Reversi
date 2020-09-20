using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private Color _fieldMatereal;
    [SerializeField] private GameObject _blackChip;
    [SerializeField] private GameObject _whiteChip;
    [SerializeField] private Camera _camera;

    private const int NextField = 1;
    private const int PreviousField = -1;
    private const int CurrentField = 0;
    private const float ChipPosY = 0.85f;
    private const string Player1 = "Player1";
    private const string Player2 = "Player2";

    private int _length;
    private bool _isStart = true;
    private Vector3 _blackChip1Pos = new Vector3(1f, 0.85f, 0f);
    private Vector3 _blackChip2Pos = new Vector3(0f, 0.85f, -1f);
    private Vector3 _whiteChip1Pos = new Vector3(0f, 0.85f, 0f);
    private Vector3 _whiteChip2Pos = new Vector3(1f, 0.85f, -1f);
    private List<GameObject> _toColor = new List<GameObject>();
    private List<GameObject> _toSupport = new List<GameObject>();
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        SpawnChipsOnStart();
        _length = _gameManager.Length - 1;
        Helper(_gameManager.Player1, _gameManager.Player2);
        _gameManager.CurrentPlayerColor = Chip.Currcolor1;
        _gameManager.PlayersScore();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (!Physics.Raycast(ray, out raycastHit))
            {
                return;
            }
            GameObject parent = raycastHit.collider.gameObject.transform.parent.gameObject;
            if (parent.layer != LayerMask.NameToLayer(_gameManager.CanSpawn))
            {
                return;
            }
            Vector3 hitPosition = raycastHit.collider.gameObject.transform.position;
            Vector3 position = new Vector3(hitPosition.x, ChipPosY, hitPosition.z);
            int posX = Mathf.FloorToInt(hitPosition.x) + 3;
            int posZ = (Mathf.FloorToInt(hitPosition.z) - 2) * -1;
            SpawnModel spawnChipModel = new SpawnModel(parent, position, posX, posZ, _gameManager.None);
            if (_gameManager.IsBlackTurn)
            {
                spawnChipModel.Chip = _blackChip;
                spawnChipModel.IsBlack = false;
                spawnChipModel.Current = _gameManager.Player1;
                spawnChipModel.Next = _gameManager.Player2;
                _gameManager.NumOfBlack++;
                _gameManager.CurrentPlayerColor = Chip.Currcolor2;
            }
            else
            {
                spawnChipModel.Chip = _whiteChip;
                spawnChipModel.IsBlack = true;
                spawnChipModel.Current = _gameManager.Player2;
                spawnChipModel.Next = _gameManager.Player1;
                _gameManager.NumOfWhite++;
                _gameManager.CurrentPlayerColor = Chip.Currcolor1;
            }
            ParametersToSpawn(spawnChipModel);
            _gameManager.PlayersScore();
        }
    }

    private void Helper(string currentLayer, string nextLayer)
    {
        for (int posX = 0; posX < _gameManager.Length; posX++)
        {
            for (int posZ = 0; posZ < _gameManager.Length; posZ++)
            {
                if (_gameManager.Field[posX, posZ].layer == LayerMask.NameToLayer(currentLayer))
                {
                    HelperModel helperModel = new HelperModel(posX, posZ, nextLayer, currentLayer);
                    helperModel.ForX = PreviousField;
                    helperModel.ForZ = CurrentField;
                    helperModel.ResultX = CurrentField;
                    helperModel.ResultZ = CurrentField;
                    Supprot(helperModel);
                    helperModel.ForZ = PreviousField;
                    Supprot(helperModel);
                    helperModel.ForX = CurrentField;
                    Supprot(helperModel);
                    helperModel.ForX = NextField;
                    helperModel.ResultX = _length;
                    Supprot(helperModel);
                    helperModel.ForZ = CurrentField;
                    Supprot(helperModel);
                    helperModel.ForZ = NextField;
                    helperModel.ResultZ = _length;
                    Supprot(helperModel);
                    helperModel.ForX = CurrentField;
                    helperModel.ResultX = CurrentField;
                    Supprot(helperModel);
                    helperModel.ForX = PreviousField;
                    Supprot(helperModel);
                }
                FieldHighlight(Color.cyan, _gameManager.CanSpawn);
            }
        }
        if (_toSupport.Count == 0 && _gameManager.Chips < (_gameManager.Length * _gameManager.Length))
        {
            Helper(nextLayer, currentLayer);
            _gameManager.IsBlackTurn = !_gameManager.IsBlackTurn;
        }
    }

    private void FieldHighlight(Color color, string layer)
    {
        for (int i = 0; i < _toSupport.Count; i++)
        {
            MeshRenderer meshRenderer = _toSupport[i].GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                return;
            }
            meshRenderer.material.color = color;
            _toSupport[i].layer = LayerMask.NameToLayer(layer);
        }
    }

    private void CheckFields(string noneLayer, string currentLayer, int posX, int posZ)
    {
        HelperModel helperModel = new HelperModel(posX, posZ, noneLayer, currentLayer);
        helperModel.ForX = PreviousField;
        helperModel.ForZ = CurrentField;
        helperModel.ResultX = CurrentField;
        helperModel.ResultZ = CurrentField;
        Fields(helperModel);
        helperModel.ForZ = PreviousField;
        Fields(helperModel);
        helperModel.ForX = CurrentField;
        Fields(helperModel);
        helperModel.ForX = NextField;
        helperModel.ResultX = _length;
        Fields(helperModel);
        helperModel.ForZ = CurrentField;
        Fields(helperModel);
        helperModel.ForZ = NextField;
        helperModel.ResultZ = _length;
        Fields(helperModel);
        helperModel.ForX = CurrentField;
        helperModel.ResultX = CurrentField;
        Fields(helperModel); 
        helperModel.ForX = PreviousField;
        Fields(helperModel);
    }

    private void Fields(HelperModel helperModel)
    {
        int x, z;
        x = helperModel.GetX();
        z = helperModel.GetZ();
        bool canClose = false;
        _toColor.Clear();
        while (true)
        {
            if (helperModel.isBorder(x, z))
            {
                break;
            }
            if (_gameManager.Field[x, z].layer == LayerMask.NameToLayer(helperModel.NoneLayer))
            {
                break;
            }
            else if (_gameManager.Field[x, z].layer == LayerMask.NameToLayer(helperModel.CurrentLayer))
            {
                canClose = true;
                break;
            }
            else
            {
                _toColor.Add(_gameManager.Field[x, z]);
            }
            x += helperModel.ForX;
            z += helperModel.ForZ;
        }
        ColorChange(canClose, helperModel.CurrentLayer);
    }

    private void Supprot(HelperModel helperModel)
    {
        int x, z;
        bool canClose;
        x = helperModel.GetX();
        z = helperModel.GetZ();
        canClose = false;
        while (true)
        {
            if (helperModel.isBorder(x, z))
            {
                break;
            }
            if (_gameManager.Field[x, z].layer == LayerMask.NameToLayer(helperModel.CurrentLayer))
            {
                break;
            }
            else if (_gameManager.Field[x, z].layer == LayerMask.NameToLayer(helperModel.NoneLayer))
            {
                canClose = true;
            }
            else
            {
                if (canClose)
                {
                    _toSupport.Add(_gameManager.Field[x, z]);
                }
                break;
            }
            x += helperModel.ForX;
            z += helperModel.ForZ;
        }
    }

    private void ParametersToSpawnChip(GameObject chip, GameObject chipParent, Vector3 pos, bool isBlackTurn, int posX, int posZ, string noneLayer, string currentlayer, string nextLayer)
    {
        FieldHighlight(_fieldMatereal, _gameManager.None);
        _toSupport.Clear();
        ChipSpawn(chip, _gameManager.ChipsParent, isBlackTurn, pos, chipParent);
        CheckFields(noneLayer, currentlayer, posX, posZ);
        Helper(nextLayer, currentlayer);
    }

    private void ParametersToSpawn(SpawnModel spawnModel)
    {
        FieldHighlight(_fieldMatereal, _gameManager.None);
        _toSupport.Clear();
        ChipSpawn(spawnModel.Chip, _gameManager.ChipsParent, spawnModel.IsBlack, spawnModel.Position, spawnModel.ChipParent);
        CheckFields(spawnModel.None, spawnModel.Current, spawnModel.PosX, spawnModel.PosZ);
        Helper(spawnModel.Next, spawnModel.Current);
    }

    private void ColorChange(bool canClose, string currentLayer)
    {
        if (!canClose)
        {
            return;
        }
        switch (currentLayer)
        {
            case Player1:
                _gameManager.NumOfBlack += _toColor.Count;
                _gameManager.NumOfWhite -= _toColor.Count;
                break;
            case Player2:
                _gameManager.NumOfWhite += _toColor.Count;
                _gameManager.NumOfBlack -= _toColor.Count;
                break;
        }
        for (int i = 0; i < _toColor.Count; i++)
        {
            _toColor[i].layer = LayerMask.NameToLayer(currentLayer);
        }
        _toColor.Clear();
    }

    private void SpawnChipsOnStart()
    {
        ChipSpawn(_whiteChip, _gameManager.ChipsParent, _gameManager.IsBlackTurn, _whiteChip1Pos, _whiteChip);
        ChipSpawn(_whiteChip, _gameManager.ChipsParent, _gameManager.IsBlackTurn, _whiteChip2Pos, _whiteChip);
        ChipSpawn(_blackChip, _gameManager.ChipsParent, _gameManager.IsBlackTurn, _blackChip1Pos, _whiteChip);
        ChipSpawn(_blackChip, _gameManager.ChipsParent, _gameManager.IsBlackTurn, _blackChip2Pos, _whiteChip);
        _isStart = false;
    }

    private void ChipSpawn(GameObject chip, Transform chipParent, bool isBlackTurn, Vector3 pos, GameObject chipLayer)
    {
        GameObject spawnChip = Instantiate(chip, pos, Quaternion.identity);
        spawnChip.transform.SetParent(chipParent);
        _gameManager.Chips++;
        _gameManager.IsBlackTurn = isBlackTurn;
        if (!_isStart)
        {
            chipLayer.layer = chip.layer;
        }
    }
}