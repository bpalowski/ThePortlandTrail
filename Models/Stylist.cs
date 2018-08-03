using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Stylist
  {
    private int _styId;
    private string _stylistName;

    public Stylist(string StylistName, int StyId = 0)
    {
      _stylistName = StylistName;
      _styId = StyId;
    }

    public override bool Equals(System.Object otherStylist)
    {
      if(!(otherStylist is Stylist))
      {
        return false;
      }
      else
      {
        Stylist newStylist = (Stylist) otherStylist;
        bool idEquality = (this.GetStyId() == newStylist.GetStyId());
        bool nameEquality = (this.GetStylistName() == newStylist.GetStylistName());
        return(idEquality && nameEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetStyId().GetHashCode();
    }
    public int GetStyId()
    {
      return _styId;
    }
    public string GetStylistName()
    {
      return _stylistName;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO stylists (Name) VALUES (@name);";

      cmd.Parameters.Add(new MySqlParameter("@name", _stylistName));

      cmd.ExecuteNonQuery();
      _styId = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }
    public static List<Stylist> GetAll()
    {
      List<Stylist> allStylists = new List<Stylist> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM stylists;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int StyId = rdr.GetInt32(0);
        string StylistName = rdr.GetString(1);
        Stylist newStylist = new Stylist(StylistName, StyId);
        allStylists.Add(newStylist);
      }
      conn.Close ();
      if (conn != null) {
        conn.Dispose ();
      }
      return allStylists;
    }

    // public List<Client> GetClients()
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   MySqlCommand cmd = new MySqlCommand(@"SELECT client_id FROM clients_stylists WHERE stylist_id = @StylistId;", conn);
    //
    //   cmd.Parameters.Add (new MySqlParameter ("@StylistId", _styId));
    //
    //   var rdr = cmd.ExecuteReader() as MySqlDataReader;
    //
    //   List<int> clientIds = new List<int> {};
    //   while(rdr.Read())
    //   {
    //     int clientId = rdr.GetInt32(0);
    //     clientIds.Add(clientId);
    //   }
    //   rdr.Dispose();
    //
    //   List<Client> clients = new List<Client> {};
    //       foreach (int clientId in clientIds)
    //       {
    //           MySqlCommand clientQuery = new MySqlCommand(@"SELECT * FROM clients WHERE id = @ClientId;", conn);
    //
    //           MySqlParameter clientIdParameter = new MySqlParameter();
    //           clientIdParameter.ParameterName = "@ClientId";
    //           clientIdParameter.Value = clientId;
    //           clientQuery.Parameters.Add(clientIdParameter);
    //
    //           var clientQueryRdr = clientQuery.ExecuteReader() as MySqlDataReader;
    //           while(clientQueryRdr.Read())
    //           {
    //               int thisClientId = clientQueryRdr.GetInt32(0);
    //               string clientDescription = clientQueryRdr.GetString(1);
    //               Client foundClient = new Client(clientDescription, thisClientId);
    //               clients.Add(foundClient);
    //           }
    //           clientQueryRdr.Dispose();
    //       }
    //       conn.Close();
    //       if (conn != null)
    //       {
    //           conn.Dispose();
    //       }
    //       return clients;
    //   }
    public List<Speciality> GetSpecialties()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT specialties.* FROM stylists
      JOIN specialties_stylists ON (stylists.id = specialties_stylists.stylist_id)
      JOIN specialties ON (specialties_stylists.specialty_id = specialties.id)
      WHERE stylists.id = @StylistId;";

      MySqlParameter SpecialityIdParameter = new MySqlParameter();
      SpecialityIdParameter.ParameterName = "@StylistId";
      SpecialityIdParameter.Value = _styId;
      cmd.Parameters.Add(SpecialityIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Speciality> specialties = new List<Speciality>{};

      while(rdr.Read())
      {
        int SpecialityId = rdr.GetInt32(0);
        string SpecialityTitle = rdr.GetString(1);
        Speciality newSpeciality = new Speciality(SpecialityTitle);
        specialties.Add(newSpeciality);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return specialties;
    }



    public void AddSpecialty(Speciality newSpeciality)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO specialties_stylists (Stylist_Id, Specialty_Id) VALUES (@StylistId, @SpecialtyId);";

      MySqlParameter Stylist_Id = new MySqlParameter();
      Stylist_Id.ParameterName = "@StylistId";
      Stylist_Id.Value = _styId;
      cmd.Parameters.Add(Stylist_Id);

      MySqlParameter speciality_id = new MySqlParameter();
      speciality_id.ParameterName = "@SpecialtyId";
      speciality_id.Value = newSpeciality.GetId();
      cmd.Parameters.Add(speciality_id);

      cmd.ExecuteNonQuery();
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
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM stylists WHERE Id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int StyId = 0;
      string StylistName = "";

      while(rdr.Read())
      {
        StyId = rdr.GetInt32(0);
        StylistName = rdr.GetString(1);
      }
      Stylist newStylist = new Stylist(StylistName, StyId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      // return new Stylist("", 0); //Test will fail
      return newStylist; //Test will pass
    }

    public List<Stylist> GetStylists()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand(@"SELECT stylist_id FROM specialties_stylists WHERE specialty_id = @SpecialtyId;", conn);

      cmd.Parameters.Add (new MySqlParameter ("@SpecialtyId", _styId));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> stylistIds = new List<int> {};
      while(rdr.Read())
      {
        int stylistId = rdr.GetInt32(0);
        stylistIds.Add(stylistId);
      }
      rdr.Dispose();

      List<Stylist> stylists = new List<Stylist> {};
      foreach (int stylistId in stylistIds)
      {
        MySqlCommand stylistQuery = new MySqlCommand(@"SELECT * FROM stylists WHERE id = @StylistId;", conn);

        MySqlParameter stylistIdParameter = new MySqlParameter();
        stylistIdParameter.ParameterName = "@StylistId";
        stylistIdParameter.Value = stylistId;
        stylistQuery.Parameters.Add(stylistIdParameter);

        var stylistQueryRdr = stylistQuery.ExecuteReader() as MySqlDataReader;
        while(stylistQueryRdr.Read())
        {
          int thisStylistId = stylistQueryRdr.GetInt32(0);
          string stylistDescription = stylistQueryRdr.GetString(1);
          Stylist foundStylist = new Stylist(stylistDescription, thisStylistId);
          stylists.Add(foundStylist);
        }
        stylistQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return stylists;
    }
    // public void AddClient(Client newClient)
    // {
    //     MySqlConnection conn = DB.Connection();
    //     conn.Open();
    //
    //     MySqlCommand cmd = new MySqlCommand(@"INSERT INTO clients_stylists (stylist_id, client_id) VALUES (@StylistId, @ClientId);", conn);
    //
    //     cmd.Parameters.Add (new MySqlParameter ("@StylistId", _styId));
    //
    //     MySqlParameter client_id = new MySqlParameter();
    //     client_id.ParameterName = "@ClientId";
    //     client_id.Value = newClient.GetId();
    //     cmd.Parameters.Add(client_id);
    //
    //     cmd.ExecuteNonQuery();
    //     conn.Close();
    //     if (conn != null)
    //     {
    //         conn.Dispose();
    //     }
    // }
    public void Edit (string newStylistName) {
      MySqlConnection conn = DB.Connection ();
      conn.Open ();

      MySqlCommand cmd = new MySqlCommand(@"UPDATE stylists SET name = @newStylistName WHERE id = @SearchId;", conn);

      cmd.Parameters.Add (new MySqlParameter ("@SearchId", _styId));
      cmd.Parameters.Add (new MySqlParameter ("@newStylistName", newStylistName));

      cmd.ExecuteNonQuery ();
      _stylistName = newStylistName;

      conn.Close ();
      if (conn != null) {
        conn.Dispose ();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM stylists WHERE id = @StylistId; DELETE FROM specialities_stylists WHERE Stylist_Id = @StylistId;", conn);
      MySqlParameter StylistId = new MySqlParameter();
      StylistId.ParameterName = "@StylistId";
      StylistId.Value = this.GetStyId();

      cmd.Parameters.Add(StylistId);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll () {
      MySqlConnection conn = DB.Connection ();
      conn.Open ();

      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM stylists; Delete From specialties_stylists;", conn);

      cmd.ExecuteNonQuery ();

      conn.Close ();
      if (conn != null) {
        conn.Dispose ();
      }
    }
    //     public void DeleteStylistClients()
    //       {
    //           MySqlConnection conn = DB.Connection();
    //           conn.Open();
    //           MySqlCommand cmd = new MySqlCommand(@"SELECT client_id FROM clients_stylists WHERE stylist_id = @StylistId; DELETE FROM clients_stylists WHERE stylist_id = @StylistId", conn);
    //
    //           cmd.Parameters.Add (new MySqlParameter ("@StylistId", this._styId));
    //
    //           var rdr = cmd.ExecuteReader() as MySqlDataReader;
    //
    //           rdr.Dispose();
    //
    //           conn.Close();
    //           if (conn != null)
    //           {
    //               conn.Dispose();
    //           }
    //
    //       }
    //       public void DeleteStylistClient(int id)
    // {
    //     MySqlConnection conn = DB.Connection();
    //     conn.Open();
    //     MySqlCommand cmd = new MySqlCommand(@"SELECT client_id FROM clients_stylists WHERE stylist_id = @StylistId AND client_id = @ClientId; DELETE FROM clients_stylists WHERE stylist_id = @StylistId AND client_id = @ClientId;", conn);
    //
    //     cmd.Parameters.Add (new MySqlParameter ("@StylistId", this._styId));
    //     cmd.Parameters.Add (new MySqlParameter ("@ClientId", id));
    //
    //     var rdr = cmd.ExecuteReader() as MySqlDataReader;
    //
    //     rdr.Dispose();
    //
    //     conn.Close();
    //     if (conn != null)
    //     {
    //         conn.Dispose();
    //     }
    //
    // }
  }
}
