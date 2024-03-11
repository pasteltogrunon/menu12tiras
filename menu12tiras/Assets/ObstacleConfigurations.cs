using System;
using System.Collections.Generic;

public static class ObstacleConfigurations
{
    public static List<List<List<char>>> ConfigurationsArea1 = new()
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
    };

    //Return a random combination of configurations in the area 1
    public static Tuple<List<List<char>>, List<List<char>>> GetArea1Configuration()
    {
        Random random = new();
        return new(ConfigurationsArea1[random.Next(ConfigurationsArea1.Count)],
                    ConfigurationsArea1Inverted[random.Next(ConfigurationsArea1Inverted.Count)]);
    }
}