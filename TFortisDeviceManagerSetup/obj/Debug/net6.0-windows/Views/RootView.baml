   M S B A M L   `   `   `  ����T  PTFortisDeviceManagerSetup, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:TFortisDeviceManagerSetup.ViewModels$TFortisDeviceManagerSetup.ViewModels  5   
   [2clr-namespace:TFortisDeviceManagerSetup.Properties$TFortisDeviceManagerSetup.Properties  5   
   .    (TFortisDeviceManagerSetup.Views.RootView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   
   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   
   I
viewModels2clr-namespace:TFortisDeviceManagerSetup.ViewModels     5   
   @d2http://schemas.microsoft.com/expression/blend/2008     5   
   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   
   S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5   
   C
properties2clr-namespace:TFortisDeviceManagerSetup.Properties  5   
   c _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag	 cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a?	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  	 5   
   A
 =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet 
 5   
   -   5        &   HandyControl.Controls.GlowWindow   Name$
  Root��5   
      
WindowGlow#��[  5	   
     MainContentBackgroundBrush#���  5
   
     PrimaryColor   ActiveGlowColor# �  5   
      InactiveGlowColor# �  5   
   "  HandyControl.Controls.Window   	ShowTitle$ False��5   
      ShowMaxButton$ False��5   
      ShowMinButton$ False��5   
    � Title$ TFortisDeviceManagerSetup��5   
    � WindowStartupLocation$ CenterScreen=�5   
    � Icon$3 -\TFortisDeviceManagerSetup;component\icon.ico��5   
   	   ShowIcon$	 False��5   
   $	��640��5   
   $	��480��5   
   
 � 
ResizeMode$
 NoResize=�5   
   .��5      �  � RowDefinitions 5   
   �� 5      $	��180 �6   �� 5       $��* �6   �� 5!      $��40 �6   $��40��6(   .+�5$   
   �� $��0��6   $3��-\TFortisDeviceManagerSetup;component\logo.png��6   $��30,30,30,30q�6T   �� 5&   
   $��1��6    �� Orientation$ Vertical=�6"   y�69   �� *ActionSelected+ ! Boolean2VisibilityReConverter �� 	Converter# [ .+�5'      �� $
��Left=�6   $��	50,30,0,0q�64   .+�5(      	� 4   .TFortisDeviceManagerSetup.Properties.Resources   Repair#��Z 6   $��16(�6O   $
��Left=�6]    d� 	IsChecked 6x   �� *RepairSelected+ �� Mode$ TwoWay=�	� 5)      $��0,20,0,0q�6   $��16(�60      Delete#��Z 6>   $
��Left=�6o    6�   �� *UninstallSelected+5*      �� 5,      $��	0,220,0,0q�6   $	��150��6(      Next#��Z 64     ButtonPrimary#��[ 6c    � Cursor$
 Hand��6�   ��6�   ! 
 Stylet.Xaml.ActionExtension  *ChangeActiveItemCommand+5.      �� 50   
   $��1��6   $��5,5,5,5q�51       
 Stylet.Xaml.View   Model 52      �� *
ActiveItem+53      54      