<%@ Page Title="Contact" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.vb" Inherits="TestApp.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <div class="col-lg-12">
        <div class="row mt-3">
            <div class="col-sm-6">
                <div class="col d-flex align-items-baseline">
                    <div class="media">
                        <div class="media-body align-self-center ms-3">
                            <h2 class="m-0 font-20">Customer List</h2>
                            <h6>Home / Customer List</h6> 
                        </div>
                    </div>
                </div>
            </div>
        </div>
         <div class="row mt-3">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">
                       <div class="col-md-12">
                           <div class="row">
                               <div class="col-sm-6 mb-3">
                                  <label for="name" class="form-label">Name:</label>
                                   <asp:TextBox ID="NameS" type="name" class="form-control"   runat="server" placeholder="Customer Name" ViewStateMode="Enabled"></asp:TextBox>
                               </div>
                               <div class="col-sm-6 mb-3">
                                  <label for="email" class="form-label">Email:</label>
                                   <asp:TextBox ID="EmailS" type="email" class="form-control"  runat="server"  placeholder="name@example.com" ViewStateMode="Enabled"></asp:TextBox>
                               </div>
                               <div class="col-sm-6 mb-3">
                                  <label for="password" class="form-label">Password:</label>
                                   <asp:TextBox ID="PasswordS" type="password" class="form-control" runat="server" placeholder="Customer Password" ViewStateMode="Enabled"></asp:TextBox>
                               </div>
                               <div class="col-sm-6 mb-3">
                                  <label for="password" class="form-label">Phone:</label>
                                   <asp:TextBox ID="PhoneS" type="number" class="form-control" runat="server" placeholder="Customer Phone #" ViewStateMode="Enabled"></asp:TextBox>
                               </div>
                               <div class="col-sm-12 mb-3">
                                  <label for="password" class="form-label">Address:</label>
                                   <textarea id="AddressS" class="form-control" name="S1" rows="3" placeholder="Customer Address"></textarea>
                               </div>
                               <div class="col-md-12">
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                   
                            &nbsp;Status</div>
                               <div class="col-sm-3 mb-3 mt-2">
                                    <asp:Button ID="saveCustomer" class="btn btn-primary col-sm-12" runat="server" Text="Save" />
                               </div>
                           </div>
                       </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-5 pb-5 mt-4">
            <div class="col-md-12 mb-5 pb-5">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="table table-striped table-responsive">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" />
                        <asp:BoundField DataField="AdminName" HeaderText="AdminName" SortExpression="AdminName" />
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                        <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Password" />
                        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Password" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Password" />


                        <asp:CommandField SelectText="Edit" ShowSelectButton="True" ControlStyle-CssClass="btn btn-warning" />
                        <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="btn btn-danger" />


                    </Columns>
                </asp:GridView>
            </div>        
        </div>
    </div>
</asp:Content>
