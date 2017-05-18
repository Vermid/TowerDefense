using System;
using UnityEngine;
using DragonBones;
public class TestAnim : MonoBehaviour
{
    void Start()
    {
        UnityFactory.factory.LoadDragonBonesData("Monster2/Monster2_ske");
        UnityFactory.factory.LoadTextureAtlasData("Monster2/Monster2_tex");

        UnityArmatureComponent armatureComponent = UnityFactory.factory.BuildArmatureComponent("Armature", "Monster2", null, "", transform.gameObject);
        //        armatureComponent.animation.timeScale *= 0.5f;

        Debug.Log(armatureComponent);

        armatureComponent.animation.Play("move");
    }
}