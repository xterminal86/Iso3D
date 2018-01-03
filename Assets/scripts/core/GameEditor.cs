using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SFB;

public class GameEditor : MonoBehaviour 
{
  public Camera RaycastCamera;

  public Transform MapGridHolder;
  public Transform CameraHolder;
  public Transform ObjectsPlacementGridHolder;
  public Transform FloorPlacementGridHolder;
  public Transform InstantiatedFloorHolder;
  public Transform InstantiatedObjectsHolder;
  public Transform LightNoShadows;
  public RectTransform CursorTextHolder;

  public GameObject Cursor;
  public GameObject ElevationCursor;
  public GameObject SelectedObjectCursor;
  public GameObject EditorMapGridPrefab;
  public GameObject EditorFloorPlacementGridPrefab;
  public GameObject EditorObjectsPlacementGridPrefab;

  public CustomControlGroup ListOfItems;
  public HighlightableText TextItemPrefab;

  public InputField MapNameInput;
  public InputField MapAuthorInput;
  public InputField MapCommentsInput;
  public Toggle DirectionalLightEnabled;

  public InputField NewMapSizeX;
  public InputField NewMapSizeY;
  public InputField NewMapSizeZ;

  public Text PageCount;
  public Text FloorCount;
  public Text InfoBarText;
  public Text CurrentLevelName;

  public TextMesh PositionText;
  public Text Text3D;

  public CustomControlGroup ModesGroup;

  public GameObject NewMapWindow;
  public GameObject MapPropertiesWindow;

  public ObjectPropertiesWindow SelectedObjectPropertiesWindow;

  float _objectsPlacementGridSize = 0.5f;

  Vector3 _mousePos = Vector3.zero;

  Texture2D[] _floorTextures;

  LevelBase _map;

  SerializedLevel _levelToSave = new SerializedLevel();

