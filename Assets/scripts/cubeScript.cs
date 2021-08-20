using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts
{

    public class cubeScript : MonoBehaviour
    {
        public enum modes { REAL_BORDER, NONE };

        modes whichMode;
        public void setValue(modes _m)
        {
            whichMode = _m;
        }

        public modes getMode()
        {
            return whichMode;
        }

    }
}