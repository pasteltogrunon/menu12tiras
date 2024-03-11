using System;
using System.Collections.Generic;

public static class ObstacleConfigurations
{
    /* public static List<List<List<char>>> ConfigurationsArea1 = new()
    {
        new List<List<char>>()
        {
            new() {'x', 'o'},
        },

        new List<List<char>>()
        {
            new() {'o', 'x'},
        },

        new List<List<char>>()
        {
            new() {'o', 'v'},
        },

        new List<List<char>>()
        {
            new() {'v', 'o'},
        },

        new List<List<char>>()
        {
            new() {'o', 'o'},
        },

        new List<List<char>>()
        {
            new() {'o', 'o'},
            new() {'o', 'o'},
        },

        new List<List<char>>()
        {
            new() {'x', 'x'},
        },

        new List<List<char>>()
        {
            new() {'x', '-', 'x'},
            new() {'v', 'o', 'v'},
        },

        new List<List<char>>()
        {
            new() {'v', 'o', 'v'},
            new() {'x', '-', 'x'},
        },

        new List<List<char>>()
        {
            new() {'o', 'o', 'o', 'o', 'o'},
            new() {'o', 'o', 'o', 'o', 'o'},
        },

        new List<List<char>>()
        {
            new() {'v', 'v', 'o', 'v', 'o', 'v'},
            new() {'x', 'x', 'x', 'o', 'x', 'x'},
        },

        new List<List<char>>()
        {
            new() {'v', 'o', 'v', 'o', 'v', 'v'},
            new() {'x', 'x', 'o', 'x', 'x', 'x'},
        },
    };

    public static List<List<List<char>>> ConfigurationsArea1Inverted = new()
    {
        new List<List<char>>()
        {
            new() {'v', 'o', 'v', 'o', 'v', 'v'},
            new() {'x', 'x', 'o', 'x', 'x', 'x'},
        },

        new List<List<char>>()
        {
            new() {'v', 'v', 'o', 'v', 'o', 'v'},
            new() {'x', 'x', 'x', 'o', 'x', 'x'},
        },

        new List<List<char>>()
        {
            new() {'o', 'o', 'o', 'o', 'o'},
            new() {'o', 'o', 'o', 'o', 'o'},
        },

        new List<List<char>>()
        {
            new() {'v', 'o', 'v'},
            new() {'x', '-', 'x'},
        },

        new List<List<char>>()
        {
            new() {'x', '-', 'x'},
            new() {'v', 'o', 'v'},
        },

        new List<List<char>>()
        {
            new() {'o', 'o', 'o', 'o', 'o'},
        },

        new List<List<char>>()
        {
            new() {'v', 'x'},
        },

        new List<List<char>>()
        {
            new() {'x', 'v'},
        },

        new List<List<char>>()
        {
            new() {'o', 'v', 'v', 'o'},
            new() {'x', 'o', 'o', 'x'},
        },

        new List<List<char>>()
        {
            new() {'v', 'v', 'o', 'v', 'v', 'v'},
            new() {'x', 'x', '-', 'x', 'x', 'x'},
        },

        new List<List<char>>()
        {
            new() {'v', 'v', 'o', 'v', 'v', 'v'},
            new() {'x', 'x', 'x', '-', 'x', 'x'},
        },
    }; */


    private static readonly List<Tuple<List<List<char>>, List<List<char>>>> ConfigurationsLevel1 = new()
    {
        new(
            new List<List<char>>()
            {
                new() {'x', 'x'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o'},
            }
        ),
    };

    private static readonly List<Tuple<List<List<char>>, List<List<char>>>> ConfigurationsLevel2 = new()
    {
        new(
            new List<List<char>>()
            {
                new() {'x', 'x', 'x'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o'},
            }
        ),
    };

    private static readonly List<Tuple<List<List<char>>, List<List<char>>>> ConfigurationsLevel3 = new()
    {
        new(
            new List<List<char>>()
            {
                new() {'x', 'x', 'x', 'x'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o'},
            }
        ),
    };

    private static readonly List<Tuple<List<List<char>>, List<List<char>>>> ConfigurationsLevel4 = new()
    {
        new(
            new List<List<char>>()
            {
                new() {'x', 'x', 'x', 'x', 'x'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o'},
            }
        ),
    };


    public static Tuple<List<List<char>>, List<List<char>>> GetLevelConfiguration(int level)
    {
        Random random = new();
        return level switch
        {
            1 => ConfigurationsLevel1[random.Next(ConfigurationsLevel1.Count)],
            2 => ConfigurationsLevel2[random.Next(ConfigurationsLevel2.Count)],
            3 => ConfigurationsLevel3[random.Next(ConfigurationsLevel3.Count)],
            4 => ConfigurationsLevel4[random.Next(ConfigurationsLevel4.Count)],
            _ => ConfigurationsLevel4[random.Next(ConfigurationsLevel1.Count)],
        };
    }
}