  List<string> _objectsPrefabsNamesList = new List<string>();
  List<string> _floorTextureNames = new List<string>();
  void Awake()
  {
    PrefabsManager.Instance.Initialize();

    for (int i = 4; i < PrefabsManager.Instance.Prefabs.Count; i++)
    {
      _objectsPrefabsNamesList.Add(PrefabsManager.Instance.Prefabs[i].name);
    }

    _listToUpdate = _floorTextureNames;

    _map = new LevelBase(10, 2, 10);
    _levelToSave.Init(10, 2, 10);

    _cameraZoom = RaycastCamera.transform.localPosition.z;

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
        Instantiate(EditorMapGridPrefab, new Vector3(x, 0.0f, z), Quaternion.identity, MapGridHolder);
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
        Instantiate(EditorFloorPlacementGridPrefab, new Vector3(x, 0.0f, z), Quaternion.identity, FloorPlacementGridHolder);
      }
    }
  }

  void CreateObjectsGrid()
  {    
    for (float x = -_gridAreaSize; x <= _gridAreaSize; x += _objectsPlacementGridSize)
    {
      for (float z = -_gridAreaSize; z <= _gridAreaSize; z += _objectsPlacementGridSize)
      {
        Instantiate(EditorObjectsPlacementGridPrefab, new Vector3(x, 0.0f, z), Quaternion.identity, ObjectsPlacementGridHolder);
      }
    }
  }

  int _totalPages = 0;
  int _currentPage = 0;
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
      _previewObject.name = prefab.name;
      _previewObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    else if (_editorMode == 3)
    {
      if (_previewObject != null)
      {
        Destroy(_previewObject);
      }

      var prefab = PrefabsManager.Instance.FindPrefabByName(GlobalConstants.WallTemplatePrefabName);
      _previewObject = Instantiate(prefab, Cursor.transform.position, Quaternion.identity);
      _previewObject.name = _selectedListObject;
      _previewObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      _previewObject.GetComponent<WallWorldObject>().ApplyTexture(_selectedListObject);
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

    if (_currentFloor != 0 && !ElevationCursor.activeSelf)
    {
      ElevationCursor.SetActive(true);
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
      ElevationCursor.SetActive(false);
    }

    UpdateFloorCounter();
  }

  Vector3 _floorCursorPosition = Vector3.zero;
  Vector3 _floorCursorScale = Vector3.one;
  void UpdateFloorCursorPosition()
  {
    _floorCursorScale.Set(1.0f, (_currentFloor == 0) ? 1.0f : _currentFloor, 1.0f);
    _floorCursorPosition.Set(Cursor.transform.position.x, Cursor.transform.position.y, Cursor.transform.position.z);
    ElevationCursor.transform.position = _floorCursorPosition;
    ElevationCursor.transform.localScale = _floorCursorScale;
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

    if (arrayOfItemNames == null)    
    {
      return;
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

  public bool EnteringText = false;

  Vector3 _cursorPositionClamped = Vector3.zero;
  RaycastHit _hitInfo;
  void Update()
  { 
    if (!EnteringText)
    {
      ControlCamera();
    }

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

    PositionText.text = string.Format("{0}\n{1}", Cursor.transform.position, _previewObjectAngle);
    Text3D.text = string.Format("{0} {1}", Cursor.transform.position, _previewObjectAngle);

    ProcessPreviewObjectRotation();

    if (_previewObject != null)
    {
      _previewObject.transform.position = Cursor.transform.position;
      _previewObject.transform.localEulerAngles = new Vector3(0.0f, _previewObjectAngle, 0.0f);
    }

    ProcessElevation();

    // "Paint" mode works only for floors and paths (_editorMode 0 and 4)
    bool condPlace = (_editorMode == 0 || _editorMode == 4) ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
    bool condRemove = (_editorMode == 0 || _editorMode == 4) ? Input.GetMouseButton(1) : Input.GetMouseButtonDown(1);

    if (!Input.GetKey(KeyCode.LeftShift))
    {
      if (condPlace && !EventSystem.current.IsPointerOverGameObject())
      {
        ProcessLMB(_editorMode);
      }
      else if (condRemove && !EventSystem.current.IsPointerOverGameObject())
      {
        ProcessRMB(_editorMode);
      }
    }
    else
    {
      if (_editorMode == 0 && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        FillFloor(_selectedListObject);
      }
      else if (_editorMode == 0 && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
      {
        FillFloor(string.Empty);
      }
    }
  }

  void ProcessElevation()
  {
    float wheel = Input.GetAxis("Mouse ScrollWheel");

    if (wheel > 0.0f)
    {
      FloorUp();
    }
    else if (wheel < 0.0f)
    {
      FloorDown();
    }
  }

  Queue<Int2> _fillQueue = new Queue<Int2>();
  void FillFloor(string replacement)
  {
    Vector3 placementPos = Vector3.zero;

    int cx = (int)Cursor.transform.position.x;
    int cy = (int)Cursor.transform.position.y;
    int cz = (int)Cursor.transform.position.z;

    string target = _map.Level[cx, cy, cz].Texture1Name;

    if (target.Equals(replacement))
    {
      return;
    }

    _fillQueue.Clear();

    ReplaceFloorTile(cx, cy, cz, replacement);
    _fillQueue.Enqueue(new Int2(cx, cz));

    int safeguard = 0;

    while (_fillQueue.Count != 0)
    {
      if (safeguard > 1000)
      {
        Debug.LogWarning("Terminated by safeguard!");
        break;
      }
        
      Int2 node = _fillQueue.Dequeue();

      int lx = node.X - 1;
      int hx = node.X + 1;
      int lz = node.Y - 1;
      int hz = node.Y + 1;

      if (lx >= 0 && _map.Level[lx, cy, node.Y].Texture1Name.Equals(target))
      {
        ReplaceFloorTile(lx, cy, node.Y, replacement);
        _fillQueue.Enqueue(new Int2(lx, node.Y));
      }

      if (lz >= 0 && _map.Level[node.X, cy, lz].Texture1Name.Equals(target))
      {
        ReplaceFloorTile(node.X, cy, lz, replacement);
        _fillQueue.Enqueue(new Int2(node.X, lz));
      }

      if (hx < _map.MapX && _map.Level[hx, cy, node.Y].Texture1Name.Equals(target))
      {
        ReplaceFloorTile(hx, cy, node.Y, replacement);
        _fillQueue.Enqueue(new Int2(hx, node.Y));
      }

      if (hz < _map.MapZ && _map.Level[node.X, cy, hz].Texture1Name.Equals(target))
      {
        ReplaceFloorTile(node.X, cy, hz, replacement);
        _fillQueue.Enqueue(new Int2(node.X, hz));
      }

      safeguard++;
    }
  }

  void ReplaceFloorTile(int x, int y, int z, string textureName)
  {
    if (string.IsNullOrEmpty(textureName))
    {
      RemoveFloorTile(x, y, z);
    }
    else
    {
      FloorBehaviour fb;

      int mask = LayerMask.GetMask("EditorMapFloor");
      Vector3 center = new Vector3(x, y - 0.5f, z);
      if (Physics.BoxCast(center, new Vector3(0.4f, 0.1f, 0.4f), Vector3.up, out _placementHitInfo, Quaternion.identity, Mathf.Infinity, mask))
      {
        fb = _placementHitInfo.collider.GetComponentInParent<FloorBehaviour>();
        if (fb != null)
        {
          Destroy(fb.gameObject);
        }
      }

      Vector3 placementPos = Vector3.zero;

      placementPos.Set(x, y, z);

      _map.Level[x, y, z].Texture1Name = _selectedListObject;
      _map.Level[x, y, z].SkipTransitionCheckHere = false;

      var go = PrefabsManager.Instance.InstantiatePrefab(GlobalConstants.FloorTemplatePrefabName, placementPos, Quaternion.identity);
      go.transform.parent = InstantiatedFloorHolder;
      go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      Util.SetGameObjectLayer(go, LayerMask.NameToLayer("EditorMapFloor"), true);
      fb = go.GetComponent<FloorBehaviour>();
      fb.Init(_map.Level[x, y, z]);
    }
  }

  void ProcessPreviewObjectRotation()
  {
    if (_editorMode == 1 || _editorMode == 3)
    {
      if (Input.GetKeyDown(KeyCode.E))
      {
        _previewObjectAngle += 45.0f;
      }
      else if (Input.GetKeyDown(KeyCode.Q))
      {
        _previewObjectAngle -= 45.0f;
      }

      _previewObjectAngle %= 360.0f;
    }
  }

  void ProcessRMB(int editorMode)
  {
    if (editorMode == 0)
    {
      RemoveFloor();
    }
    else if (editorMode == 1 || editorMode == 3 || editorMode == 5)
    {
      RemoveMapObject();
    }
    else if (_editorMode == 2)
    {
      SelectedObjectPropertiesWindow.DeselectObject();
      SelectedObjectCursor.SetActive(false);
    }
    else if (editorMode == 4)
    {
      UnsetPath();
    }
  }

  void ProcessLMB(int editorMode)
  {
    if (editorMode == 0)
    {
      PlaceFloor();
    }
    else if (editorMode == 1 || editorMode == 3)
    {
      PlaceMapObject();
    }
    else if (editorMode == 2)
    {
      TryToSelectObject();
    }
    else if (editorMode == 4)
    {      
      SetPath();
    }
    else if (editorMode == 5)
    {
      PlaceExitZone();
    }
  }

  void PlaceExitZone()
  {
    int cx = (int)Cursor.transform.position.x;
    int cy = (int)Cursor.transform.position.y;
    int cz = (int)Cursor.transform.position.z;

    var prefab = PrefabsManager.Instance.FindPrefabByName(GlobalConstants.ExitZonePrefabName);
    var go = Instantiate(prefab, new Vector3(cx, cy, cz), Quaternion.identity);
    go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    go.transform.parent = InstantiatedObjectsHolder;

    int layer = LayerMask.NameToLayer("EditorMapObject");
    Util.SetGameObjectLayer(go, layer, true);

    go.layer = layer;
  }

  WorldObjectBase _currentlySelectedObject;

  Vector3 _selectedObjectCursorPosition = Vector3.zero;
  void TryToSelectObject()
  {
    Ray r = RaycastCamera.ScreenPointToRay(_mousePos);
    int mask = LayerMask.GetMask("EditorMapObject");
    if (Physics.Raycast(r.origin, r.direction, out _objectPlacementInfo, Mathf.Infinity, mask))
    {
      // If we select different object of the same type we should "deselect" state of the currently selected one
      if (_currentlySelectedObject != null)
      {
        _currentlySelectedObject.Deselect();
      }

      _currentlySelectedObject = _objectPlacementInfo.collider.GetComponentInParent<WorldObjectBase>();
      SelectedObjectPropertiesWindow.SelectObject(_currentlySelectedObject);
      _selectedObjectCursorPosition.Set(_currentlySelectedObject.transform.position.x, _currentlySelectedObject.transform.position.y + 0.01f, _currentlySelectedObject.transform.position.z);
      SelectedObjectCursor.transform.position = _selectedObjectCursorPosition;
      SelectedObjectCursor.SetActive(true);
    }
    else
    {
      SelectedObjectPropertiesWindow.DeselectObject();
      SelectedObjectCursor.SetActive(false);

      _currentlySelectedObject = null;
    }
  }

  void SetPath()
  {    
    int x = (int)Cursor.transform.position.x;
    int y = (int)Cursor.transform.position.y;
    int z = (int)Cursor.transform.position.z;

    if (_map.Level[x, y, z].FloorBehaviourRef == null)
    {
      return;
    }

    //_map.Level[x, y, z].SkipTransitionCheckHere = true;
    _map.Level[x, y, z].AllowBlending = true;
    _map.Level[x, y, z].FloorBehaviourRef.PathMarker.SetActive(true);
  }

  void UnsetPath()
  {
    int x = (int)Cursor.transform.position.x;
    int y = (int)Cursor.transform.position.y;
    int z = (int)Cursor.transform.position.z;

    if (_map.Level[x, y, z].FloorBehaviourRef == null)
    {
      return;
    }

    //_map.Level[x, y, z].SkipTransitionCheckHere = false;
    _map.Level[x, y, z].AllowBlending = false;
    _map.Level[x, y, z].FloorBehaviourRef.PathMarker.SetActive(false);
  }

  RaycastHit _objectPlacementInfo;
  void PlaceMapObject()
  { 
    /*
    int mask = LayerMask.GetMask("EditorMapObject");
    Vector3 center = new Vector3(Cursor.transform.position.x, -1.0f, Cursor.transform.position.z);
    if (Physics.BoxCast(center, new Vector3(0.4f, 0.1f, 0.4f), Vector3.up, out _objectPlacementInfo, Quaternion.identity, 1.5f, mask))
    {
      Debug.Log("Occupied by " + _objectPlacementInfo.collider);
      return;
    }
    */

    var go = Instantiate(_previewObject);
    var wob = go.GetComponent<WorldObjectBase>();
    if (wob is WallWorldObject)
    {      
      (wob as WallWorldObject).TextureName = _previewObject.name;
    }
    else
    {
      wob.PrefabName = _previewObject.name;
    }

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
      var res = _objectPlacementInfo.collider.GetComponentInParent<WorldObjectBase>();
      Destroy(res.gameObject);
    }
  }

  RaycastHit _placementHitInfo;
  void PlaceFloor()
  {
    int x = (int)Cursor.transform.position.x;
    int y = (int)Cursor.transform.position.y;
    int z = (int)Cursor.transform.position.z;

    ReplaceFloorTile(x, y, z, _selectedListObject);
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
      _map.Level[x, y, z].Texture1Name = string.Empty;
    }
  }

  void RemoveFloorTile(int x, int y, int z)
  {
    int mask = LayerMask.GetMask("EditorMapFloor");
    Vector3 center = new Vector3(x, y - 0.5f, z);
    if (Physics.BoxCast(center, new Vector3(0.4f, 0.1f, 0.4f), Vector3.up, out _placementHitInfo, Quaternion.identity, Mathf.Infinity, mask))      
    {
      FloorBehaviour fb = _placementHitInfo.collider.GetComponentInParent<FloorBehaviour>();
      Destroy(fb.gameObject);
      _map.Level[x, y, z].Texture1Name = string.Empty;
      _map.Level[x, y, z].SkipTransitionCheckHere = false;
    }
  }

  Vector3 _gridBlockPos = Vector3.zero;
  Vector3 _cameraPos = Vector3.zero;
  float _cameraSpeed = 10.0f;
  float _actualCameraSpeed = 0.0f;
  float _cameraZoomSpeed = 10.0f;
  float _cameraRotationY = 45.0f;
  float _cameraRotationSpeed = 100.0f;
  float _cameraZoom = 0.0f;
  Vector3 _cameraZoomVector3 = Vector3.zero;
  void ControlCamera()
  {    
    if (Input.GetKey(KeyCode.F))
    {
      //RaycastCamera.orthographicSize += _cameraZoomSpeed;
      _cameraZoom += _cameraZoomSpeed * Time.deltaTime;
    }
    else if (Input.GetKey(KeyCode.R))
    {
      //RaycastCamera.orthographicSize -= _cameraZoomSpeed;
      _cameraZoom -= _cameraZoomSpeed * Time.deltaTime;
    }

    _cameraZoom = Mathf.Clamp(_cameraZoom, -30.0f, -1.0f);

    _cameraZoomVector3.z = _cameraZoom;
    RaycastCamera.transform.localPosition = _cameraZoomVector3;

    float cos = Mathf.Cos(_cameraRotationY * Mathf.Deg2Rad);
    float sin = Mathf.Sin(_cameraRotationY * Mathf.Deg2Rad);

    float strafeCos = Mathf.Cos((_cameraRotationY - 90.0f) * Mathf.Deg2Rad);
    float strafeSin = Mathf.Sin((_cameraRotationY - 90.0f) * Mathf.Deg2Rad);

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
      _cameraPos.x += strafeSin * (Time.smoothDeltaTime * _actualCameraSpeed);
      _cameraPos.z += strafeCos * (Time.smoothDeltaTime * _actualCameraSpeed);
    }
    else if (Input.GetKey(KeyCode.D))
    {
      _cameraPos.x -= strafeSin * (Time.smoothDeltaTime * _actualCameraSpeed);
      _cameraPos.z -= strafeCos * (Time.smoothDeltaTime * _actualCameraSpeed);
    }

    if (Input.GetKey(KeyCode.W))
    {    
      _cameraPos.x += sin * (Time.smoothDeltaTime * _actualCameraSpeed);
      _cameraPos.z += cos * (Time.smoothDeltaTime * _actualCameraSpeed);
    }
    else if (Input.GetKey(KeyCode.S))
    {
      _cameraPos.x -= sin * (Time.smoothDeltaTime * _actualCameraSpeed);
      _cameraPos.z -= cos * (Time.smoothDeltaTime * _actualCameraSpeed);
    }

    if (Input.GetKey(KeyCode.Z))
    {
      _cameraRotationY += Time.smoothDeltaTime * _cameraRotationSpeed;
    }
    else if (Input.GetKey(KeyCode.X))
    {
      _cameraRotationY -= Time.smoothDeltaTime * _cameraRotationSpeed;
    }

    _cameraPos.x = Mathf.Clamp(_cameraPos.x, -5.0f, _map.MapX + 5.0f);
    _cameraPos.z = Mathf.Clamp(_cameraPos.z, -5.0f, _map.MapZ + 5.0f);
    _cameraPos.y = _currentFloor;

    _gridBlockPos.x = (int)_cameraPos.x;
    _gridBlockPos.y = _currentFloor;
    _gridBlockPos.z = (int)_cameraPos.z;

    FloorPlacementGridHolder.position = _gridBlockPos;
    ObjectsPlacementGridHolder.position = _gridBlockPos;

    CameraHolder.position = _cameraPos;

    Vector3 angles = CameraHolder.transform.eulerAngles;
    angles.y = _cameraRotationY;
    CameraHolder.transform.eulerAngles = angles;
    LightNoShadows.eulerAngles = angles;

    Vector3 cursorTextAngles = CursorTextHolder.localEulerAngles;
    cursorTextAngles.y = _cameraRotationY;
    CursorTextHolder.localEulerAngles = cursorTextAngles;
  }

  List<string> _listToUpdate;

  int _editorMode = 0;
  public void SetEditorMode(int mode)
  {
    _editorMode = mode;
    _currentPage = 0;

    Cursor.SetActive(_editorMode != 2);

    if (SelectedObjectCursor.activeSelf && _editorMode != 2)
    {
      SelectedObjectPropertiesWindow.DeselectObject();
      SelectedObjectCursor.SetActive(false);
    }

    if (_editorMode == 0)
    {
      if (_previewObject != null)
      {
        Destroy(_previewObject);
      }

      _totalPages = _floorTextureNames.Count / _itemsOnPage;

      FloorPlacementGridHolder.gameObject.SetActive(true);
      ObjectsPlacementGridHolder.gameObject.SetActive(false);

      _listToUpdate = _floorTextureNames;

      UpdatePage(_listToUpdate);

      ListOfItems.Controls[0].Select();
    }
    else if (_editorMode == 1)
    {
      _totalPages = _objectsPrefabsNamesList.Count / _itemsOnPage;

      ObjectsPlacementGridHolder.gameObject.SetActive(true);
      FloorPlacementGridHolder.gameObject.SetActive(false);

      _listToUpdate = _objectsPrefabsNamesList;

      UpdatePage(_listToUpdate);

      ListOfItems.Controls[0].Select();
    }
    else if (_editorMode == 2 || _editorMode == 4 || _editorMode == 5)
    {
      if (_previewObject != null)
      {
        Destroy(_previewObject);
      }

      FloorPlacementGridHolder.gameObject.SetActive(true);
      ObjectsPlacementGridHolder.gameObject.SetActive(false);

      UpdatePage(null);
    }
    else if (_editorMode == 3)
    {
      if (_previewObject != null)
      {
        Destroy(_previewObject);
      }

      _totalPages = _floorTextureNames.Count / _itemsOnPage;

      ObjectsPlacementGridHolder.gameObject.SetActive(true);
      FloorPlacementGridHolder.gameObject.SetActive(false);

      _listToUpdate = _floorTextureNames;

      UpdatePage(_listToUpdate);

      ListOfItems.Controls[0].Select();
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

  public void MapPropertiesWindowOpen()
  {
    EnteringText = true;

    UpdateMapProperties();

    MapPropertiesWindow.SetActive(true);
  }

  public void MapPropertiesWindowClose()
  {
    EnteringText = false;
    
    SetMapProperties();

    MapPropertiesWindow.SetActive(false);
  }

  void SetMapProperties()
  {
    _levelToSave.MapPropertiesObject.MapName = MapNameInput.text;
    _levelToSave.MapPropertiesObject.MapAuthor = MapAuthorInput.text;
    _levelToSave.MapPropertiesObject.MapComments = MapCommentsInput.text;
    _levelToSave.MapPropertiesObject.DirectionalLightEnabled = DirectionalLightEnabled.isOn;
  }

  void UpdateMapProperties()
  {
    MapNameInput.text = _levelToSave.MapPropertiesObject.MapName;
    MapAuthorInput.text = _levelToSave.MapPropertiesObject.MapAuthor;
    MapCommentsInput.text = _levelToSave.MapPropertiesObject.MapComments;
    DirectionalLightEnabled.isOn = _levelToSave.MapPropertiesObject.DirectionalLightEnabled;
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

    _levelToSave.Init(mapX, mapY, mapZ);

    CreateNewLevel(mapX, mapY, mapZ);

    CloseNewMapWindow();

    SelectedObjectPropertiesWindow.DeselectObject();
    SelectedObjectCursor.SetActive(false);
  }

  public void SaveMapHandler()
  {
    string path = StandaloneFileBrowser.SaveFilePanel("Save Map", "", "level", "bytes");
    if (!string.IsNullOrEmpty(path))
    {
      SaveLevel(path);
      CurrentLevelName.text = path;
    }
  }

  public void LoadMapHandler()
  {
    string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "bytes", false);
    if (paths.Length != 0 && !string.IsNullOrEmpty(paths[0]))
    {
      LoadLevel(paths[0]);
    }
  }

  void LoadLevel(string path)
  {    
    var formatter = new BinaryFormatter();  
    Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);  
    _levelToSave = (SerializedLevel)formatter.Deserialize(stream);  
    stream.Close();

    //PrintLevelInfo(path);

    CreateNewLevel(_levelToSave.LevelSize.X, _levelToSave.LevelSize.Y, _levelToSave.LevelSize.Z);
    InstantiateLoadedLevel();

    SelectedObjectPropertiesWindow.DeselectObject();
    SelectedObjectCursor.SetActive(false);

    CurrentLevelName.text = path;
  }

  void PrintLevelInfo(string path)
  {
    string output = string.Format("{0} loaded\n", path);

    output += string.Format("Floor tiles {0}\n", _levelToSave.FloorTiles.Count);
    output += string.Format("Objects {0}\n", _levelToSave.Objects.Count);

    int number = 0;
    foreach (var item in _levelToSave.Objects)
    {
      output += string.Format("{0} - {1} {2} {3}\n", number + 1, item.PrefabName, item.RotationAngle, item.WorldPosition);
      number++;
    }

    Debug.Log(output);
  }

  void InstantiateLoadedLevel()
  {
    SerializedVector3 pos = SerializedVector3.Zero;
    GameObject go = null;
    foreach (var item in _levelToSave.FloorTiles)
    {
      pos = item.WorldPosition;
      go = PrefabsManager.Instance.InstantiatePrefab(GlobalConstants.FloorTemplatePrefabName, new Vector3(pos.X, pos.Y, pos.Z), Quaternion.identity);
      go.transform.parent = InstantiatedFloorHolder;
      go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      Util.SetGameObjectLayer(go, LayerMask.NameToLayer("EditorMapFloor"), true);
      FloorBehaviour fb = go.GetComponent<FloorBehaviour>();
      _map.Level[(int)pos.X, (int)pos.Y, (int)pos.Z].Texture1Name = item.TextureName;
      _map.Level[(int)pos.X, (int)pos.Y, (int)pos.Z].SkipTransitionCheckHere = item.SkipTransitionCheck;
      _map.Level[(int)pos.X, (int)pos.Y, (int)pos.Z].AllowBlending = item.AllowBlending;

      if (item.AllowBlending)
      {
        fb.PathMarker.SetActive(true);
      }

      fb.Init(_map.Level[(int)pos.X, (int)pos.Y, (int)pos.Z]);
    }

    foreach (var item in _levelToSave.Objects)
    {
      pos = item.WorldPosition;
      go = PrefabsManager.Instance.InstantiatePrefab(item.PrefabName, new Vector3(pos.X, pos.Y, pos.Z), Quaternion.Euler(0.0f, item.RotationAngle, 0.0f));
      go.transform.parent = InstantiatedObjectsHolder;
      go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      Util.SetGameObjectLayer(go, LayerMask.NameToLayer("EditorMapObject"), true);
      var wo = go.GetComponent<WorldObjectBase>();

      wo.SerializedObject = item;

      wo.PrefabName = item.PrefabName;
      wo.RotationAngle = item.RotationAngle;
      wo.WorldPosition.Set(item.WorldPosition.X, item.WorldPosition.Y, item.WorldPosition.Z);

      wo.Init(item);
    }
  }

  void SaveLevel(string path)
  {    
    // If we save a level, then after loading the _levelToSave.FloorTiles and _levelToSave.Objects lists will be populated.
    // If we don't clear them after loading, there will be duplicate objects during save, because we will traverse
    // the scene transform and put the same object again in the list.

    _levelToSave.Init(_map.MapX, _map.MapY, _map.MapZ);

    foreach (Transform t in InstantiatedFloorHolder)
    {
      FloorBehaviour fb = t.gameObject.GetComponent<FloorBehaviour>();
      SerializedFloor sf = new SerializedFloor();
      sf.TextureName = fb.BlockRef.Texture1Name;
      sf.SkipTransitionCheck = fb.BlockRef.SkipTransitionCheckHere;
      sf.AllowBlending = fb.BlockRef.AllowBlending;
      sf.WorldPosition.Set(fb.transform.position);
      _levelToSave.FloorTiles.Add(sf);
    }

    foreach (Transform t in InstantiatedObjectsHolder)
    {
      WorldObjectBase wo = t.gameObject.GetComponent<WorldObjectBase>();

      // Wall is not edited via properties window

      if (wo is WallWorldObject)
      {        
        (wo.SerializedObject as SerializedWall).TextureName = (wo as WallWorldObject).TextureName;
      }

      // Common properties for all

      wo.RotationAngle = wo.transform.eulerAngles.y;
      wo.WorldPosition.Set(wo.transform.position.x, wo.transform.position.y, wo.transform.position.z);

      wo.SerializedObject.PrefabName = wo.PrefabName;
      wo.SerializedObject.RotationAngle = wo.RotationAngle;
      wo.SerializedObject.WorldPosition.Set(wo.WorldPosition);

      _levelToSave.Objects.Add(wo.SerializedObject);
    }

    var formatter = new BinaryFormatter();
    Stream s = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
    formatter.Serialize(s, _levelToSave);
    s.Close();

    // Without this, files saved in the Resources folder directly won't be updated
    // until Unity editor is refreshed (e.g. minimized/maximized)
    #if UNITY_EDITOR
    UnityEditor.AssetDatabase.Refresh();
    #endif
  }

  void CreateNewLevel(int mapX, int mapY, int mapZ)
  {
    _map = new LevelBase(mapX, mapY, mapZ);

    UpdateFloorCounter();

    DestroyChildren(InstantiatedFloorHolder);
    DestroyChildren(InstantiatedObjectsHolder);
    DestroyChildren(ObjectsPlacementGridHolder);
    DestroyChildren(MapGridHolder);
    DestroyChildren(FloorPlacementGridHolder);

    // Instantiate call with parent transform argument makes object parent with preserving world position.
    // E.g. if we have object1 at (2, 0) and object2 at (-2, 0) after parenting object2 to object1
    // it will have (-4, 0) since its world position is recalculated relative to parent.
    // Our grids are created fromm -N to N around (0, 0), so we need to reset their previous positions
    // before creating new map, and then move their holders in Update() with camera.

    ObjectsPlacementGridHolder.position = Vector3.zero;
    FloorPlacementGridHolder.position = Vector3.zero;

    CreateMapGrid();
    CreateFloorGrid();
    CreateObjectsGrid();

    _cameraPos.Set(_map.MapX / 2.0f, 0.0f, _map.MapZ / 2.0f);
    CameraHolder.position = _cameraPos;

    CurrentLevelName.text = "Untitled";
  }

  void DestroyChildren(Transform t)
  {
    int childCount = t.childCount;
    for (int i = 0; i < childCount; i++)
    {
      Destroy(t.GetChild(i).gameObject);
    }
  }

  string _infoBarText = string.Empty;
  public void SetInfoBarText(int editorMode)
  {
    switch (editorMode)
    {
      case 0:
        _infoBarText = "Place floor tiles (hold Left Shift to bucket fill)";
        break;

      case 1:
        _infoBarText = "Place objects (Q E to rotate)";
        break;
      
      case 2:
        _infoBarText = "Select mode";
        break;

      case 3:
        _infoBarText = "Place quad walls (Q E to rotate)";
        break;

      case 4:
        _infoBarText = "Mark floor tiles as \"path\" for correct transition visualization";
        break;

      case 5:
        _infoBarText = "Place exit zones";
        break;        
    }

    InfoBarText.text = _infoBarText;
  }

  public void ClearInfoBarText()
  {
    _infoBarText = "";
    InfoBarText.text = _infoBarText;
  }
}
