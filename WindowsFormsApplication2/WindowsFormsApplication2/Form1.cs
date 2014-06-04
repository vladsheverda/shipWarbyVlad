using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace ShipWar
{
    public partial class Form1 : Form
    {
        private bool boy_go = false;
        private int palyb3 = 0, palyb2 = 0, palyb1 = 0, you_shot = 0, com_shot = 0, ship_com = 10, ship_user = 10, boy_ship = 0, kill = 0, koor_x = 0, koor_y = 0,
          nice_shot_com = 0, ok_shot_x, ok_shot_y, Who_game = 0, hod = 0;
        private const int
          MR = 10, // кол-во клеток по вертикали
          MC = 10, // кол-во клеток по горизонтали
          W = 30,  // ширина клетки
          H = 30;  // высота клетки
        private int status;
        // 0 - промах,
        // 1 - попал,
        // 2 – убил
        // -1 - клутка, по которой компьютер уже производил выстрел
        private int[,] Field2 = new int[MR + 4, MC + 4];//поле компьютера
        private int[,] Field1 = new int[MR + 4, MC + 4];// поле пользователя


        private System.Drawing.Graphics g;
        private System.Drawing.Graphics g1;
        public Form1()
        {
            InitializeComponent();

            textBox3.Text = ship_com.ToString();
            textBox2.Text = ship_user.ToString();
            textBox1.Text = you_shot.ToString();
            textBox4.Text = com_shot.ToString();
            g = panel1.CreateGraphics();//  поле пользователя
            g1 = panel3.CreateGraphics();// поле компьютера
            button3.Enabled = false;
            Label f = label50;
            f.Location = new Point(1000, 100);
            newGame();
        }

        private void showField(Graphics graph)// создание поля
        {
            for (int row = 1; row <= MR; row++)
            {
                {
                    for (int col = 1; col <= MC; col++)
                    {
                        this.CreateCell(graph, row, col, status);
                    }
                }
            }
        }

        private void CreateCell(Graphics graph, int row, int col, int status)
        {
            int x, y;// координаты левого верхнего угла клетки

            x = (col - 1) * W + 1;
            y = (row - 1) * H + 1; 
            if (boy_ship == 10)
            {
                button3.Enabled = true;// кнопка начала игры
            }
            if (Field1[row, col] == 0) // поле
            {
                g.FillRectangle(SystemBrushes.ControlLight, x - 1, y - 1, W, H);
            }
            if (Field1[row, col] == 1) // корабль
            {
                g.FillRectangle(SystemBrushes.ControlDark, x - 1, y - 1, W, H);// цвет кораблей, при постановке на поле
            }
            if (Field2[row, col] == 0 && status == 1)//0 - пустая клетка
            {
                graph.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                Field2[row, col] = -1;
            }
            if (Field2[row, col] == 1 && status == 1)
            {
                graph.FillRectangle(SystemBrushes.ControlDarkDark, x - 1, y - 1, W, H);// цвет при попадании в корабль
                Field2[row, col] = 5;
                kill_or_nokill(row, col);

            }
            g.DrawRectangle(Pens.Black, x - 1, y - 1, W, H); // цвет линий левого поля
            g1.DrawRectangle(Pens.Black, x - 1, y - 1, W, H); // цвет линий правого поля
            if (Field2[row, col] == -1 && status == 1)
            {
                hod_com();
            }
            status = 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            showField(g);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            showField(g1);
        }

        private void panel3_MouseClick_1(object sender, MouseEventArgs e) // преобразуем координаты мыши в индексы
        // клетки поля, в которой был сделан щелчок;
        // (e.X, e.Y) - координаты точки формы,
        // в которой была нажата кнопка мыши;  
        {
            if (hod != 1)
            {
                int row = (int)(e.Y / H) + 1,
                    col = (int)(e.X / W) + 1;
                if (e.Button == MouseButtons.Left)
                {
                    if (Field2[row, col] != -1 && Field2[row, col] != 5 && hod != 1)
                    {
                        you_shot++;
                        textBox1.Text = you_shot.ToString();
                        status = 1;
                        this.CreateCell(g1, row, col, status);// g1 для поля компьютера, чтобы прорисовывалось куда стреляет пользователь
                    }
                }
            }
        }

        public void newGame() // вид на поле игры
        {
            int x = 0, y = 0;
            palyb3 = 0; palyb2 = 0; palyb1 = 0; you_shot = 0; com_shot = 0; ship_com = 10; ship_user = 10; boy_ship = 0;
            for (int i = 0; i < MR + 4; i++)
            {
                for (int j = 0; j < MC + 4; j++)
                {
                    Field2[i, j] = 0;
                    Field1[i, j] = 0;
                    x = (i - 1) * W + 1;
                    y = (j - 1) * H + 1;
                    g.FillRectangle(SystemBrushes.ControlLight, x - 1, y - 1, W, H);
                    g1.FillRectangle(SystemBrushes.ControlLight, x - 1, y - 1, W, H);
                    g.DrawRectangle(Pens.Black, x - 1, y - 1, W, H);// Pens.Black - цвет линий квадратиков
                    g1.DrawRectangle(Pens.Black, x - 1, y - 1, W, H);
                }
            }
            status = 0;
            boy_go = false;
            Label fe = label50;
            fe.Location = new Point(650, 28);
            Panel f = panel8;// поле поле компьютера
            f.Location = new Point(524, 55);

            // корабли до расстановки на поле
            Ship f1 = ship1;
            f1.Location = new Point(412, 310);
            Ship f2 = ship2;
            f2.Location = new Point(412, 345);
            Ship f3 = ship3;
            f3.Location = new Point(377, 345);
            Ship f4 = ship4;
            f4.Location = new Point(377, 310);
            Ship f5 = ship5;
            f5.Size = new Size(60, 30);
            f5.Location = new Point(377, 266);
            Ship f6 = ship6;
            f6.Size = new Size(60, 30);
            f6.Location = new Point(377, 230);
            Ship f7 = ship7;
            f7.Size = new Size(60, 30);
            f7.Location = new Point(377, 194);
            Ship f8 = ship8;
            f8.Size = new Size(90, 30);
            f8.Location = new Point(377, 158);
            Ship f9 = ship9;
            f9.Size = new Size(90, 30);
            f9.Location = new Point(377, 121);
            Ship f10 = ship10;
            f10.Size = new Size(120, 30);
            f10.Location = new Point(377, 81);
            textBox1.Text = you_shot.ToString();
            textBox2.Text = ship_user.ToString();
            textBox4.Text = com_shot.ToString();
            textBox3.Text = ship_com.ToString();
            RandomBoat();
        }

        private void RandomBoat()// рандомное расстановка кораблей компьютером
        {
            pal4();
            while (palyb3 != 2)
                pal3();
            while (palyb2 != 3)
                pal2();
            while (palyb1 != 4)
                pal1();
        }


        // методы для проверки, убит корабль или нет
        private int kill_or_nokill(int r, int c)
        {
            int sum = 0, Flag = 0;
            for (int i = 0; i < MR; i++)
            {
                for (int j = 0; j < MC; j++)
                {
                    sum = 0;
                    sum = Field2[i, j] + Field2[i, j + 1] + Field2[i, j + 2] + Field2[i, j + 3];
                    if (sum == 16)
                    {
                        Flag = 1;
                    }
                    sum = Field2[i, j] + Field2[i + 1, j] + Field2[i + 2, j] + Field2[i + 3, j];
                    if (sum == 16)
                    {
                        Flag = 1;
                    }
                }
            }

            if ((Field2[r, c - 1] == 1 || Field2[r, c + 1] == 1 || Field2[r - 1, c] == 1 || Field2[r + 1, c] == 1 || Flag == 1) || ((Field2[r, c - 1] == 5) && (Field2[r, c - 2] == 1)) || ((Field2[r, c + 1] == 5) && (Field2[r, c + 2] == 1)) || ((Field2[r - 1, c] == 5) && (Field2[r - 2, c] == 1)) || ((Field2[r + 1, c] == 5) && (Field2[r + 2, c] == 1)) || ((Field2[r, c - 1] == 5) && (Field2[r, c - 2] == 1)) || ((Field2[r, c + 1] == 5) && (Field2[r, c + 2] == 1)) || ((Field2[r - 1, c] == 5) && (Field2[r - 2, c] == 1)) || ((Field2[r + 1, c] == 5) && (Field2[r + 2, c] == 1))) //значит карабль не убит
            {
                Field2[r, c] = 5;
            }
            else // корабль убит
            {
                ship_com--;
                textBox3.Text = ship_com.ToString();
                around();
                the_end();
            }
            return 0;
        }

        private int kill_or_nokill2(int r, int c)
        {
            int sum = 0, Flag = 0;
            for (int i = 0; i < MR; i++)
            {
                for (int j = 0; j < MC; j++)
                {
                    sum = 0;
                    sum = Field1[i, j] + Field1[i, j + 1] + Field1[i, j + 2] + Field1[i, j + 3];
                    if (sum == 16)
                    {
                        Flag = 1;
                    }
                    sum = Field1[i, j] + Field1[i + 1, j] + Field1[i + 2, j] + Field1[i + 3, j];
                    if (sum == 16)
                    {
                        Flag = 1;
                    }
                }
            }
            if ((Field1[r, c - 1] == 1 || Field1[r, c + 1] == 1 || Field1[r - 1, c] == 1 || Field1[r + 1, c] == 1 || Flag == 1) || ((Field1[r, c - 1] == 5) && (Field1[r, c - 2] == 1)) || ((Field1[r, c + 1] == 5) && (Field1[r, c + 2] == 1)) || ((Field1[r - 1, c] == 5) && (Field1[r - 2, c] == 1)) || ((Field1[r + 1, c] == 5) && (Field1[r + 2, c] == 1)) || ((Field1[r, c - 1] == 5) && (Field1[r, c - 2] == 1)) || ((Field1[r, c + 1] == 5) && (Field1[r, c + 2] == 1)) || ((Field1[r - 1, c] == 5) && (Field1[r - 2, c] == 1)) || ((Field1[r + 1, c] == 5) && (Field1[r + 2, c] == 1)))//значит карабль не убит
            {
                Field1[r, c] = 5;
                kill = 1;//попал но не убил
                nice_shot_com++;
                koor_x = r;
                koor_y = c;
                ok_shot_x = r;
                ok_shot_y = c;
                Thread.Sleep(700);
            }
            else
            {
                kill = 2;//попал и убил
                ship_user--;
                textBox2.Text = ship_user.ToString();
                the_end();
                around2();
            }
            return 0;
        }


        // методы расстановки кораблей компьютером
        private int pal4()
        {
            Random rand = new Random();
            int k1, k2, k3;
            k1 = rand.Next(1, 3);
            //1 горизонталь
            //2 вертикаль
            k2 = rand.Next(1, 8);
            k3 = rand.Next(1, 11);
            //клетка начала коробля
            if (k1 == 1)
            {
                Field2[k3, k2] = 1;
                Field2[k3, k2 + 1] = 1;
                Field2[k3, k2 + 2] = 1;
                Field2[k3, k2 + 3] = 1;
            }

            if (k1 == 2)
            {
                Field2[k2, k3] = 1;
                Field2[k2 + 1, k3] = 1;
                Field2[k2 + 2, k3] = 1;
                Field2[k2 + 3, k3] = 1;
            }
            return 0;
        }

        private int pal3()
        {
            int k1, k2, k3;
            Random rnd = new Random();
            k1 = rnd.Next(1, 3);
            //1 горизонталь
            //2 вертикаль

            k2 = rnd.Next(1, 9);
            k3 = rnd.Next(1, 11);

            if (k1 == 1 && Field2[k3, k2 - 1] != 1 && Field2[k3, k2] != 1 && Field2[k3, k2 + 1] != 1 && Field2[k3, k2 + 2] != 1 && Field2[k3, k2 + 3] != 1
                     && Field2[k3 - 1, k2 - 1] != 1 && Field2[k3 - 1, k2] != 1 && Field2[k3 - 1, k2 + 1] != 1 && Field2[k3 - 1, k2 + 2] != 1 && Field2[k3 - 1, k2 + 3] != 1 && Field2[k3 - 1, k2] != 1
                     && Field2[k3 + 1, k2 - 1] != 1 && Field2[k3 + 1, k2] != 1 && Field2[k3 + 1, k2 + 1] != 1 && Field2[k3 + 1, k2 + 2] != 1 && Field2[k3 + 1, k2 + 3] != 1 && Field2[k3 + 1, k2] != 1)
            {
                Field2[k3, k2] = 1;
                Field2[k3, k2 + 1] = 1;
                Field2[k3, k2 + 2] = 1;
                palyb3++;
            }

            if (k1 == 2 && Field2[k2 - 1, k3] != 1 && Field2[k2, k3] != 1 && Field2[k2 + 1, k3] != 1 && Field2[k2 + 2, k3] != 1 && Field2[k2 + 3, k3] != 1
                    && Field2[k2 - 1, k3 - 1] != 1 && Field2[k2, k3 - 1] != 1 && Field2[k2 + 1, k3 - 1] != 1 && Field2[k2 + 2, k3 - 1] != 1 && Field2[k2 + 3, k3 - 1] != 1 && Field2[k2, k3 - 1] != 1
                    && Field2[k2 - 1, k3 + 1] != 1 && Field2[k2, k3 + 1] != 1 && Field2[k2 + 1, k3 + 1] != 1 && Field2[k2 + 2, k3 + 1] != 1 && Field2[k2 + 3, k3 + 1] != 1 && Field2[k2, k3 + 1] != 1
                )
            {
                Field2[k2, k3] = 1;
                Field2[k2 + 1, k3] = 1;
                Field2[k2 + 2, k3] = 1;
                palyb3++;
            }
            return 0;
        }

        private int pal2()
        {
            int k1, k2, k3;
            Random rand = new Random();
            k1 = rand.Next(1, 3);
            //1 горизонталь
            //2 вертикаль

            k2 = rand.Next(1, 9);
            k3 = rand.Next(1, 11);

            if (k1 == 1 && Field2[k3, k2 - 1] != 1 && Field2[k3, k2] != 1 && Field2[k3, k2 + 1] != 1 && Field2[k3, k2 + 2] != 1
                     && Field2[k3 - 1, k2 - 1] != 1 && Field2[k3 - 1, k2] != 1 && Field2[k3 - 1, k2 + 1] != 1 && Field2[k3 - 1, k2 + 2] != 1
                     && Field2[k3 + 1, k2 - 1] != 1 && Field2[k3 + 1, k2] != 1 && Field2[k3 + 1, k2 + 1] != 1 && Field2[k3 + 1, k2 + 2] != 1)
            {
                Field2[k3, k2] = 1;
                Field2[k3, k2 + 1] = 1;
                palyb2++;
            }

            if (k1 == 2 && Field2[k2 - 1, k3] != 1 && Field2[k2, k3] != 1 && Field2[k2 + 1, k3] != 1 && Field2[k2 + 2, k3] != 1
                    && Field2[k2 - 1, k3 - 1] != 1 && Field2[k2, k3 - 1] != 1 && Field2[k2 + 1, k3 - 1] != 1 && Field2[k2 + 2, k3 - 1] != 1
                    && Field2[k2 - 1, k3 + 1] != 1 && Field2[k2, k3 + 1] != 1 && Field2[k2 + 1, k3 + 1] != 1 && Field2[k2 + 2, k3 + 1] != 1
               )
            {
                Field2[k2, k3] = 1;
                Field2[k2 + 1, k3] = 1;
                palyb2++;
            }
            return 0;
        }

        private int pal1()
        {
            int k1, k2, k3;
            Random rand = new Random();
            k1 = rand.Next(1, 3);
            //1 горизонталь
            //2 вертикаль

            k2 = rand.Next(1, 10);
            k3 = rand.Next(1, 10);

            if (k1 == 1 && Field2[k3, k2 - 1] != 1 && Field2[k3, k2] != 1 && Field2[k3, k2 + 1] != 1
                     && Field2[k3 - 1, k2 - 1] != 1 && Field2[k3 - 1, k2] != 1 && Field2[k3 - 1, k2 + 1] != 1
                     && Field2[k3 + 1, k2 - 1] != 1 && Field2[k3 + 1, k2] != 1 && Field2[k3 + 1, k2 + 1] != 1)
            {
                Field2[k3, k2] = 1;
                palyb1++;
            }

            if (k1 == 2 && Field2[k2 - 1, k3] != 1 && Field2[k2, k3] != 1 && Field2[k2 + 1, k3] != 1
                    && Field2[k2 - 1, k3 - 1] != 1 && Field2[k2, k3 - 1] != 1 && Field2[k2 + 1, k3 - 1] != 1
                    && Field2[k2 - 1, k3 + 1] != 1 && Field2[k2, k3 + 1] != 1 && Field2[k2 + 1, k3 + 1] != 1
                )
            {
                Field2[k2, k3] = 1;
                palyb1++;
            }
            return 0;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            for (int row = 1; row <= MR; row++)
                for (int col = 1; col <= MC; col++)
                {
                    if (Field2[row, col] == 1)
                    {
                        this.CreateCell(g1, row, col, 1);
                    }
                }
        }

        private void around()// окружности вокруг кораблей компьютера, после того как корабль был убит
        {
            int x, y;
            for (int i = 0; i < MR + 4; i++)
            {
                for (int j = 0; j < MC + 4; j++)
                {
                    if (Field2[i, j] == 5 && Field2[i, j + 1] != 1 && Field2[i, j - 1] != 1 && Field2[i + 1, j] != 1 && Field2[i - 1, j] != 1)
                    {
                        x = (j - 1) * W + 1;
                        y = (i - 1) * H + 1;
                        g1.FillRectangle(SystemBrushes.ControlText, x, y, W - 1, H - 1);
                        if (Field2[i - 1, j - 1] == 0)
                        {
                            x = (j - 1 - 1) * W + 1;
                            y = (i - 1 - 1) * H + 1;
                            g1.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field2[i - 1, j - 1] = -1;
                        }

                        if (Field2[i + 1, j + 1] == 0)
                        {
                            x = (j - 1 + 1) * W + 1;
                            y = (i - 1 + 1) * H + 1;
                            g1.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field2[i + 1, j + 1] = -1;
                        }

                        if (Field2[i - 1, j + 1] == 0)
                        {
                            x = (j - 1 + 1) * W + 1;
                            y = (i - 1 - 1) * H + 1;
                            g1.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field2[i - 1, j + 1] = -1;
                        }

                        if (Field2[i + 1, j - 1] == 0)
                        {
                            x = (j - 1 - 1) * W + 1;
                            y = (i - 1 + 1) * H + 1;
                            g1.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field2[i + 1, j - 1] = -1;
                        }

                        if (Field2[i, j + 1] == 0)
                        {
                            x = (j - 1 + 1) * W + 1;
                            y = (i - 1) * H + 1;
                            g1.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field2[i, j + 1] = -1;
                        }

                        if (Field2[i, j - 1] == 0)
                        {
                            x = (j - 1 - 1) * W + 1;
                            y = (i - 1) * H + 1;
                            g1.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field2[i, j - 1] = -1;
                        }

                        if (Field2[i + 1, j] == 0)
                        {
                            x = (j - 1) * W + 1;
                            y = (i - 1 + 1) * H + 1;
                            g1.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field2[i + 1, j] = -1;
                        }

                        if (Field2[i - 1, j] == 0)
                        {
                            x = (j - 1) * W + 1;
                            y = (i - 1 - 1) * H + 1;
                            g1.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field2[i - 1, j] = -1;
                        }
                    }
                }
            }

        }

        private void around2()// окружности вокруг кораблей пользователя, после того как корабль был убит
        {
            int x, y;
            for (int i = 0; i < MR + 4; i++)
            {
                for (int j = 0; j < MC + 4; j++)
                {
                    if (Field1[i, j] == 5 && Field1[i, j + 1] != 1 && Field1[i, j - 1] != 1 && Field1[i + 1, j] != 1 && Field1[i - 1, j] != 1) // для того чтобы вокруг кограбля когда его убили появлялись эллипы
                    {

                        x = (j - 1) * W + 1;
                        y = (i - 1) * H + 1;
                        g.FillRectangle(SystemBrushes.ControlText, x, y, W - 1, H - 1);// закрашивается цвет внутри квадратиков, ControlText - отвечает за цвет убитого корабля,(х,у)закрашивает координаты подбитого корабля
                        if (Field1[i - 1, j - 1] == 0) 
                        {
                            x = (j - 1 - 1) * W + 1;
                            y = (i - 1 - 1) * H + 1;
                            g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field1[i - 1, j - 1] = -1;
                        }

                        if (Field1[i + 1, j + 1] == 0)
                        {
                            x = (j - 1 + 1) * W + 1;
                            y = (i - 1 + 1) * H + 1;
                            g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field1[i + 1, j + 1] = -1;
                        }

                        if (Field1[i - 1, j + 1] == 0)
                        {
                            x = (j - 1 + 1) * W + 1;
                            y = (i - 1 - 1) * H + 1;
                            g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field1[i - 1, j + 1] = -1;
                        }

                        if (Field1[i + 1, j - 1] == 0)
                        {
                            x = (j - 1 - 1) * W + 1;
                            y = (i - 1 + 1) * H + 1;
                            g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field1[i + 1, j - 1] = -1;
                        }

                        if (Field1[i, j + 1] == 0)
                        {
                            x = (j - 1 + 1) * W + 1;
                            y = (i - 1) * H + 1;
                            g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field1[i, j + 1] = -1;
                        }

                        if (Field1[i, j - 1] == 0)
                        {
                            x = (j - 1 - 1) * W + 1;
                            y = (i - 1) * H + 1;
                            g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field1[i, j - 1] = -1;
                        }

                        if (Field1[i + 1, j] == 0)
                        {
                            x = (j - 1) * W + 1;
                            y = (i - 1 + 1) * H + 1;
                            g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field1[i + 1, j] = -1;
                        }

                        if (Field1[i - 1, j] == 0)
                        {
                            x = (j - 1) * W + 1;
                            y = (i - 1 - 1) * H + 1;
                            g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                            Field1[i - 1, j] = -1;
                        }
                    }
                }
            }
        }

        private int Check_Ship(int deck, int row, int col, int width, int height)// расстановка кораблей пользователем
        {
            if (deck == 1)
            {
                if (Field1[row, col] == 0 && Field1[row, col + 1] == 0 && Field1[row, col - 1] == 0 && Field1[row + 1, col + 1] == 0 && Field1[row + 1, col] == 0
                    && Field1[row + 1, col - 1] == 0 && Field1[row - 1, col + 1] == 0 && Field1[row - 1, col] == 0 && Field1[row - 1, col - 1] == 0)
                {
                    return 0;
                }
            }

            if (deck == 2)
            {
                if (width > 30)
                {
                    if (Field1[row, col] == 0 && Field1[row, col + 1] == 0 && Field1[row, col - 1] == 0 && Field1[row + 1, col + 1] == 0 && Field1[row + 1, col] == 0
                    && Field1[row + 1, col - 1] == 0 && Field1[row - 1, col + 1] == 0 && Field1[row - 1, col] == 0 && Field1[row - 1, col - 1] == 0
                        && Field1[row + 1, col + 2] == 0 && Field1[row, col + 2] == 0 && Field1[row - 1, col + 2] == 0)
                    {
                        return 0;
                    }
                }

                else
                {
                    if (Field1[row, col] == 0 && Field1[row, col + 1] == 0 && Field1[row, col - 1] == 0 && Field1[row + 1, col + 1] == 0 && Field1[row + 1, col] == 0
                    && Field1[row + 1, col - 1] == 0 && Field1[row - 1, col + 1] == 0 && Field1[row - 1, col] == 0 && Field1[row - 1, col - 1] == 0
                        && Field1[row + 2, col + 1] == 0 && Field1[row + 2, col] == 0 && Field1[row + 2, col - 1] == 0)
                    {
                        return 0;
                    }
                }
            }

            if (deck == 3)
            {
                if (width > 30)
                {
                    if (Field1[row, col] == 0 && Field1[row, col + 1] == 0 && Field1[row, col - 1] == 0 && Field1[row + 1, col + 1] == 0 && Field1[row + 1, col] == 0
                    && Field1[row + 1, col - 1] == 0 && Field1[row - 1, col + 1] == 0 && Field1[row - 1, col] == 0 && Field1[row - 1, col - 1] == 0
                        && Field1[row + 1, col + 2] == 0 && Field1[row, col + 2] == 0 && Field1[row - 1, col + 2] == 0
                        && Field1[row + 1, col + 3] == 0 && Field1[row, col + 3] == 0 && Field1[row - 1, col + 3] == 0)
                        return 0;
                }

                else
                {
                    if (Field1[row, col] == 0 && Field1[row, col + 1] == 0 && Field1[row, col - 1] == 0 && Field1[row + 1, col + 1] == 0 && Field1[row + 1, col] == 0
                    && Field1[row + 1, col - 1] == 0 && Field1[row - 1, col + 1] == 0 && Field1[row - 1, col] == 0 && Field1[row - 1, col - 1] == 0
                        && Field1[row + 2, col + 1] == 0 && Field1[row + 2, col] == 0 && Field1[row + 2, col - 1] == 0
                        && Field1[row + 3, col + 1] == 0 && Field1[row + 3, col] == 0 && Field1[row + 3, col - 1] == 0)
                    {
                        return 0;
                    }
                }

            }

            if (deck == 4)
            {
                if (width > 30)
                {
                    if (Field1[row, col] == 0 && Field1[row, col + 1] == 0 && Field1[row, col - 1] == 0 && Field1[row + 1, col + 1] == 0 && Field1[row + 1, col] == 0
                    && Field1[row + 1, col - 1] == 0 && Field1[row - 1, col + 1] == 0 && Field1[row - 1, col] == 0 && Field1[row - 1, col - 1] == 0
                        && Field1[row + 1, col + 2] == 0 && Field1[row, col + 2] == 0 && Field1[row - 1, col + 2] == 0
                        && Field1[row + 1, col + 3] == 0 && Field1[row, col + 3] == 0 && Field1[row - 1, col + 3] == 0
                        && Field1[row + 1, col + 4] == 0 && Field1[row, col + 4] == 0 && Field1[row - 1, col + 4] == 0)
                    {
                        return 0;
                    }
                }

                else
                {
                    if (Field1[row, col] == 0 && Field1[row, col + 1] == 0 && Field1[row, col - 1] == 0 && Field1[row + 1, col + 1] == 0 && Field1[row + 1, col] == 0
                    && Field1[row + 1, col - 1] == 0 && Field1[row - 1, col + 1] == 0 && Field1[row - 1, col] == 0 && Field1[row - 1, col - 1] == 0
                        && Field1[row + 2, col + 1] == 0 && Field1[row + 2, col] == 0 && Field1[row + 2, col - 1] == 0
                        && Field1[row + 3, col + 1] == 0 && Field1[row + 3, col] == 0 && Field1[row + 3, col - 1] == 0
                        && Field1[row + 4, col + 1] == 0 && Field1[row + 4, col] == 0 && Field1[row + 4, col - 1] == 0)
                    {
                        return 0;
                    }
                }
            }
            return 1;
        }



        // постановка кораблей пользователем
        private void ship1_MouseUp(object sender, MouseEventArgs e)
        {
            Point point = ship1.Location;
            if ((point.X > 60 && point.X < 350) && (point.Y > 65 && point.Y < 353))
            {
                int row = (int)(point.Y / H) - 1,// чтобы кораблик становлися в место, куда его ставит пользователь
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(1, row, col, 0, 0) == 0)
                {
                    Field1[row, col] = 1;// чтобы корабль отображался на поле, после того как его поставили на поле игры
                    boy_ship++;
                    Ship f = ship1;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void ship2_MouseUp(object sender, MouseEventArgs e)
        {
            Point point = ship2.Location;
            if ((point.X > 60 && point.X < 350) && (point.Y > 65 && point.Y < 353))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(1, row, col, 0, 0) == 0)
                {
                    Field1[row, col] = 1;
                    boy_ship++;
                    Ship f = ship2;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void ship3_MouseUp(object sender, MouseEventArgs e)
        {
            Point point = ship3.Location;
            if ((point.X > 60 && point.X < 350) && (point.Y > 65 && point.Y < 353))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(1, row, col, 0, 0) == 0)
                {
                    Field1[row, col] = 1;
                    boy_ship++;
                    Ship f = ship3;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void ship4_MouseUp(object sender, MouseEventArgs e)
        {
            Point point = ship4.Location;
            if ((point.X > 60 && point.X < 350) && (point.Y > 65 && point.Y < 353))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(1, row, col, 0, 0) == 0)
                {
                    Field1[row, col] = 1;
                    boy_ship++;
                    Ship f = ship4;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void ship5_MouseUp(object sender, MouseEventArgs e)
        {
            Ship p = ship5;
            Point point = ship5.Location;
            if ((point.X > 60 && point.X + p.Size.Width < 380) && (point.Y > 65 && point.Y + p.Size.Height < 390))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(2, row, col, p.Size.Width, p.Size.Height) == 0)
                {
                    if (p.Size.Width > 30)
                    {
                        Field1[row, col] = 1;
                        Field1[row, col + 1] = 1;
                        boy_ship++;
                    }
                    else
                    {
                        Field1[row, col] = 1;
                        Field1[row + 1, col] = 1;
                        boy_ship++;
                    }
                    Ship f = ship5;
                    f.Location = new Point(-200, 100);

                }
            }
        }

        private void ship6_MouseUp(object sender, MouseEventArgs e)
        {
            Ship p = ship6;
            Point point = ship6.Location;
            if ((point.X > 60 && point.X + p.Size.Width < 380) && (point.Y > 65 && point.Y + p.Size.Height < 390))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(2, row, col, p.Size.Width, p.Size.Height) == 0)
                {
                    if (p.Size.Width > 30)
                    {
                        Field1[row, col] = 1;
                        Field1[row, col + 1] = 1;
                        boy_ship++;
                    }
                    else
                    {
                        Field1[row, col] = 1;
                        Field1[row + 1, col] = 1;
                        boy_ship++;
                    }
                    Ship f = ship6;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void ship7_MouseUp(object sender, MouseEventArgs e)
        {
            Ship p = ship7;
            Point point = ship7.Location;
            if ((point.X > 60 && point.X + p.Size.Width < 380) && (point.Y > 65 && point.Y + p.Size.Height < 390))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(2, row, col, p.Size.Width, p.Size.Height) == 0)
                {
                    if (p.Size.Width > 30)
                    {
                        Field1[row, col] = 1;
                        Field1[row, col + 1] = 1;
                        boy_ship++;
                    }
                    else
                    {
                        Field1[row, col] = 1;
                        Field1[row + 1, col] = 1;
                        boy_ship++;
                    }
                    Ship f = ship7;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void ship8_MouseUp(object sender, MouseEventArgs e)
        {
            Ship p = ship8;
            Point point = ship8.Location;
            if ((point.X > 60 && point.X + p.Size.Width < 380) && (point.Y > 65 && point.Y + p.Size.Height < 390))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(3, row, col, p.Size.Width, p.Size.Height) == 0)
                {
                    if (p.Size.Width > 30)
                    {
                        Field1[row, col] = 1;
                        Field1[row, col + 1] = 1;
                        Field1[row, col + 2] = 1;
                        boy_ship++;
                    }
                    else
                    {
                        Field1[row, col] = 1;
                        Field1[row + 1, col] = 1;
                        Field1[row + 2, col] = 1;
                        boy_ship++;
                    }
                    Ship f = ship8;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void ship9_MouseUp(object sender, MouseEventArgs e)
        {
            Ship p = ship9;
            Point point = ship9.Location;
            if ((point.X > 60 && point.X + p.Size.Width < 380) && (point.Y > 65 && point.Y + p.Size.Height < 390))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(3, row, col, p.Size.Width, p.Size.Height) == 0)
                {
                    if (p.Size.Width > 30)
                    {
                        Field1[row, col] = 1;
                        Field1[row, col + 1] = 1;
                        Field1[row, col + 2] = 1;
                        boy_ship++;
                    }
                    else
                    {
                        Field1[row, col] = 1;
                        Field1[row + 1, col] = 1;
                        Field1[row + 2, col] = 1;
                        boy_ship++;
                    }
                    Ship f = ship9;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void ship10_MouseUp(object sender, MouseEventArgs e)
        {
            Ship p = ship10;
            Point point = ship10.Location;
            if ((point.X > 60 && point.X + p.Size.Width < 380) && (point.Y > 65 && point.Y + p.Size.Height < 390))
            {
                int row = (int)(point.Y / H) - 1,
                    col = (int)(point.X / W) - 1;
                if (Check_Ship(4, row, col, p.Size.Width, p.Size.Height) == 0)
                {
                    if (p.Size.Width > 30)
                    {
                        Field1[row, col] = 1;
                        Field1[row, col + 1] = 1;
                        Field1[row, col + 2] = 1;
                        Field1[row, col + 3] = 1;
                        boy_ship++;
                    }
                    else
                    {
                        Field1[row, col] = 1;
                        Field1[row + 1, col] = 1;
                        Field1[row + 2, col] = 1;
                        Field1[row + 3, col] = 1;
                        boy_ship++;
                    }
                    Ship f = ship10;
                    f.Location = new Point(-200, 100);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Label fe = label50;// слово компьютер
            fe.Location = new Point(650, 28);
            Panel f = panel8;// правое поле
            f.Location = new Point(-600, 100);
            boy_go = true;
        }

        private void button2_Click(object sender, EventArgs e)// кнопка "выход" после сыгранной игры
        {
            newGame();
        }

        private void hod_com()// компьюер делает выстрелы
        {
            //1 влево
            //2 вправо
            //3 вверх
            //4 вниз


            int i = 1, j = 1, x = 1, y = 1, direction = 0, s = 0;
            bool hod_ok = false;
            Who_game = 0;
            hod = 1;
            com_shot++;
            textBox4.Text = com_shot.ToString();
            while (!hod_ok)
            {
                Random rnd = new Random();
                s = rnd.Next(0, 100);
                i = (s % 10) + 1;
                j = (s % 10) + 1;


                if (kill == 1)//попали но не убили
                {
                    Random r = new Random();
                    if (nice_shot_com == 1)
                    {
                        if (ok_shot_x > 1 && ok_shot_x < 10 && ok_shot_y > 1 && ok_shot_y < 10)
                        {
                            direction = r.Next(1, 5);
                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1)
                                direction = r.Next(1, 4);

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1)
                                direction = r.Next(2, 5);

                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = r.Next(1, 3);
                                if (direction == 2)
                                    direction = r.Next(3, 5);
                            }

                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(3, 5);
                                if (direction == 3)
                                    direction = r.Next(1, 3);
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = r.Next(3, 5);
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x + 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(2, 4);
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(2, 4);
                                if (direction == 3)
                                    direction = 4;
                            }

                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1 && Field1[ok_shot_x + 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(1, 3);
                            }

                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(1, 3);
                                if (direction == 2)
                                    direction = 4;
                            }

                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = r.Next(1, 3);
                                if (direction == 2)
                                    direction = 3;
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                                direction = 4;
                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                                direction = 1;
                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x + 1, ok_shot_y] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                                direction = 2;
                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1 && Field1[ok_shot_x + 1, ok_shot_y] == -1)
                                direction = 3;
                        }

                        if (ok_shot_y - 1 == 0)//стрелять в лево нельзя границы поля
                        {
                            direction = r.Next(2, 5);
                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(2, 4);
                            }
                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = r.Next(2, 3);
                                if (direction == 2)
                                    direction = r.Next(3, 5);
                            }

                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(3, 5);
                                if (direction == 3)
                                    direction = r.Next(2, 3);
                            }

                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1 && Field1[ok_shot_x + 1, ok_shot_y] == -1)
                            {
                                direction = 2;
                            }

                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = 4;
                            }

                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = 3;
                            }

                        }

                        if (ok_shot_y + 1 == 11)//стрелять в право нельзя границы поля
                        {
                            direction = r.Next(1, 3);
                            if (direction == 2)
                                direction = r.Next(3, 5);

                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(1, 3);
                                if (direction == 2)
                                    direction = 3;
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1)
                                direction = r.Next(3, 5);

                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(1, 3);
                                if (direction == 2)
                                    direction = 4;
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x + 1, ok_shot_y] == -1)
                            {
                                direction = 3;
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = 4;
                            }

                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1 && Field1[ok_shot_x + 1, ok_shot_y] == -1)
                            {
                                direction = 1;
                            }

                        }

                        if (ok_shot_x - 1 == 0) // стрелять вверх нельзя границы поля
                        {
                            direction = r.Next(1, 4);
                            if (direction == 3)
                                direction = 4;

                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1)
                                direction = r.Next(1, 3);

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1)
                            {
                                direction = r.Next(2, 4);
                                if (direction == 3)
                                    direction = 4;
                            }

                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = r.Next(1, 3);
                                if (direction == 2)
                                    direction = 4;
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = 4;
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x + 1, ok_shot_y] == -1)
                            {
                                direction = 2;
                            }

                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = 1;
                            }

                        }

                        if (ok_shot_x + 1 == 11) // стрелять вниз нельзя границы поля
                        {
                            direction = r.Next(1, 4);


                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1)
                                direction = r.Next(2, 4);

                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = r.Next(1, 3);
                                if (direction == 2)
                                    direction = 3;
                            }

                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = r.Next(1, 3);

                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x, ok_shot_y + 1] == -1)
                            {
                                direction = 3;
                            }

                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = 2;
                            }

                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1 && Field1[ok_shot_x - 1, ok_shot_y] == -1)
                            {
                                direction = 1;
                            }
                        }

                        if (ok_shot_x == 1 && ok_shot_y == 1)
                        {
                            direction = r.Next(2, 4);
                            if (direction == 3)
                                direction = 4;
                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1)
                                direction = 4;
                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1)
                                direction = 2;

                        }

                        if (ok_shot_x == 1 && ok_shot_y == 10)
                        {
                            direction = r.Next(1, 3);
                            if (direction == 2)
                                direction = 4;
                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1)
                                direction = 4;
                            if (Field1[ok_shot_x + 1, ok_shot_y] == -1)
                                direction = 1;

                        }

                        if (ok_shot_x == 10 && ok_shot_y == 1)
                        {
                            direction = r.Next(2, 4);
                            if (Field1[ok_shot_x, ok_shot_y + 1] == -1)
                                direction = 3;
                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1)
                                direction = 2;

                        }

                        if (ok_shot_x == 10 && ok_shot_y == 10)
                        {
                            direction = r.Next(1, 3);
                            if (direction == 2)
                                direction = 3;
                            if (Field1[ok_shot_x, ok_shot_y - 1] == -1)
                                direction = 3;
                            if (Field1[ok_shot_x - 1, ok_shot_y] == -1)
                                direction = 1;

                        }

                        if (direction == 1)
                        {

                            i = ok_shot_x;
                            j = ok_shot_y - 1;

                        }

                        if (direction == 2)
                        {
                            i = ok_shot_x;
                            j = ok_shot_y + 1;
                        }

                        if (direction == 3)
                        {
                            i = ok_shot_x - 1;
                            j = ok_shot_y;
                        }

                        if (direction == 4)
                        {
                            i = ok_shot_x + 1;
                            j = ok_shot_y;
                        }


                    }

                    if (nice_shot_com == 2)
                    {
                        if (Field1[ok_shot_x, ok_shot_y] == 5)
                        {
                            if (Field1[ok_shot_x, ok_shot_y - 1] == 5)
                            {
                                Random z = new Random();
                                if (Field1[ok_shot_x, ok_shot_y + 1] != 5)
                                {

                                    int h = z.Next(0, 2);
                                    if (Field1[ok_shot_x, ok_shot_y - 2] == -1)
                                    {
                                        i = ok_shot_x;
                                        j = ok_shot_y + 1;
                                    }
                                    else
                                    {
                                        if (h == 0)
                                        {
                                            i = ok_shot_x;
                                            j = ok_shot_y - 2;
                                        }

                                        if (h == 1 && ok_shot_y != 10)
                                        {
                                            i = ok_shot_x;
                                            j = ok_shot_y + 1;
                                        }
                                    }
                                }
                            }

                            if (Field1[ok_shot_x, ok_shot_y + 1] == 5)
                            {
                                Random z = new Random();
                                if (Field1[ok_shot_x, ok_shot_y - 1] != 5)
                                {

                                    int h = z.Next(0, 2);
                                    if (Field1[ok_shot_x, ok_shot_y + 2] == -1)
                                    {
                                        i = ok_shot_x;
                                        j = ok_shot_y - 1;
                                    }
                                    else
                                    {
                                        if (h == 0)
                                        {
                                            i = ok_shot_x;
                                            j = ok_shot_y + 2;
                                        }

                                        if (h == 1 && ok_shot_y != 1)
                                        {
                                            i = ok_shot_x;
                                            j = ok_shot_y - 1;
                                        }
                                    }
                                }
                            }

                            if (Field1[ok_shot_x - 1, ok_shot_y] == 5)
                            {
                                Random z = new Random();
                                if (Field1[ok_shot_x + 1, ok_shot_y] != 5)
                                {

                                    int h = z.Next(0, 2);
                                    if (Field1[ok_shot_x - 2, ok_shot_y] == -1)
                                    {
                                        i = ok_shot_x + 1;
                                        j = ok_shot_y;
                                    }
                                    else
                                    {
                                        if (h == 0)
                                        {
                                            i = ok_shot_x - 2;
                                            j = ok_shot_y;
                                        }

                                        if (h == 1 && ok_shot_x != 10)
                                        {
                                            i = ok_shot_x + 1;
                                            j = ok_shot_y;
                                        }
                                    }
                                }
                            }

                            if (Field1[ok_shot_x + 1, ok_shot_y] == 5)
                            {
                                Random z = new Random();
                                if (Field1[ok_shot_x - 1, ok_shot_y] != 5)
                                {

                                    int h = z.Next(0, 2);
                                    if (Field1[ok_shot_x + 2, ok_shot_y] == -1)
                                    {
                                        i = ok_shot_x - 1;
                                        j = ok_shot_y;
                                    }
                                    else
                                    {
                                        if (h == 0)
                                        {
                                            i = ok_shot_x + 2;
                                            j = ok_shot_y;
                                        }

                                        if (h == 1 && ok_shot_x != 1)
                                        {
                                            i = ok_shot_x - 1;
                                            j = ok_shot_y;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Field1[ok_shot_x, ok_shot_y - 1] == 5)
                            {
                                i = ok_shot_x;
                                j = ok_shot_y - 3;

                            }

                            if (Field1[ok_shot_x, ok_shot_y + 1] == 5)
                            {
                                i = ok_shot_x;
                                j = ok_shot_y + 3;
                            }

                            if (Field1[ok_shot_x - 1, ok_shot_y] == 5)
                            {
                                i = ok_shot_x - 3;
                                j = ok_shot_y;

                            }

                            if (Field1[ok_shot_x + 1, ok_shot_y] == 5)
                            {

                                i = ok_shot_x + 3;
                                j = ok_shot_y;
                            }
                        }
                    }

                    if (nice_shot_com == 3)
                    {
                        
                        if (Field1[ok_shot_x, ok_shot_y] == 5 && Field1[ok_shot_x, ok_shot_y - 1] == 5 && Field1[ok_shot_x, ok_shot_y - 2] == 5)
                        {
                            i = ok_shot_x;
                            j = ok_shot_y + 1;
                            if (ok_shot_y == 10)
                            {
                                i = ok_shot_x;
                                j = ok_shot_y - 3;
                            }
                        }

                        if (Field1[ok_shot_x, ok_shot_y] == 5 && Field1[ok_shot_x, ok_shot_y + 1] == 5 && Field1[ok_shot_x, ok_shot_y + 2] == 5)
                        {
                            i = ok_shot_x;
                            j = ok_shot_y - 1;
                            if (ok_shot_y == 1)
                            {
                                i = ok_shot_x;
                                j = ok_shot_y + 3;
                            }
                        }

                        if (Field1[ok_shot_x, ok_shot_y] == 5 && Field1[ok_shot_x - 1, ok_shot_y] == 5 && Field1[ok_shot_x - 2, ok_shot_y] == 5)
                        {
                            i = ok_shot_x + 1;
                            j = ok_shot_y;
                            if (ok_shot_x == 10)
                            {
                                i = ok_shot_x - 3;
                                j = ok_shot_y;
                            }
                        }

                        if (Field1[ok_shot_x, ok_shot_y] == 5 && Field1[ok_shot_x + 1, ok_shot_y] == 5 && Field1[ok_shot_x + 2, ok_shot_y] == 5)
                        {
                            i = ok_shot_x - 1;
                            j = ok_shot_y;
                            if (ok_shot_x == 1)
                            {
                                i = ok_shot_x + 3;
                                j = ok_shot_y;
                            }
                        }

                        if (Field1[i, j] == -1 && Field1[i, j - 1] == 5 && Field1[i, j - 2] == 5 && Field1[i, j - 3] == 5)
                        {
                            i = ok_shot_x;
                            j = ok_shot_y - 3;
                        }

                        if (Field1[i, j] == -1 && Field1[i, j + 1] == 5 && Field1[i, j + 2] == 5 && Field1[i, j + 3] == 5)
                        {
                            i = ok_shot_x;
                            j = ok_shot_y + 3;
                        }

                        if (Field1[i, j] == -1 && Field1[i - 1, j] == 5 && Field1[i - 2, j] == 5 && Field1[i - 3, j] == 5)
                        {
                            i = ok_shot_x - 3;
                            j = ok_shot_y;
                        }

                        if (Field1[i, j] == -1 && Field1[i + 1, j] == 5 && Field1[i + 2, j] == 5 && Field1[i + 3, j] == 5)
                        {
                            i = ok_shot_x + 3;
                            j = ok_shot_y;
                        }
                    }
                }

                x = (j - 1) * W + 1;
                y = (i - 1) * H + 1;
                if (Field1[i, j] == 1)
                {
                    Field1[i, j] = 5;
                    hod_ok = false;
                    g.FillRectangle(SystemBrushes.ControlDarkDark, x - 1, y - 1, W, H);
                    kill_or_nokill2(i, j);
                }

                if (Field1[i, j] == 0)
                {
                    Field1[i, j] = -1;
                    hod_ok = true;
                    Pen p = new Pen(Color.Black, 2);
                    Pen p2 = new Pen(Color.Red, 2);
                    g.DrawEllipse(p2, x + 9, y + 9, 10, 10);
                    Thread.Sleep(1000);
                    g.DrawEllipse(Pens.Black, x + 9, y + 9, 10, 10);
                    hod = 0;
                }

                if (kill == 2)//если убили то дальше обычный рандом
                {
                    kill = 0;
                    nice_shot_com = 0;
                }
            }
        }

        private void the_end()// если у кого-то из сторон убиты все корабли, то игра заканчивается
        {
            if (ship_com == 0)
            {
                MessageBox.Show("игра закончена, победил пользователь");
            }
            

            if (ship_user == 0)
            {
                MessageBox.Show("игра закончена, победил компьютер");
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)// кнопка игра-> новая
        {
            newGame();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) // кнопка выход
        {
            Application.Exit();
        }

        private void ship1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship3_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship4_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship5_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship6_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship7_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship8_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship9_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ship10_Paint(object sender, PaintEventArgs e)
        {

        }


        private void играToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }    
        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }  
    }
}
