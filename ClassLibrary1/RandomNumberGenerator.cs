using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class RandomNumberGenerator
    {
        private static Random _random = new Random();
        
        public static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max + 1); 
        }
    }
}