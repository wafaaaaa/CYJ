﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYJ.Services;
using CYJ.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace CYJ.Controllers
{
    public class GoalController : Controller
    {
        // GET: Goal
        private readonly TeamServices _teamServices;
        private readonly WStreamServices _wstreamServices;
        private readonly CategoryServices _categoryServices;
        private readonly SubcategoryServices _subCategoryServices;
        private readonly GoalServices _goalServices;
        private readonly ConnectionServices _connectionServices; 


        public GoalController()
        {
            _teamServices = new TeamServices();
            _wstreamServices = new WStreamServices();
            _categoryServices = new CategoryServices();
            _subCategoryServices = new SubcategoryServices();
            _goalServices = new GoalServices();
        }

        public ActionResult Index()
        {
            var teams = _teamServices.GetAllTeams();
            var wstreams = _wstreamServices.GetAllWStreams();
            var categories = _categoryServices.GetAllCategories();
            var subcategories = _subCategoryServices.GetAllSubCategories();

            List<SelectListItem> tmNames = new List<SelectListItem>();
            //HomePageViewModel modal = new HomePageViewModel();

            List<TEAM> teamList = _teamServices.GetAllTeams().ToList();
            teamList.ForEach(x =>
            {
                tmNames.Add(new SelectListItem { Text = x.teamName, Value = x.teamID.ToString() });
            });

            List<SelectListItem> wsNames = new List<SelectListItem>();
            List<WORKSTREAM> wstreamList = _wstreamServices.GetAllWStreams().ToList();
            wstreamList.ForEach(x =>
            {
                wsNames.Add(new SelectListItem { Text = x.workstreamName, Value = x.workstreamID.ToString() });
            });

            /*var model = new HomePageViewModel
            {
                Teams = teams,
                WStreams = wstreams,
                Categories = categories,
                Subcategories = subcategories
            };*/

            return View(); //(model);
        }
        public ActionResult Changes(string teamID)
        {

            int wstreamsID;
            List<SelectListItem> wsNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(teamID))
            {
                wstreamsID = Convert.ToInt32(teamID);
                List<WORKSTREAM> wstreamList = _wstreamServices.GetWStreamsList(wstreamsID);
                wstreamList.ForEach(x =>
                {
                    wsNames.Add(new SelectListItem { Text = x.workstreamName, Value = x.workstreamID.ToString() });
                });
            }

            return Json(wsNames, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult Changing(string wstreamID)
        {

            int categoryID;
            List<SelectListItem> cyNames = new List<SelectListItem>();
            /*if (!string.IsNullOrEmpty(wstreamID))
            {
                categoryID = Convert.ToInt32(wstreamID);
                List<CATEGORY> categoryList = _categoryServices.GetAllCategories(categoryID);
                categoryList.ForEach(x =>
                {
                    cyNames.Add(new SelectListItem { Text = x.categoryName, Value = x.categoryID.ToString() });
                });
            }*/

            return Json(cyNames, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            CascadingModel model = new CascadingModel();
            model.TEAMS = PopulateDropDown("SELECT teamID, teamName FROM TEAM", "teamName", "teamID");
            return View(model);
        }

        [HttpPost]
        public JsonResult AjaxMethod(string type, int value)
        {
            CascadingModel model = new CascadingModel();
            switch (type)
            {
                case "teamID":
                    model.WORKSTREAMS = PopulateDropDown("SELECT wstreamID, wstreamName FROM WSTREAM WHERE teamID = " + value, "wstreamName", "wstreamID");
                    break;
                case "wstreamID":
                    model.CATEGORIES = PopulateDropDown("SELECT categID, categName FROM CATEGORY WHERE wstreamID = " + value, "categName", "categID");
                    break;
                case "categID":
                    model.SUBCATEGORIES = PopulateDropDown("SELECT subcategID, subcategName FROM SUBCATEGORY WHERE categID = " + value, "subcategName", "subcategID");
                    break;
                case "subcategID":
                    model.GOALS = PopulateDropDown("SELECT agoalID, agoalValue FROM AGOAL WHERE subcategID = " + value, "agoalValue", "agoalID");
                    break;

            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult Contact(int teamID, int wstreamID, int categID, int subcategID)
        {
            CascadingModel model = new CascadingModel();
            model.TEAMS = PopulateDropDown("SELECT teamID, teamName FROM TEAM", "teamName", "teamID");
            model.WORKSTREAMS = PopulateDropDown("SELECT wstreamID, wstreamName FROM WSTREAM WHERE teamID = " + teamID, "wstreamName", "wstreamID");
            model.CATEGORIES = PopulateDropDown("SELECT categID, categName FROM CATEGORY WHERE wstreamID = " + wstreamID, "categName", "categID");
            model.SUBCATEGORIES = PopulateDropDown("SELECT subcategID, subcategName FROM SUBCATEGORY WHERE categID = " + categID, "subcategName", "subcategID");
            model.GOALS = PopulateDropDown("SELECT agoalID, agoalValue FROM AGOAL WHERE subcategID = " + subcategID, "agoalValue", "agoalID");
            return View(model);
        }

        private static List<SelectListItem> PopulateDropDown(string query, string textColumn, string valueColumn)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            // Constring
            // mastermodelEntities
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr[textColumn].ToString(),
                                Value = sdr[valueColumn].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }
    }
}