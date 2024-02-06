﻿using Microsoft.Xna.Framework.Graphics;
using SpriteSheetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WinterJam
{
    internal class Item : GameObject
    {
        public int MaxDurability { get; set; }
        public int Durability { get; set; }

        public Item(int durability, Texture2D texture, GameObject parent)
        {
            Durability = durability;
            MaxDurability = durability;


        }
        public virtual void Update()
        {
            //Durability--;

            if (Durability <= 0)
                IsActive = false;
        }
    }
}
