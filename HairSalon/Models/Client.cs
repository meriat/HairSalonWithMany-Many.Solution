using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using HairSalon;

namespace HairSalon.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int Stylist_Id { get; set; }

        public Client(string name, int stylist_id, int clientId = 0)
        {
            Name = name;
            Stylist_Id = stylist_id;
            ClientId = clientId;
        }

        public override bool Equals(System.Object otherClient)
        {
            if (!(otherClient is Client))
            {
                return false;
            }
            else
            {
                Client newClient = (Client)otherClient;
                bool idEquality = (this.ClientId == newClient.ClientId);
                bool nameEquality = (this.Name == newClient.Name);
                bool stylist_idEquality = (this.Stylist_Id == newClient.Stylist_Id);
                return (nameEquality && idEquality && stylist_idEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public static List<Client> GetClients()
    {
      List<Client> allClients = new List<Client>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM clients;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int stylist_id = rdr.GetInt32(2);
        Client newClient = new Client(name,stylist_id,id);
        allClients.Add(newClient);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allClients;
    }

    public void Save()
        {MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO clients (name, stylist_id) VALUES (@name, @stylist);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this.Name;
      cmd.Parameters.Add(name);


      MySqlParameter stylist_id = new MySqlParameter();
      stylist_id.ParameterName = "@stylist";
      stylist_id.Value = this.Stylist_Id;
      cmd.Parameters.Add(stylist_id);

      cmd.ExecuteNonQuery();
      this.ClientId = (int) cmd.LastInsertedId;

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
            cmd.CommandText = @"DELETE FROM clients;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
