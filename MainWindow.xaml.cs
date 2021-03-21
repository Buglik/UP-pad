using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;

namespace UP_pad
{

    public partial class MainWindow : Window
    {
        private Controller controller;
        private IMouseSimulator mouseSimulator;
        private Timer timer;

        private bool aWasDown, bWasDown;

        public MainWindow()
        {
            InitializeComponent();
            
            controller = new Controller(UserIndex.One);
            mouseSimulator = new InputSimulator().Mouse;
            timer = new Timer(obj => Update());

            Start();
        }


        private void Start()
        {
            timer.Change(0, 1000 / 60);
        }
        private void Update()
        {
            controller.GetState(out var state);
            Movement(state);
            Scroll(state);
            LeftBtn(state);
            RightBtn(state);
            ColorChange(state);
        }

        private void ColorChange(State state)
        {
            var xDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X);
            var yDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y);

            if(xDown)
            {
                drawingBoard.Dispatcher.Invoke(() => { drawingBoard.DefaultDrawingAttributes.Color = Colors.Blue; });
            }
            if(yDown)
            {
                drawingBoard.Dispatcher.Invoke(() => { drawingBoard.DefaultDrawingAttributes.Color = Colors.Yellow; });
            }
        }
        private void Movement(State state)
        {
            var x = state.Gamepad.LeftThumbX / 2_000;
            var y = state.Gamepad.LeftThumbY / 2_000;
            mouseSimulator.MoveMouseBy(x, -y);
            xAxis.Dispatcher.Invoke(() => { xAxis.Text = (state.Gamepad.LeftThumbX.ToString()); });
            yAxis.Dispatcher.Invoke(() => { yAxis.Text = (state.Gamepad.LeftThumbY.ToString()); });
        }

        private void Scroll(State state)
        {
            var x = state.Gamepad.RightThumbX / 10_000;
            var y = state.Gamepad.RightThumbY / 10_000;
            mouseSimulator.HorizontalScroll(x);
            mouseSimulator.VerticalScroll(y);
        }

        private void LeftBtn(State state)
        {
            var aDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);

            if (aDown && !aWasDown)
            {
                mouseSimulator.LeftButtonDown();
                CheckA.Dispatcher.Invoke(() => { CheckA.IsChecked = true; });
            }

            if (!aDown && aWasDown)
            {
                mouseSimulator.LeftButtonUp();
                CheckA.Dispatcher.Invoke(() => { CheckA.IsChecked = false; });
            }
            aWasDown = aDown;
            
        }


        private void CheckBox_A(object sender, RoutedEventArgs e)
        {
            
        }

        private void CheckBox_B(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            drawingBoard.Strokes.Clear();
            drawingBoard.DefaultDrawingAttributes.Color = Colors.Red;
        }

        private void RightBtn(State state)
        {
            var bDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
            if (bDown && !bWasDown)
            {
                mouseSimulator.RightButtonDown();
                CheckB.Dispatcher.Invoke(() => { CheckB.IsChecked = true; });
            }
            if (!bDown && bWasDown)
            {
                mouseSimulator.RightButtonUp();
                CheckB.Dispatcher.Invoke(() => { CheckB.IsChecked = false; });
            }
            bWasDown = bDown;
        }

    }

    

}
