   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5      Q-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5	      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5      Q-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      /    )TFortisDeviceManager.Views.MonitoringView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      4views(clr-namespace:TFortisDeviceManager.Views  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5	      c	 _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag
 cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a?	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks 	 
 5
      @
properties-clr-namespace:TFortisDeviceManager.Properties 	 
 5      ;local-clr-namespace:TFortisDeviceManager.Converters 	 
 5      T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3f6mah,http://metro.mahapps.com/winfx/xaml/controls  5      Ghc19http://schemas.microsoft.com/netfx/2009/xaml/presentation    5        � 	Resources  5         ColumnHeaderStyle1 �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader # DataGridColumnHeaderStyle.Small �� BasedOn �� Property �� Value �� Triggers P� Property P� Value/   )TFortisDeviceManager.Properties.Resources   Sort	 �� DataType@   :TFortisDeviceManager.ViewModels.MonitoringFiltersViewModel6   0TFortisDeviceManager.Views.MonitoringFiltersView  VerticalGridSplitterStyle
 �� 
TargetType �� Orientation ! DataGridTextCenterColumnStyle*  $System.Windows.Controls.DataGridCell  DataGridCellStyleA   ;TFortisDeviceManager.Converters.EventStatusToColorConverter  EventStatusToColorConverter@   :TFortisDeviceManager.Converters.EventStatusToIconConverter  EventStatusToIconConverter7   1TFortisDeviceManager.Converters.LanguageConverter  LanguageConverter��%�  &	        7  (�� �    5$      *, +)&	 �    &	 �    7  &	 �    &	     &	 %    �� 5   
   "  6+   8   .�5      ��  �@�Cursor$
 Hand��6'   �� 5       � ��$ Center=�6;   �� 5       � ��
 �����6,   �� 5       � ��$ 0 0 1 0q�60   �� 5       � ��$ 0 0 0 5q�6'   �� 5       � ��$ 25��6'    5      P� 5       �@J�IsMouseOver$
 True��61   . �5      ��  �@�ToolTip# Z 60   5       5"      �� 5$   
   "	  6   .R�5%        5&      �� 5(   
   " ��62   .�5)      ��  � ��
 �����6+   �� 5*       �@J�Opacity$	 0.1Q�6(   �� 5+       � ��$ 2��6&   �� 5,       � �� 5-      �� 5.      "
 ��6&   .R�5/      �� #��z��6!   #��z��50      #��z��51      ._�52      �� $ Vertical=�6)   $��Center=�6@   .+�53   "   �� 
�������$��2��6;   $��2��6E   $	��0 1q�6P   �� 54   "   
�������$��2��6;   $��2��6E   $	��0 1q�6P   �� 55   "   
�������$��2��6;   $��2��6E   $	��0 1q�6P   �� 56   "   
�������$��2��6;   $��2��6E   $	��0 1q�6P   57      58      59      5;       5<      P� 5=       �@J�IsMouseOver$
 True��61   . �5>      ��  � �� �#DCDCDC63   5?      5A      �� 5C   
   "  66   8   .�5D      ��  � ��$ Center=�6;   5E        5G   
   6,     5H   
   6+     5I   
   6"   5K      .��5M      �  %� IsVirtualizing$
 True��6    � RowDefinitions 5N   
   �� 5O      $��* �6   �� 5P      $��60 �6   $��60��6(   $��60��67    � ColumnDefinitions 5S   
   �� 5T      $	��670��6   $
��Auto �6.   �� 5U      $��* �6   �� 5V      $	��300��6>   ��6   �� *FiltersWidth+6>   .+�5Y   
   �� $��Hidden=�6   $��Right=�6<   .��5Z      �� !	  HandyControl.Controls.Growl 	  GrowlParent$
 True��6   $��16(�65   $	��Top=�6M   $��
0,10,10,10q�6e   5[      � 5]   
   -   5        $��columnModel��5]      y�60   �� *ShowModelColumn+� 5^   
   -   5        $��columnIP��5^      y�6-   �� *ShowIpAddressColumn+� 5_   
   -   5        $��	columnMac��5_      y�6.   �� *ShowMacAddressColumn+� 5`   
   -   5        $��columnSerial��5`      y�61   �� *ShowSerialNumberColumn+� 5a   
   -   5        $��columnLocation��5a      y�63   �� *ShowLocationColumn+� 5b   
   -   5        $��columnDescription��5b      y�66   �� *ShowDescriptionColumn+&
   System.Windows.Controls.DataGrid
  5d      -   5         
 Name$ DevicesForMonitoring��5r      $��0��5d      $��0��6$   $��3q�64   $��12(�5e        DataGrid.Small#��[ 5h       
  	RowHeight$ 35Q�5i       
  CanUserAddRows$ False��5j       
  EnableRowVirtualization$
 True��5k       
  CanUserDeleteRows$ False��5l       
  AutoGenerateColumns$ False��5m       
  CanUserResizeRows$ False��5n       
  CanUserResizeColumns$
 True��5o       
  CanUserSortColumns$
 True��5p       
  SelectionMode$ Single=�5q      (  "HandyControl.Controls.TitleElement   Title$ Events��5s      $��Stretch=�5t      +  %MahApps.Metro.Controls.DataGridHelper   CellPadding$ -20,0,-20,0q�5v      $��Center=�5w      ��5f      �� *DevicesForMonitoring+ �� Mode$ TwoWay=� �� SelectedItem 5g      �� *SelectedDevice+$ OneWayToSource=� 
  Sorting 5u      !  Stylet.Xaml.ActionExtension  *TableSortCommand+ 
  Columns 5y      4  .System.Windows.Controls.DataGridTemplateColumn  5{      ,  &System.Windows.Controls.DataGridColumn    Width5  /System.Windows.Controls.DataGridLengthConverter$  35 6)   !   CellTemplate! 5}      �� 5~      .R�5      �� $ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�   "   �� $��Center=�6+   $��Center=�5�   +   $��Center=�5�   +   " d� 	IsChecked" 5�   +   �� *
IsSelected+# d� Checked# 5�   +     *AddSelectedDeviceCommand+$ d� 	Unchecked$ 5�   +     *RemoveSelectedDeviceCommand+5�      5�      5�        5�      $  35 6)   %   MinWidth$% 35Q�64   &   CanUserResize$& False��6B   ! 5�      �� 5�      .R�5�   "   . 
 (MahApps.Metro.IconPacks.PackIconMaterial  '   Kind$' Alert=�6�   
��� ���y�6=   �� *State+ 	 Boolean2VisibilityConverter( �� 	Converter#( [	 $ OneWay=�6�   5�      5�        5�      $  35 6-   $% 35Q�68   $& False��6F   )   
Visibility$) 	Collapsed=�6\   ! 5�      �� 5�      .R�5�   "   �� $ 
Horizontal=�6-   $��Center=�6F   $��Center=�6c   .+�5�   &   �� $��Center=�5�   %   $��Center=�6B   $��Center=�6f   " 5�   %   �� *SelectedDevice.IsSelected+$ TwoWay=�* �� UpdateSourceTrigger$* PropertyChanged=���5�   %     *SelectDeviceCommand+5�   #   5�      5�      0  *System.Windows.Controls.DataGridTextColumn  5�      $
  Auto 6)   $& False��5�   )   +   Name,   Header#, Z+ 5�   )   ) 5�   )   �� *DataContext.ShowModelColumn+- �� Source- %  System.Windows.Markup.Reference  *columnModel+1  +System.Windows.Controls.DataGridBoundColumn.   Binding. 5�   )   �� *Name+$ OneWay=�  5�      $
  Auto 6)   $& False��5�   )   /   	IpAddress#, Z/ 5�   )   0   SortMemberPath$0 	IpAddress��5�   )   ) 5�   )   �� *!DataContext.ShowIpAddressColumn+-   *
columnIP+. 5�   )   �� *	IpAddress+$ OneWay=�5�   )     5�      $
  Auto 6)   $& False��5�   )   
