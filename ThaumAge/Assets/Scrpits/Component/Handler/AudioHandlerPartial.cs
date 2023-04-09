using UnityEditor;
using UnityEngine;

public partial class AudioHandler
{
    protected int[] normalDigSound = new int[]
    {
        310
        // 354,304,305,306,307,308,309
    };

    protected int[] walkSound = new int[]
    {
        1201,1202,1203,1204,1205,1206,1207,1208,1209,1210
    };


    /// <summary>
    /// 播放普通的挖掘声
    /// </summary>
    public void PlayNormalDigSound()
    {
        int randomSound = Random.Range(0, normalDigSound.Length);
        PlaySound(normalDigSound[randomSound]);
    }

    /// <summary>
    /// 播放走路音效
    /// </summary>
    public void PlayWalkSound()
    {
        int randomSound = Random.Range(0, walkSound.Length);
        PlaySound(walkSound[randomSound]);
    }
}