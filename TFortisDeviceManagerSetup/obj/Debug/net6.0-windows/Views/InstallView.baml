   M S B A M L   `   `   `  ����T  PTFortisDeviceManagerSetup, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManagerSetup.ViewsTFortisDeviceManagerSetup.Views  5      [2clr-namespace:TFortisDeviceManagerSetup.ViewModels$TFortisDeviceManagerSetup.ViewModels  5      [2clr-namespace:TFortisDeviceManagerSetup.Properties$TFortisDeviceManagerSetup.Properties  5
      1    +TFortisDeviceManagerSetup.Views.InstallView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:TFortisDeviceManagerSetup.Views     5      I
viewModels2clr-namespace:TFortisDeviceManagerSetup.ViewModels     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5	      C
properties2clr-namespace:TFortisDeviceManagerSetup.Properties  5
      c _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag	 cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a?	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  	 5      A
 =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet 
 5      .��5      � $��1��6   .+�5   
   � -   5        $��Start��5      y�6   �� *StartScreenVisibility+   Boolean2VisibilityConverter  �� 	Converter#  [   � RowDefinitions 5      �� 5      $��* �6    �� 5      $	��120 �6    .+�5      �� $��Center=�6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $	��300��6    �� 
IsReadOnly$
 True��6W   ��6*   �� *SelectedFolder+ �� Mode$ TwoWay=�6W   �� 5      $��0q�6     SecondaryRegionBrush#���  61   $��10,0,0,0q�6�    � Cursor$
 Hand��6�   ��6e   ! 
 Stylet.Xaml.ActionExtension  *SelectFolderCommand+.��5      . 	 (MahApps.Metro.IconPacks.PackIconMaterial     Kind$ Folder=�65   5      5      �� 5      $��0,15,0,0q�6   .+�5      �� $��0,10,0,0q�6!   $ 
Horizontal=�63   .+�5      ��  d� 	IsChecked 6#   �� *NeedInstallDatabase+$ TwoWay=��� 5       $��10,0,0,0q�6$   4   .TFortisDeviceManagerSetup.Properties.Resources   IstallPostgress#��Z 66   5!      �� 5"      $��0,10,0,0q�6!   $ 
Horizontal=�63   .+�5#      ��  6#   �� *NeedInstallDotNetRuntime+$ TwoWay=��� 5$      $��10,0,0,0q�6$   	   InstallNetDesktop#��Z	 66   5%      �� 5&      $��0,10,0,0q�6!   $ 
Horizontal=�63   .+�5'      ��  6#   �� *NeedCreateShortcut+$ TwoWay=��� 5(      $��10,0,0,0q�6$   
   
CreateIcon#��Z
 66   5)      5*      5+      �� 5-      $��1��6   $	��150��6"     ButtonPrimary#��[ 6.   $
 Hand��6U      Install#��Z 6c   ��6�     *StartSetupCommand+5/      � 51   
   -   5        $��Setup��51      y�6   �� *SetupScreenVisibility+#  [   52      �� 53      $��* �6    �� 54      $	��120 �6    .+�56      �� $ Vertical=�6   $��0��60   .+�57      -  'HandyControl.Controls.CircleProgressBar  $	��150��6'   $	��150��64   $��40(�6m   $��	0,40,0,40q�6{    � Value 6@   �� *SetupProgress+$ TwoWay=�6{   58      �� 5:      $��1��6   $	��150��6"      Cancel#��Z 6.   #��[ 6_   $
 Hand��6�   ��6�     *CloseAppCommand+5<      � 5>   
   -   5        $
��Exit��5>      y�6   �� *ExitScreenVisibility+#  [   5?      �� 5@      $��* �6    �� 5A      $	��120 �6    .+�5C      �� $ Vertical=�6   .+�5D      �� $��0��6      InstallCompleted#��Z 6)   $��15(�6a   $
��Bold"�6o   $��Center=�6�   �� 5F      $ 
Horizontal=�6   $��	50,20,0,0q�66   .+�5G      ��  6   �� *RunApplicationAfterExit+$ TwoWay=��� 5H      $��10,0,0,0q�6       RunProgramm#��Z 62   5I      5K      �� 5L      $��1��6   $	��150��6"      Finish#��Z 6.   #��[ 6_   $
 Hand��6�   ��6�     *CloseAppCommand+5M      5N      5O      