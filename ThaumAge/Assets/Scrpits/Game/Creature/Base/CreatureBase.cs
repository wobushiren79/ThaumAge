using UnityEditor;
using UnityEngine;

public class CreatureBase
{
    protected CreatureCptBase creature;
    public CreatureBase(CreatureCptBase creature)
    {
        this.creature = creature;
    }
}