using CTRE.Phoenix;
using CTRE.Phoenix.Controller;
using System;

namespace Ping_Pong_Robot_Code {
    public class Program {
        static SignalLight signalLight;

        static TableController tableController;

        static GameController controller;

        static bool demo = true;
        static bool modeDown = false;

        public static void Main() {
            signalLight = new SignalLight();
            signalLight.Update();

            controller = new GameController(UsbHostDevice.GetInstance(1), 0);

            tableController = new TableController();

            signalLight.state = SignalLight.LightState.GreenFlash;

            while (true) {
                //Debug.Print("1:" + controller.GetAxis(1));

                if (controller.GetConnectionStatus() == UsbDeviceConnection.Connected) {
                    if (controller.GetButton(4))
                        tableController.ZeroAll();
                    if (controller.GetButton(3))
                        tableController.ReleaseZero();

                    if (controller.GetButton(2)) {
                        if (!modeDown)
                            demo = !demo;

                        modeDown = true;
                    } else
                        modeDown = false;

                    //if (demo)
                    //    Demo();
                    //else
                        Manual();
                }

                signalLight.state = (tableController.zeroed) ? SignalLight.LightState.GreenFlash : SignalLight.LightState.YellowFlash;
                signalLight.Update();
                Watchdog.Feed();
            }
        }

        public static void Manual() {
            tableController.height = controller.GetAxis(2) * 0.035f + 0.1f;
            tableController.xRot = (float) (controller.GetAxis(1) * 15 * (System.Math.PI / 180));
            tableController.yRot = (float) (controller.GetAxis(0) * 15 * (System.Math.PI / 180));
        }

        public static void Demo() {

        }
    }
}
