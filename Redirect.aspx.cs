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
        else if (action == "chdane")
        {
            Page.Title = "Panel użytkownika";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Dane zmienione prawidłowo.<br />Teraz nastąpi przeniesienie do poprzedniej lokalizacji.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "chemail")
        {
            Page.Title = "Panel użytkownika";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Adres E-mail zmieniony prawidłowo.<br />Teraz nastąpi przeniesienie do poprzedniej lokalizacji.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "chhaslo")
        {
            Session.Abandon();
            Page.Title = "Panel użytkownika";
            link = "./Logowanie.aspx";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Hasło zmienione prawidłowo.<br />Teraz nastąpi przeniesienie na stronę logowania.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "permission"){
            Page.Title = "Brak uprawnień";
            if (link == "admin") link = "../Logowanie.aspx";
            else link = "./Logowanie.aspx";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Aby przeglądać tą stronę musisz być zalogowany.<br />Teraz nastąpi przeniesienie na stronę logowania.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "adminperm")
        {
            Page.Title = "Brak uprawnień";
            link = "../";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Do tej strony mają dostep tylko administratorzy.<br />Teraz nastąpi przeniesienie na stronę główną.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "useredit")
        {
            Page.Title = "Admin";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Konto edytowano prawidłowo.<br />Teraz nastąpi przeniesienie do poprzedniej lokalizacji.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "newsedit")
        {
            Page.Title = "Admin";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Aktualność edytowana prawidłowo.<br />Teraz nastąpi przeniesienie do poprzedniej lokalizacji.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }
        else if (action == "newsadd")
        {
            Page.Title = "Admin";
            div.InnerHtml = "<div class=\"top\">Przychodnia</div><div class=\"middle\">Aktualność dodana prawidłowo.<br />Teraz nastąpi przeniesienie do poprzedniej lokalizacji.</div><div class=\"bottom\"><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a></div>";
        }

        HtmlMeta metaKey = new HtmlMeta();
        metaKey.HttpEquiv = "Refresh";
        metaKey.Content = "2; url=" + link;
        Page.Header.Controls.Add(metaKey);

        redirect.Controls.Add(div);
    }
}