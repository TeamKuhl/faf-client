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
        Dictionary<string, Friend> friends = new Dictionary<string, Friend>();

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
            addRandomFriend();
            addRandomFriend();
            addRandomFriend();

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
            renderer.SetItem(wallLeft);

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
            renderer.SetItem(wallRight);

            // top
            Item wallTop = renderer.CreateItem();
            wallTop.X = 32;
            wallTop.Height = 32;
            wallTop.Width = 936;
            wallTop.Texture = new Texture(153, 136, 106);
            wallTop.Texture.Stretch = false;
            wallTop.Enabled = true;
            wallTop.CheckCollision = true;
            renderer.SetItem(wallTop);

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
            renderer.SetItem(wallBottom);

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
            if (renderer.SetItem(item))
            {
                friends.Add(item.Uuid, new Friend(item));
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
                        addRandomFriend();
                    }
                    else grow = false;
                }
                else
                {
                    if (friends.Count >= 50)
                    {
                       friends.Remove(e.PassiveItem.Uuid);
                       renderer.SetItemEnabled(e.PassiveItem.Uuid, false);
                       renderer.DeleteItem(e.PassiveItem.Uuid);
                    }
                    else grow = true;
                }
            }

            if (e.ActiveItem.Enabled == false)
            {
                renderer.DeleteItem(e.ActiveItem.Uuid);
            }

            e.Cancel = true;
        }

        private void addRandomFriend()
        {
            Texture txt = new Texture(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            txt.Stretch = false;
            addFriend(rnd.Next(50, 901), rnd.Next(50, 401), txt);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void walkTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, Friend> copy = new Dictionary<string, Friend>(friends);

                foreach (KeyValuePair<string, Friend> friend in copy)
                {
                    int directionModifier = 0;
                    int lengthModifier = 0;

                    if (friend.Value.MoveLock == 0)
                    {
                        directionModifier = rnd.Next(-5, 6);
                        lengthModifier = rnd.Next(3, 5);
                    }

                    //renderer.SetItem(item);
                }
            }
            catch
            { }
        }
    }
}
