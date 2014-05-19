using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Kalendarz : System.Web.UI.Page
{
    private static string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
    private static MySqlConnection conn = new MySqlConnection(connStr);

    private string sql;
    private MySqlCommand zapytanie;
    private static DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private TimeSpan timestamp, timestamp2;
    private DateTime czas;

    private static string[] plDni = new string[7] { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };
    private static string[] plMiesiace = new string[12] { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };

    private static Dictionary<int, object[]> wizyty = new Dictionary<int, object[]>();
    private static Dictionary<int, object[]> wizytyDzien = new Dictionary<int, object[]>();

    private int rok, miesiac, dzien = 1;
    private string tip;
    
    protected string poprzedniMiesiacLink(int miesiac, int rok){
        int poprzedniMiesiac = miesiac-1 < 1 ? 12 : miesiac-1;
	    int poprzedniRok = miesiac-1 < 1 ? rok-1 : rok;
        return "<a href=\"." + Request.Path + "?miesiac=" + poprzedniMiesiac + "&rok=" + poprzedniRok + "\">«" + plMiesiace[poprzedniMiesiac - 1] + " " + poprzedniRok + "</a>";
    }
    protected string nastepnyMiesiacLink(int miesiac, int rok)
    {
        int nastepnyMiesiac = miesiac+1 > 12 ? 1 : miesiac+1;
	    int nastepnyRok = miesiac+1 > 12 ? rok+1 : rok;
        return "<a href=\"." + Request.Path + "?miesiac=" + nastepnyMiesiac + "&rok=" + nastepnyRok + "\">" + plMiesiace[nastepnyMiesiac - 1] + " " + nastepnyRok + "»</a>";
    }
    protected void szukajWizyt(int dzien)
    {
        timestamp = new DateTime(rok, miesiac, dzien, 0, 0, 0, 0, DateTimeKind.Utc) - unix;
        timestamp2 = new DateTime(rok, miesiac, dzien, 0, 0, 0, 0, DateTimeKind.Utc).AddDays(1) - unix;

        wizytyDzien.Clear();
        foreach (KeyValuePair<int, object[]> wizyta in wizyty)
        {
            if (Convert.ToInt32(wizyta.Value[0]) >= timestamp.TotalSeconds && Convert.ToInt32(wizyta.Value[0]) < timestamp2.TotalSeconds)
                wizytyDzien.Add(wizyta.Key, new object[4] { wizyta.Value[0], wizyta.Value[1], wizyta.Value[2], wizyta.Value[3] });
        }
    }
    protected void getWizyty()
    {
        try
        {
            conn.Open();
            sql = "SELECT w.id, w.`data`, s.specjalizacja, u.imie, u.nazwisko FROM wizyty w JOIN specjalisci s ON w.ids=s.id JOIN users u ON s.uid=u.id WHERE idu=@Idu AND `data` >= @DataMin AND `data` < @DataMax";

            zapytanie = new MySqlCommand(sql, conn);
            zapytanie.Parameters.Add(new MySqlParameter("@Idu", Session["id"].ToString()));

            timestamp = new DateTime(rok, miesiac, dzien, 0, 0, 0, 0, DateTimeKind.Utc) - unix;
            timestamp2 = new DateTime(rok, miesiac, dzien, 0, 0, 0, 0, DateTimeKind.Utc).AddMonths(1) - unix;

            zapytanie.Parameters.Add(new MySqlParameter("@DataMin", timestamp.TotalSeconds.ToString()));
            zapytanie.Parameters.Add(new MySqlParameter("@DataMax", timestamp2.TotalSeconds.ToString()));

            
            MySqlDataReader wynik = zapytanie.ExecuteReader();

            wizyty.Clear();

            while (wynik.Read())
            {
                wizyty.Add(Convert.ToInt32(wynik[0]), new object[4] { wynik[1], wynik[2], wynik[3], wynik[4] });
            }
            wynik.Close();
            
        }
        catch (MySqlException e)
        {
            Response.Write(e);
        }
        finally
        {
            conn.Close();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["rok"] == null || !Int32.TryParse(Request.QueryString["rok"], out rok)) rok = DateTime.Now.Year;
        if (Request.QueryString["miesiac"] == null || !Int32.TryParse(Request.QueryString["miesiac"], out miesiac) || miesiac < 1 || miesiac > 12) miesiac = DateTime.Now.Month;

        if (Session["zalogowany"] != null)
            getWizyty();
        else
        {
            wizyty.Clear();
            wizytyDzien.Clear();
        }

        DateTime czas = new DateTime(rok, miesiac, dzien, 0, 0, 0, 0, DateTimeKind.Utc);
        double start = (Convert.ToInt32(czas.DayOfWeek) == 0) ? 7d : (double)czas.DayOfWeek;

        double x = (DateTime.DaysInMonth(rok, miesiac) + start-1) / 7d;
        int tygodni = Convert.ToInt32(Math.Ceiling(x));
        bool off = false;

        HtmlGenericControl div = new HtmlGenericControl("div");
        
        // link prev/next miesiac
        div.InnerHtml += "<div><div class=\"miesiac\">" + plMiesiace[miesiac - 1] + " " + rok + "</div><div class=\"przelaczniki\">" + poprzedniMiesiacLink(miesiac, rok) + " | " + nastepnyMiesiacLink(miesiac, rok) + "</div></div>";

        div.InnerHtml += "<div id=\"tabela\"><div class=\"wiersz\">";
        for (int j = 0; j < 7; j++) {
            div.InnerHtml += "<div class=\"komorka tydzien\">" + plDni[j] + "</div>";
        }
        div.InnerHtml += "</div>";
        for (int i = 0; i < tygodni; i++) {
            div.InnerHtml += "<div class=\"wiersz\">";
            for (int j = 0; j < 7; j++) {
                if (j == start-1) off = true;
                if (off && czas.Month == miesiac){
                    szukajWizyt(czas.Day);
                    div.InnerHtml += "<div class=\"komorka dni\"><div" + ((czas == DateTime.Today && czas.Day == DateTime.Today.Day) ? " class=\"dzisiaj\" " : "") + ">" + czas.Day + "</div>";
                    foreach (KeyValuePair<int, object[]> wizyta in wizyty)
                    {
                        if (Convert.ToInt32(wizyta.Value[0]) >= timestamp.TotalSeconds && Convert.ToInt32(wizyta.Value[0]) < timestamp2.TotalSeconds)
                        {
                            czas = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToInt32(wizyta.Value[0]));
                            tip = "";
                            tip += "<div>" + czas.Day + " " + plMiesiace[czas.Month - 1] + " " + czas.Year + "</div>";
                            tip += "<div>Godzina: " + czas.Hour + ":" + ((czas.Minute > 9) ? czas.Minute.ToString() : ("0" + czas.Minute)) + "</div>";
                            tip += "<div style=\"margin-top: 4px;\">Specjalista: </div><div>" + wizyta.Value[1] + " - Dr " + wizyta.Value[2] + " " + wizyta.Value[3] + "</div>";
                            div.InnerHtml += "<div class=\"tip_trigger\">Wizyta " + czas.Hour + ":" + ((czas.Minute > 9) ? czas.Minute.ToString() : ("0" + czas.Minute)) + "<div class=\"tip\">" + tip + "</div></div>";
                        }
                    }
                    div.InnerHtml += "</div>";
                    czas = czas.AddDays(1);
                }
                else div.InnerHtml += "<div class=\"komorka dni\">&nbsp;</div>";


            }
            div.InnerHtml += "</div>";
        }
        div.InnerHtml += "</div>";

        Kalendarz_.Controls.Add(div);
    }
}