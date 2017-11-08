using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Reflection;
using Microsoft.Xna.Framework;
using System.Security.Cryptography;

namespace DushyUpgrade {
    public class UpgradeFramework {
        internal static Mod mod {
            get { return ModLoader.GetMod("DushyUpgrade"); }
        }
    }
}