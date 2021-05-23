using System;
using System.Collections.Generic;
using System.IO;
namespace PhoneDirectory
{
    public partial class addContact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string coid = "";
                if (!Session["cid"].ToString().Equals(""))
                {
                    coid = Session["cid"].ToString();
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict = DbManager.selectContact(coid);
                    if (dict.Keys.Count == 0)
                    {
                        Session["cid"] = "";
                    }
                    else
                    {
                        fname.Value = dict["fullname"];
                        nname.Value = dict["nickname"];
                        pname.Value = dict["phoneticName"];
                        string gen = dict["gender"];
                        int genindex;
                        if (gen.Equals("Male"))
                            genindex = 0;
                        else if (gen.Equals("Female"))
                            genindex = 1;
                        else
                            genindex = 3;
                        gender.SelectedIndex = genindex;
                        addr.Value = dict["address"];
                        cname.Value = dict["company"];
                        post.Value = dict["post"];
                        phones.Value = dict["phones"];
                        mails.Value = dict["mails"];
                        imgpreview.Src = dict["imglink"];
                    }
                }
            }
        }
        protected void addClick(object sender, EventArgs e)
        {
            int coid = -1;
            if (!Session["cid"].ToString().Equals(""))
            {
                coid = int.Parse(Session["cid"].ToString());
                Session["cid"] = "";
            }
            bool done = work(coid);
            if (!done)
                messagebox.InnerText = "Error While Saving.";
        }
        protected bool work(int coid)
        {
            string full_name = fname.Value;
            string nickname = nname.Value;
            string phonetic = pname.Value;
            int gender_id = gender.SelectedIndex;
            string address = addr.Value;
            string company = cname.Value;
            string cpost = post.Value;
            string pnos = phones.Value;
            string mls = mails.Value;
            string filename = imgInp.PostedFile.FileName;
            string fileloc;
            char gen;
            if (gender_id == 0)
                gen = 'M';
            else if (gender_id == 1)
                gen = 'F';
            else
                gen = 'O';
            if(imgInp.PostedFile != null && imgInp.PostedFile.ContentLength > 0)
            {
                string[] temp = System.IO.Path.GetFileName(imgInp.PostedFile.FileName).Split('.');
                string fe;
                if (temp.Length > 1)
                    fe = "." + temp[temp.Length - 1];
                else
                    fe = "";
                string fn = DateTime.Now.ToString("yyyyMMddHHmmssttfffffff") + fe;
                string savelocation = Server.MapPath("Statics/photos/") + fn;
                try
                {
                    imgInp.PostedFile.SaveAs(savelocation);
                    fileloc = "/Statics/photos/" + fn;
                }
                catch (Exception)
                {
                    fileloc = "";
                }
            }
            else
            {
                fileloc = imgpreview.Src;
            }
            DbManager.Add_update_contact(full_name, nickname, phonetic, gen, address, company, cpost, fileloc, pnos, mls, coid);
            messagebox.InnerText = "Success";
            return true;
        }
    }
}
