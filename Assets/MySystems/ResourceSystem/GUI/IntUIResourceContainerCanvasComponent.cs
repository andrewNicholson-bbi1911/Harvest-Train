using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceSystem
{
    public class IntUIResourceContainerCanvasComponent : UIResourceContainerCanvas<IntResourceData, int>
    {
        private void Start()
        {
            InitValues();
        }
    }


}

