using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.MotorControl.CAN;
using System;

namespace Ping_Pong_Robot_Code {
    class TableController {
        TalonFX motor0; //   3
        TalonFX motor1; //  / \
        TalonFX motor2; // 1---2

        public bool zeroed { get; private set; }

        public TableController() {
            zeroed = false;

            motor0 = new TalonFX(0);

            motor0.Config_kP(0, Config.Motors.P);
            motor0.Config_kI(0, Config.Motors.I);
            motor0.Config_kD(0, Config.Motors.D);
            motor0.Config_kF(0, Config.Motors.FF);

            motor0.ConfigAllowableClosedloopError(Config.Motors.maxError);

            motor0.ConfigClosedLoopPeakOutput(Config.Motors.peakOutput);

            motor0.SetNeutralMode(NeutralMode.Coast);

            //motor0.ConfigSupplyCurrentLimit(Config.Motors.supplyCurrentLimit, 100);
            //motor0.ConfigStatorCurrentLimit(Config.Motors.statorCurrentLimit, 100);

            motor1 = new TalonFX(1);

            motor1.Config_kP(0, Config.Motors.P);
            motor1.Config_kI(0, Config.Motors.I);
            motor1.Config_kD(0, Config.Motors.D);
            motor1.Config_kF(0, Config.Motors.FF);

            motor1.ConfigAllowableClosedloopError(Config.Motors.maxError);

            motor1.ConfigClosedLoopPeakOutput(Config.Motors.peakOutput);

            motor1.SetNeutralMode(NeutralMode.Coast);

            //motor1.ConfigSupplyCurrentLimit(Config.Motors.supplyCurrentLimit, 100);
            //motor1.ConfigStatorCurrentLimit(Config.Motors.statorCurrentLimit, 100);

            motor2 = new TalonFX(2);

            motor2.Config_kP(0, Config.Motors.P);
            motor2.Config_kI(0, Config.Motors.I);
            motor2.Config_kD(0, Config.Motors.D);
            motor2.Config_kF(0, Config.Motors.FF);

            motor2.ConfigAllowableClosedloopError(Config.Motors.maxError);

            motor2.ConfigClosedLoopPeakOutput(Config.Motors.peakOutput);

            motor2.SetNeutralMode(NeutralMode.Coast);

            //motor2.ConfigSupplyCurrentLimit(Config.Motors.supplyCurrentLimit, 100);
            //motor2.ConfigStatorCurrentLimit(Config.Motors.statorCurrentLimit, 100);
        }

        public void ZeroAll() {
            if (zeroed) return;

            motor0.SetSelectedSensorPosition(0);
            motor1.SetSelectedSensorPosition(0);
            motor2.SetSelectedSensorPosition(0);

            motor0.SetNeutralMode(NeutralMode.Brake);
            motor1.SetNeutralMode(NeutralMode.Brake);
            motor2.SetNeutralMode(NeutralMode.Brake);

            zeroed = true;
        }

        public void ReleaseZero() {
            if (!zeroed) return;

            motor0.SetNeutralMode(NeutralMode.Coast);
            motor1.SetNeutralMode(NeutralMode.Coast);
            motor2.SetNeutralMode(NeutralMode.Coast);

            motor0.Set(ControlMode.PercentOutput, 0);
            motor1.Set(ControlMode.PercentOutput, 0);
            motor2.Set(ControlMode.PercentOutput, 0);

            zeroed = false;
        }

        public float height {
            get {
                return GetHeight();
            }
            set {
                _height = value;
                if (zeroed) GoTo(_height, _xRot, _yRot);
            }
        }
        private float _height = 0;

        public float xRot {
            get {
                return GetXRot();
            }
            set {
                _xRot = value;
                if (zeroed) GoTo(_height, _xRot, _yRot);
            }
        }
        private float _xRot = 0;

        public float yRot {
            get {
                return GetYRot();
            }
            set {
                _yRot = value;
                if (zeroed) GoTo(_height, _xRot, _yRot);
            }
        }
        private float _yRot = 0;

