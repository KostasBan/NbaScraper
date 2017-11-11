using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NbaScraper
{
    //if htmlAgitlityPack isnt install
    //1.right click on references and select Manage NuGet packages
    //2.search and install the HtmlAgilityPack
    class Scrapper
    {
        static void Main(string[] args)
        {
            String homeUrl = "";
            //String octoberGames = "";
            String octoberGames = "";
            List<string> gamesUrls = new List<string>();
            List<Game> gamesPlayed = new List<Game>();
            gamesUrls = FetchUrls(homeUrl, octoberGames);


            foreach (string url in gamesUrls)
            {
                //Stops for 10 secs
                System.Threading.Thread.Sleep(10000);
                foreach (Game temp in FetchGame(homeUrl, url))
                {
                    gamesPlayed.Add(temp);
                }
            }
            //Console.WriteLine(gamesPlayed.Count);

            List<string> lines = new List<string>();
            lines.Add("TeamName,won,home,score,ortg,fg,fga,3p,3pa,ft,fta,orb,drb,ast,stl,blk,tov,pf");
            foreach (Game temp in gamesPlayed)
            {
                lines.Add(temp.teamName + "," + temp.won + "," + temp.home + "," + temp.score + "," + temp.ortg + "," + temp.fg + ","+temp.fgA + "," 
                    +temp.p3 + "," +temp.p3A + "," +temp.ft + "," +temp.ftA + "," +temp.orb + "," +temp.drb + "," +temp.ast + "," +temp.stl + "," 
                    +temp.blk + "," +temp.tov + "," +temp.pf);
            }
            //lines.Add(gamesPlayed.Count.ToString());

            System.IO.File.WriteAllLines(@"C:\Users\user\Desktop\NovemberMatches.txt", lines);


            //Console.ReadKey();
        }
        static HtmlDocument FetchPage(string homeUrl, string extention)
        {
            HtmlWeb web = new HtmlWeb();
            return (web.Load(homeUrl + extention));
        }
        static List<HtmlNode> LookUp(HtmlDocument doc, string xpath){
            return (doc.DocumentNode.SelectNodes(xpath).ToList());
        }
        static List<string> FetchUrls(string homeUrl, string monthUrl)
        {
            string lookFor = "//td[@data-stat='box_score_text']";
            
            var boxScores = LookUp( FetchPage(homeUrl, monthUrl),lookFor);
            List<string> scoreUrls = new List<string>();
            char[] stringSeparators = new char[] { '"' };
            string[] temp;
            foreach (var Scores in boxScores)
            {
                temp = Scores.InnerHtml.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (temp.Length >= 3)
                {
                    scoreUrls.Add(temp[1]);
                }
            }
            return (scoreUrls);
        }

        static List<Game> FetchGame(string homeUrl,string gameUrl)
        {
            Game guest = new Game();
            Game home = new Game();
            guest.home = false;
            home.home = true;
            HtmlDocument doc = FetchPage(homeUrl, gameUrl);

            var answer = LookUp(doc, "//a[@itemprop='name']");
            guest.teamName = answer[0].InnerText;
            home.teamName= answer[1].InnerText;
            answer = LookUp(doc, "//div[@class='score']");
            guest.score = float.Parse(answer[0].InnerText);
            home.score = float.Parse(answer[1].InnerText);
            if (guest.score > home.score)
            {
                guest.won = true;
                home.won = false;
            }
            else
            {
                guest.won = false;
                home.won = true;
            }
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='off_rtg']");
            guest.ortg = float.Parse(answer[0].InnerText);
            home.ortg = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='fg']");
            guest.fg = float.Parse(answer[0].InnerText);
            home.fg = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='fga']");
            guest.fgA = float.Parse(answer[0].InnerText);
            home.fgA = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='fg3']");
            guest.p3 = float.Parse(answer[0].InnerText);
            home.p3 = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='fg3a']");
            guest.p3A = float.Parse(answer[0].InnerText);
            home.p3A = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='ft']");
            guest.ft = float.Parse(answer[0].InnerText);
            home.ft = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='fta']");
            guest.ftA = float.Parse(answer[0].InnerText);
            home.ftA = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='orb']");
            guest.orb = float.Parse(answer[0].InnerText);
            home.orb = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='drb']");
            guest.drb = float.Parse(answer[0].InnerText);
            home.drb = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='ast']");
            guest.ast = float.Parse(answer[0].InnerText);
            home.ast = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='stl']");
            guest.stl = float.Parse(answer[0].InnerText);
            home.stl = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='blk']");
            guest.blk = float.Parse(answer[0].InnerText);
            home.blk = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='tov']");
            guest.tov = float.Parse(answer[0].InnerText);
            home.tov = float.Parse(answer[1].InnerText);
            answer = LookUp(doc, "//tfoot/tr/td[@data-stat='pf']");
            guest.pf = float.Parse(answer[0].InnerText);
            home.pf = float.Parse(answer[1].InnerText);

            List<Game> results= new List<Game>();
            results.Add(guest);
            results.Add(home);
            return (results);
        }

    }

    class Game
    {
        public float score,ortg, fg,fgA,p3,p3A,ft,ftA,orb,drb,ast,stl,blk,tov,pf;
        public string teamName;
        public bool won,home;
        public Game()
        {

        }
    }
}
