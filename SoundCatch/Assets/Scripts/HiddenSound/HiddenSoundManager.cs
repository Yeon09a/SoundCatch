using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenSoundManager : MonoBehaviour
{
    public AudioInfoSO _ClickRightBlock;
    public AudioEventChannelSO _ClickRightBlockEC;
    public AudioInfoSO _ClickWrongBlock;
    public AudioEventChannelSO _ClickWrongBlockEC;
    
    public void ClickRightBlock() //정답 블록
    {
        _ClickRightBlockEC.RaisePlayAudio(_ClickRightBlock);
    }
     public void WrongBlock()
    {
        _ClickWrongBlockEC.RaisePlayAudio(_ClickRightBlock);
    }
     public void ClickBackGround()
    {
        Debug.Log("배경입니다");
    }
   
}
