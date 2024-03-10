using System;
using System.Collections.Generic;

public static class ObstacleConfigurations
{
    public static Tuple<List<List<char>>, List<List<char>>> Configuration0 = new(
        new List<List<char>>(){
            new() {'o', 'o'},
            new() {'o', 'o', 'o', 'o', 'o'},
        },
        new List<List<char>>(){
            new() {'v', 'o', 'o', 'o', 'v'},
            new() {'o', 'o', 'o'},
        }
    );
}