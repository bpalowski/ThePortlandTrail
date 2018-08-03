using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace HairSalon.Models
{
    public class Client
    {
        private string _name;
        private int _id;
        private int _stylistId;

        public Client(string Name, int StylistId, int Id = 0)
        {
            _name = Name;
            _id = Id;
            _stylistId = StylistId;
        }
        public override bool Equals(System.Object otherClient)
        {
          if (!(otherClient is Client))
          {
            return false;
          }
          else
          {
             Client newClient = (Client) otherClient;
             bool idEquality = this.GetId() == newClient.GetId();
             bool nameEquality = this.GetName() == newClient.GetName();
             bool stylistIdEquality = this.GetStylistId() == newClient.GetStylistId();
             return (idEquality && nameEquality && stylistIdEquality);
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
        public string GetName()
        {
            return _name;
        }
        public int GetStylistId()
        {
            return _stylistId;
        }
        public void Save()
        {
          MySqlConnection conn = DB.Connection();
    conn.Open();

    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"INSERT INTO clients (Name, Stylist_Id) VALUES (@name, @stylistId);";

    cmd.Parameters.Add(new MySqlParameter("@name", _name));
    cmd.Parameters.Add(new MySqlParameter("@stylistId", _stylistId));

    cmd.ExecuteNonQuery();
    _id = (int) cmd.LastInsertedId;
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
            }


        public static List<Client> GetAll()
        {
            List<Client> allClients = new List<Client> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM clients;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int Id = rdr.GetInt32(0);
              string Name = rdr.GetString(1);
              int StylistId = rdr.GetInt32(2);
              Client newClient = new Client(Name, StylistId, Id);
              allClients.Add(newClient);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allClients;
        }
        public static List<Client> GetClientsById(int id)
    {
      List<Client> allClients = new List<Client> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM clients WHERE Stylist_Id = @stylistId ;";

      cmd.Parameters.Add(new MySqlParameter("@stylistId", id));
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int Id = rdr.GetInt32(0);
        string Name = rdr.GetString(1);
        int StylistId = rdr.GetInt32(2);
        Client newClient = new Client(Name, StylistId, Id);
        allClients.Add(newClient);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      // return new List<Client>{}; //Test will fail
      return allClients; //Test will pass
    }
        public static Client Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM clients WHERE Id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int Id = 0;
            string Name = "";
            int StylistId = 0;

            while(rdr.Read())
            {
              Id = rdr.GetInt32(0);
              Name = rdr.GetString(1);
              StylistId = rdr.GetInt32(2);
            }
            Client newClient = new Client(Name, StylistId,Id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newClient;
        }
        public void Edit(string newName)
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"UPDATE clients SET Name = @newName WHERE Id = @searchId;";

                cmd.Parameters.Add(new MySqlParameter("@searchId", _id));
                cmd.Parameters.Add(new MySqlParameter("@newName", newName));


                cmd.ExecuteNonQuery();
                _name = newName;

                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        //     public void UpdateDescription(string newDescription)
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     var cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";
        //
        //     MySqlParameter searchId = new MySqlParameter();
        //     searchId.ParameterName = "@searchId";
        //     searchId.Value = _id;
        //     cmd.Parameters.Add(searchId);
        //
        //     MySqlParameter description = new MySqlParameter();
        //     description.ParameterName = "@newDescription";
        //     description.Value = newDescription;
        //     cmd.Parameters.Add(description);
        //
        //     cmd.ExecuteNonQuery();
        //     _description = newDescription;
        //     conn.Close();
        //     if (conn != null)
        //     {
        //         conn.Dispose();
        //     }
        //
        // }
       //      public void AddStylist(Stylist newStylist)
       // {
       //     MySqlConnection conn = DB.Connection();
       //     conn.Open();
       //     var cmd = conn.CreateCommand() as MySqlCommand;
       //     cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";
       //
       //     MySqlParameter category_id = new MySqlParameter();
       //     category_id.ParameterName = "@CategoryId";
       //     category_id.Value = newCategory.GetId();
       //     cmd.Parameters.Add(category_id);
       //
       //     MySqlParameter item_id = new MySqlParameter();
       //     item_id.ParameterName = "@ItemId";
       //     item_id.Value = _id;
       //     cmd.Parameters.Add(item_id);
       //
       //     cmd.ExecuteNonQuery();
       //     conn.Close();
       //     if (conn != null)
       //     {
       //         conn.Dispose();
       //     }
       // }


    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM clients WHERE Id = @thisId;";

      cmd.Parameters.Add(new MySqlParameter("@thisId", _id));

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
