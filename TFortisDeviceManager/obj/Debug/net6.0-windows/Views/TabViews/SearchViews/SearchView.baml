   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5      Q-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5
      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5      Q-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      +    %TFortisDeviceManager.Views.SearchView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      4views(clr-namespace:TFortisDeviceManager.Views  5      c _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag	 cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a?	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  	 5      S
 OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol 
 5	      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels 
 5
      >
properties-clr-namespace:TFortisDeviceManager.Properties 
 5      9local-clr-namespace:TFortisDeviceManager.Converters 
 5        � 	Resources  5       �� MergedDictionaries �� DataType=   7TFortisDeviceManager.ViewModels.DeviceSettingsViewModel3   -TFortisDeviceManager.Views.DeviceSettingsView;   5TFortisDeviceManager.ViewModels.SntpSettingsViewModel1   +TFortisDeviceManager.Views.SntpSettingsView8   2TFortisDeviceManager.ViewModels.DfuUpdateViewModel.   (TFortisDeviceManager.Views.DfuUpdateView   ColumnHeaderStyle1 �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader  DataGridColumnHeaderStyle �� BasedOn �� Property �� Value �� Triggers P� Property	 P� Value/   )TFortisDeviceManager.Properties.Resources
   Sort  GridRowStyle)	  #System.Windows.Controls.DataGridRow  DataGridRowStyleE
   ?TFortisDeviceManager.Converters.MonitoringStatusToIconConverter # MonitoringStatusToIconConverter  IpTextBoxStyle  TextBoxExtend ��Name �� Self �� RelativeSource �� Path>   8TFortisDeviceManager.Converters.ObjectToBooleanConverter  ObjectToBooleanConverter�� 5   
    5      %�  (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)&	  �     7  &	 %    7  &	 �    &	 �    7  &	     �� 5      "  6   .R�5        5      �� 5      "  6   .R�5        5      �� 5      "  6   .R�5        5      �� 5!      "  6/   8   .�5"      ��  �@�Cursor$
 Hand��6+   �� 5#       � ��$ Center=�6?   �� 5$       � ��
 �����60   �� 5%       � ��$ 0 0 1 0q�64   �� 5&       � ��$ 0 0 0 5q�6+   �� 5'       � ��$ 25��6+    5)      P� 5*       �@J�IsMouseOver$
	 True��65   . �5+      ��  �@�ToolTip# Z
 64   5,      5.      �� 50      " 	 6)   8    51      P� 52       �@	 
IsSelected$
	 True��64   . �53      ��  �@�ToolTip 64   �� *	ToolTip+54      56      
  58      64   �� 5:      " ��6+   8   .�5;      ��  �@=�ErrorTemplate 5<      �� 5=      .R�5>      ]� .+�5?   "   �� 
