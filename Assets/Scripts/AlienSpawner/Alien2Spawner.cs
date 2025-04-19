using UnityEngine;

public class Alien2Spawner : BaseAlienSpawner
{
  public override float minX { get; set; } = -5f; // Minimum X position for spawning
  public override float maxX { get; set; } = 5f; // Maximum X position for spawning 
  public override float minY { get; set; } = 0f; // Minimum Y position for spawning
  public override float maxY { get; set; } = 5f; // Maximum Y position for spawning
}


