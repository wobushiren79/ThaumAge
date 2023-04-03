using UnityEditor;
using UnityEngine;

public partial class AudioHandler
{
    protected int[] normalDigSound = new int[]
    {
        354,304,305,306,307,308,309
    };

    /// <summary>
    /// 播放普通的挖掘声
    /// </summary>
    public void PlayNormalDigSound()
    {
        int randomSound = Random.Range(0, normalDigSound.Length);
        PlaySound(normalDigSound[randomSound]);
    }
}