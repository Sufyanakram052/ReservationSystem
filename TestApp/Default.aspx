<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="TestApp._Default" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-lg-12 ">
        <!-- Added 'center-div' class -->
        <div class="row" id="center-div">
            <div class="col-md-4 mb-5">
                
                <div class="card mt-3 ">
                    <div class="card-body">
                        <h2 class="card-title text-center">Login</h2>
                        <div class="text-center">
                             <img src="/54474.jpg" width="100px" />
                        </div>
                       
                       <div class="col-md-12">
                           <div class="row">
                               <div class="col-sm-12 mb-3">
                                  <label for="email" class="form-label">Email:</label>
                                   <asp:TextBox ID="email" type="email" class="form-control"  placeholder="name@example.com" runat="server"></asp:TextBox>
                               </div>
                               <div class="col-sm-12 mb-3">
                                  <label for="password" class="form-label">Password:</label>
                                   <asp:TextBox ID="password" type="password" class="form-control" runat="server" placeholder="Your Password"></asp:TextBox>
                               </div>
                              
                               <div class="col-md-12 mb-3">
                                    <input type="checkbox" id="inline41">
                                    <label for="inline41">Remember Me?</label>
                                </div>
                               <div class="col-sm-12 mb-3">
                                   <asp:Button ID="ButtonLogin" class="btn btn-primary col-sm-12" runat="server" Text="Login" />
                                   
                               </div>
                                <div class="col-md-12 mb-3">
                                    <label for="inline41">SignUp As Customer?  
                                         <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                                            SignUp
                                        </button>
                                    </label>
                                </div>
                           </div>
                       </div>
                    </div>
                </div>
            </div>
             <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <h5 class="modal-title text-white" id="exampleModalLabel">Add Customer</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="col-sm-12 mb-3">
                            <label for="name" class="form-label">Name:</label>
                            <asp:TextBox ID="NameS" type="name" class="form-control" runat="server" placeholder="Customer Name" ViewStateMode="Enabled"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 mb-3">
                            <label for="email" class="form-label">Email:</label>
                            <asp:TextBox ID="EmailS" type="email" class="form-control" runat="server" placeholder="name@example.com" ViewStateMode="Enabled"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 mb-3">
                            <label for="password" class="form-label">Password:</label>
                            <asp:TextBox ID="PasswordS" type="password" class="form-control" runat="server" placeholder="Customer Password" ViewStateMode="Enabled"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 mb-3">
                            <label for="password" class="form-label">Phone:</label>
                            <asp:TextBox ID="PhoneS" type="number" class="form-control" runat="server" placeholder="Customer Phone #" ViewStateMode="Enabled"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 mb-3">
                            <label for="password" class="form-label">Address:</label>
                            <textarea id="AddressS" class="form-control" name="S1" rows="3" placeholder="Customer Address"></textarea>
                        </div>
                       
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        <asp:Button ID="saveCustomer" class="btn btn-primary" runat="server" Text="Save" />
                    </div>
                </div>
            </div>
        </div>
        </div>
    </div>

    <style>
        #center-div {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 80vh; /* Adjust the height as needed */
        }
    </style>

    <!-- Bootstrap 5 JavaScript dependencies -->

</asp:Content>
