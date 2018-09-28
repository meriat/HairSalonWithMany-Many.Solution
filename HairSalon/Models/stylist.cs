using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using HairSalon;

namespace HairSalon.Models
{
    public class Stylist
    {
        public int Id { get; set; }
        public string StylistName { get; set; }

        public Stylist(string name, int id = 0)
        {
            StylistName = name;
            Id = id;
        }

        public override bool Equals(System.Object otherStylist)
        {
            if (!(otherStylist is Stylist))
            {
                return false;
            }
            else
            {
                Stylist newStylist = (Stylist)otherStylist;
                bool idEquality = (this.Id == newStylist.Id);
                bool nameEquality = (this.StylistName == newStylist.StylistName);
                return (nameEquality && idEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.StylistName.GetHashCode();
        }

        public static List<Stylist> GetAll()
        {
            List<Stylist> allStylists = new List<Stylist> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM stylists;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                Stylist newStylist = new Stylist(name, id);
                allStylists.Add(newStylist);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStylists;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO stylists (stylist_name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this.StylistName;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Stylist Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM stylists WHERE id=@thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            int stylistId = 0;
            string stylistName = "";

            while (rdr.Read())
            {
                stylistId = rdr.GetInt32(0);
                stylistName = rdr.GetString(1);
            }

            Stylist foundstylist = new Stylist(stylistName, stylistId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundstylist;
        }

        public void Edit(string newStylist)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE stylists SET stylist_name = @newStylist WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = this.Id;
            cmd.Parameters.Add(searchId);

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@newStylist";
            name.Value = newStylist;
            cmd.Parameters.Add(name);

            this.StylistName = newStylist;
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddClient(Client newClient)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO client_stylist (client_id, stylist_id) VALUES (@clientId, @stylistId);";

            cmd.Parameters.AddWithValue("@clientId", newClient.Id);
            cmd.Parameters.AddWithValue("@stylistId", this.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Client> GetClients()
        {
            List<Client> allClients = new List<Client> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT clients.* FROM stylists JOIN client_stylist ON (stylists.id = client_stylist.stylist_id) JOIN clients ON (client_stylist.client_id = clients.id ) WHERE stylists.id = @searchId;";

            MySqlParameter parameterStylistId = new MySqlParameter();
            parameterStylistId.ParameterName = "@searchId";
            parameterStylistId.Value = this.Id;
            cmd.Parameters.Add(parameterStylistId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);

                Client newClient = new Client(name, this.Id);
                allClients.Add(newClient);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allClients;
        }

        public void AddSpecialty(Specialty newSpecialty)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO specialty_stylist (specialty_id, stylist_id) VALUES (@specialtyId, @stylistId);";

            cmd.Parameters.AddWithValue("@specialtyId", newSpecialty.Id);
            cmd.Parameters.AddWithValue("@stylistId", this.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Specialty> GetSpecialties()
        {
            List<Specialty> allSpecialties = new List<Specialty> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT specialties.* FROM stylists JOIN specialty_stylist ON (stylists.id = specialty_stylist.stylist_id) JOIN specialties ON (specialty_stylist.specialty_id = specialties.id ) WHERE stylists.id = @searchId;";

            MySqlParameter parameterStylistId = new MySqlParameter();
            parameterStylistId.ParameterName = "@searchId";
            parameterStylistId.Value = this.Id;
            cmd.Parameters.Add(parameterStylistId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);

                Specialty newSpecialty = new Specialty(name, this.Id);
                allSpecialties.Add(newSpecialty);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allSpecialties;
        }
        public static void DeleteStylist(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

            // Stylist existingStylist = Stylist.Find(id);

            // List<Client> clientList = existingStylist.GetClients();

            // foreach( var client in clientList)
            // {
            //     int clientId = client.Id;
            //     Client.DeleteClient(clientId);
            // }

            cmd.CommandText = @"DELETE FROM stylists WHERE id = @searchId;DELETE FROM client_stylist WHERE stylist_id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"TRUNCATE TABLE stylists;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
