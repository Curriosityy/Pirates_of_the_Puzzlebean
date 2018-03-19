using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapControler : MonoBehaviour
{
    private float[] yRange = { -7f, -1f };
    public float newRouteProb;
    public float probWhenFail;
    public int nextRoute;
    public int lvlCount;
    public GameObject startIsland;
    public GameObject endIsland;
    public Transform tStartPoint;
    public Transform tEndPoint;
    public Transform tPointHolder;
    public Transform lineDumbster;
    public Sprite[] islands;
    public GameObject[] pointType;
    public Dictionary<int, Level> levels = new Dictionary<int, Level>();
    public LineRenderer lr;
    public Point pointOfStaying;
    public Point pointToGo;
    public int currLevel;

    private void Awake()
    {
        if (GameObject.FindObjectsOfType<MapControler>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private GameObject randAPointType()
    {
        if (Random.Range(0, 100) <= 70)
        {
            return pointType[0];
        }
        else
        {
            return pointType[Random.Range(1, 4)];
        }
    }

    private void CreateLevelOnScreen(int xlevel)
    {
        float centerPoint = (yRange[1] - yRange[0]) / 2;
        centerPoint = yRange[1] - centerPoint;
        int pointsInLevel = levels[xlevel].GetPoints().Count;
        float i = pointsInLevel > 1 ? centerPoint - (pointsInLevel / 2) : centerPoint + 1.5f;
        float xSpaceBetweenPoints = (tEndPoint.position.x - tStartPoint.position.x) / lvlCount;
        float ySpaceBetweenPoints = (yRange[1] - yRange[0]) / pointsInLevel;
        levels[xlevel].GetPoints().ForEach(x =>
        {
            float xpos = (tStartPoint.position.x + (xlevel * xSpaceBetweenPoints));
            xpos = Random.Range(xpos - 0.3f, xpos + 0.3f);
            float ypos = Random.Range(i + 0.2f - ySpaceBetweenPoints / 2f, i - 0.2f + ySpaceBetweenPoints / 2f);
            x.SetInstantion(Instantiate(x.GetPointType(), new Vector3(xpos, ypos, -1), Quaternion.identity, tPointHolder) as GameObject);
            x.GetInstantion().GetComponent<PointControler>().thisPoint = x;
            i += ySpaceBetweenPoints;
        });
    }

    private void AddStartAndEndPoint()
    {
        Point startPoint = new Point(tStartPoint.gameObject);
        pointOfStaying = startPoint;
        currLevel = 0;
        startPoint.SetInstantion(tStartPoint.gameObject);
        Point bossPoint = new Point(tEndPoint.gameObject);
        bossPoint.SetInstantion(tEndPoint.gameObject);
        Level startingLevel = new Level();
        Level bossLevel = new Level();
        startingLevel.addPointToLevel(startPoint);
        bossLevel.addPointToLevel(bossPoint);
        levels.Add(0, startingLevel);
        levels.Add(lvlCount, bossLevel);
    }

    private void BuildARoutes()
    {
        float prob = 1f;
        Point newPoint;
        for (int j = 0; j < nextRoute; j++)
        {
            for (int i = 0; i < lvlCount; i++)
            {
                if (j == 0) prob = 1f;
                float rand = Random.Range(0f, 1f);
                if (i < lvlCount - 1)
                {
                    if (rand < prob)
                    {
                        if (i == lvlCount - 2)
                        {
                            newPoint = new Point(pointType[1]);
                        }
                        else
                        {
                            newPoint = new Point(randAPointType());
                        }
                        levels[i].GetPoints()[levels[i].GetPoints().Count - 1].AddConnectionBetween(newPoint);
                        levels[i + 1].addPointToLevel(newPoint);
                        prob = newRouteProb;
                    }
                    else
                    {
                        Point point = levels[i].GetPoints()[levels[i].GetPoints().Count - 1];
                        Point point2 = levels[i + 1].GetPoints()[levels[i + 1].GetPoints().Count - 1];
                        if (!point.IsConnectedTo(point2))
                        {
                            point.AddConnectionBetween(point2);
                        }
                        prob += probWhenFail;
                    }
                }
                else
                {
                    Point point = levels[i].GetPoints()[levels[i].GetPoints().Count - 1];
                    Point point2 = levels[i + 1].GetPoints()[levels[i + 1].GetPoints().Count - 1];
                    if (!point.IsConnectedTo(point2))
                    {
                        point.AddConnectionBetween(point2);
                    }
                    prob = newRouteProb;
                }
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        startIsland.GetComponent<SpriteRenderer>().sprite = islands[Random.Range(0, islands.Length)];
        endIsland.GetComponent<SpriteRenderer>().sprite = islands[Random.Range(0, islands.Length)];

        AddStartAndEndPoint();
        for (int i = 1; i < lvlCount; i++)
        {
            levels.Add(i, new Level());
        }
        BuildARoutes();
        for (int i = 1; i < lvlCount; i++)
        {
            CreateLevelOnScreen(i);
        }

        for (int i = 0; i < lvlCount; i++)
        {
            levels[i].GetPoints().ForEach(point =>
            {
                point.GetConnection().ForEach(connectedPoint =>
                {
                    Vector3 posOfpoint = point.GetInstantion().transform.position;
                    posOfpoint.x += 0.2f;
                    Vector3 posOfConnPoint = connectedPoint.GetInstantion().transform.position;
                    posOfConnPoint.x -= 0.2f;
                    lr.SetPosition(0, posOfpoint);
                    lr.SetPosition(1, posOfConnPoint);
                    Instantiate(lr.gameObject, lineDumbster);

                    //Debug.DrawLine(posOfpoint, posOfConnPoint, Color.green, 100f);
                });
            });
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public class Level
    {
        private List<Point> pointOnLevel = new List<Point>();

        public Level()
        {
        }

        public void addPointToLevel(Point xpoint)
        {
            pointOnLevel.Add(xpoint);
        }

        public List<Point> GetPoints()
        {
            return pointOnLevel;
        }
    }

    public class Point
    {
        private GameObject pointType;
        private GameObject instantion;
        private List<Point> connectedWitch = new List<Point>();

        public Point(GameObject xpointType)
        {
            pointType = xpointType;
        }

        public void SetInstantion(GameObject xinstantion)
        {
            instantion = xinstantion;
        }

        public GameObject GetInstantion()
        {
            return instantion;
        }

        public void AddConnectionBetween(Point xpoint)
        {
            connectedWitch.Add(xpoint);
        }

        public List<Point> GetConnection()
        {
            return connectedWitch;
        }

        public GameObject GetPointType()
        {
            return pointType;
        }

        public bool IsConnectedTo(Point xpoint)
        {
            return connectedWitch.Contains(xpoint);
        }
    }
}