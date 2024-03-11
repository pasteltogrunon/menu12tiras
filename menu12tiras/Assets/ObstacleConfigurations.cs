using System;
using System.Collections.Generic;

public static class ObstacleConfigurations
{

    private static readonly List<Tuple<List<List<char>>, List<List<char>>>> ConfigurationsLevel1 = new()
    {
        new(
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'o', 'o', 'o'},
                new() {'o', 'x', 'o'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'o', 'x', 'o'},
                new() {'x', 'o', 'x'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'o', 'x', 'o'},
                new() {'o', 'o', 'o'},
            },
            new List<List<char>>()
            {
                new() {'x', 'o', 'x', 'o'},
                new() {'o', 'o', 'o', 'o'},
                new() {'v'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'x', 'o'},
                new() {'o', 'x', 'o'},
                new() {'o', 'x', 'o'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'x', '-', 'x'},
                new() {'o', 'o', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'o', 'x', 'o'},
                new() {'x', 'o', 'x'},
            },
            new List<List<char>>()
            {
                new() {'o', 'x', 'o'},
                new() {'x', 'o', 'x'},
                new() {'o', 'o', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'x', '-', 'x'},
                new() {'o', 'o', 'o'},
            },
            new List<List<char>>()
            {
                new() {'o', 'x', 'o'},
                new() {'o', 'x', 'o'},
                new() {'o', 'x', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'x', 'o', 'x', 'o'},
                new() {'o', 'o', 'o', 'o'},
                new() {'v'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'o', 'o', 'o'},
                new() {'o', 'x', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'x', 'x', 'o', 'x', 'x', 'x'},
                new() {'v', 'o', 'v', 'o', 'v', 'v'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o', 'o', 'o', 'o'},
                new() {'o', 'o', 'o', 'o', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'o', 'o', 'o', 'o'},
                new() {'o', 'o', 'o', 'o', 'o'},
            },
            new List<List<char>>()
            {
                new() {'x', 'x', 'o', 'x', 'x', 'x'},
                new() {'v', 'o', 'v', 'o', 'v', 'v'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'x', 'o'},
                new() {'x', 'o', 'x'},
                new() {'o', 'x', 'o'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'o', 'x', 'o'},
                new() {'o', 'o', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'x', 'o', 'x'},
                new() {'o', 'o', 'o'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o', 'o'},
                new() {'x', 'o', 'o'},
                new() {'o', 'o', 'x'},
            }
        )
    };

    private static readonly List<Tuple<List<List<char>>, List<List<char>>>> ConfigurationsLevel2 = new()
    {
        new(
            new List<List<char>>()
            {
                new() {'o', 'o', 'o', 'o', 'o', 'o'},
                new() {'o', 'o', 'o', 'o', 'o', 'o'},
                new() {'o', 'o', 'o', 'o', 'o', 'o'},
            },
            new List<List<char>>()
            {
                new() {'x', 'x', 'x', 'x', 'x', 'x'},
                new() {'x', 'x', 'x', 'x', 'x', 'x'},
                new() {'v'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'x', 'x', 'x', 'x', 'x', 'x'},
                new() {'x', 'x', 'x', 'x', 'x', 'x'},
                new() {'v'},
            },
            new List<List<char>>()
            {
                new() {'o', 'o', 'o', 'o', 'o', 'o'},
                new() {'o', 'o', 'o', 'o', 'o', 'o'},
                new() {'o', 'o', 'o', 'o', 'o', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'x', 'o', 'x', 'o'},
                new() {'o', 'x', 'o', 'x', 'o'},
            },
            new List<List<char>>()
            {
                new() {'x'},
                new() {'x'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'x', 'x', '-', 'x', 'x'},
                new() {'x', 'o', 'o', 'o', 'x'},
            },
            new List<List<char>>()
            {
                new() {'o'},
                new() {'o', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'x', 'x', 'x'},
                new() {'x', 'x', 'x'},
                new() {'x', 'x', 'x'},
            },
            new List<List<char>>()
            {
                new() {'x', 'o', 'x'},
                new() {'x', 'o', 'x'},
                new() {'x', 'o', 'x'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'o'},
                new() {'o', 'x', 'o'},
                new() {'x', 'o', 'x'},
            },
            new List<List<char>>()
            {
                new() {'x', 'x', 'x'},
                new() {'x', 'x', 'x'},
                new() {'x', 'x', 'x'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'o', 'x', 'v', 'o', 'v'},
                new() {'x', '-', 'x', 'x', '-', 'x'},
            },
            new List<List<char>>()
            {
                new() {'o', 'x', 'o', 'x', 'o', 'x'},
                new() {'v'}
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'v', 'v', 'v', 'o'},
                new() {'o', 'v', 'v', 'v', 'o'},
                new() {'o', 'v', 'v', 'v', 'o'},
                new() {'o', 'v', 'v', 'v', 'o'},
            },
            new List<List<char>>()
            {
                new() {'o', 'v', 'x', 'v', 'o'},
                new() {'o', 'v', 'x', 'v', 'o'},
                new() {'o', 'v', 'x', 'v', 'o'},
                new() {'x', 'x', 'x', 'o', 'x'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'x', 'o', 'o', 'x'},
                new() {'v'},
                new() {'v'},
                new() {'v'},
            },
            new List<List<char>>()
            {
                new() {'o', 'v', 'v', 'v', 'o'},
                new() {'o', 'v', 'o', 'v', 'o'},
                new() {'o', 'x', '-', 'x', 'o'},
                new() {'o', 'v', 'v', 'v', 'o'},
            }
        ),
        new(
            new List<List<char>>()
            {
                new() {'o', 'x', 'o', 'x', 'o', 'x'},
                new() {'v', 'x', '-', 'x', '-', 'x'},
                new() {'o', 'v', 'o', 'x', 'o', 'x'},
            },
            new List<List<char>>()
            {
                new() {'o', 'x', '-', 'x', 'o', 'x'},
                new() {'x', 'v', 'o', 'v', 'x', 'v'},
                new() {'v'},
            }
        )
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
            _ => ConfigurationsLevel4[random.Next(ConfigurationsLevel4.Count)],
        };
    }
}