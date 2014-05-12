using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Redirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl div = new HtmlGenericControl("div");
        string action = (!string.IsNullOrEmpty(Request.QueryString["a"])) ? Request.QueryString["a"] : "";
        string link = (!string.IsNullOrEmpty(Request.QueryString["link"])) ? Request.QueryString["link"] : "./";

        if (action == "login"){
            Page.Title = "Logowanie";
            if (link.IndexOf("Logowanie.aspx") != -1 || link.IndexOf("Rejestracja.aspx") != -1)
                link = "./";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Zalogowano prawidłowo.<br />Teraz nastąpi przeniesienie do poprzedniej lokalizacji.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "logout"){
            Page.Title = "Wyloguj";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Wylogowano prawidłowo.<br />Teraz nastąpi przeniesienie na stronę główną.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "permission"){
            Page.Title = "Brak uprawnień";
            link = "./Logowanie.aspx";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Aby przeglądać tą stronę musisz być zalogowany.<br />Teraz nastąpi przeniesienie na stronę logowania.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }

        HtmlMeta metaKey = new HtmlMeta();
        metaKey.HttpEquiv = "Refresh";
        metaKey.Content = "2; url=" + link;
        Page.Header.Controls.Add(metaKey);

        redirect.Controls.Add(div);
    }
}