﻿Imports System.Data.SqlClient

Public Class About
    Inherits Page
    Dim Con As New SqlConnection("Data Source=DESKTOP-5DVUDJE;Initial Catalog=ParkingReservation;Integrated Security=True;Connect Timeout=30")


    Dim Key = 0
    Protected Sub saveAdmin1_Click(sender As Object, e As EventArgs) Handles saveAdmin1.Click
        Key = If(Integer.TryParse(Session("SelectedAdminId")?.ToString(), Key), Key, 0)
        If Key > 0 Then

            If NameS.Text = "" Or EmailS.Text = "" Or PasswordS.Text = "" Then
                MsgBox("Please fill all the fielss.")
            Else
                Con.Open()
                Dim query As String
                query = "Update Admin set Name='" & NameS.Text & "',Email='" & EmailS.Text & "',Password='" & PasswordS.Text & "' where Id='" & Key & "'"
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                MsgBox("Admin Updated Successfully.")
                Con.Close()
                NameS.Text = ""
                EmailS.Text = ""
                PasswordS.Text = ""
                Key = 0
                Session("SelectedAdminId") = Key
                Populate()
            End If

        Else
            Dim Name1 = NameS.Text
            Dim Email1 = EmailS.Text
            Dim Password1 = PasswordS.Text
            If Name1 = "" Or Email1 = "" Or Password1 = "" Then
                MsgBox("Please fill all the fields.")
            Else
                Try
                    Con.Open()

                    ' Query to get the maximum Id from the Admin table
                    Dim getMaxIdQuery As String = "SELECT MAX(Id) FROM Admin"
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
                    Dim insertQuery As String = "INSERT INTO Admin (Id, Name, Email, Password) VALUES (@Id, @Name, @Email, @Password)"
                    Dim cmdInsert As New SqlCommand(insertQuery, Con)
                    cmdInsert.Parameters.AddWithValue("@Id", newId)
                    cmdInsert.Parameters.AddWithValue("@Name", Name1)
                    cmdInsert.Parameters.AddWithValue("@Email", Email1)
                    cmdInsert.Parameters.AddWithValue("@Password", Password1)

                    cmdInsert.ExecuteNonQuery()

                    MsgBox("Admin Saved Successfully.")

                    NameS.Text = ""
                    EmailS.Text = ""
                    PasswordS.Text = ""
                    Key = 0
                    Session("SelectedAdminId") = Key
                Catch ex As Exception
                    ' Handle exceptions
                    MsgBox("An error occurred: " & ex.Message & vbCrLf & ex.StackTrace)
                Finally
                    If Con.State = ConnectionState.Open Then
                        Con.Close()
                        Populate()
                    End If
                End Try
            End If
        End If

    End Sub


    Private Sub Populate()
        Try
            Con.Open()

            Dim query = "SELECT * FROM Admin"
            Dim adapter As SqlDataAdapter = New SqlDataAdapter(query, Con)
            Dim builder As SqlCommandBuilder = New SqlCommandBuilder(adapter)
            Dim ds As DataSet = New DataSet
            adapter.Fill(ds)

            ' Set the DataGridView DataSource
            GridView1.DataSource = ds
            GridView1.DataBind()

            ' Clear textboxes and reset key


        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)

        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()

            End If
        End Try
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populate()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged
        Try
            Dim Id As Integer = Integer.Parse(GridView1.SelectedRow.Cells(0).Text)
            Con.Open()
            Using Con
                Dim query As String = "SELECT * FROM Admin WHERE id = @Id"

                Using command As New SqlCommand(query, Con)
                    command.Parameters.AddWithValue("@Id", Id)

                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            ' Data found, populate the textboxes
                            NameS.Text = reader("Name").ToString()
                            EmailS.Text = reader("Email").ToString()
                            PasswordS.Text = reader("Password").ToString()
                            Key = reader("Id")
                            Session("SelectedAdminId") = Key
                            ' You may also store the ID in a variable for further use
                            ' Key = Id
                        Else
                            ' No data found for the selected ID
                            ' Handle the case where the ID doesn't exist in the database
                            MsgBox("No data found for the selected ID.")
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)

        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try

    End Sub

    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Try
            Dim index As Integer = e.RowIndex
            Dim Id As Integer = Integer.Parse(GridView1.Rows(index).Cells(0).Text)

            Con.Open()
            Dim query As String = "DELETE FROM Admin WHERE Id = @Id"
            Dim cmd As SqlCommand = New SqlCommand(query, Con)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.ExecuteNonQuery()
            MsgBox("User Deleted Successfully.")
            Con.Close()
            Populate()
        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub
End Class