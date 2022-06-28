using System;
using CTRE.Gadgeteer.Module;
using CTRE.HERO;

namespace Ping_Pong_Robot_Code {
    class SignalLight {
        DriverModule driverModule;

        public LightState state = LightState.Red;

        long lastTime = 0;
        long timeSinceLastToggle = 0;
        bool flashOn = false;

        public SignalLight() {
            driverModule = new DriverModule(IO.Port5);

            lastTime = DateTime.Now.Ticks;
        }

        public void Update() {
            timeSinceLastToggle += DateTime.Now.Ticks - lastTime;

            lastTime = DateTime.Now.Ticks;

            while (timeSinceLastToggle >= 2500000) {
                timeSinceLastToggle -= 2500000;
                flashOn = !flashOn;
            }

            /*if (state & LightState.Red != 0) {
                driverModule.Set(1, true);
                driverModule.Set(2, false);
                driverModule.Set(3, false);
            }*/

            switch (state) {
                case LightState.Red:
                    driverModule.Set(1, true);
                    driverModule.Set(2, false);
                    driverModule.Set(3, false);
                    break;
                case LightState.Yellow:
                    driverModule.Set(1, false);
                    driverModule.Set(2, true);
                    driverModule.Set(3, false);
                    break;
                case LightState.Green:
                    driverModule.Set(1, false);
                    driverModule.Set(2, false);
                    driverModule.Set(3, true);
                    break;
                case LightState.RedFlash:
                    driverModule.Set(1, flashOn);
                    driverModule.Set(2, false);
                    driverModule.Set(3, false);
                    break;
                case LightState.YellowFlash:
                    driverModule.Set(1, false);
                    driverModule.Set(2, flashOn);
                    driverModule.Set(3, false);
                    break;
                case LightState.GreenFlash:
                    driverModule.Set(1, false);
                    driverModule.Set(2, false);
                    driverModule.Set(3, flashOn);
                    break;
            }
        }

        [Flags]
        public enum LightState {
            Red = 2^0,
            RedFlash = 2^1,
            Yellow = 2^2,
            YellowFlash = 2^3,
            Green = 2^4,
            GreenFlash = 2^5
        };
    }
}
