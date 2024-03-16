using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{

    public static ObstacleGenerator Instance;

    public int nActiveSections = 10;
    public float ustep = 0.1f;
    private List<ObstacleSection> obstacleSections;
    private int currentLevel;

    public static int CurrentLevel
    {
        get => Instance.currentLevel;
    }

    private static readonly Dictionary<int, float> TIME_LEVELS = new()
    {
        {1, 60},
        {2, 150},
        {3, 300},
        {4, -1}
    };

    public class ObstacleSection
    {
        private int nrows;
        public int NRows
        {
            get => nrows;
        }
        private float u;
        public float U
        {
            get => u;
        }
        private float ustep;
        public float UStep
        {
            get => ustep;
        }
        private Tuple<List<List<char>>, List<List<char>>> configuration;
        private Tuple<List<List<GameObject>>, List<List<GameObject>>> objects = new(new List<List<GameObject>>(), new List<List<GameObject>>());

        public ObstacleSection(Tuple<List<List<char>>, List<List<char>>> configuration, float u, float ustep = 0.05f)
        {
            this.u = u;
            this.ustep = ustep;
            this.configuration = configuration;
            nrows = configuration.Item1.Count;
        }

        public void GenerateObjects()
        {

            float vstep1, vstep2;

            for (int i = 0; i < nrows; i++)
            {
                List<GameObject> row1 = new();
                List<GameObject> row2 = new();

                vstep1 = 2 / (float)configuration.Item1[i].Count;
                vstep2 = 2 / (float)configuration.Item2[i].Count;

                for (int j = 0; j < configuration.Item1[i].Count; j++)
                {
                    InstantiateObject(row1, configuration.Item1[i][j], i, j, u, ustep, vstep1);
                }

                for (int j = 0; j < configuration.Item2[i].Count; j++)
                {
                    InstantiateObject(row2, configuration.Item2[i][j], i, j, u, ustep, vstep2, true);
                }

                objects.Item1.Add(row1);
                objects.Item2.Add(row2);
            }
        }

        public void InstantiateObject(List<GameObject> row, char type, float i, float j, float u, float ustep,
                                        float vstep, bool inverted = false)
        {
            GameObject obstacle = null;
            switch (type)
            {
                case 'o':
                    obstacle = Instantiate(GameManager.instance.coin);
                    obstacle.GetComponent<Coin>().Init(u + i * ustep, -1 + vstep * (j + 0.5f), inverted);
                    row.Add(obstacle);
                    break;
                case 'v':
                    break;
                case 'x':
                    obstacle = Instantiate(GameManager.instance.wall);
                    obstacle.GetComponent<Obstacle>().Init(u + i * ustep, -1 + vstep * (j + 0.5f), inverted);
                    row.Add(obstacle);
                    break;
                case '-':
                    obstacle = Instantiate(GameManager.instance.slidingBar);
                    obstacle.GetComponent<Obstacle>().Init(u + i * ustep, -1 + vstep * (j + 0.5f), inverted);
                    row.Add(obstacle);
                    break;
                case 'e':
                    obstacle = Instantiate(GameManager.instance.electricField);
                    obstacle.GetComponent<ElectricFloor>().Init(u + i * ustep, -1 + vstep * j, ustep, vstep, inverted);
                    row.Add(obstacle);
                    break;
                default:
                    break;
            }
            row.Add(obstacle);
        }

        public void DestroyObjects()
        {
            foreach (List<GameObject> row in objects.Item1)
            {
                foreach (GameObject obj in row)
                {
                    Destroy(obj);
                }
            }
            foreach (List<GameObject> row in objects.Item2)
            {
                foreach (GameObject obj in row)
                {
                    Destroy(obj);
                }
            }
        }

    }

    // Start is called before the first frame update
    public void Start()
    {
        Instance = this;


        currentLevel = 1;
        obstacleSections = new List<ObstacleSection>();
        for (int i = 0; i < nActiveSections; i++)
        {
            obstacleSections.Add(new ObstacleSection(ObstacleConfigurations.GetLevelConfiguration(currentLevel), (i + 1) * ustep));
        }
        foreach (ObstacleSection section in obstacleSections)
        {
            section.GenerateObjects();
        }
    }

    public void Restart()
    {
        foreach (ObstacleSection os in obstacleSections)
        {
            os.DestroyObjects();
        }
        
        currentLevel = 1;
        obstacleSections = new List<ObstacleSection>();
        for (int i = 0; i < nActiveSections; i++)
        {
            obstacleSections.Add(new ObstacleSection(ObstacleConfigurations.GetLevelConfiguration(currentLevel), (i + 1) * ustep));
        }
        foreach (ObstacleSection section in obstacleSections)
        {
            section.GenerateObjects();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevel();
        float uplayer = GameManager.instance.player.UPos;
        float usectionFirst = obstacleSections[0].U + obstacleSections[0].NRows * obstacleSections[0].UStep;
        float usectionLast = obstacleSections[^1].U + obstacleSections[^1].NRows * obstacleSections[^1].UStep;
        if (uplayer > usectionFirst)
        {
            obstacleSections[0].DestroyObjects();
            obstacleSections.RemoveAt(0);
            obstacleSections.Add(new ObstacleSection(ObstacleConfigurations.GetLevelConfiguration(currentLevel), usectionLast + ustep));
            obstacleSections[^1].GenerateObjects();
        }
    }

    void UpdateLevel()
    {
        if (TIME_LEVELS[currentLevel] != -1 && GameManager.instance.TotalTime > TIME_LEVELS[currentLevel])
            currentLevel++;
    }


}
