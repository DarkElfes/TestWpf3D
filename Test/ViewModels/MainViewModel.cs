using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Test.Models;
using Test.Utilities;

namespace Test.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        protected Window _window;
        public Viewport3D Model { get; } = new Viewport3D();
        public Camera3D Camera3D;

        public object Window
        {
            get => _window;
            set
            {
                _window = value as Window;

                _window.KeyDown += Window_PreviewKeyDown;
                _window.MouseMove += Window_PreviewMouseMove;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }

        public MainViewModel()
        {

            Mouse.OverrideCursor = Cursors.None;
            Camera3D = new(Model)
            {
                Camera = new()
                {
                    Position = new Point3D(-24, 24, 0),
                    FieldOfView = 60,
                    LookDirection = new Vector3D(1, -1, 0)
                },
            };
            Model.Camera = Camera3D.Camera;

            Cube redCube = new Cube(new Point3D(10, 5, 0), new Vector3D(5, 3, 5), Brushes.Red);
            Cube blueCube = new Cube(new Point3D(5, 0, 0), new Vector3D(10, 4, 10), Brushes.Blue);

            ModelVisual3D directionLightModel = new ModelVisual3D()
            {
                Content = new DirectionalLight(Color.FromRgb(255, 255, 255), new Vector3D(-1, -1, -1)),
            }; 

            Model.Children.Add(directionLightModel);
            Model.Children.Add(redCube);
            Model.Children.Add(blueCube);
            //Model3D model = LoadModel();
        }

        #region Camera Handlers
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e) 
            => Camera3D.MoveBy(e.Key);

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
            => Camera3D.Rotate(e.GetPosition(Model));
        #endregion

        #region FPS indicator
        public int fps { get; set; }
        private int frameCount;
        private DateTime lastCheckTime = DateTime.Now;
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            // Збільшити лічильник кадрів
            frameCount++;

            // Перевірити, чи пройшло 1 секунда з останньої перевірки
            var currentTime = DateTime.Now;
            var timeDiff = currentTime - lastCheckTime;
            if (timeDiff.TotalSeconds >= 1)
            {
                // Вивести кількість кадрів на секунду
                fps = (int)(frameCount / timeDiff.TotalSeconds);
                OnPropertyChanged(nameof(fps));

                // Скинути лічильник
                frameCount = 0;
                lastCheckTime = currentTime;
            }
        }
        #endregion
    }
}
