using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR4._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        class Figure                             //базовый класс
        {

            virtual public bool isClicked(MouseEventArgs e)
            {
                return true;
            }
            virtual public void DrawEllipse(Panel panel1, Graphics g)
            {
            }
            virtual public void DoCheckTrue()
            {
            }
            virtual public void DoCheckFalse()
            {
            }
            virtual public bool isChecked()
            {
                return true;
            }
        }

        class CCircle : Figure//класс круга
        {
            private int x;
            private int y;
            private int rad;
            private bool Checked;
            public CCircle(int x, int y, int rad)
            {
                this.x = x;
                this.y = y;
                this.rad = rad;
                Checked = false;
            }


            override public bool isClicked(MouseEventArgs e)
            {
                if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= 80 * 80)
                {
                    Checked = true;
                    return true;
                }
                else
                {
                    Checked = false;
                    return false;
                }
            }

            public override void DrawEllipse(Panel panel1, Graphics g)
            {
                Rectangle rect = new Rectangle(x - rad, y - rad, rad * 2, rad * 2);
                if (Checked == true)
                {
                    g.DrawEllipse(Pens.Tomato, rect);
                }
                else
                {
                    g.DrawEllipse(Pens.Black, rect);
                }
            }
            override public void DoCheckTrue()
            {
                Checked = true;
            }
            override public void DoCheckFalse()
            {
                Checked = false;
            }
            override public bool isChecked()
            {
                return Checked;
            }

        }

        class MyStorage
        {
            private int size;
            Figure[] storage;              //массив указателей
            public MyStorage()
            {
                size = 0;
            }
            public MyStorage(int size)
            {
                this.size = size;
                storage = new Figure[size];
            }
            public void SetObjects(int index, Figure obj)
            {
                storage[index] = obj;
            }
            public void AddObject(Figure obj)
            {
                Figure[] new_storage = new Figure[size + 1];
                for (int i = 0; i < size; i++)
                {
                    new_storage[i] = storage[i];
                }
                storage = new_storage;
                storage[size] = obj;
                size = size + 1;
            }
            public void DeleteObject(int index)
            {

                Figure[] new_storage = new Figure[size - 1];
                for (int i = 0; i < index; i++)
                {
                    new_storage[i] = storage[i];
                }
                for (int i = index; i < size - 1; i++)
                {
                    new_storage[i] = storage[i + 1];
                }
                size = size - 1;
                storage = new_storage;

            }
            public bool isCheckedStorage(MouseEventArgs e)
            {
                for (int i = 0; i < size; i++)
                {
                    if (storage[i].isClicked(e) == true)
                    {
                        storage[i].DoCheckTrue();
                        return true;
                    }
                }
                return false;
            }
            public void DeleteCheckObject(MyStorage storage)         //удаление выделенных кругов 
            {
                for (int i = 0; i < size; i++)
                {
                    if (this.storage[i].isChecked() == true)
                    {
                        storage.DeleteObject(i);
                        i = i - 1;
                    }
                }
            }
            public void NotChecked()
            {
                for (int i = 0; i < size; i++)
                {
                    storage[i].DoCheckFalse();
                }
            }
            public void Draw(Panel panel1, Graphics g)
            {
                for (int i = 0; i < size; i++)
                {
                    storage[i].DrawEllipse(panel1, g);
                }
            }
        }

        MyStorage storage = new MyStorage();
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            storage.Draw(panel1, g);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (storage.isCheckedStorage(e) == false)
            {

                CCircle ellipse = new CCircle(e.X, e.Y, 40);
                storage.AddObject(ellipse);
                storage.NotChecked();
            }
            else
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    storage.isCheckedStorage(e);
                }
                else
                {
                    storage.NotChecked();
                    storage.isCheckedStorage(e);
                }
            }
            Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
            {
                Graphics g = panel1.CreateGraphics();
                storage.DeleteCheckObject(storage);
                g.Clear(Color.White);
                Refresh();
            }
        }
    }
}
