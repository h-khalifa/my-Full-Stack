<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="people.aspx.cs" Inherits="People_CRUD.people" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>People CRUD</title>
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <link href="https://code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css">
    <script src="Scripts/bootstrap.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <div class="navbar navbar-inverse">
    </div>
   <%-- <div style="margin:150px"></div>--%>
    <form id="form1" runat="server">
        <div class="container" >
            <asp:GridView ID="GridView1" runat="server" DataSourceID="EntityDataSource1" AutoGenerateColumns="False" DataKeyNames="Id" ShowFooter="True" Class="table table-hover table-responsive text-center" style="width:100%;border:none;">
                <Columns>
                   <%-- <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />--%>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update" ValidationGroup="Edit"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="LinkButton_AddPerson" runat="server" OnClick="LinkButton_AddPerson_Click1" ValidationGroup="Add">Add New Person</asp:LinkButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" SortExpression="Name" HeaderStyle-CssClass="text-center">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Name") %>' placeholder="Full Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_EditPersonName" runat="server" ErrorMessage="Name is required Fiels" ControlToValidate="TextBox4" Text="*" ForeColor="Red" ValidationGroup="Edit"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator_EditPersonName" runat="server" ErrorMessage="Please enter correct name" ControlToValidate="TextBox4" Text="*" ForeColor="Red" ValidationExpression="[a-zA-Z ]{1,50}" ValidationGroup="Edit"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="text-center"></HeaderStyle>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBox_AddPersonName" runat="server" placeholder="Full Name" ValidationGroup="Add"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_AddPersonName" runat="server" ErrorMessage="Name is required Fiels" ControlToValidate="TextBox_AddPersonName" Text="*" ForeColor="Red" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator_AddPersonName" runat="server" ErrorMessage="Please enter correct name" ControlToValidate="TextBox_AddPersonName" Text="*" ForeColor="Red" ValidationExpression="[a-zA-Z ]{1,50}" ValidationGroup="Add"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone" SortExpression="Phone" HeaderStyle-CssClass="text-center">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Phone") %>' placeholder="Phone Number" ValidationGroup="Edit"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator_EditPersonPhone" runat="server" ErrorMessage="please enter a valid phone number(only digits of length 5:15)" ControlToValidate="TextBox3" Text="*" ForeColor="Red" ValidationExpression="[0-9]{5,15}" ValidationGroup="Edit"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Phone") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="text-center"></HeaderStyle>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBox_AddPersonPhone" runat="server" placeholder="Phone Number" ValidationGroup="Add"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator_EditPersonPhone" runat="server" ErrorMessage="please enter a valid phone number(only digits of length 5:15)" ControlToValidate="TextBox_AddPersonPhone" Text="*" ForeColor="Red" ValidationExpression="[0-9]{5,15}" ValidationGroup="Add"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Is Married" SortExpression="IsMarried" HeaderStyle-CssClass="text-center">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsMarried") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsMarried") %>' Enabled="false" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="text-center"></HeaderStyle>
                        <FooterTemplate>
                            <asp:CheckBox ID="CheckBox_AddPersonIsMarried" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
     
                    <asp:TemplateField HeaderText="Gender" SortExpression="Gender" HeaderStyle-CssClass="text-center">
                        <EditItemTemplate>
                            <asp:DropDownList ID="DropDownList1" runat="server" selectedvalue='<%# Bind("Gender") %>'>
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Gender") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="text-center"></HeaderStyle>
                        <FooterTemplate>
                            <asp:DropDownList ID="DropDownList_AddPersonGender" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Birth Date" SortExpression="BDate" HeaderStyle-CssClass="text-center">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("BDate","{0:d}") %>' class="EditBDate" ></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("BDate","{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="text-center"></HeaderStyle>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBox_AddPersonBirthDate" runat="server" placeholder="Birth Date" class="TextBox_AddPersonBirthDate"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=PeopleEntities" DefaultContainerName="PeopleEntities" EnableDelete="True" EnableFlattening="False" EnableInsert="True" EnableUpdate="True" EntitySetName="people" EntityTypeFilter="person">
            </asp:EntityDataSource>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Add" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Edit" />
            <hr />
            <h2 class="text-center">Get or Refresh People Names</h2>
            <button class="btn btn-default" id="getnames" type="button">Get</button>
            </asp:DataList>
            <ul ID="BulletedList1"  class="list-group" >
            </ul>
            


        </div>
    </form>
    <div class="container">
        <hr />
        <footer >
            People CRUD - DEMO
        </footer>
    </div>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        $('.EditBDate').datepicker({ dateFormat: 'dd/mm/yy' });
        $('.TextBox_AddPersonBirthDate').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#getnames').click(function () {
            var blist = $('.list-group');
            blist.empty();
            $.ajax({
                type: 'GET',
                url: '/api/people/GetNames',
                contentType: "application/json",
                success: function (data) {
                    data.forEach(function (e) {
                        var listItem = '<li class="list-group-item" >' + e + '</li>';
                        blist.append(listItem);
                    });
                },
            });
        });
    });
</script>