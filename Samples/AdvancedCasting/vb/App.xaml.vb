'*********************************************************
'
' Copyright (c) Microsoft. All rights reserved.
' This code is licensed under the MIT License (MIT).
' THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
' ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
' IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
' PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
'
'*********************************************************
Imports System
Imports Windows.ApplicationModel
Imports Windows.ApplicationModel.Activation
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation

Namespace Global.ScreenCasting

    ''' <summary>
    ''' Provides application-specific behavior to supplement the default Application class.
    ''' </summary>
    NotInheritable Partial Class App
        Inherits Application

        Public Sub New()
            Me.InitializeComponent()
            AddHandler Me.Suspending, AddressOf OnSuspending
        End Sub

        ''' <summary>
        ''' Invoked when the application is launched normally by the end user.  Other entry points
        ''' will be used such as when the application is launched to open a specific file.
        ''' </summary>
        ''' <param name="e">Details about the launch request and process.</param>
        Protected Overrides Sub OnLaunched(e As LaunchActivatedEventArgs)
            If System.Diagnostics.Debugger.IsAttached Then
                Me.DebugSettings.EnableFrameRateCounter = False
            End If

            AddHandler Application.Current.UnhandledException, AddressOf Current_UnhandledException
            Window.Current.Activate()
            Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
            If rootFrame Is Nothing Then
                rootFrame = New Frame()
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages(0)
                AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed
                If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                End If

                Window.Current.Content = rootFrame
                Window.Current.Activate()
            End If

            If rootFrame.Content Is Nothing Then
                Try
                    rootFrame.Navigate(GetType(MainPage), e)
                Catch ex As Exception
                    Window.Current.Content = New Frame()
                    Window.Current.Activate()
                    rootFrame.Navigate(GetType(UnhandledExceptionPage), e.Arguments)
                    Dim content = CType(rootFrame.Content, UnhandledExceptionPage)
                    content.StatusMessage = ex.Message & ex.StackTrace
                End Try
            End If

            Window.Current.Activate()
        End Sub

        Private Sub Current_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
            Window.Current.Content = New Frame()
            Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
            Window.Current.Activate()
            rootFrame.Navigate(GetType(UnhandledExceptionPage))
            Dim content = CType(rootFrame.Content, UnhandledExceptionPage)
            content.StatusMessage = e.Exception.Message & e.Exception.StackTrace
        End Sub

        Protected Overrides Sub OnActivated(args As IActivatedEventArgs)
            Try
                Window.Current.Content = New Frame()
                Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                Window.Current.Activate()
                If args.Kind = ActivationKind.DialReceiver Then
                    rootFrame.Navigate(GetType(MainPage), args)
                Else
                    rootFrame.Navigate(GetType(MainPage))
                End If

                MyBase.OnActivated(args)
            Catch ex As Exception
                Window.Current.Content = New Frame()
                Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
                Window.Current.Activate()
                rootFrame.Navigate(GetType(UnhandledExceptionPage))
                Dim content = CType(rootFrame.Content, UnhandledExceptionPage)
                content.StatusMessage = ex.Message & ex.StackTrace
            End Try
        End Sub

        ''' <summary>
        ''' Invoked when Navigation to a certain page fails
        ''' </summary>
        ''' <param name="sender">The Frame which failed navigation</param>
        ''' <param name="e">Details about the navigation failure</param>
        Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            Throw New Exception("Failed to load Page " & e.SourcePageType.FullName)
        End Sub

        ''' <summary>
        ''' Invoked when application execution is being suspended.  Application state is saved
        ''' without knowing whether the application will be terminated or resumed with the contents
        ''' of memory still intact.
        ''' </summary>
        ''' <param name="sender">The source of the suspend request.</param>
        ''' <param name="e">Details about the suspend request.</param>
        Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs)
            Dim deferral = e.SuspendingOperation.GetDeferral()
            deferral.Complete()
        End Sub
    End Class
End Namespace
