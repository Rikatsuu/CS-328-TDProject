using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    public void SetQuality(int qIndex) {
        
        // Using Unity's Quality System, we can change the game's quality settings

        QualitySettings.SetQualityLevel(qIndex);

    }

}
