   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5	      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5
      Q-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      1    +TFortisDeviceManager.Views.EventsExportView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      4ei+http://schemas.microsoft.com/xaml/behaviors  5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5	      >
properties-clr-namespace:TFortisDeviceManager.Properties  5
      9local-clr-namespace:TFortisDeviceManager.Converters  5      A	 =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet 	 5      c
 _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a?	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks 
  5      Ghc19http://schemas.microsoft.com/netfx/2009/xaml/presentation    5      -   5        "  HandyControl.Controls.Window   Name$  EventsExport��5       � Icon$; 5\TFortisDeviceManager;component\Views\Images\icon.ico��5      /   )TFortisDeviceManager.Properties.Resources   EventExport � Title# Z 5      $	��800��5      $
��1600��6    � 	Resources 5       �� MergedDictionaries?   9TFortisDeviceManager.Converters.FilterParametersConverter   FilterParametersConverterA   ;TFortisDeviceManager.Converters.EventStatusToColorConverter  EventStatusToColorConverter@   :TFortisDeviceManager.Converters.EventStatusToIconConverter  EventStatusToIconConverter7   1TFortisDeviceManager.Converters.LanguageConverter  LanguageConverter  ColumnHeaderStyle1 �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value	 �� Triggers
 P� Property P� Value�� 5   
    5      %D  &	        &	      &	 &     &	 9     &	 L       5      6.     5      60     5      6/     5      6&   �� 5!      "  6/   .�5"      ��  �@�Cursor$
 Hand��6+   �� 5#       � ��$ Center=�6?   �� 5$       � ��
 �����60   �� 5%       � ��
 �����6/   �� 5&       � ��$ 0 0 1 0q�64   �� 5'       � ��$ 0 0 0 5q�6+   �� 5(       � ��$ 30��6+   	 5*      P� 5+      
 �@J�IsMouseOver$
 True��65   . �5,      ��  �@�ToolTip$ Сортировать��64   5-      5/      50      .��53      �  � ColumnDefinitions 54   
   �� 55      $��* �6   �� 56      $	��300 �6    � RowDefinitions 59   
   �� 5:      �� 5;      $
