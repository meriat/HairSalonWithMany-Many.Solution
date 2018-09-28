using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using HairSalon;

namespace HairSalon.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Specialty(string name, int id = 0)
        {
            Name = name;
            Id = id;
        }

        public override bool Equals(System.Object otherSpecialty)
        {
            if (!(otherSpecialty is Specialty))
            {
                return false;
            }
            else
            {
                Specialty newSpecialty = (Specialty)otherSpecialty;
                bool idEquality = (this.Id == newSpecialty.Id);
                bool nameEquality = (this.Name == newSpecialty.Name);
                return (nameEquality && idEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public static List<Specialty> GetAll()
        {
            List<Specialty> allSpecialties = new List<Specialty> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM specialties;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                Specialty newSpecialty = new Specialty(name, id);
                allSpecialties.Add(newSpecialty);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allSpecialties;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO specialties (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this.Name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            this.Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Specialty Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM specialties WHERE id=@searchId;";

            MySqlParameter parameterId = new MySqlParameter();
            parameterId.ParameterName = "@searchId";
            parameterId.Value = id;
            cmd.Parameters.Add(parameterId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            Specialty foundSpecialty = new Specialty("", 0);
            if (rdr.Read())
            {
                string name = rdr.GetString(1);
                int specialtyid = rdr.GetInt32(0);

                foundSpecialty = new Specialty(name, id);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundSpecialty;
        }

        public void Edit(string newSpecialty)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE specialties SET name = @newSpecialty WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = this.Id;
            cmd.Parameters.Add(searchId);

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@newSpecialty";
            name.Value = newSpecialty;
            cmd.Parameters.Add(name);

            this.Name = newSpecialty;

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteSpecialty(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM specialties WHERE id = @searchId;";

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

        public void AddStylist(Stylist newStylist)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO specialty_stylist (specialty_id, stylist_id) VALUES (@specialtyid, @stylistId);";

            cmd.Parameters.AddWithValue("@specialtyId", this.Id);
            cmd.Parameters.AddWithValue("@stylistId", newStylist.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public List<Stylist> GetStylists()
        {
            List<Stylist> allStylists = new List<Stylist> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT stylists.* FROM specialties JOIN specialty_stylist ON (specialties.id = specialty_stylist.specialty_id) JOIN stylists ON (specialty_stylist.stylist_id = stylists.id ) WHERE specialties.id = @searchId;";

            MySqlParameter parameterStylistId = new MySqlParameter();
            parameterStylistId.ParameterName = "@searchId";
            parameterStylistId.Value = this.Id;
            cmd.Parameters.Add(parameterStylistId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);

                Stylist newStylist = new Stylist(name, this.Id);
                allStylists.Add(newStylist);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStylists;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"TRUNCATE TABLE specialties;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
