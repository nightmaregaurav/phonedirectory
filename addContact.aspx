<%@ Page Language="C#" CodeBehind="addContact.aspx.cs" Inherits="PhoneDirectory.addContact" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <link rel="stylesheet" type="text/css" href="/Statics/css/main.css">
    <title>Add Contact</title>
    <style>
        .attatched{
          height: 20px;
          align-items: center;
        }
        input, select{
            border-width: 0px 0px 2px 0px !important;
            height: 20px;
            padding-left: 25px;
        }
        #imgpreview{
            width: 100px;
            height: 100px;
            border-radius: 50px;
        }
        .overlay{
            opacity:0.8;
            position:fixed;
            width:100%;
            height:100%;
            top:0px;
            left:0px;
            z-index:1000;
            vertical-align:middle;
        }
    </style>
</head>
<body>
    <!--#include file="nav.html"-->
    <div class="container-fluid h-100" style="text-align: center !important; margin-top: 25px;">
        <div class="forms boxed" style="display: inline-block;">
            <form runat="server" method="post" id="form">
                <h2 class="text-center">Add/Update Contact Detail</h2><hr>
                <img id="imgpreview" runat="server" src="/Statics/photos/male.png" alt="Photo" />
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="fas fa-user attatched"></i>
                    <input runat="server" id="fname" type="text" class="form-control" placeholder="Full Name">
                </div>
                <asp:RegularExpressionValidator runat="server" id="rex1" ControlToValidate="fname" ValidationExpression="^[a-zA-Z ,.'-]+$" ErrorMessage="Invalid Name" />
                <asp:RequiredFieldValidator runat="server" id="req1" ControlToValidate="fname" ErrorMessage="Name is Required" />
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="far fa-user attatched"></i>
                    <input runat="server" id="nname" type="text" class="form-control" placeholder="Nickname"><br>
                </div>
                <asp:RegularExpressionValidator runat="server" id="rex2" ControlToValidate="nname" ValidationExpression="^[^0-9]\w+$" ErrorMessage="Invalid Nickname" />
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="far fa-user attatched"></i>
                    <input runat="server" id="pname" type="text" class="form-control" placeholder="Phonetic Name"><br>
                </div>
                <asp:RegularExpressionValidator runat="server" id="rex3" ControlToValidate="pname" ValidationExpression="^[^0-9]\w+$" ErrorMessage="Invalid Phonetic Name" />
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="fas fa-venus-mars attatched"></i>
                    <select runat="server" id="gender" class="form-control" onchange="genderChanged">
                        <option>Male</option>
                        <option>Female</option>
                        <option>Others</option>
                    </select>
                </div><br>
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="fas fa-map-marker-alt attatched"></i>
                    <input runat="server" id="addr" type="text" class="form-control" placeholder="Address"><br>
                </div>
                <asp:RegularExpressionValidator runat="server" id="rex4" ControlToValidate="addr" ValidationExpression="^[#.0-9a-zA-Z\s,-]+$" ErrorMessage="Invalid Address" />
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="fas fa-building attatched"></i>
                    <input runat="server" id="cname" type="text" class="form-control" placeholder="Company Name"><br>
                </div><br>
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="fas fa-user-md attatched"></i>
                    <input runat="server" id="post" type="text" class="form-control" placeholder="Post"><br>
                </div><br>
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="fas fa-phone attatched"></i>
                    <input runat="server" id="phones" type="text" class="form-control" placeholder="Phone no (; saperated values)"><br>
                </div>
                <asp:RegularExpressionValidator runat="server" id="rex5" ControlToValidate="phones" ValidationExpression="^([0-9]+(;|; ){0,1})*$" ErrorMessage="Invalid Phone Number" />
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="fas fa-mail-bulk attatched"></i>
                    <input runat="server" id="mails" type="text" class="form-control" placeholder="Mail (; saperated values)"><br>
                </div>
                <asp:RegularExpressionValidator runat="server" id="rex6" ControlToValidate="mails" ValidationExpression="^(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*(;|; ){0,1})*$" ErrorMessage="Invalid Emails" />
                <div class="input-group" style="align-items: center; margin-top: 5px; margin-bottom: 5px;">
                    <i class="fas fa-image attatched"></i>
                    <input runat="server" type="file" class="form-control" id="imgInp" name="img" accept="image/*" placeholder="Photo">
                </div><br>
                <div class="form-group text-center">
                    <asp:Button runat="server" id="add" class="btn btn-primary" onclick="addClick" style="font-weight: bold; height:40px; width:90px;" Text="Done"/>
                </div>
            </form>  
        </div>
    </div>
    <div class="overlay bg-primary text-light mx-auto text-center" id="messagediv" style="font-size: 50px;font-weight: 900;font-family: cursive; display: none;">
        <br><br><br><i class="" id="msgico"></i><br>
        <span runat="server" id="messagebox"></span>
    </div>

        <script type="text/javascript" src="/Statics/jquery/jquery.min.js"></script>
        <script type="text/javascript" src="/Statics/popper.js/popper.min.js"></script>
        <script type="text/javascript" src="/Statics/bootstrap/js/bootstrap.min.js"></script>
        <script>
            if ($("#messagebox")[0].innerText != ""){
                if($("#messagebox")[0].innerText == "Success"){
                    $("#msgico").attr("class", "fas fa-check-circle");
                    $("input[type=text]").val('');
                    $("select")[0].selectedIndex = 0;
                    $("#imgpreview")[0].src = "/Statics/photos/male.png";
                }
                else
                    $("#msgico").attr("class", "fas fa-times-circle");
                $("#messagediv").show();
                setTimeout(function() {$("#messagediv").hide();}, 5000);
            }
            function readURL(input) {
                if (input.files && input.files[0]) {
                    var reader = new FileReader();    
                    reader.onload = function(e) {
                        $('#imgpreview').attr('src', e.target.result);
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            }
            $("#imgInp").change(function() {
                readURL(this);
            });
            $("#gender").change(function() {
                genderChanged();
            });
            function genderChanged() {
                gen = document.getElementById('gender');
                imgp = document.getElementById('imgpreview');
                if (gen.selectedIndex == 1 && imgp.src.endsWith("male.png"))
                        imgp.src = "/Statics/photos/female.png"
                else if (gen.selectedIndex != 1 && imgp.src.endsWith("female.png"))
                        imgp.src = "/Statics/photos/male.png";
            }
        </script>
    </body>
</html>