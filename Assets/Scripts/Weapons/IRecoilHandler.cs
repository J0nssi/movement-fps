using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecoilHandler 
{
    void AddRecoil(float upRecoil, float sideRecoil, bool auto);

    void ResetRecoil();

}
