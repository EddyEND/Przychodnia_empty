using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Kalendarz : System.Web.UI.Page
{
    string[] plDni = new string[7] {"Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };
    string[] plMiesiace = new string[12] {"Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
    
    string poprzedniMiesiacLink(int miesiac, int rok){
        int poprzedniMiesiac = miesiac-1 < 1 ? 12 : miesiac-1;
	    int poprzedniRok = miesiac-1 < 1 ? rok-1 : rok;
        return "<a href=\"." + Request.Path + "?miesiac=" + poprzedniMiesiac + "&rok=" + poprzedniRok + "\">«" + plMiesiace[poprzedniMiesiac - 1] + " " + poprzedniRok + "</a>";
    }
    string nastepnyMiesiacLink(int miesiac, int rok)
    {
        int nastepnyMiesiac = miesiac+1 > 12 ? 1 : miesiac+1;
	    int nastepnyRok = miesiac+1 > 12 ? rok+1 : rok;
        return "<a href=\"." + Request.Path + "?miesiac=" + nastepnyMiesiac + "&rok=" + nastepnyRok + "\">" + plMiesiace[nastepnyMiesiac - 1] + " " + nastepnyRok + "»</a>";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        int rok = (Request.QueryString["rok"] != null) ? Convert.ToInt32(Request.QueryString["rok"]) : DateTime.Now.Year;
        int miesiac = (Request.QueryString["miesiac"] != null
            && Convert.ToInt32(Request.QueryString["miesiac"]) >= 1
            && Convert.ToInt32(Request.QueryString["miesiac"]) <= 12)
            ? Convert.ToInt32(Request.QueryString["miesiac"])
            : DateTime.Now.Month;
        int dzien = 1;

        DateTime czas = new DateTime(rok, miesiac, dzien, 0, 0, 0, 0);
        double start = (Convert.ToInt32(czas.DayOfWeek) == 0) ? 7d : (double)czas.DayOfWeek;

        // Label1.Text = start.ToString();

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
                    div.InnerHtml += "<div class=\"komorka dni\">" + czas.Day + "</div>";
                    czas = czas.AddDays(1);
                }
                else div.InnerHtml += "<div class=\"komorka dni\"></div>";


            }
            div.InnerHtml += "</div>";
        }
        div.InnerHtml += "</div>";

        Kalendarz_.Controls.Add(div);
    }
}