        private float GetHeight() {
            return (float) Kinematics.TableHeightForward(Kinematics.ActuatorForward(TicksToRadians(motor0.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Kinematics.ActuatorForward(TicksToRadians(motor1.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Kinematics.ActuatorForward(TicksToRadians(motor2.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Config.Table.linkageDistance);
        }
        private float GetXRot() {
            return (float) Kinematics.TableRotationXForward(Kinematics.ActuatorForward(TicksToRadians(motor0.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Kinematics.ActuatorForward(TicksToRadians(motor1.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Kinematics.ActuatorForward(TicksToRadians(motor2.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Config.Table.linkageDistance);
        }
        private float GetYRot() {
            return (float) Kinematics.TableRotationYForward(Kinematics.ActuatorForward(TicksToRadians(motor0.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Kinematics.ActuatorForward(TicksToRadians(motor1.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Kinematics.ActuatorForward(TicksToRadians(motor2.GetSelectedSensorPosition()), Config.Table.linkageInnerLength, Config.Table.linkageOuterLength), Config.Table.linkageDistance);
        }

        private void GoTo(float height, float xRot, float yRot) {
            Microsoft.SPOT.Debug.Print("h:" + height + "\tx:" + xRot + "\ty:" + yRot);

            if (!zeroed) return;

            double motor0Length = Kinematics.TableLength1Reverse(height, xRot, yRot, Config.Table.linkageDistance);
            float motor0Target = (float) Clamp(
                                         RadiansToTicks(
                                         Kinematics.ActuatorReverse(motor0Length, Config.Table.linkageInnerLength, Config.Table.linkageOuterLength)),
                                         Config.Motors.ticksPerTravel, 0);

            double motor1Length = Kinematics.TableLength2Reverse(height, xRot, yRot, Config.Table.linkageDistance);
            float motor1Target = (float) Clamp(
                                         RadiansToTicks(
                                         Kinematics.ActuatorReverse(motor1Length, Config.Table.linkageInnerLength, Config.Table.linkageOuterLength)),
                                         Config.Motors.ticksPerTravel, 0);

            double motor2Length = Kinematics.TableLength3Reverse(height, xRot, yRot, Config.Table.linkageDistance);
            float motor2Target = (float) Clamp(
                                         RadiansToTicks(
                                         Kinematics.ActuatorReverse(motor2Length, Config.Table.linkageInnerLength, Config.Table.linkageOuterLength)),
                                         Config.Motors.ticksPerTravel, 0);

            motor0.Set(ControlMode.Position, -motor0Target);
            motor1.Set(ControlMode.Position, -motor1Target);
            motor2.Set(ControlMode.Position, -motor2Target);

            //Microsoft.SPOT.Debug.Print("0:" + motor0.GetOutputCurrent() + "\t1:" + motor1.GetOutputCurrent() + "\t2:" + motor2.GetOutputCurrent() + "\tt:" + (motor2.GetOutputCurrent() + motor1.GetOutputCurrent() + motor0.GetOutputCurrent()));
            //Microsoft.SPOT.Debug.Print("0l:" + motor0Length + "\t1l:" + motor1Length + "\t2l:" + motor2Length);
            //Microsoft.SPOT.Debug.Print("0t:" + motor0Target + "\t1t:" + motor1Target + "\t2t:" + motor2Target);
        }

        private double TicksToRadians(double t) {
            return ((t / (2048 * 7)) * (2 * Math.PI));

        }
        private double RadiansToTicks(double r) {
            return ((r / (2 * Math.PI)) * (2048 * 7));
        }

        private double Clamp(double x, double max, double min) {
            if (x > max) return max;
            if (x < min) return min;
            return x;
        }

        public class Kinematics {
            public static double ActuatorForward(double theta, double lengthInner, double lengthOuter) {
                return Math.Sqrt(Math.Pow(lengthInner, 2) + Math.Pow(lengthOuter, 2) - 2 * lengthInner * lengthOuter * Math.Cos(Math.PI - theta - Math.Asin(lengthInner * (Math.Sin(theta) / lengthOuter))));
            }
            public static double ActuatorReverse(double distance, double lengthInner, double lengthOuter) {
                return Math.Acos((Math.Pow(distance, 2) + Math.Pow(lengthInner, 2) - Math.Pow(lengthOuter, 2)) / (2 * lengthInner * distance));
            }

            public static double ComputeInnerRadius(double side) {
                return (side * Math.Sqrt(3)) / 6;
            }
            public static double ComputeOuterRadius(double side) {
                return (side / Math.Sqrt(3));
            }

            public static double TableRotationXForward(double length1, double length2, double length3, double side) {
                double outerRadius = ComputeOuterRadius(side);
                double innerRadius = ComputeInnerRadius(side);
                return Math.Asin(((length1 + length2) / 2 - length3) / (outerRadius + innerRadius));
            }
            public static double TableRotationYForward(double length1, double length2, double length3, double side) {
                return Math.Asin((length1 - length2) / side);
            }
            public static double TableHeightForward(double length1, double length2, double length3, double side) {
                double outerRadius = ComputeOuterRadius(side);
                double innerRadius = ComputeInnerRadius(side);
                return length3 + outerRadius * (((length1 + length2) / 2 - length3) / (outerRadius + innerRadius));
            }

            public static double TableLength1Reverse(double height, double rotationX, double rotationY, double side) {
                double outerRadius = ComputeOuterRadius(side);
                double innerRadius = ComputeInnerRadius(side);
                return height + (innerRadius * Math.Sin(rotationX)) - (0.5 * side * Math.Sin(rotationY));
            }
            public static double TableLength2Reverse(double height, double rotationX, double rotationY, double side) {
                double outerRadius = ComputeOuterRadius(side);
                double innerRadius = ComputeInnerRadius(side);
                return height + (innerRadius * Math.Sin(rotationX)) + (0.5 * side * Math.Sin(rotationY));
            }
            public static double TableLength3Reverse(double height, double rotationX, double rotationY, double side) {
                double outerRadius = ComputeOuterRadius(side);
                double innerRadius = ComputeInnerRadius(side);
                return height - (outerRadius * Math.Sin(rotationX));
            }
        }
    }
}
