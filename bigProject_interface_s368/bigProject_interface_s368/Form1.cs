using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bigProject_interface_s368
{
    public partial class Form1 : Form
    {
        public abstract class Location
        {
            public string Name;
            public Location[] Exits = new Location[];

        }

        public class Room : Location
        {

        }

        public class Outside : Location
        {

        }

        public Form1()
        {
            InitializeComponent();
        }
    }
}
