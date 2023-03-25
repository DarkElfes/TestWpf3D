using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace Test.Models
{
    public class Camera3D
    {
        //Properties
        public PerspectiveCamera Camera { get; set; } = new();

        private Viewport3D Model { get; set; }
        private double _speedRotate = 0.05;

        public double SpeedRotate
        {
            get => _speedRotate;
            set => ValidateValue.Double(ref _speedRotate, value, 0, 10);
        }


        //Constructors
        public Camera3D(Viewport3D model)
        {
            Model = model;
        }

        public Vector3D GetYawAxis() => Camera.UpDirection;
        public Vector3D GetRollAxis() => Camera.LookDirection;
        public Vector3D GetPitchAxis() => Vector3D.CrossProduct(Camera.UpDirection, Camera.LookDirection);

        private void Move(Vector3D axis, double step)
            => Camera.Position += axis * step;

        public void MoveBy(Key key)
        {
            double step = Camera.FieldOfView / 30d;
            switch (key)
            {
                case Key.W: Move(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) ? GetYawAxis() : GetRollAxis(), +step); break;
                case Key.S: Move(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) ? GetYawAxis() : GetRollAxis(), -step); break;
                case Key.A: Move(GetPitchAxis(), +step); break;
                case Key.D: Move(GetPitchAxis(), -step); break;
            }
        }

        public void Rotate(Point currentMousePosition)
        {
            double deltaX = currentMousePosition.X - Model.ActualWidth / 2;
            double deltaY = Model.ActualHeight / 2 - currentMousePosition.Y;

            Matrix3D matrix = new();
            matrix.RotateAt(new(new Vector3D(0, -1, 0), deltaX * _speedRotate), Camera.Position);
            matrix.RotateAt(new(-Vector3D.CrossProduct(Camera.UpDirection, Camera.LookDirection), deltaY * _speedRotate), Camera.Position);
            Camera.LookDirection *= matrix;

            WinAPI.SetCursorPos((int)(Model.ActualWidth / 2), (int)(Model.ActualHeight / 2));
        }

    }
}
