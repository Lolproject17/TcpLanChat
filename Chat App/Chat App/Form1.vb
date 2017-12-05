Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading.Thread
Public Class Form1

    Dim server As TcpListener = New TcpListener(IPAddress.Parse("192.168.1.3"), "13000")
    Dim client As New TcpClient
    Dim TcpClient As New TcpClient

    Dim thread1 As New Threading.Thread(AddressOf ServerListen)
    Dim thread3 As New Threading.Thread(AddressOf ClientListen)

    Sub ClientListen()

        TcpClient.Connect(IPAddress.Parse("192.168.1.3"), "13000")

        Dim stream As NetworkStream = TcpClient.GetStream()

        While True

            If stream.DataAvailable = True Then
                Dim array(TcpClient.ReceiveBufferSize) As Byte
                Dim i As Integer
                i = stream.Read(array, 0, array.Length)
                RichTextBox2.Text = RichTextBox2.Text + "Him : " + System.Text.ASCIIEncoding.ASCII.GetString(array, 0, i) + vbNewLine
                stream.Flush()
            End If

        End While



    End Sub

    Sub ClientAnswere()

        Dim array() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(TextBox2.Text)
        TcpClient.Client.Send(array)
        RichTextBox2.Text = RichTextBox2.Text + "Me : " + TextBox2.Text + vbNewLine
        TextBox2.Text = ""

    End Sub


    Sub ServerListen()

        server.Start()
        client = server.AcceptTcpClient

        Dim stream As NetworkStream = client.GetStream()

        While True


            If client.Available <> 0 Then


                Dim Array(client.ReceiveBufferSize) As Byte
                Dim i As Integer
                Dim data As String
                i = stream.Read(Array, 0, Array.Length)
                data = System.Text.ASCIIEncoding.ASCII.GetString(Array, 0, i)
                RichTextBox1.Text = RichTextBox1.Text + "Him : " + data + vbNewLine

            End If

        End While

    End Sub

    Sub serverWriteToClient()
        Dim stream As NetworkStream = client.GetStream()

        Dim array() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(TextBox1.Text)
        stream.Write(array, 0, array.Length)

        RichTextBox1.Text = RichTextBox1.Text + "Me : " + TextBox1.Text + vbNewLine

        TextBox1.Text = ""

    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        thread1.Start()
        thread3.Start()

        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim thread2 As New Threading.Thread(AddressOf serverWriteToClient)
        thread2.Start()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim thread4 As New Threading.Thread(AddressOf ClientAnswere)
        thread4.Start()
    End Sub
End Class
