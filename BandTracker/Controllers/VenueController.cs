using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace BandTracker.Controllers
{
    public class VenueController : Controller
    {

      [HttpGet("/venues")]
      public ActionResult Venues()
      {
        Dictionary<string, object> model = new Dictionary<string,object>{};

        model.Add("bands", Band.GetAll());
        model.Add("venues", Venue.GetAll());
        return View(model);
      }

      [HttpPost("/venues")]
      public ActionResult VenuesPostNewVenue()
      {

        Venue newVenue = new Venue(Request.Form["name"], Request.Form["address"], int.Parse(Request.Form["capacity"]));
        newVenue.Save();

        Dictionary<string, object> model = new Dictionary<string,object>{};

        model.Add("bands", Band.GetAll());
        model.Add("venues", Venue.GetAll());
        return View("venues",model);
      }

      [HttpPost("/venues/new-bands")]
      public ActionResult VenuesAddBands()
      {
        Venue foundVenue = Venue.Find(int.Parse(Request.Form["venueId"]));

        string bandValues = (Request.Form["bands"]);
        string[] bandIds = bandValues.Split(',');

        foreach(var bandId in bandIds)
        {
            foundVenue.AddBand(int.Parse(bandId));
        }

        Dictionary<string, object> model = new Dictionary<string,object>{};
        model.Add("bands", Band.GetAll());
        model.Add("venues", Venue.GetAll());

        return View("venues",model);
      }

      [HttpPost("/venueDetails")]
      public ActionResult VenueDetails()
      {

        Venue foundVenue = Venue.Find(int.Parse(Request.Form["venueId"]));
        return View("VenueDetails",foundVenue);
      }

      [HttpPost("/venues/edited")]
      public ActionResult VenueDetailsEdit()
      {
        Venue foundVenue = Venue.Find(int.Parse(Request.Form["venueId"]));
        foundVenue.Update(Request.Form["name"], Request.Form["address"], int.Parse(Request.Form["capacity"]));

        Dictionary<string, object> model = new Dictionary<string,object>{};
        model.Add("bands", Band.GetAll());
        model.Add("venues", Venue.GetAll());

        return View("venues",model);
      }

      [HttpPost("/venues/removed")]
      public ActionResult VenuesRemoved()
      {
        Venue foundVenue = Venue.Find(int.Parse(Request.Form["venueId"]));
        foundVenue.Delete();
        Dictionary<string, object> model = new Dictionary<string,object>{};

        model.Add("bands", Band.GetAll());
        model.Add("venues", Venue.GetAll());
        return View("venues",model);
      }

      [HttpPost("/venues/removeBand")]
      public ActionResult VenuesBandRemoved()
      {
        Venue foundVenue = Venue.Find(int.Parse(Request.Form["venueId"]));

        foundVenue.RemoveBand(int.Parse(Request.Form["bandId"]));

        Dictionary<string, object> model = new Dictionary<string,object>{};

        model.Add("bands", Band.GetAll());
        model.Add("venues", Venue.GetAll());
        return View("venues",model);
      }
    }
  }
