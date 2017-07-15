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

    public string GetArmaureName()
    {
        return name;
    }

    void Start()
    {
        UnityFactory.factory.LoadDragonBonesData(name + "/" + name + "_ske");
        UnityFactory.factory.LoadTextureAtlasData(name + "/" + name + "_tex");

        armatureComponent = UnityFactory.factory.BuildArmatureComponent(armature, name, null, "", transform.gameObject);
        //        armatureComponent.animation.timeScale *= 0.5f;

<<<<<<< HEAD:TowerDefense/Assets/Graphix.cs


=======
>>>>>>> cc9ff0d120fd59b20d3b68ff9ef5d2cd7119c5bf:TowerDefense/Assets/Scripts/Graphix.cs
        armatureComponent.animation.Play(status);

    }
}