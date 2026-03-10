using UnityEngine;
using System.Collections.Generic;

public class TabNavigationView : MonoBehaviour
{
   [Tooltip("В порядке, соответствующем TabType!")]
   public List<GameObject> tabPanels;

   public void ShowTab(TabType tab)
   {
      for (var i = 0; i < tabPanels.Count; i++)
      {
         var tabPanel = tabPanels[i];
         var isActive = i == (int) tab;
         tabPanel.SetActive(isActive);

         if (isActive)
            tabPanel.transform.SetXY(0f, 0f);
         else
            tabPanel.transform.SetXY(-100f, 0f);
      }
   }
}