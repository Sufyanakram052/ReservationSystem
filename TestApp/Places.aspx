<%@ Page Title="Places" Language="VB" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Places.aspx.vb" Inherits="TestApp.Places" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-lg-12">
        <div class="row mt-3">
            <div class="col-sm-6">
                <div class="col d-flex align-items-baseline">
                    <div class="media">
                        <div class="media-body align-self-center ms-3">
                            <h2 class="m-0 font-20">Places List</h2>
                            <h6>Home / Places List</h6> 
                        </div>
                    </div>
                </div>
            </div>
        </div>
         <div class="row mt-3">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                       <div class="col-md-12">
                           <div class="row">
                               <div class="col-sm-12 mb-3">
                                  <label for="name" class="form-label">Name:</label>
                                   <asp:TextBox ID="NameS" type="name" class="form-control"   runat="server" placeholder="Your Name" ViewStateMode="Enabled"></asp:TextBox>
                               </div>
                               <div class="col-sm-12 mb-3">
                                  <label for="location" class="form-label">Location:</label>
                                   <asp:TextBox ID="LocationS" type="text" class="form-control"  runat="server"  placeholder="Your Location" ViewStateMode="Enabled"></asp:TextBox>
                               </div> 
                               <div class="col-sm-12 mb-3">
                                  <label for="price" class="form-label">Price:</label>
                                   <asp:TextBox ID="PriceS" type="number" class="form-control"  runat="server"  placeholder="23554" ViewStateMode="Enabled"></asp:TextBox>
                               </div>
                               <div class="col-md-12">
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                   
                            &nbsp;Status</div>
                               <div class="col-sm-3 mb-3">
                                    <asp:Button ID="saveAdmin1" class="btn btn-primary col-sm-12" runat="server" Text="Save" />
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
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                        <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />


                        <asp:CommandField SelectText="Edit" ShowSelectButton="True" ControlStyle-CssClass="btn btn-warning" />
                        <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="btn btn-danger" />


                    </Columns>
                </asp:GridView>
            </div>        
        </div>
    </div>
</asp:Content>