��Auto �6   $��60��6*   .+�5>   
   &   System.Windows.Controls.DataGrid  -   5          Name$ MonitoringEvents��5M      $��0��5>      $��10,10,10,10q�5?      $��0��5@      $��12(�5A         CanUserAddRows$ False��5C        DataGrid.Small#��[ 5D         	RowHeight$ 35Q�5E         CanUserDeleteRows$ False��5F         AutoGenerateColumns$ False��5G         CanUserResizeRows$ False��5H         CanUserResizeColumns$ False��5I         CanUserSortColumns$
 True��5J         SelectionMode$ Single=�5K      $��Stretch=�5L      ��5B      �� *MonitoringEvents+ �� Mode$ OneWay=�   Columns 5O      0	  *System.Windows.Controls.DataGridTextColumn	  5Q         Time,
  &System.Windows.Controls.DataGridColumn 
  Header# Z 6%    
  Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 5S   %   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5R   %   �� *Time+$ OneWay=�5S   %   	  5U         Name# Z 6%   $
 Auto 5W   %    5V   %   �� *
DeviceName+$ OneWay=�5W   %   	  5Y         	IpAddress# Z 6%   $
 Auto 5[   %    5Z   %   �� *Ip+$ OneWay=�5[   %   	  5]         Location# Z 6%     
  CanUserResize$
  True��5_   %   $ * 5`   %    5^   %   �� *DeviceLocation+$ OneWay=�5`   %   	  5b      !   DeviceDescription# Z! 6%   $
  True��5d   %   $ * 5e   %    5c   %   �� *DeviceDescription+$ OneWay=�5e   %   	  5g      "   Sensor# Z" 6%   $
 Auto 5i   %    5h   %   �� *
SensorName+$ OneWay=�5i   %   	  5k      #   Value# Z# 6%   $
 Auto 5m   %    5l   %   �� *SensorValueText+$ OneWay=�5m   %   	  5o      $   Description# Z$ 6%   $ * 5p   %   $
  True��5q   %    5r   %   �� *Description+$ OneWay=�% �� 	Converter#% [ 5w      #  HandyControl.Controls.GotoTop  5y   
   &   Animated$
& True��6   '   
AutoHiding$
' True��6%   (   HidingHeight$( 20Q�67   )   AnimationTime$
) 1000Q�6I   $��Right=�6�   $��Bottom=�6�   $��	0,0,20,20q�6�   *   Target* 6^   �� + �� ElementName$+ MonitoringEvents��6�   �� 5{   
   $��1��6   $��0��6"   , �� Orientation$, 
Horizontal=�62   $��Center=�6K   $��3��6f   .+�5|      �� -   From#��Z- 6   $��Center=�6H   $��	10,0,10,0q�6c   &   HandyControl.Controls.DatePicker  5}      $	��120��6   $��	10,0,10,0q�6(   (  "System.Windows.Controls.DatePicker.   SelectedDate. 6;   �� *
FromDate+/   SelectedDateChanged/ 6]   ! 	 Stylet.Xaml.ActionExtension  *RefreshTableCommand+5      �� 5�      	0   To#��Z0 6   $��Center=�6F   $��	10,0,10,0q�6a     5�      $	��120��6   $��	10,0,10,0q�6(   . 6;   �� *ToDate+/ 6[     *RefreshTableCommand+5�      �� 5�      1   Export#��Z1 6   $��	10,0,10,0q�6F     ButtonPrimary#��[ 6Y   2 � Cursor$
2 Hand��6�   ��6�     *ExportEventsCommand+6�   5�      �� 5�   
   $��0��6   $��3��6"   $	��300��62   $, Vertical=�6>   .+�5�      �� $��0��6   $��
20,20,20,0q�6&   .+�5�      )  #HandyControl.Controls.CheckComboBox  -   5        3  Name$3 ModelFilter��5�   #   $��0,0,0,20q�5�   '   (  "HandyControl.Controls.TitleElement4   
TitleWidth$	4 140 �5�   '   5   TitlePlacement$	5 Top=�5�   '   '  !HandyControl.Controls.InfoElement6   Placeholder$	6 Any��5�   '   7   Title#7 Z 5�   '     CheckComboBoxPlus#��[ 5�   '   )  #HandyControl.Controls.ListBoxAttach8   SelectedItems8 5�   '   �� *SelectedModels+$ OneWay=���5�   '   �� *EventModels+5�   '     5�      -   5        $3 IpFilter��5�   #   $��0,0,0,20q�5�   '   $	4 140 �5�   '   $	5 Top=�5�   '   $	6 Any��5�   '   #7 Z 5�   '   #��[ 5�   '   8 5�   '   �� *SelectedAddresses+$ OneWay=���5�   '   �� *
EventIps+5�   '     5�      -   5        $3 ParameterFilter��5�   #   $��0,0,0,20q�5�   '   $	4 140 �5�   '   $	5 Top=�5�   '   $	6 Any��5�   '   9   	Parameter#7 Z9 5�   '   #��[ 5�   '   8 5�   '   �� *SelectedParameters+$ OneWay=���5�   '   �� *EventParameters+5�   '     5�      -   5        $3 StateFilter��5�   #   $��0,0,0,20q�5�   '   $	4 140 �5�   '   $	5 Top=�5�   '   $	6 Any��5�   '   :   State#7 Z: 5�   '   #��[ 5�   '   $y�	Collapsed=�5�   '   8 5�   '   �� *SelectedStates+$ OneWay=���5�   '   �� *EventStates+5�   '   5�      �� 5�      $��1��6   $, 
Horizontal=�6&   $��Center=�6?   $��	20,0,20,0q�6\   .+�5�      �� ;   Confirm#��Z; 6   $��0,0,30,0q�5�      $	��100��5�      $��Center=�5�      $
��Left=�5�      #��[ 5�      $
2 Hand��5�      ��5�        *SetFiltersCommand+��5�      `� 5�      < `� 	Converter#< [  6'   .1�5�      �� $+ ModelFilter��6&   = �� Path$= SelectedItems�6@   �� 5�      $+ IpFilter��6&   $= SelectedItems�6=   �� 5�      $+ ParameterFilter��6&   $= SelectedItems�6D   �� 5�      $+ StateFilter��6&   $= SelectedItems�6@   5�      5�      �� 5�      >   Default#��Z> 6   $��30,0,0,0q�5�      $	��100��5�      $��Center=�5�      $��Right=�5�      #��[ 5�      $
2 Hand��5�      ��5�        *SetDefaultFiltersCommand+5�      5�      5�      5�      5�      