1   Mac#, Z1 5�   )   ) 5�   )   �� *" DataContext.ShowMacAddressColumn+-   *	columnMac+. 5�   )   �� *Mac+$ OneWay=�  5�      2   SerialNumber#, Z2 5�   )   $% 80Q�5�   )   $
  Auto 5�   )   ) 5�   )   �� *$"DataContext.ShowSerialNumberColumn+-   *columnSerial+. 5�   )   �� *SerialNumber+$ OneWay=�5�   )     5�      3   Location#, Z3 5�   )   $	% 120Q�5�   )   $  * 5�   )   ) 5�   )   �� * DataContext.ShowLocationColumn+-   *columnLocation+. 5�   )   �� *
Location+$ OneWay=�5�   )     5�      4   Description#, Z4 5�   )   $	% 120Q�5�   )   $  * 5�   )   ) 5�   )   �� *#!DataContext.ShowDescriptionColumn+-   *columnDescription+. 5�   )   �� *Description+$ OneWay=�5�   )   5 � ContextMenu5 5�      �� 5�      y�6"   �� *,*PlacementTarget.DataContext.SelectedDevice+6 �� RelativeSource6 �� 7 �� Mode$
7 Self=� 
 Object2VisibilityConverter#( [
 .@�5�      m� 8   DeleteFromMonitoring#��Z8 6#   9 m� Command9 5�        *DeleteFromMonitoringCommand+: m� CommandParameter: 5�      �� *SelectedDevice+m� 5�      ;   MonitoringSettings#��Z; 6#   9 5�        *ConfigureDevice+: 5�      �� *SelectedDevice+m� 5�      <   AddToMap#��Z< 6#   9 5�        *AddDeviceToMap+: 5�      �� *SelectedDevice+5�      5�      �� 5�      $��1��6   $��Center=�6&   .+�5�      �� $
