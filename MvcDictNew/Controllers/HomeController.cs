using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using MvcDictNew.Models;
using System.Collections;
using System.Net;

namespace MvcDictNew.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Dictionary(FormCollection FormItems)
        {
            string myWord = FormItems["searchTerm"];
                        
            using(WebClient wc = new WebClient())
            {               
                    var json = wc.DownloadString("https://dictionaryapi.com/api/v3/references/thesaurus/json/" + myWord + "?key=xxxxxxxxx");

                    JArray dict = JArray.Parse(json);

                    // List Declarations & Definitions

                    List<string> definition = new List<string>();

                    List<string> list = new List<string>();

                    List<string> antList = new List<string>();

                    UsageClass usgObj = new UsageClass();

                    foreach (var dictItem in dict)
                    {
                        // DEFINITION

                        IList<JToken> wordDef = dictItem["shortdef"].Children().ToList();

                        foreach (JToken def in wordDef)
                        {
                            string d = def.ToObject<string>();
                            definition.Add(d);
                        }

                        // SYNONYMS

                        IList<JToken> syns = dictItem["meta"]["syns"].Children().ToList();

                        foreach (JToken syn in syns)
                        {
                            list.AddRange(syn.ToObject<List<string>>());

                        }

                        // ANTONYMS

                        IList<JToken> ants = dict[0]["meta"]["ants"].Children().ToList();

                        foreach (JToken ant in ants)
                        {
                            antList.AddRange(ant.ToObject<List<string>>());

                        }

                        // USAGE

                        IList<JToken> usgs = dict[0]["def"][0]["sseq"][0][0][1]["dt"][1][1].Children().ToList();

                        foreach (JToken usg in usgs)
                        {
                            usgObj.Sample = (string)usg["t"];

                        }
                    }

                    List<DictModel> model = new List<DictModel>();
                    model.Add(new DictModel
                    {
                        Def = definition,
                        Syns = list,
                        Ants = antList,
                        Usage = new UsageClass { Sample = usgObj.Sample }
                    });

                    ViewData["Word"] = myWord;

                    return View(model);
            }  
        }
    }
}
