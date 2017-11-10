using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameEditor : MonoBehaviour 
{
  public Camera RaycastCamera;

  public Transform GridHolder;
  public Transform CameraHolder;
  public Transform EditorGridRaycastPlane;
  public Transform ObjectsGrid;
  public Transform FloorGrid;

  public GameObject Object;
  public GameObject Cursor;
  public GameObject EditorMapGridPrefab;
  public GameObject EditorGridFloorPrefab;
  public GameObject EditorGridObjectsPrefab;

  public CustomControlGroup ListOfItems;
  public HighlightableText TextItemPrefab;

  public Text PageCount;
  public TextMesh PositionText;

  public CustomControlGroup ModesGroup;

  public GameObject NewMapWindow;

  float _gridSize = 0.5f;

  Vector3 _mousePos = Vector3.zero;
  Vector3 _worldPos = Vector3.zero;
  Vector3 _oldWorldPos = Vector3.zero;

  int _mapSize = 25;

  Texture2D[] _floorTextures;

  void Awake()
  {
    InitializeTextItems();
    LoadFloorTextures();
    CreateMapGrid();
    CreateFloorGrid();
    CreateObjectsGrid();

    UpdatePage();
  }

  void CreateMapGrid()
  {
    for (int x = -_mapSize + 1; x < _mapSize - 1; x++)
    {
      for (int z = -_mapSize + 1; z < _mapSize - 1; z++)
      {
        Instantiate(EditorMapGridPrefab, new Vector3(x, 0.0f, z), Quaternion.identity, GridHolder);
      }
    }
  }

  public void ShowNewMapWidnow()
  {
    NewMapWindow.SetActive(true);
  }

  public void CloseNewMapWindow()
  {
    NewMapWindow.SetActive(false);
  }

  public void ClearMap()
  {
  }

  int _gridAreaSize = 12;
  void CreateFloorGrid()
  {    
    for (int x = -_gridAreaSize; x <= _gridAreaSize; x++)
    {
      for (int y = -_gridAreaSize; y <= _gridAreaSize; y++)
      {
        Instantiate(EditorGridFloorPrefab, new Vector3(x, 0.0f, y), Quaternion.identity, FloorGrid);
      }
    }
  }

  void CreateObjectsGrid()
  {    
    for (float x = -_gridAreaSize; x <= _gridAreaSize; x += _gridSize)
    {
      for (float y = -_gridAreaSize; y <= _gridAreaSize; y += _gridSize)
      {
        Instantiate(EditorGridObjectsPrefab, new Vector3(x, 0.0f, y), Quaternion.identity, ObjectsGrid);
      }
    }
  }

  int _totalPages = 0;
  int _currentPage = 0;
  int _currentItem = 0;
  int _itemsOnPage = 17;
  void LoadFloorTextures()
  {    
    _floorTextures = Resources.LoadAll<Texture2D>("textures");
    _totalPages = _floorTextures.Length / _itemsOnPage;
  }

  void InitializeTextItems()
  {
    for (int i = 0; i < _itemsOnPage; i++)
    {
      var go = Instantiate(TextItemPrefab, ListOfItems.transform);
      var ht = go.GetComponent<HighlightableText>();
      var rt = go.GetComponent<RectTransform>();
      rt.anchoredPosition = new Vector2(5.0f, -5.0f - 20.0f * i);
      ListOfItems.Controls.Add(ht);
      ht.ControlGroupRef = ListOfItems;
    }
  }

  public void PreviousPage()
  {
    _currentPage--;

    if (_currentPage < 0)
    {
      _currentPage = 0;
    }

    UpdatePage();
  }

  public void NextPage()
  {
    _currentPage++;

    if (_currentPage > _totalPages)
    {
      _currentPage = _totalPages;
    }

    UpdatePage();
  }

  void UpdatePage()
  {
    foreach (var t in ListOfItems.Controls)
    {
      (t as HighlightableText).NormalText.text = "";
      (t as HighlightableText).HighlightedText.text = "";
    }

    int indexStart = _currentPage * _itemsOnPage;
    int indexEnd = _floorTextures.Length - indexStart;

    if (indexEnd > _itemsOnPage)
    {
      indexEnd = indexStart + _itemsOnPage;
    }
    else
    {
      indexEnd = indexStart + indexEnd;
    }

    int itemsIndex = 0;
    for (int i = indexStart; i < indexEnd; i++)
    {
      var s = _floorTextures[i];
      (ListOfItems.Controls[itemsIndex] as HighlightableText).NormalText.text = s.name;
      (ListOfItems.Controls[itemsIndex] as HighlightableText).HighlightedText.text = s.name;
      itemsIndex++;
    }

    PageCount.text = string.Format("{0} / {1}", _currentPage + 1, _totalPages + 1);
  }

  RaycastHit _hitInfo;
  void Update()
  { 
    ControlCamera();

    //_gridSize = (_editorMode == 0) ? 1.0f : 0.5f;

    _mousePos = Input.mousePosition;
    _mousePos.z = Camera.main.nearClipPlane;

    Ray r = RaycastCamera.ScreenPointToRay(_mousePos);
    int mask = LayerMask.GetMask("EditorGrid");
    if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity, mask))
    {
      Cursor.transform.position = _hitInfo.collider.transform.position;
    }

    PositionText.text = string.Format("{0}", Cursor.transform.position);

    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    {
      //Debug.Log(Cursor.transform.position);
    }

    /*
    Ray r = RaycastCamera.ScreenPointToRay(_mousePos);
    int mask = LayerMask.GetMask("EditorGrid");
    if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity, mask))
    {
      Plane hPlane = new Plane(Vector3.up, Vector3.zero);
      float distance = 0; 
      Vector3 point = Vector3.zero;
      if (hPlane.Raycast(r, out distance))
      {
        point = r.GetPoint(distance);
        point.y = 0.0f;

        float d = Vector3.Distance(point, _oldWorldPos);

        if (d > _gridSize)
        {          
          float fx = point.x % _gridSize;
          float fz = point.z % _gridSize;

          point.x -= fx;
          point.z -= fz;

          point.x = Mathf.Clamp(point.x, -_mapSize + 1, _mapSize - 1);
          point.z = Mathf.Clamp(point.z, -_mapSize + 1, _mapSize - 1);

          Cursor.transform.position = point;
          _oldWorldPos = point;
        }
      }
    }

    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    {
      Vector3 pos = Cursor.transform.position;
      pos.y = 0.5f;

      var go = Instantiate(Object, pos, Quaternion.identity);
      go.SetActive(true);
    }
    else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
    {
      if (Physics.BoxCast(new Vector3(Cursor.transform.position.x, -1.0f, Cursor.transform.position.z), new Vector3(0.4f, 0.4f, 0.4f), Vector3.up, out _hitInfo, Quaternion.identity, Mathf.Infinity, ~mask))
      {
        if (_hitInfo.collider.name == "object(Clone)")
        {
          Destroy(_hitInfo.collider.gameObject);
        }
      }
    }
    */
  }

  Vector3 _gridBlockPos = Vector3.zero;
  Vector3 _cameraPos = Vector3.zero;
  float _cameraSpeed = 6.0f;
  float _actualCameraSpeed = 0.0f;
  float _cameraZoomSpeed = 0.3f;
  void ControlCamera()
  {
    if (Input.GetAxis("Mouse ScrollWheel") < 0)
    {
      RaycastCamera.orthographicSize += _cameraZoomSpeed;
    }
    else if (Input.GetAxis("Mouse ScrollWheel") > 0)
    {
      RaycastCamera.orthographicSize -= _cameraZoomSpeed;
    }

    RaycastCamera.orthographicSize = Mathf.Clamp(RaycastCamera.orthographicSize, 1.0f, 10.0f);

    if (Input.GetKey(KeyCode.LeftShift))
    {
      _actualCameraSpeed = _cameraSpeed * 2.0f;
    }
    else
    {
      _actualCameraSpeed = _cameraSpeed;
    }

    if (Input.GetKey(KeyCode.A))
    {
      _cameraPos.x -= Time.smoothDeltaTime * _actualCameraSpeed;
      _cameraPos.z += Time.smoothDeltaTime * _actualCameraSpeed;
    }
    else if (Input.GetKey(KeyCode.D))
    {
      _cameraPos.x += Time.smoothDeltaTime * _actualCameraSpeed;
      _cameraPos.z -= Time.smoothDeltaTime * _actualCameraSpeed;
    }
    else if (Input.GetKey(KeyCode.W))
    {
      _cameraPos.x += Time.smoothDeltaTime * _actualCameraSpeed;
      _cameraPos.z += Time.smoothDeltaTime * _actualCameraSpeed;
    }
    else if (Input.GetKey(KeyCode.S))
    {
      _cameraPos.x -= Time.smoothDeltaTime * _actualCameraSpeed;
      _cameraPos.z -= Time.smoothDeltaTime * _actualCameraSpeed;
    }

    _cameraPos.x = Mathf.Clamp(_cameraPos.x, -_mapSize, _mapSize);
    _cameraPos.z = Mathf.Clamp(_cameraPos.z, -_mapSize, _mapSize);

    _gridBlockPos.x = (int)_cameraPos.x;
    _gridBlockPos.z = (int)_cameraPos.z;

    FloorGrid.position = _gridBlockPos;
    ObjectsGrid.position = _gridBlockPos;

    CameraHolder.position = _cameraPos;
    EditorGridRaycastPlane.position = _cameraPos;
  }

  int _editorMode = 0;
  public void SetEditorMode(int mode)
  {
    if (mode == 0)
    {
      ObjectsGrid.gameObject.SetActive(false);
      FloorGrid.gameObject.SetActive(true);
    }
    else
    {
      ObjectsGrid.gameObject.SetActive(true);
      FloorGrid.gameObject.SetActive(false);
    }

    _editorMode = mode;
  }
}
