   M S B A M L   `   `   `  ����Z  VTFortisDeviceManagerUninstaller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullg8clr-namespace:TFortisDeviceManagerUninstaller.ViewModels*TFortisDeviceManagerUninstaller.ViewModels  5   
   4    .TFortisDeviceManagerUninstaller.Views.RootView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   
   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   
   O
viewModels8clr-namespace:TFortisDeviceManagerUninstaller.ViewModels     5   
   @d2http://schemas.microsoft.com/expression/blend/2008     5   
   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   
   S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5   
   A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5   
   -   5        &   HandyControl.Controls.GlowWindow   Name$
  Root��5   
      
WindowGlow#��[  5   
     MainContentBackgroundBrush#���  5	   
     PrimaryColor   ActiveGlowColor# �  5
   
      InactiveGlowColor# �  5   
   "  HandyControl.Controls.Window   	ShowTitle$ False��5   
      ShowMaxButton$ False��5   
      ShowMinButton$ False��5   
    � Title$% TFortisDeviceManagerUninstaller��5   
    � WindowStartupLocation$ CenterScreen=�5   
    � Icon$9 3\TFortisDeviceManagerUninstaller;component\icon.ico��5   
   	   ShowIcon$	 False��5   
   $	��640��5   
   $	��480��5   
   
 � 
ResizeMode$
 NoResize=�5   
   .��5      �  � RowDefinitions 5   
   �� 5      $	��180 �6   �� 5      $��* �6   �� 5      $	��120 �6   .+�5"   
   �� $��0��6   $9��3\TFortisDeviceManagerUninstaller;component\logo.png��6   $��30,30,30,30q�6Z   � 5#   
   $��1��6   .+�5$   
   �� $��0��6    �� Orientation$ Vertical=�6"   .+�5%      �� $��15(�6E   $
��Bold"�6S   $	��400��6e   $
��Wrap=�6q   $��Center=�6�   ��6   �� *CurrentMessage+ �� Mode$ TwoWay=�6�   -  'HandyControl.Controls.CircleProgressBar  5'      $��0��6#   $	��150��60   $	��150��6=   $��40(�6z   $��	0,40,0,40q�6�    � Value 6I   �� *UninstallProgress+$ TwoWay=�y�6�   �� *
IsProgress+  Boolean2VisibilityConverter �� 	Converter# [ 5(      5)      �� 5*   
   $��2��6   $	��150��6     ButtonPrimary#��[ 6Y    � Cursor$
 Hand��6�   ��6*   �� *CurrentAction+$ TwoWay=���6�   !  Stylet.Xaml.ActionExtension  *UninstallAppCommand+y�6�   �� *
IsProgress+ ! Boolean2VisibilityReConverter# [ 5,      5-      