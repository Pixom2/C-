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
        Location currentLocation;

        RoomWithDoor livingRoom;    // drzwi zewnetrzne
        Room diningRoom;
        RoomWithDoor kitchen;

        OutsideWithDoor frontYard;
        Outside garden;
        OutsideWithDoor backYard;
        
        public Form1()
        {
            InitializeComponent();
            CreateObject();
            MoveToANewLocation(livingRoom);
        }

        private void CreateObject()
        {
            livingRoom = new RoomWithDoor("Pokoj dzienny", "antyczny dywan", "debowe drzwi");
            diningRoom = new Room("jadalnia", "krysztalowy zyrandol");
            kitchen = new RoomWithDoor("Kuchnia", "stol w kuchni", "plastikowe drzwi");
            frontYard = new OutsideWithDoor("Podworko przed domem", true, "debowe drzwi");
            garden = new Outside("Ogrod", false);
            backYard = new OutsideWithDoor("Podworko z tylu", true, "plastikowe drzwi2");

            diningRoom.Exits = new Location[] { livingRoom, kitchen };
            livingRoom.Exits = new Location[] { diningRoom };
            kitchen.Exits = new Location[] { diningRoom };
            frontYard.Exits = new Location[] { backYard, garden };
            garden.Exits = new Location[] { frontYard, backYard };
            backYard.Exits = new Location[] { frontYard, garden };

            livingRoom.DoorLocation = frontYard;
            kitchen.DoorLocation = backYard;

            frontYard.DoorLocation = livingRoom;
            backYard.DoorLocation = kitchen; 
        }

        private void MoveToANewLocation(Location newLocation)
        {
            currentLocation = newLocation;
            exits.Items.Clear();

            for (int i = 0; i < currentLocation.Exits.Length; i++)
            {
                exits.Items.Add(currentLocation.Exits[i].Name);
            }
            exits.SelectedIndex = 0;
            description.Text = currentLocation.Description;

            if (currentLocation is IHasExteriorDoor)
                goThroughTheDoor.Visible = true;
            else
                goThroughTheDoor.Visible = false;
           

        }

        private void goThroughTheDoor_Click(object sender, EventArgs e)
        {
            IHasExteriorDoor hasDoor = currentLocation as IHasExteriorDoor; // przeanalizowac
            MoveToANewLocation(hasDoor.DoorLocation);
        }

        private void goHere_Click(object sender, EventArgs e)
        {
            MoveToANewLocation(currentLocation.Exits[exits.SelectedIndex]);
        }
    }

    interface IHasExteriorDoor // internal to public dla innych klas zestawu
    {
        Location DoorLocation { get; set; }
        string DoorDescription { get; }
    }

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

        public override string Description
        {
            get
            {
                return base.Description + "Widzisz tutaj " + decoration + ".";
            }
        }
    }

    public class RoomWithDoor : Room, IHasExteriorDoor
    {
        public RoomWithDoor(string name, string decoration, string doorDescription) : base(name, decoration)
        {
            DoorDescription = doorDescription;
        }
        public Location DoorLocation { get; set; }
        public string DoorDescription { get; private set; }
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

    public class OutsideWithDoor : Outside, IHasExteriorDoor
    {
        public OutsideWithDoor(string name, bool hot, string doorDescriptions) : base(name, hot)
        {
            this.DoorDescription = doorDescriptions;
        }

        public string DoorDescription { get; private set; }

        public Location DoorLocation { get; set; }

        public override string Description
        {
            get
            {
                return base.Description + "Widzisz teraz " + DoorDescription + ".";
            }
        }

    }

}
