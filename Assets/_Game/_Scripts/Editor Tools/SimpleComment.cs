#if UNITY_EDITOR
using UnityEngine;

/// <summary>
/// Simple script to add comments to GameObjects in the editor.
/// </summary>
public class SimpleComment : MonoBehaviour
{
    [TextArea(5, 25)]
    // [Tooltip("Doesn't do anything. Just allows to type and store comments in the inspector")]
    [SerializeField] private string Notes = "Write your comment here...";
}
#endif
