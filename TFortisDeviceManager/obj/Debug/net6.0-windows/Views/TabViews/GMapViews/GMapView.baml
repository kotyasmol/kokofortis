   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:TFortisDeviceManager.ModelsTFortisDeviceManager.Models  5      Q-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5	      c _GMap.NET.WindowsPresentation, Version=2.1.7.0, Culture=neutral, PublicKeyToken=b85b9027b614afefqPclr-namespace:GMap.NET.WindowsPresentation;assembly=GMap.NET.WindowsPresentationGMap.NET.WindowsPresentation 5      G(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5      Q-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      Q-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      )    #TFortisDeviceManager.Views.GMapView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <models)clr-namespace:TFortisDeviceManager.Models     5      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      >
converters-clr-namespace:TFortisDeviceManager.Converters  5      >
properties-clr-namespace:TFortisDeviceManager.Properties  5	      S	 OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol 	 5
      \gmapsPclr-namespace:GMap.NET.WindowsPresentation;assembly=GMap.NET.WindowsPresentation 	 5      c
 _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a?	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks 
  5      6views(clr-namespace:TFortisDeviceManager.Views 
  5      ;local-clr-namespace:TFortisDeviceManager.Converters 
  5      @
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels 
  5        � 	Resources  5      @   :TFortisDeviceManager.Converters.DeviceOnMapToIconConverter   DeviceOnMapToIconConverterA   ;TFortisDeviceManager.Converters.DeviceStateToColorConverter  DeviceStateToColorConverter@   :TFortisDeviceManager.Converters.DeviceStateToIconConverter  DeviceStateToIconConverterA  ;TFortisDeviceManager.Converters.MarkerToMarkerViewConverter  MarkerToMarkerViewConverter��%t   &	        &	      &	 &     &	 9       5   
   6+     5   
   6,     5   
   6+     5   
   6,   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $	��520 �6   .+�5!   
   .  (GMap.NET.WindowsPresentation.GMapControl  -   5        $��gmapControl��5$      $��-5,-5,-5,-5q�5!      $��0��5"      $��2��5#      5*      �� 5,   
   $��1��6    �� Orientation$ Vertical=�6%   $��Right=�6<   $	��Top=�6X   .+�5-      d� -   5        $��ButtonVisibilitySwitch��5-   �   $��Right=�6    � Cursor$
 Hand��67   $��0q�6E   ) 	 #HandyControl.Controls.BorderElement   CornerRadius$ 25��6Y     ToggleButtonPrimary#��[ 6|   $��50��6�   $��50��6�   $��12q�6�    d� 	IsChecked$ FalseY�6�   .��5.      .  (MahApps.Metro.IconPacks.PackIconMaterial     Kind$
 Menu=�6-   
�������$��20��6L   $��20��6X   5/      A� 50       A� Rows$ 3��6�    A� Columns$ 1��6�   y�6   �� *	IsChecked+	 �� ElementName$	 ButtonVisibilitySwitch��  Boolean2VisibilityConverter
 �� 	Converter#
 [ .+�52      7 	 1HandyControl.Controls.TransitioningContentControl  $	��250��61      TransitionMode$ Top2BottomWithFade=�6>   .��53      �� /	   )TFortisDeviceManager.Properties.Resources 	  DevicesInMonitoring#��Z 6   $��5q�6\   .��55      &
   System.Windows.Controls.DataGrid
  -   5         
 Name$ DevicesForMonitoring��5H      $��0q�55   #   $��0��56      ! 	 HandyControl.Controls.Empty   	ShowEmpty$
 True��57      $��Center=�58      $��12(�59      $	��500��5:        DataGrid.Small#��[ 5=       
  	RowHeight$ 35Q�5>       
  CanUserAddRows$ False��5?      * 	 $HandyControl.Controls.DataGridAttach   ShowRowNumber$ False��5@       
  CanUserDeleteRows$ False��5A       
  AutoGenerateColumns$ False��5B       
  CanUserResizeRows$ False��5C       
  CanUserResizeColumns$
 True��5D       
  CanUserSortColumns$
 True��5E       
  SelectionMode$ Single=�5F      $��Center=�5I      ��5;      �� *MonitoringDevices+ �� Mode$ TwoWay=� �� SelectedItem 5<      �� *SelectedDevice+$ OneWayToSource=� 
  Sorting 5G      !  Stylet.Xaml.ActionExtension  *TableSortCommand+ 
  Columns 5K      4  .System.Windows.Controls.DataGridTemplateColumn  5M   "   ,  &System.Windows.Controls.DataGridColumn   MinWidth$ 35Q�69      Width5  /System.Windows.Controls.DataGridLengthConverter$ 35 6G      CanUserResize$ False��6R      CellTemplate 5O   &   �� 5P   *   .R�5Q   .   �� $ 
Horizontal=�69   $��Center=�6R   $��Center=�6o   .+�5R   2      6M   �� *	Available+#
 [ ��6�   �� *	Available+#
 [ 5S   /   5T   +   5W   #   0  *System.Windows.Controls.DataGridTextColumn  5Y   "   $ * 65   $ 80Q�6?   $ False��6M     	  Name!   Header#! Z  6c   1  +System.Windows.Controls.DataGridBoundColumn"   Binding" 6�   �� *Name+$ OneWay=�  5Z   "   $ * 65   $ 80Q�6?   $ False��6M   # 	  	IpAddress#! Z# 6c   $   SortMemberPath$$ 	IpAddress��6�   " 6�   �� *	IpAddress+$ OneWay=�6�     5[   "   $ * 65   $	 100Q�6?   % 	  Description#! Z% 6N   " 6�   �� *Description+$ OneWay=�  5]   "   $ 68 69   & 	  OnMap#! Z& 6D    5_   &   �� 5`   *   .R�5a   .   �� $ 
Horizontal=�69   $��Center=�6R   $��Center=�6o   .+�5b   2      6M   �� *OnMap+#
 [  5c   /   5d   +   5f   #   5i      5j      5k        5m      $	��250��61   $ Top2BottomWithFade=�6>   .��5o      �� ' 	  Markers#��Z' 6   $��5q�6P   .��5q      
  $��0q�6   $��1��5r      $
 True��5s      $	��500��5t      $��Center=�5u      $��3q�5v      $��12(�5w      #��[ 5y      $ 35Q�5z      $ False��5{      $ False��5|      $ False��5}      $ False��5~      $ False��5      $
 True��5�      $ Single=�5�      $��Center=�5�      ��5x      �� *	Markers+$ TwoWay=� 5�        *TableSortCommand+ 5�        5�   "   $
 Auto 65   $ 80Q�6B   $ False��6P   #! Z  6f   " 6�   �� *
DeviceName+$ OneWay=�  5�   "   $
 Auto 65   $ 80Q�6B   $ False��6P   #! Z# 6f   $$ 	IpAddress��6�   " 6�   �� *Ip+$ OneWay=�6�     5�   "   $ * 65   $ 80Q�6?   $ False��6M   ( 	  N#! Z( 6c   " 6�   �� *X+$ OneWay=�  5�   "   $ * 65   $	 100Q�6?   $ False��6N   ) 	  E#! Z) 6d   " 6�   �� *Y+$ OneWay=�  5�   "   $ 55Q�69   $ 55 6G   $ False��6R    5�   &   �� 5�   *   .R�5�   .   �� $��0q�6~   
������ $
 Hand��6�   ��65     *DeleteMarkerCommand+��6^   �� *Ip+.��5�   2     $ TrashCan=�6M   5�   /   5�   +   5�   #   5�      5�      5�      5�      5�      5�      5�      