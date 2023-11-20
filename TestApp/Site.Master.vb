Public Class SiteMaster
    Inherits MasterPage
    Dim IsAdmin = ""
    Dim IsCustomer = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not (String.IsNullOrEmpty(Session("IsAdmin"))) Then
            IsAdmin = Session("IsAdmin")
        Else
            IsAdmin = ""
        End If

        If String.IsNullOrEmpty(IsAdmin) Then
            HomeBtn.Visible = False
            AdminBtn.Visible = False
            CustomerBtn.Visible = False
            LogoutBtn.Visible = False
            ParkingBtn.Visible = False
            PlacesBtn.Visible = False
            CustomerHome12Btn.Visible = False

            If Not (String.IsNullOrEmpty(Session("IsCustomer"))) Then
                IsCustomer = Session("IsCustomer")
            Else
                IsCustomer = ""
            End If

            If String.IsNullOrEmpty(IsCustomer) Then
                CustomerHome12Btn.Visible = False
                LogoutBtn.Visible = False
            Else
                CustomerHome12Btn.Visible = True
                LogoutBtn.Visible = True
            End If
        Else
            HomeBtn.Visible = True
            AdminBtn.Visible = True
            CustomerBtn.Visible = True
            LogoutBtn.Visible = True
            ParkingBtn.Visible = True
            PlacesBtn.Visible = True
            CustomerHome12Btn.Visible = False

        End If



    End Sub

    Protected Sub saveAdmin_Click(sender As Object, e As EventArgs) Handles HomeBtn.Click
        Response.Redirect("Admin.aspx")
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles AdminBtn.Click
        Response.Redirect("Admin.aspx")
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles CustomerBtn.Click
        Response.Redirect("Customer.aspx")
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles LogoutBtn.Click
        Dim A = ""
        Session("IsAdmin") = A
        Session("IsCustomer") = A
        HomeBtn.Visible = False
        AdminBtn.Visible = False
        CustomerBtn.Visible = False
        LogoutBtn.Visible = False
        ParkingBtn.Visible = False
        PlacesBtn.Visible = False
        CustomerHome12Btn.Visible = False
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub ParkingBtn_Click(sender As Object, e As EventArgs) Handles ParkingBtn.Click
        Response.Redirect("Parking.aspx")
    End Sub

    Protected Sub PlacesBtn_Click(sender As Object, e As EventArgs) Handles PlacesBtn.Click
        Response.Redirect("Places.aspx")
    End Sub

    Protected Sub CustomerHome12Btn_Click(sender As Object, e As EventArgs) Handles CustomerHome12Btn.Click
        Response.Redirect("CustomerHome.aspx")
    End Sub
End Class