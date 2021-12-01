using UnityEditor;
using UnityEngine;

public class AICreatureIntentIdle : AIBaseIntent
{
    public AICreatureIntentIdle() : base()
    {
        aiIntent = AICreatureIntentEnum.Idle;
    }
}