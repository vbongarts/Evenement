﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SchoolTemplate.Database;
using SchoolTemplate.Models;

namespace SchoolTemplate.Controllers
{
    public class HomeController : Controller
    {
        // zorg ervoor dat je hier je gebruikersnaam (leerlingnummer) en wachtwoord invult
        string connectionString = "Server=172.16.160.21;Port=3306;Database=109750;Uid=109750;Pwd=SIDEpROp;";

        public IActionResult Index()
        {
            return View(GetFestivals());
        }

        [Route("festival/{id}/{naam}")]
        public IActionResult festival(int id)
        {
            ViewData["id"] = id;
            Festival model = GetFestival(id);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Festival> GetFestivals()
        {
            List<Festival> festivals = new List<Festival>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand($"select * from festival", conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Festival p = new Festival
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString()
                        };
                        festivals.Add(p);
                    }
                }
            }

            return festivals;
        }

        private Festival GetFestival(int id)
        {
            List<Festival> festivals = new List<Festival>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand($"select * from festival where id = {id}", conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Festival p = new Festival
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Afbeelding = reader["Afbeelding"].ToString(),
                        };
                        festivals.Add(p);
                    }
                }
            }

            return festivals[0];
        }


        [Route("agenda")]
        public IActionResult Agenda()
        {
            return View(GetFestivals());
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("contact")]
        [HttpPost]
        public IActionResult Contact(PersonModel model)
        {
            // geen valide model? Dan tonen we de foutmeldingen
            if (!ModelState.IsValid)
                return View(model);

            // Model is valide. Dus we kunnen opslaan
            SavePerson(model);


            return Redirect("/gelukt");
        }
        private void SavePerson(PersonModel person)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO klant(naam, achternaam, emailadres, geb_datum)", conn);

                cmd.Parameters.Add("?voornaam", MySqlDbType.VarChar).Value = person.Voornaam;
                cmd.Parameters.Add("?achternaam", MySqlDbType.VarChar).Value = person.Achternaam;
                cmd.Parameters.Add("?email", MySqlDbType.VarChar).Value = person.Email;
                cmd.Parameters.Add("?geb_datum", MySqlDbType.VarChar).Value = person.Geboortedatum;
                cmd.ExecuteNonQuery();
            }
        }
    }
}