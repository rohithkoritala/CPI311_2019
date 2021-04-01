using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine
{
    public class Component
    {
        public GameObject GameObject { get; set; }
        public Transform Transform { get; set; }
    }
}
