using UnityEngine;

[CreateAssetMenu(menuName = "Configs/TabConfig")]
public class TabConfigSO : ScriptableObject
{
   public TabType startTab;
}

public enum TabType
{
   Clicker   = 0,
   Weather   = 1,
   DogBreeds = 2
}