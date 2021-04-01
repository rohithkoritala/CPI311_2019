using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class GameConstants
    {
        // public const float PlayerSpeed = 2500f;
        //public const float PlayerRotationSpeed = 3.0f;

        //camera constants
        public const float CameraHeight = 13000.0f;
        public const float PlayfieldSizeX = 14000f;
        public const float PlayfieldSizeY = 12000f;
        //asteroid constants
        public const int Numgods = 2;
        public const int Numvirus = 20;
        public const float AsteroidMinSpeed = 100.0f;
        public const float AsteroidMaxSpeed = 300.0f;
        public const float AsteroidMinSpeed1 = 250.0f;
        public const float AsteroidMaxSpeed1 = 350.0f;
        public const float AsteroidSpeedAdjustment = 5.0f;
        public const float AsteroidBoundingSphereScale = 0.95f;  //95% size

        public const float ShipBoundingSphereScale = 0.5f;  //50% size
        public const int NumBullets = 30;
        public const float BulletSpeedAdjustment = 100.0f;
        public const float Playerspeed = 3500f;

        public const float Health = 10f;

        
           
    }
}
