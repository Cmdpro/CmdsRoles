using System;
using System.Linq;
using System.Text;
using CrowdedRoles.Options;
using CrowdedRoles.Roles;
using Reactor;
using TMPro;
using UnityEngine;

namespace CrowdedRoles.Components
{
    [RegisterInIl2Cpp]
    public class CustomGameOptions : MonoBehaviour
    {
        public CustomGameOptions(IntPtr ptr) : base(ptr)
        {
        }
        
        public TextMeshPro Text { get; private set; } = null!;

        public void Start()
        {
           
        }

        internal void UpdateText()
        {
            
        }
    }
}