��Left=�6   $��Center=�68     ButtonPrimary#��[ 6z   $��0q�6�   
������ = � Cursor$
= Hand��6�   ��6S     *AddDeviceManually+.��5�        $
' Plus=�61     PrimaryBrush#���  6=   5�      5�      �� 5�   
   $��0��6   $��0��6'   $��2��64   $��-1q�6>   #��[ 6J   � 5�   
   -   5        $��
columnTime��5�      y�6/   �� *ShowTimeColumn+� 5�   
   -	   5        $��
columnName��5�      y�6/   �� *ShowNameColumn+� 5�   
   -
   5        $��columnEventIpAddress��5�      y�69   �� *ShowEventIpAddressColumn+� 5�   
   -   5        $��columnSensor��5�      y�61   �� *ShowSensorColumn+� 5�   
   -   5        $��columnValue��5�      y�60   �� *ShowValueColumn+� 5�   
   -   5        $��	columnAge��5�      y�6.   �� *ShowAgeColumn+� 5�   
   -   5        $��columnEventDescription��5�      y�6;   �� *ShowEventDescriptionColumn+� 5�   
   -   5        $��columnState��5�      y�60   �� *ShowStateColumn+� 5�   
   -   5        $��columnEventLocation��5�      y�68   �� *ShowEventLocationColumn+� 5�   
   -   5        $��columnDeviceDescription��5�      y�6<   �� *ShowDeviceDescriptionColumn+� 5�   
   -   5        $��columnConfirm��5�      y�62   �� *ShowConfirmColumn+
  5�   
   -   5        $ MonitoringEvents��5      $��1��5�      $��2��5�      $��3,3,3,3q�5�      $��12(�5�      $ False��5�      $ Single=�5�      #��[ 5�      $ 35Q�5�      $��Stretch=�5�      !> 
  EnableColumnVirtualization$> False��5�      $
 True��5     $��Center=�5     $ -20,0,-20,0q�5     ��5�      �� *MonitoringEvents+$ OneWay=�? �� SelectedIndex? 5�      �� *SelectedIndex+$ TwoWay=� 5�      �� *SelectedEvent+ 5       *TableSortCommand+ 5       5     $
  Auto 6%   $& False��5	  %   @   Time#, Z@ 5  %   ) 5
  %   �� *DataContext.ShowTimeColumn+-   *
columnTime+. 5  %   �� *Time+$ OneWay=�  5     $
  Auto 6%   $& False��5  %   #, Z+ 5  %   ) 5  %   �� *DataContext.ShowNameColumn+-   *
columnName+. 5  %   �� *
DeviceName+$ OneWay=�  5     $
  Auto 6%   $& False��5  %   #, Z/ 5  %   $0 	IpAddress��5  %   ) 5  %   �� *&$DataContext.ShowEventIpAddressColumn+-   *columnEventIpAddress+. 5  %   �� *Ip+$ OneWay=�5  %     5     $  * 6%   #, Z3 5  %   ) 5  %   �� *%#DataContext.ShowEventLocationColumn+-   *columnEventLocation+. 5  %   �� *DeviceLocation+$ OneWay=�  5      $  * 6%   #, Z4 5"  %   ) 5!  %   �� *)'DataContext.ShowDeviceDescriptionColumn+-   *columnDeviceDescription+. 5#  %   �� *DeviceDescription+$ OneWay=�  5%     $
  Auto 6%   $& False��5&  %   A   Sensor#, ZA 5(  %   ) 5'  %   �� *DataContext.ShowSensorColumn+-   *columnSensor+. 5)  %   �� *
SensorName+$ OneWay=�  5+     $
  Auto 6%   $& False��5,  %   B   Value#, ZB 5.  %   ) 5-  %   �� *DataContext.ShowValueColumn+-   *columnValue+. 5/  %   �� *SensorValueText+$ OneWay=�  51     $
  Auto 6%   $& False��52  %   
C   Age#, ZC 54  %   ) 53  %   �� *DataContext.ShowAgeColumn+-   *	columnAge+. 55  %   �� *Age+  57     $  * 6%   D   EventDescription#, ZD 59  %   ) 58  %   �� *(&DataContext.ShowEventDescriptionColumn+-   *columnEventDescription+. 5:  %   �� *Description+$ OneWay=�#( [   5<     $
  Auto 6%   $% 70Q�5=  %   $& False��5>  %   E   State#, ZE 5@  %   ) 5?  %   �� *DataContext.ShowStateColumn+-   *columnState+. 5A  %   �� *Status+$ OneWay=�  5C     $
  Auto 6)   $% 30Q�66   $& False��6D   ! 5D     �� 5E     .R�5F     �� $ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5G  "     ' 6=   �� *Status+#( [ ��6�   �� *Status+#( [ 5H     5I     5K       5M     $
  Auto 6)   $& False��66   F   HeaderTemplateF 5N     �� 5O     .R�5P     ��   SecondaryRegionBrush#���  6Q   $��0q�6�   $
