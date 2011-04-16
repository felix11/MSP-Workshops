<%@ Page Title="Startseite" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Courses.aspx.cs" Inherits="ZPA.Courses" %>

<asp:Content ID="HeaderContent2" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent2" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Willkommen beim lokalen ZPA</h2>
    <asp:Button ID="ButtonLogout" runat="server"
    PostBackUrl="~/Default.aspx?todo=logout" Text="Abmelden" />
    <br />
    Kurse wählen:<br />
    <table border="0" cellspacing="0">
        <thead>
            <tr>
                <td bgcolor="#99CCFF">Kursname</td>
                <td bgcolor="#99CCFF">Teilnehmer</td>
                <td bgcolor="#99CCFF">Anmeldestatus</td>
                <td bgcolor="#99CCFF">Aktion</td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td bgcolor="#EEEEEE">
                    Mathe 1
                </td>
                <td>
                    55
                </td>
                <td bgcolor="#EEEEEE">
                    <asp:Label ID="LabelStatusCourse1" runat="server" Text="Nicht angemeldet"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="ButtonRegisterCourse1" runat="server" Text="Anmelden" 
                        PostBackUrl="~/Courses.aspx?todo=register&course=1" />
                </td>
            </tr>
            <tr>
                <td bgcolor="#EEEEEE">
                    Softwareentwicklung 1
                </td>
                <td>
                    32
                </td>
                <td bgcolor="#EEEEEE">
                    <asp:Label ID="LabelStatusCourse2" runat="server" Text="Nicht angemeldet"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="ButtonRegisterCourse2" runat="server" Text="Anmelden" 
                        PostBackUrl="~/Courses.aspx?todo=register&course=2" />
                </td>
            </tr>
            <tr>
                <td bgcolor="#EEEEEE">
                    AW Fach
                </td>
                <td>
                    63
                </td>
                <td bgcolor="#EEEEEE">
                    <asp:Label ID="LabelStatusCourse3" runat="server" Text="Nicht angemeldet"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="ButtonRegisterCourse3" runat="server" Text="Anmelden" 
                        PostBackUrl="~/Courses.aspx?todo=register&course=3" />
                </td>
            </tr>
            <tr>
                <td bgcolor="#EEEEEE">
                    F#
                </td>
                <td>
                    163
                </td>
                <td bgcolor="#EEEEEE">
                    <asp:Label ID="LabelStatusCourse4" runat="server" Text="Nicht angemeldet"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="ButtonRegisterCourse4" runat="server" Text="Anmelden" 
                        PostBackUrl="~/Courses.aspx?todo=register&course=4" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
