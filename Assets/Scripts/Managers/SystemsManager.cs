using Player.Tools;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class SystemsManager : Singleton<SystemsManager>
    {
        [Header("Systems")] 
        [SerializeField] private WireTool wireTool;
    }
}