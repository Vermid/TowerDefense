using DragonBones;
using UnityEngine;

public class Graphix : MonoBehaviour
{
    [SerializeField]
    private string armaturName;

    [SerializeField]
    private string status;

    [SerializeField]
    private string armature;

    private UnityArmatureComponent armatureComponent;

    public UnityArmatureComponent GetCurrentArmature()
    {
        return armatureComponent;
    }

    public string GetArmaureName()
    {
        return armaturName;
    }

    void Start()
    {
        UnityFactory.factory.LoadDragonBonesData(armaturName + "/" + armaturName + "_ske");
        UnityFactory.factory.LoadTextureAtlasData(armaturName + "/" + armaturName + "_tex");

        armatureComponent = UnityFactory.factory.BuildArmatureComponent(armature, armaturName, null, "", transform.gameObject);
        //        armatureComponent.animation.timeScale *= 0.5f;

        armatureComponent.animation.Play(status);

    }
}