using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace parser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = @"C:\Users\Spencer Wright\Desktop\Lrad20200811.txt";
            button1.Text = "Parse";
            textBox2.Text = @"C:\Users\Spencer Wright\Desktop\Test.txt";
            textBox3.Text = "VBatt:";
            button2.Text = "4 hour shift";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox2.Text) == true)
            {
                DialogResult button = MessageBox.Show("Do you want to overwrite text file?", "Warning", MessageBoxButtons.OKCancel);
                if (button != DialogResult.OK)
                {
                    return;
                }
            }
            Console.WriteLine("Parse started");
            ReadFile(textBox1.Text);
            Console.WriteLine("Parse Complete");
            MessageBox.Show("Parse Complete", "Done", MessageBoxButtons.OKCancel);
        }
        public void ReadFile(string fileName)
        {
            string data = "";
            List<string> one = new List<string>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    if( line.Contains("PowerModule: Status:") == false)
                    {
                        continue;
                    }

                    string[] words = line.Split(' ');
                    for (int index = 0; index < words.Length; index++)
                    {
                        string word = words[index];                        
                        if (word == "State:")
                        {
                            if (words[index + 1] == "Solar_Charging," || words[index + 2] == "Solar_Charging,")
                            {
                                data += "green ";
                            }

                            else
                            {
                                data += "red ";

                            }
                        }
                        else if(word =="VBatt:" )
                        {
                            string value = words[index + 1];
                            data += value;
                            data = data.Substring(1,data.Length-2);
                            one.Add(data);
                            data = "";
                        }
                        
                        else if (index == 0)
                        {
                            string date = words[index];
                            string[] value = date.Split(',');
                            DateTime original = DateTime.Parse(value[0]);
                            DateTime update = original.Add(new TimeSpan(-7, 0, 0));
                            //date = update.ToString("MM/dd/yyyy hh:mm");
                            date = update.ToString();
                            data += " ";
                            data += date + " ";
                            string time = words[index + 1];
                            data += time + " ";
                        }
                    }
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            WriteToText(one);
        }

        public void WriteToText(List<string> value)
        {
            string filename = textBox2.Text;
            try
            {
                System.IO.File.AppendAllLines(filename, value);
                
            }
            
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox2.Text) == true)
            {
                DialogResult button = MessageBox.Show("Do you want to overwrite text file?", "Warning", MessageBoxButtons.OKCancel);
                if (button != DialogResult.OK)
                {
                    return;
                }
            }
            Console.WriteLine("Parse started");
            TimeFile(textBox1.Text);
            Console.WriteLine("Parse Complete");
            MessageBox.Show("Parse Complete", "Done", MessageBoxButtons.OKCancel);
        }
        public void TimeFile(string fileName)
        {
            List<string> one = new List<string>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    string[] words = line.Split(' ');
                    string setup = words[0] +" " +words[1];
                    DateTime original = DateTime.Parse(setup);
                    DateTime update = original.Add(new TimeSpan(-4, 0, 0));
                    string data = update.ToString("MM/dd/yyyy HH:mm");
                    string power = " "+words[2]+" "+words[3]+" "+words[4];
                    data += power;
                    one.Add(data);                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            WriteToText(one);
        }
    }
}
    
    