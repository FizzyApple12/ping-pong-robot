using CTRE.Phoenix.MotorControl;
using System;

namespace Ping_Pong_Robot_Code {
    class Config {

        public static class Motors {
            public static float P = 0.035f;
            public static float I = 0.0003f;
            public static float D = 0.7f;
            public static float FF = 0;//0.00002f;

            public static int maxError = 10;
            public static float peakOutput = 1f;

            public static SupplyCurrentLimitConfiguration supplyCurrentLimit = new SupplyCurrentLimitConfiguration(true, 32, 32, 0.001f);
            public static StatorCurrentLimitConfiguration statorCurrentLimit = new StatorCurrentLimitConfiguration(true, 32, 32, 0.001f);

            public static float ticksPerTravel = 5250;
        }

        public static class Table {
            public static float linkageDistance = 0.18875324875369f;
            public static float linkageInnerLength = 0.04445000000000f;
            public static float linkageOuterLength = 0.10160000000000f;
        }

        public static class BallControl {
            public static class Positional {
                public static float P = 0f;
                public static float I = 0f;
                public static float D = 0f;
                public static float FF = 0f;
            }

            public static class Height {
                public static float P = 0f;
                public static float I = 0f;
                public static float D = 0f;
                public static float FF = 0f;
            }
        }
    }
}
