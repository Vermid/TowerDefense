using DragonBones;
using UnityEngine;

public class Graphix : MonoBehaviour
{
    [SerializeField]
    private string name;

    [SerializeField]
    private string status;

    [SerializeField]
    private string armature;

    private UnityArmatureComponent armatureComponent;

    public UnityArmatureComponent GetCurrentArmature()
    {
        return armatureComponent;
    }
    void Start()
    {
        UnityFactory.factory.LoadDragonBonesData(name + "/" + name + "_ske");
        UnityFactory.factory.LoadTextureAtlasData(name + "/" + name + "_tex");

        armatureComponent = UnityFactory.factory.BuildArmatureComponent(armature, name, null, "", transform.gameObject);
        //        armatureComponent.animation.timeScale *= 0.5f;

        Debug.Log(armatureComponent);

        armatureComponent.animation.Play(status);
    }
}