Imports System.IO
Public Class Form1
    Private allDrives As New List(Of DriveInfo)
    Private Const WM_DEVICECHANGE As Integer = &H219
    Private Const DBT_DEVICEARRIVAL As Integer = &H8000
    Private Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004
    Protected Overloads Overrides Sub WndProc(ByRef msg As Message)
        MyBase.WndProc(msg)
        If msg.Msg = WM_DEVICECHANGE AndAlso msg.WParam = DBT_DEVICEARRIVAL Then
            'alle laufwerke durchlaufen und in der liste suchen 
            For Each s As String In Directory.GetLogicalDrives
                find_driveinfo_arg = s
                Dim d As DriveInfo = allDrives.Find(AddressOf find_driveinfo)
                If d Is Nothing Then
                    MessageBox.Show("Usb Stick Plugged In = " & s)
                    allDrives.Add(New DriveInfo(s))
                End If
            Next
        End If
        If msg.Msg = WM_DEVICECHANGE AndAlso msg.WParam = DBT_DEVICEREMOVECOMPLETE Then
            'alle laufwerke aus der liste prüfen
            Dim temp As List(Of DriveInfo) = GetAllDrives()
            For Each d As DriveInfo In allDrives
                find_driveinfo_arg = d.Name
                Dim lost As DriveInfo = temp.Find(AddressOf find_driveinfo)
                If lost Is Nothing Then
                    MessageBox.Show("Usb Stick Plugged off = " & d.Name)
                    allDrives.Remove(d)
                    Exit For
                End If
            Next
End If
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        allDrives = GetAllDrives()
    End Sub
    Private Function GetAllDrives() As List(Of DriveInfo)
    Dim ret As New List(Of DriveInfo)
        'und wieder befüllen
        For Each d As String In Directory.GetLogicalDrives
            ret.Add(New DriveInfo(d))
        Next
        Return ret
    End Function
    'suchfunktion zum finden der laufwerke in der collection 
    Private find_driveinfo_arg As String
    Private Function find_driveinfo(ByVal d As DriveInfo)
        If d.Name = find_driveinfo_arg Then
            Return True
        Else
            Return False
        End If
    End Function

End Class

