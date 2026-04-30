using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace lab2
{
    public class Form1 : Form
    {
        private ShapeList shapes = new ShapeList();
        private ShapeRenderer renderer = new ShapeRenderer();
        private ShapeFactoryRegistry registry = new ShapeFactoryRegistry();

        private Panel canvas;
        private ComboBox comboBox;
        private TextBox inputBox;
        private Button addButton;

        public Form1()
        {
            InitFactories();
            InitUI();
        }

        /// Register all shape factories
        private void InitFactories()
        {
            registry.Register("Line", a => new LineShape(a[0], a[1], a[2], a[3]));
            registry.Register("Rectangle", a => new RectangleShape(a[0], a[1], a[2], a[3]));
            registry.Register("Square", a => new SquareShape(a[0], a[1], a[2]));
            registry.Register("Ellipse", a => new EllipseShape(a[0], a[1], a[2], a[3]));
            registry.Register("Circle", a => new CircleShape(a[0], a[1], a[2]));
            registry.Register("Triangle", a => new TriangleShape(a[0], a[1], a[2], a[3], a[4], a[5]));
        }

        private void InitUI()
        {
            this.Text = "Graphic Editor";
            this.Size = new Size(800, 600);

            canvas = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(600, 500),
                BackColor = Color.White
            };
            canvas.Paint += Canvas_Paint;
            Controls.Add(canvas);

            comboBox = new ComboBox
            {
                Location = new Point(620, 30),
                Width = 150
            };
            comboBox.Items.AddRange(registry.Keys.ToArray());
            comboBox.SelectedIndex = 0;
            Controls.Add(comboBox);

            inputBox = new TextBox
            {
                Location = new Point(620, 70),
                Width = 150,
                Text = "50,50,100,60"
            };
            Controls.Add(inputBox);

            addButton = new Button
            {
                Text = "Add",
                Location = new Point(620, 110)
            };
            addButton.Click += AddButton_Click;
            Controls.Add(addButton);
        }

        /// Draw all shapes
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            foreach (var shape in shapes.GetAll())
            {
                renderer.Draw(e.Graphics, shape);
            }
        }

        /// Handle shape creation
        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                string type = comboBox.SelectedItem.ToString();

                int[] args = inputBox.Text
                    .Split(',')
                    .Select(x => int.Parse(x.Trim()))
                    .ToArray();

                Shape shape = registry.Create(type, args);

                shapes.Add(shape);

                canvas.Invalidate();
            }
            catch
            {
                MessageBox.Show("Invalid input!");
            }
        }
    }
}