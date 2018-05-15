using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace BandTracker.Controllers
{
    public class BandController : Controller
    {

    [HttpGet("/bands")]
    public ActionResult Bands()
    {
      Dictionary<string, object> model = new Dictionary<string,object>{};

      model.Add("bands", Band.GetAll());
      model.Add("venues", Venue.GetAll());
      return View(model);
    }

    [HttpPost("/bands")]
    public ActionResult BandsPostNewBand()
    {

      Band newBand = new Band(Request.Form["name"], Request.Form["genre"]);
      newBand.Save();

      Dictionary<string, object> model = new Dictionary<string,object>{};

      model.Add("bands", Band.GetAll());
      model.Add("venues", Venue.GetAll());
      return View("bands",model);
    }

    [HttpPost("/bands/new-venues")]
    public ActionResult BandsAddVenues()
    {

      Band foundBand = Band.Find(int.Parse(Request.Form["bandId"]));

      string venueValues = (Request.Form["venues"]);
      string[] venueIds = venueValues.Split(',');

      foreach(var venueId in venueIds)
      {
          foundBand.AddVenue(int.Parse(venueId));
      }

      Dictionary<string, object> model = new Dictionary<string,object>{};
      model.Add("bands", Band.GetAll());
      model.Add("venues", Venue.GetAll());

      return View("bands",model);
    }

    [HttpPost("/bandDetails")]
    public ActionResult BandDetails()
    {
      Band foundBand = Band.Find(int.Parse(Request.Form["bandId"]));
      return View("BandDetails",foundBand);
    }

    [HttpPost("/bands/edited")]
    public ActionResult BandDetailsEdit()
    {
      Band foundBand = Band.Find(int.Parse(Request.Form["bandId"]));
      foundBand.Update(Request.Form["name"], Request.Form["genre"]);

      Dictionary<string, object> model = new Dictionary<string,object>{};
      model.Add("bands", Band.GetAll());
      model.Add("venues", Venue.GetAll());

      return View("bands",model);
    }

    [HttpPost("/bands/removed")]
    public ActionResult BandsRemoved()
    {
      Band foundBand = Band.Find(int.Parse(Request.Form["bandId"]));
      foundBand.Delete();
      Dictionary<string, object> model = new Dictionary<string,object>{};

      model.Add("bands", Band.GetAll());
      model.Add("venues", Venue.GetAll());
      return View("bands",model);
    }

    [HttpPost("/bands/removeVenue")]
    public ActionResult BandsVenueRemoved()
    {
      Band foundBand = Band.Find(int.Parse(Request.Form["bandId"]));
      foundBand.RemoveVenue(int.Parse(Request.Form["venueId"]));

      Dictionary<string, object> model = new Dictionary<string,object>{};
      model.Add("bands", Band.GetAll());
      model.Add("venues", Venue.GetAll());
      return View("bands",model);
    }
  }
}
