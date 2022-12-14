using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buildings : MonoBehaviour
{
    private GridCell _gridCell;
    private GameGrid gameGrid;
    public Button firstButton;
    public Button secondButton;
    public Button thirdButton;
    public GameObject Target { get; set; }
    Vector3 truePos;
    private float _gridSpaceSize = 1.0f;
    [SerializeField] GameObject object1, object2;
    readonly List<GameObject> _buildingsList = new List<GameObject>();
    public Transform objectToPlace { get; set; }
    [SerializeField] private Camera gameCamera;
    [SerializeField] private bool followPointer=true;
    [SerializeField] public bool buildingMode=false;
    public int _buildingCount = 0;
    void Start()
    {
        gameGrid = GameObject.Find("GameGrid").GetComponent<GameGrid>();
        InstantiateObject();

    }
    public void InstantiateObject()
    {
        firstButton.gameObject.SetActive(false);
        secondButton.gameObject.SetActive(false);
        thirdButton.gameObject.SetActive(false);
        buildingMode = true;
        _buildingsList.Add(Instantiate(object2, new Vector3(gameGrid.Width / 2, gameGrid.Height / 2, -0.5f), Quaternion.identity));
        _buildingsList[_buildingCount].name="factory " + _buildingCount;
        _buildingsList[_buildingCount].transform.SetParent(object1.transform);
        Target = _buildingsList[_buildingCount];
        objectToPlace = _buildingsList[_buildingCount].transform;
        followPointer =true;
        
    }
    public bool fallowPointer
    {
        get { return followPointer; }
        set { followPointer = value; }
    }
    private void LateUpdate()
    {
        if (buildingMode == true)
        {
            truePos.x = Mathf.Floor(Target.transform.position.x / _gridSpaceSize) * _gridSpaceSize + 0.5f;
            truePos.y = Mathf.Floor(Target.transform.position.y / _gridSpaceSize) * _gridSpaceSize + 0.5f;
            truePos.z = Mathf.Floor(Target.transform.position.z / _gridSpaceSize) * _gridSpaceSize + 0.5f;
       
        Target.transform.position = truePos;
        }
    }

    void Update()
    {
        if (buildingMode == true)
        {
            if (followPointer == false)
            {
                _buildingsList[_buildingCount].GetComponent<Factory_1>().BluePrintOff();
                _gridCell.objectInThisGridSpace = _buildingsList[_buildingCount] as GameObject;
                _buildingCount++;
                buildingMode = false;
            }
            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            Vector2 pos;
            pos.x = Mathf.Floor(objectToPlace.position.x);
            pos.y = Mathf.Floor(objectToPlace.position.y);
            _gridCell = GameObject.Find(pos.x + "," + pos.y).GetComponent<GridCell>();
            if (followPointer == true && Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                objectToPlace.position = hitInfo.point;
                if (_buildingCount == 0)
                {
                    objectToPlace.position = new Vector3(gameGrid.Width / 2, gameGrid.Height / 2, -0.5f);
                    fallowPointer = false;
                    firstButton.gameObject.SetActive(true);
                }
            }
           
        }
    }
}
