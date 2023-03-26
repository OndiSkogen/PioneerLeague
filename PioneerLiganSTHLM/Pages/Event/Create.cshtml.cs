using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PioneerLiganSTHLM.Data;
using PioneerLiganSTHLM.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace PioneerLiganSTHLM.Pages.Event
{
    //[Authorize]
    public class CreateModel : PageModel
    {
        private readonly PioneerLiganSTHLMContext _context;

        public CreateModel(PioneerLiganSTHLMContext context)
        {
            _context = context;
        }


        public IActionResult OnGet(string? selectedId)
        {
            LoadData();

            if (selectedId != null)
            {
                SelectedLeague = int.Parse(selectedId);
                DisplayEvents = Events.Where(i => i.LeagueID == SelectedLeague).ToList();
            }
            else
            {
                SelectedLeague = 0;
            }

            return Page();
        }

        [BindProperty]
        public string HtmlContent { get; set; } = string.Empty;

        [BindProperty]
        public Models.Event Event { get; set; } = default!;
        public List<Models.Event> Events { get; set; } = new List<Models.Event>();
        public List<Models.Event> DisplayEvents { get; set; } = new List<Models.Event>();
        public List<Models.League> Leagues { get; set; } = new List<Models.League>();
        public List<Models.Player> Players { get; set; } = new List<Models.Player>();
        public int SelectedLeague { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            //return RedirectToPage("./Index");
            SelectedLeague = int.Parse(Request.Form["league-id"]);

            if (!ModelState.IsValid || _context.Event == null || _context.EventResult == null || _context.Player == null || SelectedLeague == 0)
            {
                return Page();
            }

            LoadData();

            Event.LeagueID = SelectedLeague;
            _context.Event.Add(Event);
            await _context.SaveChangesAsync();

            //UpdateEvent();

            var doc = new HtmlDocument();
            doc.LoadHtml(HtmlContent);

            // Select the table element you want to extract data from
            var table = doc.DocumentNode.SelectSingleNode("//table");

            // Extract the table rows
            var rows = table.SelectNodes(".//tr");

            // Loop through each row and extract the data from the cells
            foreach (var row in rows)
            {
                // Extract the cells in this row
                var cells = row.SelectNodes(".//td");

                var eventResult = new Models.EventResult();
                eventResult.EventId = Event.ID;

                // Loop through each cell and extract the data
                if (cells != null)
                {
                    int i = 0;
                    foreach (var cell in cells)
                    {
                        // Extract the inner text of the cell
                        var cellText = cell.InnerText;
                        string[] temp;

                        // Do something with the cell text
                        if (cellText != null)
                        {
                            switch (i)
                            {
                                case 0:
                                    eventResult.Placement = int.Parse(cellText);
                                    break;
                                case 1:
                                    eventResult.PlayerName = ExtractPlayerName(cellText);
                                    break;
                                case 2:
                                    eventResult.Points = int.Parse(cellText);
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    temp = cellText.Split('%');
                                    eventResult.OMW = float.Parse(temp[0]);
                                    break;
                                case 5:
                                    temp = cellText.Split('%');
                                    eventResult.GW = float.Parse(temp[0]);
                                    break;
                                case 6:
                                    temp = cellText.Split('%');
                                    eventResult.OGW = float.Parse(temp[0]);
                                    break;
                                default:
                                    break;
                            }
                        }

                        i++;
                    }
                }

                var playerExists = Players.Where(n => n.Name == eventResult.PlayerName).ToList();
                if (playerExists.Any())
                {
                    var playerToUpdate = playerExists.First();
                    playerToUpdate.Events++;
                    playerToUpdate.Points += eventResult.Points;

                    playerToUpdate = AddWinLossTie(playerToUpdate, eventResult.Points);

                    _context.Player.Update(playerToUpdate);
                    await _context.SaveChangesAsync();

                    eventResult.PlayerId = playerToUpdate.ID;

                    _context.EventResult.Add(eventResult);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var playerToAdd = new Models.Player();

                    playerToAdd.Name = eventResult.PlayerName;
                    playerToAdd.Events = 1;
                    playerToAdd.Points = eventResult.Points;

                    playerToAdd = AddWinLossTie(playerToAdd, eventResult.Points);

                    _context.Player.Add(playerToAdd);
                    await _context.SaveChangesAsync();

                    eventResult.PlayerId = playerToAdd.ID;
                    _context.EventResult.Add(eventResult);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }

        private string ExtractPlayerName(string cellText)
        {
            string nameToExtract = string.Empty;
            cellText = cellText.Trim();
            string[] splits = cellText.Split(' ');

            foreach (var str in splits)
            {
                if (IsOnlyLetters(str))
                {
                    nameToExtract += str + " ";
                }
            }
            nameToExtract = nameToExtract.Trim();
            nameToExtract = HandleEdgeCases(nameToExtract);

            return nameToExtract;
        }

        private string HandleEdgeCases(string edgeCase)
        {
            switch (edgeCase.ToLower())
            {
                case "bo":
                    return "Bo Strandin Pers";
                case "österberg":
                    return "Fredrik Österberg";
                default:
                    return edgeCase;
            }
        }

        private bool IsOnlyLetters(string cellText)
        {
            string pattern = "^([A-Z]|Å|Ä|Ö|Í|Á)([a-z]|å|ä|ö|í|á)+$";
            Regex regex = new Regex(pattern);
            bool containsOnlyLetters = regex.IsMatch(cellText);

            return containsOnlyLetters;
        }

        private void LoadData()
        {
            var leagues = from l in _context.League select l;
            Leagues = leagues.OrderBy(l => l.ID).ToList();
            var events = from e in _context.Event select e;
            Events = events.OrderBy(l => l.ID).ToList();
            var players = from p in _context.Player select p;
            Players = players.ToList();
        }

        private Models.Player AddWinLossTie(Models.Player player, int points)
        {
            switch (points)
            {
                case 0:
                    player.Losses += 4;
                    break;
                case 1:
                    player.Ties++;
                    player.Losses += 3;
                    break;
                case 2:
                    player.Ties += 2;
                    player.Losses += 2;
                    break;
                case 3:
                    player.Wins++;
                    player.Losses += 3;
                    break;
                case 4:
                    player.Wins++;
                    player.Ties++;
                    player.Losses += 2;
                    break;
                case 5:
                    player.Wins++;
                    player.Ties += 2;
                    player.Losses++;
                    break;
                case 6:
                    player.Wins += 2;
                    player.Losses += 2;
                    break;
                case 7:
                    player.Wins += 2;
                    player.Ties++;
                    player.Losses++;
                    break;
                case 8:
                    player.Wins += 2;
                    player.Ties += 2;
                    break;
                case 9:
                    player.Wins += 3;
                    player.Losses++;
                    break;
                case 10:
                    player.Wins += 3;
                    player.Ties++;
                    break;
                case 12:
                    player.Wins += 4;
                    break;
                default:
                    break;
            }

            return player;
        }

        public string NamePlayer(int id)
        {
            var str = "player" + id.ToString();
            return str;
        }

        public string OmwPlayer(int id)
        {
            var str = "omw" + id.ToString();
            return str;
        }

        public string GwPlayer(int id)
        {
            var str = "gw" + id.ToString();
            return str;
        }

        public string OgwPlayer(int id)
        {
            var str = "ogw" + id.ToString();
            return str;
        }

        public string PlacementPlayer(int id)
        {
            var str = "placement" + id.ToString();
            return str;
        }

        public string PointsPlayer(int id)
        {
            var str = "points" + id.ToString();
            return str;
        }
    }
}
