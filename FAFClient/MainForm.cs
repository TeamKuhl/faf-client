using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KuhlEngine;

namespace FAFClient
{
    public partial class MainForm : Form
    {

        public delegate void emptyFunction();
        Dictionary<string, Item> friends = new Dictionary<string, Item>();

        private Renderer renderer = new Renderer();

        Random rnd = new Random();

        private bool grow = true;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Event.NewFrame += new Event.RenderHandler(rendererEvent);
            Event.Collision += new Event.CollisionHandler(collisionEvent);

            renderer.FPS = 60;
            renderer.Height = 500;
            renderer.Width = 1000;
            renderer.Background = new Texture(203, 186, 156);
            renderer.Background.Stretch = false;
            
            addFriend(50, 50, new Texture(120, 34, 73));
            addFriend(900, 400, new Texture(54, 118, 32));

            // left
            Item wallLeft = renderer.CreateItem();
            wallLeft.Height = 500;
            wallLeft.Width = 32;
            wallLeft.X = 0;
            wallLeft.Y = 0;
            wallLeft.Texture = new Texture(153, 136, 106);
            wallLeft.Texture.Stretch = false;
            wallLeft.Enabled = true;
            wallLeft.CheckCollision = true;
            renderer.SetItem(wallLeft.Uuid, wallLeft);

            // right
            Item wallRight = renderer.CreateItem();
            wallRight.Y = 0;
            wallRight.X = 968;
            wallRight.Height = 500;
            wallRight.Width = 32;
            wallRight.Texture = new Texture(153, 136, 106);
            wallRight.Texture.Stretch = false;
            wallRight.Enabled = true;
            wallRight.CheckCollision = true;
            renderer.SetItem(wallRight.Uuid, wallRight);

            // top
            Item wallTop = renderer.CreateItem();
            wallTop.X = 32;
            wallTop.Height = 32;
            wallTop.Width = 936;
            wallTop.Texture = new Texture(153, 136, 106);
            wallTop.Texture.Stretch = false;
            wallTop.Enabled = true;
            wallTop.CheckCollision = true;
            renderer.SetItem(wallTop.Uuid, wallTop);

            // bottom
            Item wallBottom = renderer.CreateItem();
            wallBottom.X = 32;
            wallBottom.Y = 468;
            wallBottom.Height = 32;
            wallBottom.Width = 936;
            wallBottom.Texture = new Texture(153, 136, 106);
            wallBottom.Texture.Stretch = false;
            wallBottom.Enabled = true;
            wallBottom.CheckCollision = true;
            renderer.SetItem(wallBottom.Uuid, wallBottom);

            renderer.Start();

            walkTimer.Enabled = true;

        }

        private void addFriend(int X, int Y, Texture texture)
        {
            Item item = renderer.CreateItem();
            item.X = X;
            item.Y = Y;
            item.Width = 10;
            item.Height = 10;
            item.Enabled = true;
            item.CheckCollision = true;
            texture.Stretch = false;
            item.Texture = texture;
            if (renderer.SetItem(item.Uuid, item))
            {
                friends.Add(item.Uuid, item);
            }
        }

        private void rendererEvent(Image aFrame)
        {
            try
            {
                if (this.InvokeRequired) this.Invoke(new emptyFunction(delegate()
                {
                    pictureBoxFrame.Width = aFrame.Width;
                    pictureBoxFrame.Height = aFrame.Height;
                    pictureBoxFrame.Image = aFrame;
                }));
            }
            catch { Environment.Exit(0); }

        }

        private void collisionEvent(CollisionEventArgs e)
        {
            if (friends.ContainsKey(e.ActiveItem.Uuid) && friends.ContainsKey(e.PassiveItem.Uuid))
            {
                if(grow)
                {
                    if (friends.Count <= 500)
                    {
                        Texture txt = new Texture(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
                        txt.Stretch = false;
                        addFriend(rnd.Next(50, 901), rnd.Next(50, 401), txt);
                    }
                    else grow = false;
                }
                else
                {
                    if (friends.Count >= 50)
                    {
                       friends.Remove(e.PassiveItem.Uuid);
                       renderer.SetItemEnabled(e.PassiveItem.Uuid, false);
                    }
                    else grow = true;
                }
            }

            e.Cancel = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void walkTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, Item> copy = new Dictionary<string, Item>(friends);

                foreach (KeyValuePair<string, Item> friend in copy)
                {

                    int amount = rnd.Next(1, 30);

                    Item item = renderer.GetItem(friend.Value.Uuid);

                    switch (rnd.Next(1, 5))
                    {
                        case 1:
                            item.X = item.X + amount;
                            break;
                        case 2:
                            item.X = item.X - amount;
                            break;
                        case 3:
                            item.Y = item.Y + amount;
                            break;
                        case 4:
                            item.Y = item.Y - amount;
                            break;
                    }

                    renderer.SetItem(friend.Value.Uuid, item);
                }
            }
            catch
            { }
        }
    }
}