���  ��$��1q�6;   ._�5@   &   �� $ controlWithError��6@   5A   #   �� 5B   "   
���  ��$��20(�6=   $��3,0,0,0q�6K   !6_   5C      5D      5F       5G      P� 5H       �@=�HasError$
	 true��6=   . �5I      ��  �@�ToolTip 64   �� # Z $) #(Validation.Errors)[0].ErrorContent��� 5J       �@�Tag 60   �� # Z $) #(Validation.Errors)[0].ErrorContent�5K      5M        5O      6-   5Q      .��5U      �  � RowDefinitions 5V   
   �� 5W      �� 5X      $
��Auto �6   $��60��6*    � ColumnDefinitions 5[   
   �� 5\      $��* �6   �� 5]      $	��500��6B   ��6   �� *CamerasListWidth+6B   .+�5`   
   &   System.Windows.Controls.DataGrid  -   5          Name$ 
DeviceList��5l      * 
 $HandyControl.Controls.DataGridAttach   ShowRowNumber$
 True��5`         AutoGenerateColumns$ False��5a         RowHeaderWidth$ 60Q�5b      $��0��5e        DataGrid.Small#��[ 5f         	RowHeight$ 35Q�5g         CanUserResizeColumns$
 True��5h         CanUserSortColumns$
 True��5i         RowStyle# [ 5j         SelectionMode$ Single=�5k      ��5c      �� *FoundDevices+ �� SelectedItem 5d      �� *SelectedDevice+ �� Mode$ OneWayToSource=�   Sorting 5m      !  Stylet.Xaml.ActionExtension  *TableSortCommand+ �� SelectionChanged 5n        *ChangeSelectionCommand+ �� Selected 5o        *ChangeSelectionCommand+   Columns 5q      4  .System.Windows.Controls.DataGridTemplateColumn  5s      ,  &System.Windows.Controls.DataGridColumn    MinWidth$  35Q�6)   !   Width5  /System.Windows.Controls.DataGridLengthConverter$! 35 67   "   CanUserResize$" False��6B   #   HeaderTemplate# 5t      �� 5u      .R�5w   "   �� $ �� Orientation$$ 
Horizontal=�6-   .+�5x   "   �� $��1��6+   $��Center=�5y   +   % d� 	IsChecked$% FalseY�5z   +   ��5|   +     *SelectAllCommand+��5}   +   �� *	IsChecked+ �� *Self+5~      5      &   CellTemplate& 5�      �� 5�      .R�5�      �� $$ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�   "   �� $��Center=�6+   $��Center=�5�   +   $��Center=�5�   +   % 5�   +   �� *
IsSelected+$ TwoWay=�' �� UpdateSourceTrigger$' PropertyChanged=�5�      5�      5�      0  *System.Windows.Controls.DataGridTextColumn  5�      (   Name)   Header#) Z( 6%   $
! Auto 5�   %   $" False��5�   %   1  +System.Windows.Controls.DataGridBoundColumn*   Binding* 5�   %   �� *Name+$ OneWay=�  5�      +   	IpAddress#) Z+ 6%   $
! Auto 5�   %   $" False��5�   %   ,   SortMemberPath$, 	IpAddress��5�   %   * 5�   %   �� *	IpAddress+$ OneWay=�5�   %     5�      -   NetworkMask#) Z- 6%   $
! Auto 5�   %   $" False��5�   %   * 5�   %   �� *NetworkMask+$ OneWay=�  5�      .   Gateway#) Z. 6%   $
! Auto 5�   %   $" False��5�   %   * 5�   %   �� *	Gateway+$ OneWay=�  5�      
/   Mac#) Z/ 6%   $
! Auto 5�   %   $" False��5�   %   * 5�   %   �� *Mac+$ OneWay=�  5�      0   SerialNumber#) Z0 6%   $
! Auto 5�   %   $" False��5�   %   * 5�   %   �� *SerialNumber+$ OneWay=�  5�      1   Description#) Z1 6%   $! * 5�   %   $  0Q�5�   %   * 5�   %   �� *Description+$ OneWay=�  5�      2   Location#) Z2 6%   $! * 5�   %   * 5�   %   �� *
Location+$ OneWay=�  5�      3   Firmware#) Z3 6%   $
! Auto 5�   %   $" False��5�   %   * 5�   %   �� *
Firmware+$ OneWay=�  5�      4   UpTime#) Z4 6%   $
! Auto 5�   %   $  70Q�5�   %   $" False��5�   %   5   CanUserSort$5 False��5�   %   * 5�   %   �� *UpTime+$ OneWay=�5�   %     5�      6   MaxWidth$6 60Q�6)   $! 60 67   $  60Q�6B   $" False��6P   # 5�      �� 5�      .R�5�      ��  	 SecondaryRegionBrush#��� 	 6J   $��0q�6~   7 � Cursor$
7 Hand��6�   ��6%     *ShowCamerasList+.��5�   "   . 	 (MahApps.Metro.IconPacks.PackIconMaterial  8   Kind$	8 Cog=�6=    
 PrimaryBrush#��� 
 6H   5�      5�      & 5�      �� 5�      .R�5�      �� $$ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�   "     8 6=   �� *InMonitoring+9 �� 	Converter#9 [ $ TwoWay=�5�      5�      5�      : � ContextMenu: 5�      �� 5�      y�6   �� *,*PlacementTarget.DataContext.SelectedDevice+ �� *Self+  Object2VisibilityConverter#9 [ .@�5�      m� ;   AddToMonitoring#��Z; 6   < m� Command< 5�        *AddToMonitoringCommand+= m� CommandParameter= 5�      �� *SelectedDevice+m� 5�      >   OpenInBrowser#��Z> 6   < 5�        *OpenInBrowserCommand+m� 5�      ?   RebootDevice#��Z? 6   < 5�        *RebootDeviceCommand+m� 5�      @   ResetSettings#��Z@ 6   < 5�        *ResetSettingsCommand+m� 5�      A   ImportSettings#��ZA 6   < 5�        *ImportSettingsCommand+5�      5�      �� 5�   
   $	��600��6   $��32q�6!   $��Center=�6-   .+�5�      ' 
 !HandyControl.Controls.LoadingLine  $
��0,30q�6�   #��� 
 6�     LoadingLineLarge#��[ 6�   y�6   �� *
IsProgress+  Boolean2VisibilityConverter#9 [ $ OneWay=�6�   5�      � 5�   
   $��1��6   $��Center=�6    5�      �� 5�      $
��Auto �6#   �� 5�      $
��115* �6#   �� 5�      $
��302* �6#   �� 5�      $
��416* �6#   �� 5�      $
��Auto �6#   .+�5�      �� $��0��6   $$ 
Horizontal=�6)   {�5�      �� *
IsProgress+$ OneWay=�  Boolean2BooleanReConverter#9 [ .+�5�      �� $��25 0 0 0q�6   B   Auto#��ZB 5�      $
7 Hand��5�      % 5�      �� *IsAutoSearch+5�      �� 5     $$ 
Horizontal=�6   {�5     �� *IsAutoSearch+$ OneWay=�#9 [ .+�5     �� $	��142��6   $��25 0 0 0q�5     C   From' 
 !HandyControl.Controls.InfoElementD   Placeholder#D ZC 5     $7 IBeam��5     ��5     �� *FromIpAddress+5     �� 5	     $	��142��6   $��25 0 0 0q�5
     	E   To#D ZE 5     $7 IBeam��5     ��5     �� *ToIpAddress+$ TwoWay=�$' PropertyChanged=�"F �� ValidatesOnNotifyDataErrors$
F True��5     5     5     * 
 $HandyControl.Controls.ProgressButton  5     $��4��6k     ToggleButtonLoadingPrimary#��[ 5      G   Progress$G 0Q�5      $	��160��5      $��30��5      $
7 Hand��5      ��5        *StartSearchCommand+% 5      �� *
IsProgress+$ OneWay=�{�5      �� *'%DeviceSettingsTabViewModel.IsProgress+#9 [ .��5     �� .��5     �� $$ 
Horizontal=�6!   $��0,0,10,0q�6:   .+�5       $8 Magnify=�65   $��0,0,10,0q�6D   
�������6V   �� 5      H   StartSearching#��ZH 6$   5"     5#     5%     5&     � 5(  
   $��1��6   $��0��6    5*     �� 5+     $��* �6    �� 5,     $	��200��6    .+�5/     & 
  HandyControl.Controls.TabControl  $��0��6   I   IsScrollable$
I True��6)   J   IsAnimationEnabled$
J True��6=   K   ShowContextMenu$K False��6W   L   ShowOverflowButton$L False��6o   M   ShowScrollButton$
M True��6�     TabControlSliding#��[ 6�   $��10q�6�   .@�51     # 
 HandyControl.Controls.TabItem  ��52     �� 53     $$ 
Horizontal=�6%   $��0,0,10,0q�6>   .+�54       $
8 Cogs=�69   $��0,0,5,0q�6E     PrimaryTextBrush#���  6V   �� 55     N   Configuring#��ZN 6(   56     .��59     ( 
 "HandyControl.Controls.ScrollViewer  .��5:     �� $��10q�6%   {�61   �� O �� ElementName$O 
DeviceList��$ SelectedItem�#9 [ $ TwoWay=�.+�5;     �� $
��Auto��6i   ��6-   �� *DeviceSettingsTabViewModel+$ TwoWay=�6i   5<     5=     5>       5@     ��5B     �� 5C     $$ 
Horizontal=�6%   $��0,0,10,0q�6>   .+�5E       $8 Video=�69   $��0,0,5,0q�6F   #���  6W   �� 5F     P   
CameraList#��ZP 6(   5H     .��5K       $ False��6   $��0��5L     #��[ 5M     $ 35Q�5N     $ False��5O     $ 60Q�5P     Q   CanUserAddRows$Q False��5Q     R   CanUserDeleteRows$R False��5R     S   CanUserResizeRows$S False��5S     $ False��5T     $
 True��5U     $ Single=�5V     ��5W     �� *SelectedDevice.ListCameras+$ OneWay=� 5Y       5Z     T   Port#) ZT 61   $
! Auto 5[  1   * 5\  1   �� *Port+$ OneWay=�  5^     #) Z+ 61   $! * 5_  1   * 5`  1   �� *Ip+$ OneWay=�  5b     #) Z/ 61   $	  150Q�5c  1   $! * 5d  1   * 5e  1   �� *Mac+$ OneWay=�5i     5k       5m     ��5o     �� 5p     $$ 
Horizontal=�6%   $��0,0,10,0q�6>   .+�5r       $8 Upload=�69   $��0,0,5,0q�6G   #���  6X   �� 5s     U   Updating#��ZU 6(   5u     .��5w       .��5y     �� $��10q�6%   {�61   �� $O 
DeviceList��$ SelectedItem�#9 [ $ TwoWay=�.+�5{     �� ��6-   �� *DfuUpdateTabViewModel+$ TwoWay=�5}     5~     5�       5�     ��5�     �� 5�     $$ 
Horizontal=�6%   $��0,0,10,0q�6>   .+�5�       $8 Clock=�69   $��0,0,5,0q�6F   #���  6W   �� 5�     V   SNTP#��ZV 6(   5�     .��5�     �� $��10q�6!   {�6-   �� $O 
DeviceList��$ SelectedItem�#9 [ $ TwoWay=�.+�5�     �� ��6)   �� *SntpSettingsTabViewModel+$ TwoWay=�5�     5�     5�     �� 5�     $��1��6   $��	10,0,10,0q�6$   ��5�     �� 5�     W   GroupSettings#��ZW 6    .��5�     �� .+�5�     �� $��10q�6!   .+�5�     �� $$ 
Horizontal=�6%   .+�5�     �� $��5q�6'   % 5�  '   �� *GroupSnmpEnabled+{�5�  '   �� $O 
DeviceList��$ HasItems�$ OneWay=��� 5�     X   
EnableSnmp#��ZX 6(   $��5q�6Z   5�     �� 5�     $$ 
Horizontal=�6%   .+�5�     �� $��5q�6'   % 5�  '   �� *GroupLldpEnabled+{�5�  '   �� $O 
DeviceList��$ HasItems�$ OneWay=��� 5�     Y   
EnableLLDP#��ZY 6(   $��5q�6Z   5�     �� 5�     $��10q�6S   $��Center=�6_   ��6$   �� *GroupSettingsApplyingProgress+6_     5�     $��4��6,   #��[ 5�  ,   $G 0Q�5�  ,   $	��160��5�  ,   $��30��5�  ,   $
7 Hand��5�  ,   Z   Apply#��ZZ 5�  ,   ��5�  ,     *ConfirmGroupSettingsCommand+% 5�  ,   �� * GroupSettingApplyingInProgress+$ OneWay=�{�5�  ,   �� $O 
DeviceList��$ HasItems�$ OneWay=�5�     5�     5�     5�     5�     5�     5�     