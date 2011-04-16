<%@ Page Title="Startseite" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ZPA._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Willkommen beim lokalen ZPA</h2>
    <asp:Button ID="ButtonLogin" runat="server"
    PostBackUrl="~/Courses.aspx?todo=login" Text="Anmelden" />
    <br />
</asp:Content>
