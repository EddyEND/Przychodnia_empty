<%@ Page Title="Stron główna" Language="C#" MasterPageFile="~/Szablon.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ContentPlaceholderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceholderID="MainContent" runat="server">
    <div id="TrescGlowna">
        <%-- %><div><%=Application["i"] %></div><%-- wyświetlanie zmiennych --%>
    </div>
    <div id="InformacjeFull">
        <div>
            <div class="InformacjePol">
                <div>
                    <h2>Aktualności</h2>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                </div>
            </div>
            <div class="InformacjePol">
                <div>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                </div>
            </div>
            <div class="InformacjePol">
                <div>
                    <p>Trzecia Informacja</p>
                    <p>Trzecia Informacja</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>