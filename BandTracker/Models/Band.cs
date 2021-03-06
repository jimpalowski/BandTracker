using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Linq;


namespace BandTracker.Models
{

  public class Band
  {
    private int _id;
    private string _name;
    private string _genre;

    public Band(string name, string genre,int id = 0)
    {
      _name = name;
      _genre = genre;
      _id = id;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetGenre()
    {
      return _genre;
    }


    public override bool Equals(System.Object otherBand)
    {
      if(!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = this.GetId() == newBand.GetId();
        bool nameEquality = this.GetName() == newBand.GetName();
        bool genreEquality = this.GetGenre() == newBand.GetGenre();
        return (idEquality && nameEquality && genreEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Band> GetAll()
    {
      List<Band> allBands = new List<Band> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string genre = rdr.GetString(2);

        Band newBand = new Band(name, genre, id);
        allBands.Add(newBand);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allBands;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands (name, genre) VALUES (@name, @genre);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = _name;
      cmd.Parameters.Add(name);

      MySqlParameter genre = new MySqlParameter();
      genre.ParameterName = "@genre";
      genre.Value = _genre;
      cmd.Parameters.Add(genre);


      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands; DELETE FROM bands_venues;";

      cmd.ExecuteNonQuery();
      conn.Close();

      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public static Band Find (int id)
    {

      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands WHERE id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int myId = 0;
      string name = "";
      string genre = "";

      while(rdr.Read())
      {
        myId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        genre = rdr.GetString(2);
      }
      Band newBand = new Band(name, genre, myId);
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return newBand;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands WHERE id = @thisId; DELETE FROM bands_venues WHERE band_id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public void Update(string newName, string newGenre, string newImage)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE bands SET name = @name, genre = @genre WHERE id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = newName;
      cmd.Parameters.Add(name);

      MySqlParameter genre = new MySqlParameter();
      genre.ParameterName = "@genre";
      genre.Value = newGenre;
      cmd.Parameters.Add(genre);

      _name = newName;
      _genre = newGenre;

      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddVenue(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands_venues (band_id, venue_id) VALUES (@thisId, @venueId);";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      MySqlParameter venueId = new MySqlParameter();
      venueId.ParameterName = "@venueId";
      venueId.Value = id;
      cmd.Parameters.Add(venueId);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Venue> GetVenues()
    {
      List<Venue> bandVenues = new List<Venue>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT venues.* FROM bands
      JOIN bands_venues ON (bands.id = bands_venues.band_id)
      JOIN venues ON (bands_venues.venue_id = venues.id)
      WHERE bands.id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string address = rdr.GetString(2);
        int capacity = rdr.GetInt32(3);

        Venue newVenue = new Venue(name, address,capacity, id);
        bandVenues.Add(newVenue);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return bandVenues;
    }

    public void RemoveVenue(int venueId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands_venues WHERE band_id = @thisId AND venue_id = @venueId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      MySqlParameter venueIdParameter = new MySqlParameter();
      venueIdParameter.ParameterName = "@venueId";
      venueIdParameter.Value = venueId;
      cmd.Parameters.Add(venueIdParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    // public static List<Band> Search(string bandName)
    // {
    //   List<Band> allBandsFound = new List<Band> {};
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   var cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"SELECT * FROM bands WHERE name LIKE CONCAT('%',@bandName,'%');";
    //
    //   MySqlParameter bandNameParameter = new MySqlParameter();
    //   bandNameParameter.ParameterName = "@bandName";
    //   bandNameParameter.Value = bandName;
    //   cmd.Parameters.Add(bandNameParameter);
    //
    //   var rdr = cmd.ExecuteReader() as MySqlDataReader;
    //   while(rdr.Read())
    //   {
    //     int id = rdr.GetInt32(0);
    //     string name = rdr.GetString(1);
    //     string genre = rdr.GetString(2);
    //
    //     Band newBand = new Band(name, genre, id);
    //     allBandsFound.Add(newBand);
    //   }
    //   conn.Close();
    //   if(conn != null)
    //   {
    //     conn.Dispose();
    //   }
    //   return allBandsFound;
    // }
    //
    // public List<Venue> GetUniqueVenues()
    // {
    //   List<Venue> allVenues = Venue.GetAll();
    //   List<Venue> bandVenues = this.GetVenues();
    //
    //   return allVenues.Except(bandVenues).ToList();
    // }

  }

}
