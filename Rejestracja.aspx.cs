using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Rejestracja : System.Web.UI.Page
{
    public string gwiazdka { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        gwiazdka = "<span class=\"gwiazdka\">*</span>";

        List<string> bledy = new List<string>();

        if (Page.IsPostBack){
            if (Request.Form["ImieInput"] == "")
                bledy.Add("Brak imienia");
            if (Request.Form["NazwiskoInput"] == "")
                bledy.Add("Brak nazwiska");
            if (Request.Form["NazwaInput"] == "")
                bledy.Add("Brak nazwy użytkownika");
        }
        
        if (bledy.Count > 0){
            string bledyHTML = "<ul>";
            foreach (string val in bledy){
                bledyHTML += "<li>"+ val +"</li>";
            }
            bledyHTML += "</ul>";
            Blad.Text = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
        }

    }
}