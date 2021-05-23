using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using Mono.Data.Sqlite;

namespace PhoneDirectory
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["cid"] = "";
            SqliteConnection conn = DbManager.start();
            ArrayList al = DbManager.Get_all_contacts();
            if(al.Count == 0)
            {
                TableRow row = new TableRow();
                TableCell none = new TableCell();
                none.Text = "No Record Found.";
                row.Cells.Add(none);
                ContactTable.Rows.Add(row);
            }
            else
            {
                foreach (TableRow tr in al)
                {
                    ContactTable.Rows.Add(tr);
                }
            }
        }
        [WebMethod(EnableSession =true)]
        public static string Getdata(string id)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict = DbManager.selectContact(id);
            if (dict.Keys.Count == 0)
                return "No Data Found in server";
            HttpContext.Current.Session["cid_temp"] = dict["id"];
            string retdata = "" +
            "<div class='text-center'>" +
                "<img id='imgpreview' runat='server' src='" + dict["imglink"] + "' alt='Photo' style='width: 150px; height: 150px; border-radius: 75px;' data-toggle='tooltip' data-placement='bottom' title='Photo Preview'/><br>" +
                "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Contact ID'>" +
                    "<i class='fas fa-id-card-alt'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["id"] + "</span>" +
                "</div>" +
                "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Full Name'>" +
                    "<i class='fas fa-user'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["fullname"] + "</span>" +
                "</div>" +
                "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Nickname'>" +
                    "<i class='far fa-user'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["nickname"] + "</span>" +
                "</div>" +
                "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Phonetic Name'>" +
                    "<i class='far fa-user'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["phoneticName"] + "</span>" +
                "</div>" +
                "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Gender'>" +
                    "<i class='fas fa-venus-mars'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["gender"] + "</span>" +
                "</div>" +
                "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Address'>" +
                    "<i class='fas fa-map-marker-alt'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["address"] + "</span>" +
                "</div>" +
                "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Company'>" +
                    "<i class='fas fa-building'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["company"] + "</span>" +
                "</div>" +
                "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Post in Company'>" +
                    "<i class='fas fa-user-md'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["post"] + "</span>" +
                "</div>";
            string[] phones = dict["phones"].Split(';');
            foreach (string phone in phones)
            {
                retdata += "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Phone No'>" +
                    "<i class='fas fa-phone'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span><a href=tel:'" + phone.Replace(";","").Replace(" ", "") + "'>" + phone.Replace(";", "").Replace(" ", "") + "</a></span>" +
                     "</div>";
            }
            string[] mails = dict["mails"].Split(';');
            foreach (string mail in mails)
            {
                retdata += "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Mail Address'>" +
                    "<i class='fas fa-mail-bulk'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span><a href=mailto:'" + mail.Replace(";", "").Replace(" ", "") + "'>" + mail.Replace(";", "").Replace(" ", "") + "</a></span>" +
                     "</div>";
            }
            retdata += "<div class='input-group' style='align-items: center; margin-top: 5px; margin-bottom: 5px;' data-toggle='tooltip' data-placement='bottom' title='Last Updated On'>" +
                    "<i class='fas fa-clock'></i> &nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<span>" + dict["lastupdateon"] + "</span>" +
                "</div>" +
            "</div>";
            return retdata;
        }
        protected void updateClick(object sender, EventArgs e)
        {
            Session["cid"] = Session["cid_temp"];
            Response.Redirect("/addContact.aspx");
        }
    }
}
