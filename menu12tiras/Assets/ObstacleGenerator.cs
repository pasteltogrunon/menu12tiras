using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{

    public int nActiveSections = 10;
    private List<ObstacleSection> obstacleSections;

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
        private Tuple<List<List<char>>, List<List<char>>> configuration;
        private Tuple<List<List<GameObject>>, List<List<GameObject>>> objects = new(new List<List<GameObject>>(), new List<List<GameObject>>());

        public ObstacleSection(Tuple<List<List<char>>, List<List<char>>> configuration, float u)
        {
            this.u = u;
            this.configuration = configuration;
            nrows = configuration.Item1.Count;
        }

        public void GenerateObjects()
        {

            float ustep = 0.01f;
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
            switch (type)
            {
                case 'o':
                    GameObject obstacle = Instantiate(GameManager.instance.coin);
                    obstacle.GetComponent<Coin>().Init(u + i * ustep, -1 + vstep * (j + 0.5f), inverted);
                    row.Add(obstacle);
                    break;
                case 'v':
                    row.Add(null);
                    break;
                default:
                    row.Add(null);
                    break;
            }
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
    void Start()
    {
        obstacleSections = new List<ObstacleSection>();
        for (int i = 0; i < nActiveSections; i++)
        {
            obstacleSections.Add(new ObstacleSection(ObstacleConfigurations.Configuration0, i * 0.1f));
        }
        foreach (ObstacleSection section in obstacleSections)
        {
            section.GenerateObjects();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float uplayer = GameObject.Find("Player").GetComponent<Player>().UPos;
        float usection = obstacleSections[0].U + obstacleSections[0].NRows * 0.1f;
        if (uplayer > usection)
        {
            ObstacleSection section = obstacleSections[0];
            obstacleSections.RemoveAt(0);
            obstacleSections.Add(new ObstacleSection(ObstacleConfigurations.Configuration0, usection + 0.01f));
            section.DestroyObjects();
            section.GenerateObjects();
        }
    }
}
