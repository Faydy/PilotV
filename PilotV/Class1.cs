using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using LemonUI;
using LemonUI.Menus;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PilotV
{
    public class airport
    {
        public Vector3 airport_job;
        public Vector4 airport_runway;
        
        public airport(Vector3 job_position, Vector4 runway_position)
        {
            airport_job = job_position;
            airport_runway = runway_position;
        }
    }
    public class Main : Script
    {
        // LemonUI Object Pool
        ObjectPool pool = new ObjectPool();
        NativeMenu mainMenu = new NativeMenu("Pilot Mod", "Main Menu");
        NativeListItem<string> destination = new NativeListItem<string>("Destination");
        //

        //  Airport Locations
        List<airport> airports = new List<airport>()
        {
            new airport(new Vector3(-1166, -2735, 18.7f), new Vector4(-968, -3161, 18f, 55)), // Los Santos Airport
            new airport(new Vector3(1700, 3292, 47.7f), new Vector4( 1600, 3220, 41, 106)), // Sandy Shores Airfield  
        };
        List<Blip> airport_blips = new List<Blip>();
        Blip destination_blip = null;
        List<string> airport_names = new List<string>()
        {
            "Los Santos Airport",
            "Sandy Shores Airfield",
        };
        //

        // Misc
        int pay_int = 100;
        Color green = Color.FromArgb(29, 191, 51);
        Vehicle plane = null;
        Blip plane_blip = null;
        VehicleHash plane_hash;
        List<VehicleHash> passenger_planes = new List<VehicleHash>()
        {
            VehicleHash.Luxor,
            VehicleHash.Shamal,
            VehicleHash.Jet,
        };
        List<VehicleHash> cargo_planes = new List<VehicleHash>()
        {
            VehicleHash.CargoPlane,
        };

        /* NEXT UPDATE
        List<VehicleHash> military_planes = new List<VehicleHash>()
        {
            VehicleHash.Besra,
            VehicleHash.Lazer,
            VehicleHash.Hydra,
        };
        */
        //

        // Verifications

        int destination_index = 0; // Mode is going to be 0 if the player is at LA airport, 1 if at Sandy Shores, etc.
        int current_airport_index = 0;
        int type = 0; // Type is going to be 0 for passenger flight, 1 for cargo flight
        bool inJob = false;

        //
        public Main()
        {
            LoadLemonUiMenus();
            
            foreach (var airport in airports) {
                airport_blips.Add(World.CreateBlip(airport.airport_job));
                airport_blips.Last().Sprite = BlipSprite.Airport;
                airport_blips.Last().Color = BlipColor.Green;
            }

            Tick += onTick;
            KeyDown += onKeyDown;
        }
        private void job_on_tick()
        {
            if (inJob)
            {
                if(plane_blip != null && plane_blip.Exists() && plane != null && plane.Exists())
                {
                    plane_blip.Position = plane.Position;
                }
                Game.Player.Character.Weapons.Give(WeaponHash.Parachute, 1, true, true);
                Game.Player.Character.IsInvincible = true;
                if (plane == null || !plane.Exists() || !plane.IsDriveable)
                {
                    GTA.UI.Screen.ShowSubtitle("You have lost your plane! Job failed.");
                    failedJob();
                    return;
                }
                float distanceToDestination = plane.Position.DistanceTo(((Vector3)airports[destination_index].airport_runway));
                if (distanceToDestination < 20.0f)
                {
                    GTA.UI.Screen.ShowSubtitle("You have reached your destination!");
                    StopFlightJob();
                }
            }
        }

        private void failedJob()
        {
            try
            {
                Game.Player.Character.IsInvincible = false;
                GTA.UI.Screen.FadeOut(1000);
                Wait(2000);
                inJob = false;
                foreach (var blip in airport_blips)
                    if (blip != null && blip.Exists())
                        blip.Alpha = 255;
                if (destination_blip != null && destination_blip.Exists())
                {
                    destination_blip.Alpha = 0;
                    destination_blip.Delete();
                    destination_blip = null;
                }
                Game.Player.Character.Position = (airports[current_airport_index].airport_job);
                if(plane != null && plane.Exists())
                {
                    plane.Delete();
                    plane = null;
                }
                if(plane_blip != null && plane_blip.Exists())
                {
                    plane_blip.Delete();
                    plane_blip = null;
                }
                GTA.UI.Screen.FadeIn(1000);
                GTA.UI.Notification.Show("You failed the job!", false);
            }
            catch (Exception ex)
            {
                inJob = false;
                GTA.UI.Screen.ShowSubtitle("Failed to stop job: " + ex.Message);
            }
        }

        private void StopFlightJob()
        {
            try
            {
                Game.Player.Character.IsInvincible = false;
                Game.Player.Money +=  pay_int;
                GTA.UI.Screen.FadeOut(1000);
                Wait(2000);

                inJob = false;
                foreach (var blip in airport_blips)
                    if (blip != null && blip.Exists())
                        blip.Alpha = 255;
                if (destination_blip != null && destination_blip.Exists())
                {
                    destination_blip.Alpha = 0;
                    destination_blip.Delete();
                    destination_blip = null;
                }
                if (plane != null && plane.Exists())
                {
                    plane.Delete();
                    plane = null;
                }
                if (plane_blip != null && plane_blip.Exists())
                {
                    plane_blip.Delete();
                    plane_blip = null;
                }
                Game.Player.Character.Position = (airports[destination_index].airport_job);
                GTA.UI.Screen.FadeIn(1000);
                GTA.UI.Notification.Show("You finished the job!", false);
            }
            catch (Exception ex)
            {
                inJob = false;
                GTA.UI.Screen.ShowSubtitle("Failed to stop job: " + ex.Message);
            }
        }

        private void StartFlightJob()
        {
            inJob = true;
            GTA.UI.Screen.FadeOut(1000);
            Wait(2000);

            // Make airport blips invisible
            foreach (var blip in airport_blips)
                blip.Alpha = 0;


            // Defensive: ensure destination_index is not the same as current; pick an alternative if it is
            if (destination_index == current_airport_index)
            {
                destination_index = (current_airport_index + 1) % airports.Count;
            }

            destination_blip = World.CreateBlip(((Vector3)airports[destination_index].airport_runway));
            destination_blip.IsFlashing = false;
            destination_blip.Sprite = BlipSprite.SonicWave;

            // Ensure a plane model is selected; fall back to a safe default based on selected type
            if (plane_hash == 0)
            {
                if (type == 0) // passenger
                    plane_hash = passenger_planes.Count > 0 ? passenger_planes[0] : VehicleHash.Luxor;
                else // cargo
                    plane_hash = cargo_planes.Count > 0 ? cargo_planes[0] : (passenger_planes.Count > 0 ? passenger_planes[0] : VehicleHash.Luxor);
            }

            // Request and verify model is loaded before creating the vehicle
            var model = new Model(plane_hash);
            model.Request(1000);
            if (!model.IsLoaded)
            {
                GTA.UI.Screen.FadeIn(1000);
                GTA.UI.Screen.ShowSubtitle("Failed to load plane model.");
                inJob = false;
                return;
            }

            // Create the vehicle and verify it spawned successfully
            plane = World.CreateVehicle(plane_hash, ((Vector3)airports[current_airport_index].airport_runway), airports[current_airport_index].airport_runway.W);
            if (plane == null)
            {
                GTA.UI.Screen.FadeIn(1000);
                GTA.UI.Screen.ShowSubtitle("Failed to spawn plane.");
                inJob = false;
                model.MarkAsNoLongerNeeded();
                return;
            }
            else
            {
                plane.CurrentRPM = 1.0f; // Start engine
                 // Make plane blip
                plane_blip = World.CreateBlip(plane.Position);
                plane_blip.Sprite = BlipSprite.Plane;
                plane_blip.Color = BlipColor.Blue;
            }

            // Only call SetIntoVehicle if plane is valid
            Game.Player.Character.SetIntoVehicle(plane, VehicleSeat.Driver);

            model.MarkAsNoLongerNeeded();

            GTA.UI.Screen.FadeIn(1000);
            GTA.UI.Screen.ShowSubtitle("Flight job started! Follow the instructions. " + airport_names[destination_index]);
        }

        private void LoadLemonUiMenus()
        {
            pool.Add(mainMenu);

            var type = new NativeListItem<string>("Flight Type");
            type.Add("Passenger Flight");
            type.Add("Cargo Flight");

            var vehicles = new NativeListItem<VehicleHash>("Available Planes");
            foreach (var plane in passenger_planes)
                vehicles.Add(plane);

            pay_int = 100;
            var pay = new NativeItem("Pay: " + pay_int.ToString() + '$');

            type.ItemChanged += (s, e) =>
            {
                this.type = type.SelectedIndex;
                vehicles.Clear();
                if (type.SelectedIndex == 0) // Passenger Flight
                {
                    foreach (var p in passenger_planes)
                        vehicles.Add(p);
                }
                else if (type.SelectedIndex == 1) // Cargo Flight
                {
                    foreach (var p in cargo_planes)
                        vehicles.Add(p);
                }

                // Make selection deterministic after switching types
                if (vehicles.Items.Count > 0)
                {
                    vehicles.SelectedIndex = 0;
                    plane_hash = vehicles.SelectedItem;
                }

                // Update pay based on the (new) selected plane
                if (this.type == 0) // Passenger pricing by index
                {
                    switch (vehicles.SelectedIndex)
                    {
                        case 0: pay_int = 100; break;
                        case 1: pay_int = 200; break;
                        case 2: pay_int = 300; break;
                        default: pay_int = 100; break;
                    }
                }
                else // Cargo flat price
                {
                    pay_int = 300;
                }
                pay.Title = "Pay: " + pay_int.ToString() + '$';
            };

            vehicles.ItemChanged += (s, e) =>
            {
                plane_hash = vehicles.SelectedItem;
                if (this.type == 0) // Passenger
                {
                    switch (vehicles.SelectedIndex)
                    {
                        case 0: pay_int = 100; break;
                        case 1: pay_int = 200; break;
                        case 2: pay_int = 300; break;
                        default: pay_int = 100; break;
                    }
                }
                else // Cargo
                {
                    pay_int = 300;
                }
                pay.Title = "Pay: " + pay_int.ToString() + '$';
            };

            destination.ItemChanged += (s, e) =>
            {
                switch (destination.SelectedItem)
                {
                    case "Los Santos Airport":
                        destination_index = 0;
                        break;
                    case "Sandy Shores Airfield":
                        destination_index = 1;
                        break;
                }
            };

            var cont = new NativeItem("Continue");
            cont.Activated += (s, e) =>
            {
                StartFlightJob();
                mainMenu.Visible = false;
            };

            mainMenu.Add(type);
            mainMenu.Add(destination);
            mainMenu.Add(vehicles);
            mainMenu.Add(pay);
            mainMenu.Add(cont);
        }

        // Pseudocode / Plan:
        // 1. Find the nearest airport to the player by iterating all airports and tracking the minimum distance and index.
        // 2. After the loop, set current_airport_index to the nearest index (if any).
        // 3. If the nearest distance is < 10f, draw the marker for that single nearest airport and show contextual help for that airport only.
        // 4. If the nearest distance is < 2f and not already in a job, show the menu and allow starting the job via Context control.
        // 5. Otherwise hide the menu. Process the LemonUI pool at the end.
        private void onTick(object sender, EventArgs e)
        {
            job_on_tick();

            // 1) Find nearest airport first so current_airport_index is up-to-date
            int nearestIndex = -1;
            float nearestDistance = float.MaxValue;
            Vector3 playerPos = Game.Player.Character.Position;

            for (int i = 0; i < airports.Count; i++)
            {
                var ap = airports[i];
                float dist = playerPos.DistanceTo(ap.airport_job);
                if (dist < nearestDistance)
                {
                    nearestDistance = dist;
                    nearestIndex = i;
                }
            }

            if (nearestIndex != -1)
            {
                current_airport_index = nearestIndex;
            }

            // 2) Then build filtered destination list (exclude the current airport)
            if (!inJob)
            {
                List<string> filtered;
                if (current_airport_index >= 0 && current_airport_index < airport_names.Count)
                    filtered = airport_names.Where((name, idx) => idx != current_airport_index).ToList();
                else
                    filtered = new List<string>(airport_names);

                destination.Items = filtered;

                if (filtered.Count > 0)
                {
                    destination.SelectedIndex = 0;
                    destination_index = airport_names.IndexOf(filtered[0]);
                    if (destination_index < 0) destination_index = 0;
                }
            }

            // 3) Use the up-to-date nearestDistance/current_airport_index for markers & menu
            if (current_airport_index != -1)
            {
                var nearestAirport = airports[current_airport_index];

                if (nearestDistance < 10.0f)
                {
                    World.DrawMarker(MarkerType.Cylinder, nearestAirport.airport_job, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1f, 1f, 1f), green);
                }

                if (nearestDistance < 2.0f && !inJob)
                {
                    mainMenu.Visible = true;
                    GTA.UI.Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to start a flight job.");
                }
                else
                {
                    mainMenu.Visible = false;
                }
            }
            else
            {
                mainMenu.Visible = false;
            }

            pool.Process();
        }


        private void onKeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}