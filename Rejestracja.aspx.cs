using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Rejestracja : System.Web.UI.Page
{
    protected void validacja(){
        List<string> bledy = new List<string>();

        if (Page.IsPostBack)
        { // Validacje
            if (ImieInput.Value == "")
                bledy.Add("Brak imienia");

            if (NazwiskoInput.Value == "")
                bledy.Add("Brak nazwiska");

            if (NazwaInput.Value == "")
                bledy.Add("Brak nazwy użytkownika");
            else
            {
                if (NazwaInput.Value.Length < 6)
                    bledy.Add("Nazwa ma mniej niż 6 znaków");
                // if znaków specjalnych
            }

            if (HasloInput.Value == "")
                bledy.Add("Brak hasła");
            else
            {
                if (HasloInput.Value.Length < 8)
                    bledy.Add("Hasło ma mniej niż 8 znaków");
                // dodatkowe 
            }

            if (HasloInput2.Value == "")
                bledy.Add("Brak powtórzenia hasła");

            if (HasloInput.Value != "" && (HasloInput2.Value != ""))
                if (HasloInput.Value != HasloInput2.Value)
                    bledy.Add("Hasła nie są identyczne");

            if (EmailInput.Value == "")
                bledy.Add("Brak adresu E-mail");

            if (EmailInput2.Value == "")
                bledy.Add("Brak powtórzenia adresu E-mail");


        }

        if (bledy.Count > 0)
        {
            string bledyHTML = "<ul>";
            foreach (string val in bledy)
            {
                bledyHTML += "<li>" + val + "</li>";
            }
            bledyHTML += "</ul>";
            Blad.Text = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
        }
    }
    public string gwiazdka { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        gwiazdka = "<span class=\"gwiazdka\">*</span>";

        validacja();

    }
}