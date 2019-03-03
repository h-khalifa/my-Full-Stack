using People_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace People_CRUD
{
    public partial class people : System.Web.UI.Page
    {
        PersonRepo repo = new PersonRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void LinkButton_AddPerson_Click1(object sender, EventArgs e)
        {
            TextBox txtbx_name = (TextBox)GridView1.FooterRow.FindControl("TextBox_AddPersonName");
            TextBox txtbx_phone = (TextBox)GridView1.FooterRow.FindControl("TextBox_AddPersonPhone");
            TextBox txtbx_bdate = (TextBox)GridView1.FooterRow.FindControl("TextBox_AddPersonBirthDate");
            DropDownList ddl_gender = (DropDownList)GridView1.FooterRow.FindControl("DropDownList_AddPersonGender");
            CheckBox chkbx_ismarried = (CheckBox)GridView1.FooterRow.FindControl("CheckBox_AddPersonIsMarried");
            person p = new person()
            {
                Name = txtbx_name.Text,
                Phone = (String.IsNullOrEmpty(txtbx_phone.Text)) ? null : txtbx_phone.Text,
                IsMarried = chkbx_ismarried.Checked,
                Gender = (String.IsNullOrEmpty(ddl_gender.SelectedValue)) ? (Nullable<Gender>)null : (Gender)Enum.Parse(typeof(Gender), ddl_gender.SelectedValue),
                BDate = (String.IsNullOrEmpty(txtbx_bdate.Text)) ? (Nullable<System.DateTime>)null : DateTime.ParseExact(txtbx_bdate.Text, "dd/mm/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            };
            //txtbx_name.Text = "";
            p.ToString();
            repo.AddPerson(p);
            p.ToString();
            Response.Redirect("/people.aspx");
        }
    }
}