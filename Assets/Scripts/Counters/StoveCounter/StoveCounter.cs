using System;
using UnityEngine;

public class StoveCounter : ClearCounter, IProgressTracked
{
    public event EventHandler<IProgressTracked.ProgressChangedArg> OnProgressChanged;

    public float GetNormalizedProgress()
    {
        throw new NotImplementedException();
    }
}

