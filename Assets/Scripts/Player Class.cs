using System.Collections.Generic;

public class Player
{
    public PlayerData playerData;
    public Run currentRun;
}

public class PlayerData
{
    public List<Run> previousRuns;
}

public class Run
{
    public float runTime = 0;
}