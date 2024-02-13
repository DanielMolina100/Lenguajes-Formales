using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoLFA
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                AnalizarArchivo(openFileDialog1.FileName);
            }
            else
            {
                MessageBox.Show(@"Error al leer el archivo.");
            }
        }
        private void AnalizarArchivo(string file)
        {
            textBox1.Text = file;
            Gramatica.Select(0, Gramatica.Lines.Length);
            Gramatica.SelectionColor = Color.Black;

            try
            {
                int line1 = 0;
                string text = File.ReadAllText(file);
                //Send line
                textBox2.Text = Class.ExpresionRegular.Archivo(text, ref line1);
                Gramatica.Text = text;

                if (textBox2.Text.Contains("Correcto"))
                {
                    textBox2.BackColor = Color.LightGray;
                    textBox2.ForeColor = Color.Green;

                }
                else
                {
                    textBox2.BackColor = Color.LightGray;
                    textBox2.ForeColor = Color.Crimson;

                    //Ubicacion del error
                    int lineCounter = 0;

                    foreach (string line in Gramatica.Lines)
                    {
                        if (line1 - 1 == lineCounter)
                        {
                            Gramatica.Select(Gramatica.GetFirstCharIndexFromLine(lineCounter), line.Length);
                            Gramatica.SelectionColor = Color.Red;
                        }
                        lineCounter++;
                    }
                }

            }
            catch (Exception ex)
            {

                textBox2.BackColor = Color.LightGray;
                textBox2.ForeColor = Color.Crimson;
                textBox2.Text = @"Error en TOKENS";
                MessageBox.Show(ex.Message);

                //muestra en rojo los tokens
                int lineCounter = 0;

                foreach (string line in Gramatica.Lines)
                {
                    if (line.Contains("TOKEN"))
                    {
                        Gramatica.Select(Gramatica.GetFirstCharIndexFromLine(lineCounter), line.Length);
                        Gramatica.SelectionColor = Color.Red;
                    }
                    lineCounter++;

                }
            }
        }
    }
}
