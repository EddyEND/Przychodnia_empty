<%@ Page Title="Admin" Language="C#" MasterPageFile="~/admin/MasterAdmin.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../js/admin.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="ramka">
        <div id="admin" runat="server"> 
            <div id="Blad" runat="server"></div>
            <%--<div class="naglowek">Umów wizytę w naszej przychodni</div>
            <div id="Blad" runat="server"></div>
            <div class="wiersz">
                <div id="dataInput" class="pole">
                    <label for="DzienInput">Data: </label>
                    <select id="DzienInput" name="DzienInput" runat="server"></select>
                    <select id="MiesiacInput" name="MiesiacInput" runat="server"></select>
                    <select id="RokInput" name="RokInput" runat="server"></select>
                </div>
            </div>
            <div class="wiersz">
                <div id="specInput" class="pole"><label for="SpecjalistaInput">Specjalista: </label><select id="SpecjalistaInput" name="SpecjalistaInput" runat="server"></select></div>
            </div>
            <input type="submit" id="Submit1" value="Umów wizytę" />--%>
        </div>
    </div>
</asp:Content>