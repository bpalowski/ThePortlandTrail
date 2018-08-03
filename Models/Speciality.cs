using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HairSalon.Models
{
    public class Speciality
    {
        private string _description;
        private int _id;


        public Speciality(string Description, int Id = 0)
        {
            _description = Description;
            _id = Id;
        }
        public override bool Equals(System.Object otherSpeciality)
        {
          if (!(otherSpeciality is Speciality))
          {
            return false;
          }
          else
          {
             Speciality newSpeciality = (Speciality) otherSpeciality;
             bool idEquality = this.GetId() == newSpeciality.GetId();
             bool descriptionEquality = this.GetDescription() == newSpeciality.GetDescription();
             return (idEquality && descriptionEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetId().GetHashCode();
        }
        public int GetId()
        {
            return _id;
        }
        public string GetDescription()
        {
            return _description;
        }

        public void Save()
        {
          MySqlConnection conn = DB.Connection();
    conn.Open();

    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"INSERT INTO specialties (Description) VALUES (@description);";

    cmd.Parameters.Add(new MySqlParameter("@description", _description));

    cmd.ExecuteNonQuery();
    _id = (int) cmd.LastInsertedId;
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
            }
            public static List<Speciality> GetAll()
            {
                List<Speciality> allSpecialties = new List<Speciality> {};
                MySqlConnection conn = DB.Connection();
                conn.Open();
                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT * FROM specialties;";
                var rdr = cmd.ExecuteReader() as MySqlDataReader;
                while(rdr.Read())
                {
                  int Id = rdr.GetInt32(0);
                  string Description = rdr.GetString(1);
                  Speciality newSpeciality = new Speciality(Description, Id);
                  allSpecialties.Add(newSpeciality);
                }
                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }
                return allSpecialties;
            }
            public static Speciality Find(int id)
              {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"SELECT * FROM specialties WHERE Id = (@searchId);";

           MySqlParameter searchId = new MySqlParameter();
           searchId.ParameterName = "@searchId";
           searchId.Value = id;
           cmd.Parameters.Add(searchId);

           var rdr = cmd.ExecuteReader() as MySqlDataReader;
           int Id = 0;
           string SpecialtyDescription = "";

           while(rdr.Read())
           {
             Id = rdr.GetInt32(0);
             SpecialtyDescription = rdr.GetString(1);
           }
           Speciality newSpecialty = new Speciality(SpecialtyDescription, Id);
           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
           // return new Stylist("", 0); //Test will fail
           return newSpecialty; //Test will pass
          }

          public void AddStylist(Stylist newStylist)
        {
          MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO specialties_stylists (specialty_id, stylist_id) VALUES (@SpecialtyId, @StylistId);";

      MySqlParameter specialty_id = new MySqlParameter();
      specialty_id.ParameterName = "@SpecialtyId";
      specialty_id.Value = _id;
      cmd.Parameters.Add(specialty_id);

      MySqlParameter stylist_id = new MySqlParameter();
      stylist_id.ParameterName = "@StylistId";
      stylist_id.Value = newStylist.GetStyId();
      cmd.Parameters.Add(stylist_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
        }

        public List<Stylist> GetStylist()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT stylists.* FROM specialties
      JOIN specialties_stylists ON (specialties.id = specialties_stylists.specialty_id)
      JOIN stylists ON (specialties_stylists.stylist_id = stylists.id)
      WHERE specialties.id = @SpecialtyId;";

      MySqlParameter specialty_idIdParameter = new MySqlParameter();
      specialty_idIdParameter.ParameterName = "@SpecialtyId";
      specialty_idIdParameter.Value = _id;
      cmd.Parameters.Add(specialty_idIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Stylist> stylists = new List<Stylist>{};

      while(rdr.Read())
      {
        int stylistid = rdr.GetInt32(0);
        string stylistName = rdr.GetString(1);
        Stylist newStylist = new Stylist(stylistName, stylistid);
        stylists.Add(newStylist);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return stylists;
    }

          public void UpdateDescription(string newDescription)
          {
              MySqlConnection conn = DB.Connection();
              conn.Open();
              var cmd = conn.CreateCommand() as MySqlCommand;
              cmd.CommandText = @"UPDATE specialties SET description = @newDescription WHERE id = @searchId;";

              MySqlParameter searchId = new MySqlParameter();
              searchId.ParameterName = "@searchId";
              searchId.Value = _id;
              cmd.Parameters.Add(searchId);

              MySqlParameter description = new MySqlParameter();
              description.ParameterName = "@newDescription";
              description.Value = newDescription;
              cmd.Parameters.Add(description);

              cmd.ExecuteNonQuery();
              _description = newDescription;
              conn.Close();
              if (conn != null)
              {
                  conn.Dispose();
              }

          }

      //     public void UpdateSpecialty (string newSpecialtyName) {
      //     MySqlConnection conn = DB.Connection ();
      //     conn.Open ();
      //
      //     MySqlCommand cmd = new MySqlCommand(@"UPDATE specialties SET description = @newDescription WHERE id = @SearchId;", conn);
      //
      //     cmd.Parameters.Add (new MySqlParameter ("@SearchId", _id));
      //     cmd.Parameters.Add (new MySqlParameter ("@newDescription", _description));
      //
      //     cmd.ExecuteNonQuery ();
      //     _description = newDescription;
      //
      //     conn.Close ();
      //     if (conn != null) {
      //         conn.Dispose ();
      //     }
      // }
          public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("DELETE FROM specialties WHERE id = @SpecialtyId; DELETE FROM specialties_stylists WHERE stylist_id = @SpecialtyId;", conn);

            cmd.Parameters.Add (new MySqlParameter ("@SpecialtyId", _id));

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

          public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM specialties;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  // public void AddStylist(Stylist newStylist)
  //   {
  //     MySqlConnection conn = DB.Connection();
  //     conn.Open();
  //     var cmd = conn.CreateCommand() as MySqlCommand;
  //     cmd.CommandText = @"INSERT INTO specialties_stylists (specialty_id, stylist_id) VALUES (@SpecialtyId, @StylistId);";
  //
  //     MySqlParameter specialty_id = new MySqlParameter();
  //     specialty_id.ParameterName = "@SpecialtyId";
  //     specialty_id.Value = _id;
  //     cmd.Parameters.Add(specialty_id);
  //
  //     MySqlParameter stylist_id = new MySqlParameter();
  //     stylist_id.ParameterName = "@StylistId";
  //     stylist_id.Value = newStylist.GetId();
  //     cmd.Parameters.Add(stylist_id);
  //
  //     cmd.ExecuteNonQuery();
  //     conn.Close();
  //     if (conn != null)
  //     {
  //       conn.Dispose();
  //     }
  //   }





      }
    }
