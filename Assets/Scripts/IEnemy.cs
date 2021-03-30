using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void Damage(int amount);

    void Kill();
}