= Hand��6�   ��6)     *ShowFiltersCommand+.��5Q  "     $' Filter=�6=   #���  6K   5R     5S     ! 5V     �� 5W     .R�5X     �� $��OK��6O   $
= Hand��6\   ��6%     *ConfirmEventReloaded+��5Y  "   �� 5Z  &   " ��6,   # [ 6I   .�5[  *   ��  � y�$ 	Collapsed=�6G   6[    5\  *   ^� 5]  .   G ^� 
ConditionsG 5^  2   �� 5_  6   H   HostStatusReloadedI �� Value#I ZH 6d   J �� BindingJ 6@   �� *SensorValueText+6d   �� 5`  6   K   
StatusInfo#I ZK 6[   J 6@   �� *Status+6[   .0�5b  2   ��  � y�$ Visible=�6O   5c  /   5e  '   5g     5h     5j     5m     #  HandyControl.Controls.GotoTop  5o  
   $��1��6   $��1��6%   L   Animated$
L True��62   M   
AutoHiding$
M True��6B   N   HidingHeight$N 20Q�6T   O   AnimationTime$
O 1000Q�6f   $��Right=�6�   $��	0,0,30,10q�6�   $��Bottom=�6�   P   TargetP 6{   �� Q �� ElementName$Q MonitoringEvents��6�   �� 5q  
   $��0��6   $��2��6"   .+�5r     �� -   5        R ��Name$R Filters��5r     ��6.   �� *SelectedViewModel+5s     5t     5u     