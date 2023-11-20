Imports System.Data.SqlClient
Imports System.Web.Services.Description

Public Class _Default
    Inherits Page
    Dim Con As New SqlConnection("Data Source=DESKTOP-5DVUDJE;Initial Catalog=ParkingReservation;Integrated Security=True;Connect Timeout=30")

    Protected Sub ShowSweetAlertError(message As String)
        Dim script As String = "Swal.fire({
                                title: 'Error!',
                                text: '" & message & "',
                                icon: 'error',
                                confirmButtonText: 'OK'
                            });"

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SweetAlert", script, True)
    End Sub



    Protected Sub ButtonLogin_Click(sender As Object, e As EventArgs) Handles ButtonLogin.Click
        Dim email1 = email.Text
        Dim password1 = password.Text

        Con.Open()

        Dim query As String = "SELECT Id, COUNT(*) as CountOfAdmin FROM Admin WHERE Email=@Email AND Password=@Password GROUP BY Id"

        Using cmd As New SqlCommand(query, Con)
            ' Assuming txtUsername and txtPassword are your TextBox controls for username and password
            cmd.Parameters.AddWithValue("@Email", email1)
            cmd.Parameters.AddWithValue("@Password", password1) ' Note: In a production scenario, you should use hashed passwords.

            ' Check if there is a matching record in the database
            Dim result = cmd.ExecuteScalar()

            If result > 0 Then
                Session("IsAdmin") = "True"
                Session("AdminId") = result
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Login Successfully',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       }).then(function() {
                                          window.location.href = 'Admin.aspx';
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
            Else
                Con.Close()
                Con.Open()

                Dim query1 As String = "SELECT Id FROM Customers WHERE Email=@CustomerEmail AND Password=@CustomerPassword AND Status=@Status"

                Using cmd1 As New SqlCommand(query1, Con)
                    ' Assuming txtUsername and txtPassword are your TextBox controls for username and password
                    cmd1.Parameters.AddWithValue("@CustomerEmail", email1)
                    cmd1.Parameters.AddWithValue("@CustomerPassword", password1) ' Note: In a production scenario, you should use hashed passwords.
                    cmd1.Parameters.AddWithValue("@Status", True)
                    ' Check if there is a matching record in the database
                    Dim result1 = cmd1.ExecuteScalar()

                    If String.IsNullOrEmpty(result1) Then
                        result1 = 0
                    End If

                    If result1 > 0 Then
                        Session("IsCustomer") = "True"
                        Session("CustomerId") = result
                        Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Login Successfully',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       }).then(function() {
                                          window.location.href = 'CustomerHome.aspx';
                                       });
                                    </script>"

                        ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                    Else

                        ShowSweetAlertError("Invalid username or password.")
                    End If
                End Using
            End If
        End Using

        Con.Close()

    End Sub


End Class