<%@ Page Language="C#" %>


<%@ Import Namespace="System.Web.Services" %>
<script type="text/C#" runat="server">
    [WebMethod]
    public static string SayHello(string name)
    {
        return "Hello " + name;
    }
</script>
<!DOCTYPE html>
<html>
<head>
    <title></title>
   
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
<script type = "text/javascript">
function ShowCurrentTime() {
    $.ajax({
        type: "POST",
        url: "WebForm2.aspx/GetCurrentTime",
        data: '{name: "' + $("#<%=txtUserName.ClientID%>")[0].value + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        failure: function(response) {
            alert(response.d);
        }
    });
}
function OnSuccess(response) {
    alert(response.d);
}
</script>

</head>
<body>
    <form id="Form1" runat="server">
<div>
Your Name :
<asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
<input id="btnGetTime" type="button" value="Show Current Time"
    onclick = "ShowCurrentTime()" />
</div>

    </form>
</body>
</html>