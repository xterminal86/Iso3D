﻿using System.IO;
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

  public GameObject Cursor;
  public GameObject ElevationCursor;
  public GameObject EditorMapGridPrefab;
  public GameObject EditorFloorPlacementGridPrefab;
  public GameObject EditorObjectsPlacementGridPrefab;

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

  float _objectsPlacementGridSize = 0.5f;
  float _wallsPlacementGridSize = 0.25f;

  Vector3 _mousePos = Vector3.zero;

  Texture2D[] _floorTextures;

  LevelBase _map;

  SerializedLevel _levelToSave = new SerializedLevel();

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

    _map = new LevelBase(10, 2, 10);
    _levelToSave.Init(10, 2, 10);

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

      var prefab = PrefabsManager.Instance.FindPrefabByName("wall-template");
      _previewObject = Instantiate(prefab, Cursor.transform.position, Quaternion.identity);
      _previewObject.name = _selectedListObject;
      _previewObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      _previewObject.GetComponent<WallWorldObject>().Init(_selectedListObject);
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

    PositionText.text = string.Format("{0}\n{1}", Cursor.transform.position, _previewObjectAngle);

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

  void RemoveObject(int editorMode)
  {
    if (editorMode == 0)
    {
      RemoveFloor();
    }
    else if (editorMode == 1 || editorMode == 3)
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
    else if (editorMode == 1 || editorMode == 3)
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
    var wob = go.GetComponent<WorldObjectBase>();
    if (wob is WallWorldObject)
    {
      (wob as WallWorldObject).PrefabName = "wall-template";
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

    _map.Level[x, y, z].Texture1Name = _selectedListObject;

    var go = PrefabsManager.Instance.InstantiatePrefab("floor-template", Cursor.transform.position, Quaternion.identity);
    go.transform.parent = InstantiatedFloorHolder;
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

    FloorPlacementGridHolder.position = _gridBlockPos;
    ObjectsPlacementGridHolder.position = _gridBlockPos;

    CameraHolder.position = _cameraPos;
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
    else if (_editorMode == 2)
    {
      if (_previewObject != null)
      {
        Destroy(_previewObject);
      }

      ObjectsPlacementGridHolder.gameObject.SetActive(true);
      FloorPlacementGridHolder.gameObject.SetActive(false);

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
  }

  public void SaveMapHandler()
  {
    string path = StandaloneFileBrowser.SaveFilePanel("Save Map", "", "level", "lvl");
    if (!string.IsNullOrEmpty(path))
    {
      SaveLevel(path);
    }
  }

  public void LoadMapHandler()
  {
    string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "lvl", false);
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

    CreateNewLevel(_levelToSave.LevelSize.X, _levelToSave.LevelSize.Y, _levelToSave.LevelSize.Z);
    InstantiateLoadedLevel();

    // If we save a level, then after loading the _levelToSave.FloorTiles and _levelToSave.Objects lists will be populated.
    // If we don't clear them after loading, there will be duplicate objects during save, because wew will traverse
    // the scene transform and put the same object again in the list.

    _levelToSave.Init(_map.MapX, _map.MapY, _map.MapZ);
  }

  void InstantiateLoadedLevel()
  {
    SerializedVector3 pos = SerializedVector3.Zero;
    GameObject go = null;
    foreach (var item in _levelToSave.FloorTiles)
    {
      pos = item.WorldPosition;
      go = PrefabsManager.Instance.InstantiatePrefab("floor-template", new Vector3(pos.X, pos.Y, pos.Z), Quaternion.identity);
      go.transform.parent = InstantiatedFloorHolder;
      go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      Util.SetGameObjectLayer(go, LayerMask.NameToLayer("EditorMapFloor"), true);
      FloorBehaviour fb = go.GetComponent<FloorBehaviour>();
      _map.Level[(int)pos.X, (int)pos.Y, (int)pos.Z].Texture1Name = item.TextureName;
      fb.Init(_map.Level[(int)pos.X, (int)pos.Y, (int)pos.Z]);
    }

    foreach (var item in _levelToSave.Objects)
    {
      pos = item.WorldPosition;
      go = PrefabsManager.Instance.InstantiatePrefab(item.PrefabName, new Vector3(pos.X, pos.Y, pos.Z), Quaternion.Euler(0.0f, item.Angle, 0.0f));
      go.transform.parent = InstantiatedObjectsHolder;
      go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      Util.SetGameObjectLayer(go, LayerMask.NameToLayer("EditorMapObject"), true);
      var wall = go.GetComponent<WallWorldObject>();
      if (wall != null)
      {
        wall.Init((item as SerializedWall).TextureName);
      }
    }
  }

  void SaveLevel(string path)
  {    
    foreach (Transform t in InstantiatedFloorHolder)
    {
      FloorBehaviour fb = t.gameObject.GetComponent<FloorBehaviour>();
      SerializedFloor sf = new SerializedFloor();
      sf.TextureName = fb.BlockRef.Texture1Name;
      sf.WorldPosition.Set(fb.transform.position);
      _levelToSave.FloorTiles.Add(sf);
    }

    foreach (Transform t in InstantiatedObjectsHolder)
    {
      WorldObjectBase wo = t.gameObject.GetComponent<WorldObjectBase>();
      if (wo is WallWorldObject)
      {
        SerializedWall sw = new SerializedWall();
        sw.PrefabName = (wo as WallWorldObject).PrefabName;
        sw.TextureName = (wo as WallWorldObject).TextureName;
        sw.Angle = (wo as WallWorldObject).transform.eulerAngles.y;
        sw.WorldPosition.Set((wo as WallWorldObject).transform.position);
        _levelToSave.Objects.Add(sw);
      }
      else
      {
        SerializedWorldObject swo = new SerializedWorldObject();
        swo.PrefabName = (wo as WorldObjectBase).PrefabName;
        swo.Angle = (wo as WorldObjectBase).transform.eulerAngles.y;
        swo.WorldPosition.Set((wo as WorldObjectBase).transform.position);
        _levelToSave.Objects.Add(swo);
      }
    }

    var formatter = new BinaryFormatter();
    Stream s = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
    formatter.Serialize(s, _levelToSave);
    s.Close();
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
