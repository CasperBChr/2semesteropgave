using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Size
    {
        public byte ClothingEUSize { get; set; }
        public byte ClothingUSSize { get; set; }
        public float ShoeEUSize { get; set; }
        public Size(byte clothingEUSize, byte clothingUSSize, float shoeEUSize)
        {
            ClothingEUSize = clothingEUSize;
            ClothingUSSize = clothingUSSize;
            ShoeEUSize = shoeEUSize;
        }
        public Size(byte clothingUSSize)
        {
            ClothingUSSize = clothingUSSize;
            ClothingEUSize = CalculateEUSize(clothingUSSize);
        }
        private byte CalculateEUSize(byte clothingUSSize)
        {
            // Placeholder implementation - replace with actual size conversion logic
            return clothingUSSize;
        }
    }
}
