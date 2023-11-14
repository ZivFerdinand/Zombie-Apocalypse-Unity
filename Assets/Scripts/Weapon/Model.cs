using System;
using UnityEngine;

public class Model : MonoBehaviour
{
    #region - Weapons -

    public enum WeaponFireType
    {
        SemiAuto,
        FullyAuto
    }

    [Serializable]
    public class WeaponSettingsModel
    {
        [Header("Sway")]
        public float SwayAmount;
        public bool SwayYInverted;
        public bool SwayXInverted;
        public float SwaySmoothing;
        public float SwayResetSmoothing;
        public float SwayClampX;
        public float SwayClampY;
    }

    #endregion
}
