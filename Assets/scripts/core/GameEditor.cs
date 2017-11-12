using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameEditor : MonoBehaviour 
{
  public Camera RaycastCamera;

  public Transform MapHolder;
  public Transform CameraHolder;
  public Transform EditorGridRaycastPlane;
  public Transform ObjectsGrid;
  public Transform FloorGrid;
  public Transform InstantiatedObjectsHolder;

  public GameObject Cursor;
  public GameObject FloorCursor;
  public GameObject EditorMapGridPrefab;
  public GameObject EditorGridFloorPrefab;
  public GameObject EditorGridObjectsPrefab;

  public CustomControlGroup ListOfItems;
  public HighlightableText TextItemPrefab;

  public InputField NewMapSizeX;
  public InputField NewMapSizeY;
  public InputField NewMapSizeZ;

  public Text PageCount;
  public Text FloorCount;

  public TextMesh PositionText;

  public CustomControlGroup ModesGroup;

  public GameObject NewMapWindow;

  float _gridSize = 0.5f;

  Vector3 _mousePos = Vector3.zero;
  Vector3 _worldPos = Vector3.zero;
  Vector3 _oldWorldPos = Vector3.zero;

  Texture2D[] _floorTextures;

  EditorLevel _map;

  List<string> _objectsPrefabsNamesList = new List<string>();
  List<string> _floorTextureNames = new List<string>();
  void Awake()
  {
    PrefabsManager.Instance.Initialize();

    for (int i = 1; i < PrefabsManager.Instance.Prefabs.Count; i++)
    {
      _objectsPrefabsNamesList.Add(PrefabsManager.Instance.Prefabs[i].name);
    }

    _listToUpdate = _floorTextureNames;

    _map = new EditorLevel(10, 2, 10);

    _cameraPos.Set(_map.MapX / 2.0f, 0.0f, _map.MapZ / 2.0f);
    CameraHolder.position = _cameraPos;

    UpdateFloorCounter();

    InitializeTextItems();
    LoadFloorTextures();
    CreateMapGrid();
    CreateFloorGrid();
    CreateObjectsGrid();

    UpdatePage(_floorTextureNames);
  }

  void CreateMapGrid()
  {
    for (int x = 0; x < _map.MapX; x++)
    {
      for (int z = 0; z < _map.MapZ; z++)
      {
        Instantiate(EditorMapGridPrefab, new Vector3(x, 0.0f, z), Quaternion.identity, MapHolder);
      }
    }
  }

  int _gridAreaSize = 12;
  void CreateFloorGrid()
  {    
    for (int x = -_gridAreaSize; x <= _gridAreaSize; x++)
    {
      for (int z = -_gridAreaSize; z <= _gridAreaSize; z++)
      {
        Instantiate(EditorGridFloorPrefab, new Vector3(x, 0.0f, z), Quaternion.identity, FloorGrid);
      }
    }
  }

  void CreateObjectsGrid()
  {    
    for (float x = -_gridAreaSize; x <= _gridAreaSize; x += _gridSize)
    {
      for (float z = -_gridAreaSize; z <= _gridAreaSize; z += _gridSize)
      {
        Instantiate(EditorGridObjectsPrefab, new Vector3(x, 0.0f, z), Quaternion.identity, ObjectsGrid);
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

    for (int i = 0; i < _floorTextures.Length; i++)
    {
      _floorTextureNames.Add(_floorTextures[i].name);
    }
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
      ht.MethodToCall.AddListener(HandleTextItem);
    }
  }

  float _previewObjectAngle = 0.0f;
  GameObject _previewObject;

  string _selectedListObject = string.Empty;
  void HandleTextItem(HighlightableControl control)
  {
    _selectedListObject = (control as HighlightableText).NormalText.text;

    if (_editorMode == 1)
    {
      if (_previewObject != null)
      {
        Destroy(_previewObject);
      }

      var prefab = PrefabsManager.Instance.FindPrefabByName(_selectedListObject);
      _previewObject = Instantiate(prefab, Cursor.transform.position, Quaternion.identity);
      _previewObject.name = "preview";
      _previewObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
  }

  void UpdateFloorCounter()
  {
    FloorCount.text = string.Format("{0}/{1}", _currentFloor + 1, _map.MapY);
  }

  int _currentFloor = 0;
  public void FloorUp()
  {
    _currentFloor++;

    if (_currentFloor > _map.MapY - 1)
    {
      _currentFloor = _map.MapY - 1;
    }

    if (_currentFloor != 0 && !FloorCursor.activeSelf)
    {
      FloorCursor.SetActive(true);
    }

    UpdateFloorCounter();
  }

  public void FloorDown()
  {
    _currentFloor--;

    if (_currentFloor < 0)
    {
      _currentFloor = 0;
    }

    if (_currentFloor == 0)
    {
      FloorCursor.SetActive(false);
    }

    UpdateFloorCounter();
  }

  Vector3 _floorCursorPosition = Vector3.zero;
  Vector3 _floorCursorScale = Vector3.one;
  void UpdateFloorCursorPosition()
  {
    _floorCursorScale.Set(1.0f, (_currentFloor == 0) ? 1.0f : _currentFloor, 1.0f);
    _floorCursorPosition.Set(Cursor.transform.position.x, Cursor.transform.position.y, Cursor.transform.position.z);
    FloorCursor.transform.position = _floorCursorPosition;
    FloorCursor.transform.localScale = _floorCursorScale;
  }

  public void PreviousPage()
  {
    _currentPage--;

    if (_currentPage < 0)
    {
      _currentPage = 0;
    }

    UpdatePage(_listToUpdate);
  }

  public void NextPage()
  {
    _currentPage++;

    if (_currentPage > _totalPages)
    {
      _currentPage = _totalPages;
    }

    UpdatePage(_listToUpdate);
  }

  void UpdatePage(List<string> arrayOfItemNames)
  {
    foreach (var t in ListOfItems.Controls)
    {
      (t as HighlightableText).ResetStatus();
      (t as HighlightableText).NormalText.text = "";
      (t as HighlightableText).HighlightedText.text = "";
    }

    int indexStart = _currentPage * _itemsOnPage;
    int indexEnd = arrayOfItemNames.Count - indexStart;

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
      string s = arrayOfItemNames[i];
      (ListOfItems.Controls[itemsIndex] as HighlightableText).NormalText.text = s;
      (ListOfItems.Controls[itemsIndex] as HighlightableText).HighlightedText.text = s;
      itemsIndex++;
    }

    PageCount.text = string.Format("{0} / {1}", _currentPage + 1, _totalPages + 1);
  }

  Vector3 _cursorPositionClamped = Vector3.zero;
  RaycastHit _hitInfo;
  void Update()
  { 
    ControlCamera();

    _mousePos = Input.mousePosition;
    _mousePos.z = Camera.main.nearClipPlane;

    Ray r = RaycastCamera.ScreenPointToRay(_mousePos);
    int mask = LayerMask.GetMask("PlacementGrid");
    if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity, mask))
    {
      _cursorPositionClamped = _hitInfo.collider.transform.position;

      _cursorPositionClamped.x = Mathf.Clamp(_cursorPositionClamped.x, 0.0f, _map.MapX - 1);
      _cursorPositionClamped.z = Mathf.Clamp(_cursorPositionClamped.z, 0.0f, _map.MapZ - 1);

      Cursor.transform.position = _cursorPositionClamped;
      UpdateFloorCursorPosition();
    }

    DrawFloorDebugLines();

    PositionText.text = string.Format("{0}", Cursor.transform.position);

    ProcessPreviewObjectRotation();

    if (_previewObject != null)
    {
      _previewObject.transform.position = Cursor.transform.position;
      _previewObject.transform.localEulerAngles = new Vector3(0.0f, _previewObjectAngle, 0.0f);
    }

    bool condPlace = (_editorMode == 0) ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
    bool condRemove = (_editorMode == 0) ? Input.GetMouseButton(1) : Input.GetMouseButtonDown(1);

    if (condPlace && !EventSystem.current.IsPointerOverGameObject())
    {
      PlaceObject(_editorMode);
    }
    else if (condRemove && !EventSystem.current.IsPointerOverGameObject())
    {
      RemoveObject(_editorMode);
    }
  }

  RaycastHit _debugRayInfo;
  void DrawFloorDebugLines()
  {
    if (_currentFloor != 0)
    {
      int mask = LayerMask.GetMask("EditorMapGrid");
      int x = (int)Cursor.transform.position.x;
      int y = (int)Cursor.transform.position.y;
      int z = (int)Cursor.transform.position.z;
      Vector3 center = new Vector3(x, y - 0.5f, z);
      if (Physics.BoxCast(center, new Vector3(0.4f, 0.1f, 0.4f), Vector3.down, out _debugRayInfo, Quaternion.identity, Mathf.Infinity, mask))
      {
        Debug.DrawRay(new Vector3(x, y, z), Vector3.down * _currentFloor, Color.green, 1.0f);
      }
    }
  }

  void ProcessPreviewObjectRotation()
  {
    if (_editorMode == 1)
    {
      if (Input.GetKeyDown(KeyCode.E))
      {
        _previewObjectAngle += 45.0f;
      }
      else if (Input.GetKeyDown(KeyCode.Q))
      {
        _previewObjectAngle -= 45.0f;
      }
    }
  }

  void RemoveObject(int editorMode)
  {
    if (editorMode == 0)
    {
      RemoveFloor();
    }
    else if (editorMode == 1)
    {
      RemoveMapObject();
    }
  }

  void PlaceObject(int editorMode)
  {
    if (editorMode == 0)
    {
      PlaceFloor();
    }
    else if (editorMode == 1)
    {
      PlaceMapObject();
    }
  }

  RaycastHit _objectPlacementInfo;
  void PlaceMapObject()
  { 
    int mask = LayerMask.GetMask("EditorMapObject");
    Vector3 center = new Vector3(Cursor.transform.position.x, -1.0f, Cursor.transform.position.z);
    if (Physics.BoxCast(center, new Vector3(0.4f, 0.1f, 0.4f), Vector3.up, out _objectPlacementInfo, Quaternion.identity, Mathf.Infinity, mask))
    {
      Debug.Log("Occupied by " + _objectPlacementInfo.collider);
      return;
    }

    var go = Instantiate(_previewObject);
    go.transform.parent = InstantiatedObjectsHolder;

    int layer = LayerMask.NameToLayer("EditorMapObject");
    Util.SetGameObjectLayer(go, layer, true);

    go.layer = layer;
  }

  void RemoveMapObject()
  {
    Ray r = RaycastCamera.ScreenPointToRay(_mousePos);
    int mask = LayerMask.GetMask("EditorMapObject");
    if (Physics.Raycast(r.origin, r.direction, out _objectPlacementInfo, Mathf.Infinity, mask))          
    {
      Destroy(_objectPlacementInfo.collider.transform.parent.gameObject);
    }

    /*
    int mask = LayerMask.GetMask("EditorMapObject");
    Vector3 center = new Vector3(Cursor.transform.position.x, -1.0f, Cursor.transform.position.z);
    if (Physics.BoxCast(center, new Vector3(0.4f, 0.1f, 0.4f), Vector3.up, out _objectPlacementInfo, Quaternion.identity, Mathf.Infinity, mask))
    {
      Debug.Log(_objectPlacementInfo.collider);
    }
    */
  }

  string _lastTextureName = string.Empty;
  RaycastHit _placementHitInfo;
  void PlaceFloor()
  {
    int x = (int)Cursor.transform.position.x;
    int y = (int)Cursor.transform.position.y;
    int z = (int)Cursor.transform.position.z;

    FloorBehaviour fb;

    int mask = LayerMask.GetMask("EditorMapFloor");
    Vector3 center = new Vector3(x, y - 0.5f, z);
    if (Physics.BoxCast(center, new Vector3(0.4f, 0.1f, 0.4f), Vector3.up, out _placementHitInfo, Quaternion.identity, Mathf.Infinity, mask))
    {
      if (_map.Level[x, y, z].Texture1Name.Equals(_selectedListObject))
      {
        return;
      }

      fb = _placementHitInfo.collider.GetComponentInParent<FloorBehaviour>();
      Destroy(fb.gameObject);
    }

    /*
    Ray r = RaycastCamera.ScreenPointToRay(_mousePos);
    int mask = LayerMask.GetMask("PlacementGrid") | LayerMask.GetMask("EditorMapGrid");
    if (Physics.Raycast(r.origin, r.direction, out _placementHitInfo, Mathf.Infinity, ~mask))          
    {
      if (_map.Level[x, y, z].Texture1Name.Equals(_selectedListObject))
      {
        return;
      }

      fb = _placementHitInfo.collider.GetComponentInParent<FloorBehaviour>();
      Destroy(fb.gameObject);
    }
    */

    _map.Level[x, y, z].Texture1Name = _selectedListObject;
    _lastTextureName = _selectedListObject;

    var go = PrefabsManager.Instance.InstantiatePrefab("floor-template", Cursor.transform.position, Quaternion.identity);
    go.transform.parent = InstantiatedObjectsHolder;
    go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    Util.SetGameObjectLayer(go, LayerMask.NameToLayer("EditorMapFloor"), true);
    fb = go.GetComponent<FloorBehaviour>();
    fb.Init(_map.Level[x, y, z]);
  }

  void RemoveFloor()
  {
    int x = (int)Cursor.transform.position.x;
    int y = (int)Cursor.transform.position.y;
    int z = (int)Cursor.transform.position.z;

    int mask = LayerMask.GetMask("EditorMapFloor");
    Vector3 center = new Vector3(x, y - 0.5f, z);
    if (Physics.BoxCast(center, new Vector3(0.4f, 0.1f, 0.4f), Vector3.up, out _placementHitInfo, Quaternion.identity, Mathf.Infinity, mask))      
    {
      FloorBehaviour fb = _placementHitInfo.collider.GetComponentInParent<FloorBehaviour>();
      Destroy(fb.gameObject);
    }
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

    _cameraPos.x = Mathf.Clamp(_cameraPos.x, 0.0f, _map.MapX);
    _cameraPos.z = Mathf.Clamp(_cameraPos.z, 0.0f, _map.MapZ);

    _gridBlockPos.x = (int)_cameraPos.x;
    _gridBlockPos.y = _currentFloor;
    _gridBlockPos.z = (int)_cameraPos.z;

    FloorGrid.position = _gridBlockPos;
    ObjectsGrid.position = _gridBlockPos;

    CameraHolder.position = _cameraPos;
    EditorGridRaycastPlane.position = _cameraPos;
  }

  List<string> _listToUpdate;

  int _editorMode = 0;
  public void SetEditorMode(int mode)
  {
    _editorMode = mode;
    _currentPage = 0;

    if (_editorMode == 0)
    {
      if (_previewObject != null)
      {
        Destroy(_previewObject);
      }

      _totalPages = _floorTextureNames.Count / _itemsOnPage;

      ObjectsGrid.gameObject.SetActive(false);
      FloorGrid.gameObject.SetActive(true);

      _listToUpdate = _floorTextureNames;

      UpdatePage(_listToUpdate);
    }
    else
    {
      _totalPages = _objectsPrefabsNamesList.Count / _itemsOnPage;

      ObjectsGrid.gameObject.SetActive(true);
      FloorGrid.gameObject.SetActive(false);

      _listToUpdate = _objectsPrefabsNamesList;

      UpdatePage(_listToUpdate);
    }

    ListOfItems.Controls[0].Select();
  }

  public void ShowNewMapWidnow()
  {
    NewMapWindow.SetActive(true);
  }

  public void CloseNewMapWindow()
  {
    NewMapWindow.SetActive(false);
  }

  public void NewMapHandler()
  {
    if (NewMapSizeX.text.Length == 0)
    {
      Debug.LogWarning("Please enter map size X!");
      return;
    }

    if (NewMapSizeY.text.Length == 0)
    {
      Debug.LogWarning("Please enter map size Y!");
      return;
    }

    if (NewMapSizeZ.text.Length == 0)
    {
      Debug.LogWarning("Please enter map size Z!");
      return;
    }

    int mapX = int.Parse(NewMapSizeX.text);
    int mapY = int.Parse(NewMapSizeY.text);
    int mapZ = int.Parse(NewMapSizeZ.text);

    _map = new EditorLevel(mapX, mapY, mapZ);

    UpdateFloorCounter();

    DestroyChildren(InstantiatedObjectsHolder);
    DestroyChildren(ObjectsGrid);
    DestroyChildren(MapHolder);
    DestroyChildren(FloorGrid);

    // Instantiate call with parent transform argument makes object parent with preserving world position.
    // E.g. if we have object1 at (2, 0) and object2 at (-2, 0) after parenting object2 to object1
    // it will have (-4, 0) since its world position is recalculated relative to parent.
    // Our grids are created fromm -N to N around (0, 0), so we need to reset their previous positions
    // before creating new map, and then move their holders in Update() with camera.

    ObjectsGrid.position = Vector3.zero;
    FloorGrid.position = Vector3.zero;

    CreateMapGrid();
    CreateFloorGrid();
    CreateObjectsGrid();

    _cameraPos.Set(_map.MapX / 2.0f, 0.0f, _map.MapZ / 2.0f);
    CameraHolder.position = _cameraPos;

    CloseNewMapWindow();
  }

  void DestroyChildren(Transform t)
  {
    int childCount = t.childCount;
    for (int i = 0; i < childCount; i++)
    {
      Destroy(t.GetChild(i).gameObject);
    }
  }
}
