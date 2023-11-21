Imports System.Data.SqlClient
Imports System.Web.Services.Description

Public Class _Default
    Inherits Page
    Dim Con As New SqlConnection("Data Source=DESKTOP-5DVUDJE;Initial Catalog=ParkingReservation;Integrated Security=True;Connect Timeout=30")

    Dim Key = 0
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
                Session("IsCustomer") = ""
                Session("CustomerId") = 0
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
                        Session("IsAdmin") = ""
                        Session("AdminId") = 0
                        Session("IsCustomer") = "True"
                        Session("CustomerId") = result1
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

    Protected Sub saveCustomer_Click(sender As Object, e As EventArgs) Handles saveCustomer.Click
        Key = If(Integer.TryParse(Session("SelectedAdminId")?.ToString(), Key), Key, 0)


        Dim Address As String = Request.Form("S1")

        Dim Status As Boolean = True


        If Key > 0 Then

            If NameS.Text = "" Or EmailS.Text = "" Or PasswordS.Text = "" Then
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Please Fill All Fields.',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)

                Session("SelectedAdminId") = Key
            End If

        Else
            Dim Name1 = NameS.Text
            Dim Email1 = EmailS.Text
            Dim Password1 = PasswordS.Text
            If Name1 = "" Or Email1 = "" Or Password1 = "" Then
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Please Fill All Fields.',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
            Else
                Try
                    Con.Open()
                    If EmailExistsInTable("Admin", Email1) Then
                        Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Email already exists',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                        ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                        'MsgBox("Error: Email already exists in the Admin table.")
                    ElseIf EmailExistsInTable("Customers", Email1) Then
                        Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Email already exists',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                        ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                        'MsgBox("Error: Email already exists in the Customer table.")
                    Else
                        ' Query to get the maximum Id from the Admin table
                        Dim getMaxIdQuery As String = "SELECT MAX(Id) FROM Customers"
                        Dim cmdGetMaxId As New SqlCommand(getMaxIdQuery, Con)

                        ' ExecuteScalar is used to get a single value (in this case, the maximum Id)
                        Dim maxId As Object = cmdGetMaxId.ExecuteScalar()

                        ' Debugging information - print maxId to console
                        Console.WriteLine("Debug - maxId: " & If(maxId IsNot Nothing, maxId.ToString(), "null"))

                        ' Check if the result is DBNull before casting
                        Dim newId As Integer

                        If maxId IsNot DBNull.Value Then
                            If Integer.TryParse(maxId.ToString(), newId) Then
                                ' Increment the maximum Id to get the new Id
                                newId += 1
                            Else
                                ' Handle the case where parsing fails
                                MsgBox("Error: Unable to parse maximum Id from the database.")
                            End If
                        Else
                            ' Set newId to 0 when maxId is DBNull
                            newId = 1
                        End If

                        ' Insert the new record with the calculated Id
                        Dim insertQuery As String = "INSERT INTO Customers (Id, Name, Email, Password, Phone, Address, Status) VALUES (@Id, @Name, @Email, @Password, @Phone, @Address, @Status)"
                        Dim cmdInsert As New SqlCommand(insertQuery, Con)
                        cmdInsert.Parameters.AddWithValue("@Id", newId)
                        cmdInsert.Parameters.AddWithValue("@Name", Name1)
                        cmdInsert.Parameters.AddWithValue("@Email", Email1)
                        cmdInsert.Parameters.AddWithValue("@Password", Password1)
                        cmdInsert.Parameters.AddWithValue("@Phone", PhoneS.Text)
                        cmdInsert.Parameters.AddWithValue("@Address", Address)
                        cmdInsert.Parameters.AddWithValue("@Status", Status)

                        cmdInsert.ExecuteNonQuery()
                        Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Customer Saved Successfully.',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                        ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                        'MsgBox("Customer Saved Successfully.")

                        NameS.Text = ""
                        EmailS.Text = ""
                        PasswordS.Text = ""
                        PhoneS.Text =
                        PhoneS.Text = ""
                        Key = 0
                        Session("SelectedAdminId") = Key
                    End If
                Catch ex As Exception
                    ' Handle exceptions
                    MsgBox("An error occurred: " & ex.Message & vbCrLf & ex.StackTrace)
                Finally
                    If Con.State = ConnectionState.Open Then
                        Con.Close()
                    End If
                End Try
            End If
        End If
    End Sub

    Private Function EmailExistsInTable(tableName As String, email As String) As Boolean
        Dim emailExistsQuery As String = $"SELECT COUNT(*) FROM {tableName} WHERE Email = @Email"
        Dim cmdEmailExists As New SqlCommand(emailExistsQuery, Con)
        cmdEmailExists.Parameters.AddWithValue("@Email", email)

        Dim emailExistsCount As Integer = Convert.ToInt32(cmdEmailExists.ExecuteScalar())

        Return emailExistsCount > 0
    End Function
End Class