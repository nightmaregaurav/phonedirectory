<%@ Page Language="C#" CodeBehind="index.aspx.cs" Inherits="PhoneDirectory.index" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <link rel="stylesheet" type="text/css" href="/Statics/css/main.css">
    <style type="text/css">
        td{
            vertical-align: middle !important;
        }
        #ContactTable > tbody tr td:not(:last-child){
            white-space: nowrap;
            text-align: center;
            color: gray;
        }
        #ContactTable > tbody tr td:last-child{
            width: 99%;
        }
        #ContactTable > tbody tr td{
            font-weight: bold;
        }
        .btn-circle{
            width: 70px;
            height: 70px;
            padding: 10px 16px;
            border-radius: 35px;
            text-align: center;
            font-size: 20px;
        }
    </style>
    <title>Public Phone Directory</title>
</head>
<body>
    <!--#include file="nav.html"-->
    <asp:Table id="ContactTable" runat="server" class="table table-light table-hover table-striped table-bordered shadow-lg" align="center" style="margin-top: 20px; margin-bottom:20px; width:90%;">
    </asp:Table>
    
    <div class="modal fade" id="viewContact" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Detail</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                </div>
                <div id="modalBody" class="modal-body text-center">
                </div>
                <div class="modal-footer">
                    <form runat="server">
                        <asp:Button runat="server" id="update" class="btn btn-primary" onclick="updateClick" Text="Update"/>
                    </form>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    
    <div style="position:fixed; bottom:50px; right:50px;">
        <a href="/addContact.aspx" class="btn-lg btn-primary text-light btn-circle shadow-lg"><i class="fas fa-user-plus"></i></a>
    </div>

    <script type="text/javascript" src="/Statics/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="/Statics/popper.js/popper.min.js"></script>
    <script type="text/javascript" src="/Statics/bootstrap/js/bootstrap.min.js"></script>
    <script>
        $("#ContactTable > tbody tr").click(function() {
            cid = this.cells[1].innerText
            $.ajax({
                type: "POST",
                url: "index.aspx/Getdata",
                data: "{id:'"+cid+"'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $("#modalBody")[0].innerHtml = "Network Failure.";
                    $("#viewContact").modal();
                },
                success: function (result) {
                    $("#modalBody")[0].innerHTML = result.d;
                    $("#viewContact").modal();
                }
            });
        });
    </script>
</body>
</html>
