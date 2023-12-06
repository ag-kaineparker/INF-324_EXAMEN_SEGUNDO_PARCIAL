using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication24
{
    public partial class Form1 : Form
    {
        private List<PointInfo> pointsList = new List<PointInfo>();
        private int L = 10;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            button3.Click += button3_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Archivos JPG|*.JPG|Archivos BMP|*.bmp";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != string.Empty)
            {
                Bitmap bmp = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = bmp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Obtener información del color en las coordenadas actuales del ratón
            PointInfo pointInfo = GetPointInfo(pictureBox1.PointToClient(MousePosition));

            textBox1.Text = pointInfo.Color.R.ToString();
            textBox2.Text = pointInfo.Color.G.ToString();
            textBox3.Text = pointInfo.Color.B.ToString();

        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            // Obtener información del color en las coordenadas del clic del ratón
            PointInfo pointInfo = GetPointInfo(e.Location);

            // Verificar si el punto ya está en la lista antes de agregarlo
            if (!IsPointInList(pointInfo))
            {
                // Agregar la información del punto a la lista
                pointsList.Add(pointInfo);

                // Guardar la lista en un archivo de texto con cada punto
                SavePointsListToFile();
            }

            // Actualizar la visualización del punto en la imagen
            // UpdateImage();
        }
        private bool IsPointInList(PointInfo pointInfo)
        {
            // Verificar si el punto ya está en la lista
            foreach (PointInfo existingPoint in pointsList)
            {
                if (existingPoint.Location == pointInfo.Location && existingPoint.Color == pointInfo.Color)
                {
                    return true;
                }
            }
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Cargar la lista de puntos desde el archivo sin borrar la lista existente
            LoadPointsListFromFile();

            // Mostrar los colores de los primeros tres puntos en los TextBox
            if (pointsList.Count >= 1)
            {
                textBox1.BackColor = pointsList[0].Color;
            }

            if (pointsList.Count >= 2)
            {
                textBox2.BackColor = pointsList[1].Color;
            }

            if (pointsList.Count >= 3)
            {
                textBox3.BackColor = pointsList[2].Color;
            }

            // Obtener la imagen actual del PictureBox
            Bitmap bmp = new Bitmap(pictureBox1.Image);

            // Crear un objeto Random para generar colores aleatorios
            Random random = new Random();

            // Iterar sobre cada punto y pintarlo con su lógica de coloreado
            foreach (PointInfo pointInfo in pointsList)
            {
                // Pintar el punto actual en la imagen
                ProcessPoint(bmp, pointInfo, random);
            }

            // Establecer la nueva imagen en el PictureBox
            pictureBox1.Image = bmp;
        }

        private void ProcessPoint(Bitmap bmp, PointInfo pointInfo, Random random)
        {
            // Crear una nueva imagen con las mismas dimensiones que la original
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);

            // Colores aleatorios para el punto actual
            Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

            // Iterar sobre todos los píxeles de la imagen
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    // Obtener el color del píxel en la posición (i, j)
                    Color c = bmp.GetPixel(i, j);

                    // Calcular las diferencias en los canales de color (RGB) entre el color del píxel y el color del punto
                    int distanceR = Math.Abs(c.R - pointInfo.Color.R);
                    int distanceG = Math.Abs(c.G - pointInfo.Color.G);
                    int distanceB = Math.Abs(c.B - pointInfo.Color.B);

                    // Si está dentro del rango de 10 tonos en términos de RGB, ajustar el color al color aleatorio
                    if (distanceR <= 10 && distanceG <= 10 && distanceB <= 10)
                    {
                        // Establecer el píxel en la nueva imagen con el color aleatorio
                        bmp2.SetPixel(i, j, randomColor);
                    }
                    else
                    {
                        // Mantener el color original del píxel
                        bmp2.SetPixel(i, j, c);
                    }
                }
            }

            // Copiar la nueva imagen (bmp2) a la imagen original (bmp)
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(i, j, bmp2.GetPixel(i, j));
                }
            }
        }


        private void ProcessPoint(Bitmap bmp, Bitmap bmp2, PointInfo pointInfo, Random random)
        {
            // Colores aleatorios para el punto actual
            Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

            // Iterar sobre todos los píxeles de la imagen
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    // Obtener el color del píxel en la posición (i, j)
                    Color c = bmp.GetPixel(i, j);

                    // Calcular las diferencias en los canales de color (RGB) entre el color del píxel y el color del punto
                    int distanceR = Math.Abs(c.R - pointInfo.Color.R);
                    int distanceG = Math.Abs(c.G - pointInfo.Color.G);
                    int distanceB = Math.Abs(c.B - pointInfo.Color.B);

                    // Si está dentro del rango de 10 tonos en términos de RGB, ajustar el color al color aleatorio
                    if (distanceR <= 10 && distanceG <= 10 && distanceB <= 10)
                    {
                        // Establecer el píxel en la nueva imagen con el color aleatorio
                        bmp2.SetPixel(i, j, randomColor);
                    }
                    else
                    {
                        // Mantener el color original del píxel
                        bmp2.SetPixel(i, j, c);
                    }
                }
            }
        }




        private Color[] GenerateRandomColors()
        {
            Color[] randomColors = new Color[3];
            Random random = new Random();

            for (int i = 0; i < 3; i++)
            {
                randomColors[i] = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }

            return randomColors;
        }

        private void SavePointsListToFile()
        {
            using (StreamWriter writer = new StreamWriter("pointsList.txt"))
            {
                foreach (PointInfo pointInfo in pointsList)
                {
                    writer.WriteLine($"{pointInfo.Location.X},{pointInfo.Location.Y},{pointInfo.Color.R},{pointInfo.Color.G},{pointInfo.Color.B}");
                }
            }
        }

        private void LoadPointsListFromFile()
        {
            pointsList.Clear();
            try
            {
                using (StreamReader reader = new StreamReader("pointsList.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 5)
                        {
                            int x = int.Parse(parts[0]);
                            int y = int.Parse(parts[1]);
                            int r = int.Parse(parts[2]);
                            int g = int.Parse(parts[3]);
                            int b = int.Parse(parts[4]);

                            pointsList.Add(new PointInfo { Location = new Point(x, y), Color = Color.FromArgb(r, g, b) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción (por ejemplo, el archivo no existe)
            }
        }

        private void SaveRandomColorsToFile(Color[] randomColors)
        {
            using (StreamWriter writer = new StreamWriter("randomColors.txt"))
            {
                foreach (Color color in randomColors)
                {
                    writer.WriteLine($"{color.R},{color.G},{color.B}");
                }
            }
        }

        private Color[] LoadRandomColorsFromFile()
        {
            Color[] randomColors = new Color[3];

            try
            {
                using (StreamReader reader = new StreamReader("randomColors.txt"))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        string line = reader.ReadLine();
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            int r = int.Parse(parts[0]);
                            int g = int.Parse(parts[1]);
                            int b = int.Parse(parts[2]);

                            randomColors[i] = Color.FromArgb(r, g, b);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción (por ejemplo, el archivo no existe)
            }

            return randomColors;
        }

        private PointInfo GetPointInfo(Point location)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);

            int Rmc = 0, Gmc = 0, Bmc = 0;

            for (int i = location.X - L / 2; i < location.X + L / 2; i++)
            {
                for (int j = location.Y - L / 2; j < location.Y + L / 2; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                    Rmc += c.R;
                    Gmc += c.G;
                    Bmc += c.B;
                }
            }

            Rmc = (int)Rmc / (L * L);
            Gmc = (int)Gmc / (L * L);
            Bmc = (int)Bmc / (L * L);

            return new PointInfo { Location = location, Color = Color.FromArgb(Rmc, Gmc, Bmc) };
        }
      
        private class PointInfo
        {
            public Point Location { get; set; }
            public Color Color { get; set; }
        }
    }
}
