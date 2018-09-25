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

        public List<Client> GetClients()
        {
            List<Client> allClients = new List<Client> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM clients WHERE stylist_id = @stylist_id;";

            MySqlParameter parameterStylistId = new MySqlParameter();
            parameterStylistId.ParameterName = "@stylist_id";
            parameterStylistId.Value = this.Id;
            cmd.Parameters.Add(parameterStylistId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);

                Client newClient = new Client(name, this.Id, id);
                allClients.Add(newClient);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allClients;
        }

        public static void DeleteStylist(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

            Stylist existingStylist = Stylist.Find(id);
            
            List<Client> clientList = existingStylist.GetClients();

            foreach( var client in clientList)
            {
                int clientId = client.Id;
                Client.DeleteClient(clientId);
            }

            cmd.CommandText = @"DELETE FROM stylists WHERE id = @searchId;";

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
            cmd.CommandText = @"DELETE FROM stylists;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
