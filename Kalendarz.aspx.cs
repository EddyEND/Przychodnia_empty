using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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

        double x = (DateTime.DaysInMonth(rok, miesiac) + start) / 7d;
        int tygodni = Convert.ToInt32(Math.Ceiling(x));
        bool off = false;

        
        // link prev/next miesiac
        Label1.Text += "<div>" + plMiesiace[miesiac - 1] + " " + rok + "</div>";
        Label1.Text += "<div>" + poprzedniMiesiacLink(miesiac, rok) + " " + nastepnyMiesiacLink(miesiac, rok) + "</div>";

        Label1.Text += "<table><tr>";
        for (int j = 0; j < 7; j++) {
            Label1.Text += "<td>" + plDni[j] + "</td>";
        }
        Label1.Text += "</tr>";
        for (int i = 0; i < tygodni; i++) {
            Label1.Text += "<tr>";
            for (int j = 0; j < 7; j++) {
                if (j == start-1) off = true;
                if (off && czas.Month == miesiac){
                    Label1.Text += "<td>" + czas.Day + "</td>";
                    czas = czas.AddDays(1);
                }
                    else Label1.Text += "<td></td>";


            }
            Label1.Text += "</tr>";
        }
        Label1.Text += "</table>";
    }
}