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
            public Location(string name)
            {
                this.Name = name;
            }
            public string Name { get; private set; }
            public Location[] Exits;

            public virtual string Description
            {
                get
                {
                    string description = "Stoisz w: " + Name + ". Widzisz wyjścia do nastepujacych lokalizacji:";
                    for (int i = 0; i < Exits.Length; i++)
                    {
                        description += " " + Exits[i].Name;
                        if (i != Exits.Length - 1) // -1 bo zaczynam od i=0
                            description += ", ";
                    }
                    description += ".";
                    return description;
                }
            }
        }

        public class Room : Location
        {
            private string decoration;
            public Room(string name, string decoration) : base(name)
            {
                this.decoration = decoration;
            }
        }

        public class Outside : Location
        {
            private bool hot;
            public Outside(string name, bool hot) : base(name)
            {
                this.hot = hot;
            }
            
            public override string Description
            {
                get
                {
                    string NewDescription = base.Description;   // TUTAJ CIEKAWE
                    if (hot)
                        NewDescription += "Tutaj jest bardzo goraco.";
                    return NewDescription;
                }
            }
        }

        class OutsideWithDoor : Outside, IHasExteriorDoor
        {
                        
            private string hot { get; }

            string DoorLocation { get; }
            string DoorDescription { get; set; }
        }

        class RoomWithDoor : Room, IHasExteriorDoor
        {
            string DoorLocation { get; }
            string DoorDescription { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }
    }

    internal interface IHasExteriorDoor // internal to public dla innych klas zestawu
    {
        string DoorLocation { get; }
        string DoorDescription { get; set; }
    }
}
