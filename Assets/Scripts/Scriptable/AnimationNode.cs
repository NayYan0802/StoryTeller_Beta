using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animation Node", menuName = "Node/Animation Node")]
public class AnimationNode : ScriptableObject
{
    private SSAnimation animation;

    [SerializeField] private Transform StartPos;
    [SerializeField] private Transform EndPos;

    [SerializeField] private float timeSum;

}
