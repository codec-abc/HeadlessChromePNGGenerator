using System.Collections.Generic;

public class Vector2 
{
    public int x {get; set;}
    public int y {get; set;}
}

public class Crop 
{
    public Vector2 size {get; set;}
    public Vector2 topLeftCorner {get; set;}
}

public class Action 
{
    public string url {get; set;}
    public string path {get; set;}
    public Vector2 size {get; set;}
    public Crop crop {get; set;}
}

public class Settings 
{
    public string chromePath {get; set;}
    public List<Action> actions {get; set;}
}
