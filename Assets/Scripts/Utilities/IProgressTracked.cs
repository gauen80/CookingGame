using System;
using UnityEngine;

public interface IProgressTracked 
{
    public event EventHandler<ProgressChangedArg> OnProgressChanged;

    public interface ProgressChangedArg
    {
        //진행도 바 활성화
        public bool IsBarActive();

        //음식이 타거나 오버쿡될때 경고 표시
        public bool IsWarning();
    }

    public float GetNormalizedProgress();
}
