   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5      Q-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      G(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5	      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5
      3    -TFortisDeviceManager.Views.DeviceSettingsView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      :local(clr-namespace:TFortisDeviceManager.Views     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5      4views(clr-namespace:TFortisDeviceManager.Views  5	      >
properties-clr-namespace:TFortisDeviceManager.Properties  5
      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      .��5   
   �   � RowDefinitions  5   
   �� 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   $��70��6*   �� 5      $��40 �6   $��40��6(   .+�5   
   �� $��Visible=�6   .��5      ��  �� Orientation$ Vertical=�6   .+�5   
   �� $��Center=�6   $��	45,0,45,0q�60   $��0��6C   .+�5      ��    BorderBrush#���   6   $��40��5      $��0,0,0,1q�5      ._�5      �� $ 
Horizontal=�6   $��5q�66   $��Center=�6A   .+�5      �� $	��150��6    /   )TFortisDeviceManager.Properties.Resources   	IpAddress#��Z 6,   $��Center=�6]   #  HandyControl.Controls.TextBox  5       $	��200��6!    �� TextAlignment$ Center=�6-    � Cursor$ IBeam��6�   ��6D   �� *	IpAddress+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�" �� ValidatesOnNotifyDataErrors$
 True��6�   5!      5"      �� 5$      #���   6   $��40��5%      $��0,0,0,1q�5&      ._�5(      �� $ 
Horizontal=�6   $��5q�66   $��Center=�6A   .+�5)      �� $	��150��6$      NetworkMask#��Z 60   $��Center=�6c     5*      $	��200��6!   $ Center=�6-   $ IBeam��6�   ��6D   �� *NetworkMask+$ TwoWay=�$
 True��6�   5+      5,      �� 5.      #���   6   $��40��5/      $��0,0,0,1q�50      ._�52      �� $ 
Horizontal=�6   $��5q�66   $��Center=�6A   .+�53      �� $	��150��6    	   Gateway#��Z	 6,   $��Center=�6[     54      $	��200��6!   $ Center=�6-   $ IBeam��6�   ��6D   �� *	Gateway+$ TwoWay=�$ PropertyChanged=�$
 True��6�   55      56      �� 58      #���   6   $��40��59      $��0,0,0,1q�5:      ._�5<      �� $ 
Horizontal=�6   $��5q�66   $��Center=�6A   .+�5=      �� $	��150��6    
   Description#��Z
 6,   $
��Wrap=�6_    �� TextAlignment$
 Left=�6s   $
��Left=�6�   $��Center=�6�     5>      $	��200��6!   $ Center=�6-   $ IBeam��6�   ��6D   �� *Description+$ PropertyChanged=�$ TwoWay=�6�   5?      5@      �� 5B      #���   6   $��40��5C      $��0,0,0,1q�5D      ._�5F      �� $ 
Horizontal=�6   $��5q�66   $��Center=�6A   .+�5G      �� $	��150��6       Location#��Z 6,   $
��Wrap=�6\   $
 Left=�6p   $
��Left=�6�   $��Center=�6�     5H      $	��200��6!   $ Center=�6-   $ IBeam��6�   ��6D   �� *
Location+$ PropertyChanged=�$ TwoWay=�6�   5I      5J      �� 5L      #���   6   $��40��5M      $��0,0,0,1q�5N      ._�5P      �� $ 
Horizontal=�6   $��5q�66   $��Center=�6A   .+�5Q      �� $	��150��6       
EnableSnmp#��Z 6,   $��Center=�6^   �� 5R      $	��200��6   $
 Hand��6Z    d� 	IsChecked 6+   �� *SnmpEnabled+$ TwoWay=�6Z   5S      5T      5U      &   System.Windows.Controls.DataGrid  5W   
   -   5          Name$ PortList��5W         AutoGenerateColumns$ False��5X        DataGrid.Small#��[ 5Y         	RowHeight$ 35Q�5Z      $��1��5[         CanUserAddRows$ False��5\         CanUserDeleteRows$ False��5]         CanUserResizeRows$ False��5^         CanUserResizeColumns$
 True��5_      $	��180��5`         CanUserSortColumns$
 True��5a         SelectionMode$ Single=�5b      $��	15,0,15,0q�5c      $��0q�5d         HeadersVisibility$ Column=�5f      y�5e      ��  �� ElementName$ PortList�� �� Path$ HasItems�$ OneWay=�  Boolean2VisibilityConverter �� 	Converter# [ ��5g      �� *Ports+$ TwoWay=�   Columns 5i      0  *System.Windows.Controls.DataGridTextColumn  5j      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��6%      MinWidth$ 50Q�5k   %      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.5* 5l   %   1  +System.Windows.Controls.DataGridBoundColumn    Binding  5m   %   �� *Id+$ OneWay=�  5o      $	 Poe��6%   $	 150Q�5p   %   $ * 5q   %     5r   %   �� *Poe+$ OneWay=�4  .System.Windows.Controls.DataGridTemplateColumn  5t      $ * 6)   !   
EnablePort# Z! 63   "   CellTemplate" 5v      �� 5w      .R�5x      �� $ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5z   "   �� $��Center=�6+   $��Center=�5{   +   $��Center=�5|   +    5}   +   �� *
EnablePort+$ TwoWay=�$ PropertyChanged=�5      5�      5�        5�      $ * 6)   #   	EnablePoe# Z# 63   " 5�      �� 5�      .R�5�      �� $ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�   "   �� $��Center=�6+   $��Center=�5�   +   $��Center=�5�   +    5�   +   �� *	EnablePoe+$ TwoWay=�$ PropertyChanged=�5�      5�      5�      5�      �� 5�   
   $��2��6   $��70��5�      $ 
Horizontal=�5�      $��Center=�5�      .+�5�      �� $   	UserLogin#��Z$ 6   $
 Left=�5�      $��40��5�      $��0,0,5,0q�5�      $��Center=�5�        5�      $��30��5�      $	��100��5�      $ IBeam��5�      ��5�      �� *Login+5�      /	  )HandyControl.Controls.UniformSpacingPanel	  5�      $��20��6%   �� 5�      %   UserPassword#��Z% 6   $��0,0,10,0q�6L   $��Center=�6^   '
  !HandyControl.Controls.PasswordBox
  5�      & 
  ShowEyeButton$
& True��5�   !   ' 
  IsSafeEnabled$' False��5�   !   $��30��5�   !   $	��100��5�   !   $ IBeam��5�   !   ( 
  UnsafePassword( 5�   !   �� *
Password+5�   !   	  5�      $��20��6%   *  $HandyControl.Controls.ProgressButton  5�        ToggleButtonLoadingPrimary#��[ 5�       )   Progress$) 0Q�5�       $	��100��5�       *   Apply#��Z* 5�       $��30��5�       $
 Hand��5�       ��5�       !  Stylet.Xaml.ActionExtension  *ConfirmSettingsCommand+��5�       �� *NetworkDevice+ 5�       �� *
IsProgress+$ TwoWay=�{�5�       �� *	HasErrors+$ OneWay=�  Boolean2BooleanReConverter# [ 5�       5�      �� 5�   
   $��3��6   $
��10,5q�5�      $��14(�5�      ��5�      �� *ErrorMessage+$ OneWay=���5�      �� *
ErrorColor+5�      5�      5�      5�      5�      