   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5      Q-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5
      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5      Q-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      7    1TFortisDeviceManager.Views.AddingToMonitoringView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      4ei+http://schemas.microsoft.com/xaml/behaviors  5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      :local(clr-namespace:TFortisDeviceManager.Views     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5	      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5
      >
properties-clr-namespace:TFortisDeviceManager.Properties  5      A	 =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet 	 5      >
converters-clr-namespace:TFortisDeviceManager.Converters 	 5      Ghc19http://schemas.microsoft.com/netfx/2009/xaml/presentation    5      -   5        "  HandyControl.Controls.Window   Name$  MonitoringConfiguration��5       � Icon$; 5\TFortisDeviceManager;component\Views\Images\icon.ico��5      /   )TFortisDeviceManager.Properties.Resources   AddingToMonitoring � Title# Z 5      $	��800��5      $
��1200��6    � 	Resources 5       �� MergedDictionaries   ColumnHeaderStyle1 �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader  DataGridColumnHeaderStyle �� BasedOn �� Property	 �� Value7   1TFortisDeviceManager.Converters.LanguageConverter  LanguageConverter�� 5   
    5      %  &	        7  &	 �     �� 5      "  6/   8   .�5      ��  �@�Cursor$
	 Hand��6+   �� 5       � ��$	 Center=�6?   �� 5       � ��
	 �����60   �� 5        � ��$	 0 0 0 5q�6+   �� 5!       � ��$	 30��6+   5#        5%      6+   5'      .��5+      � 
 � ColumnDefinitions
 5,   
   �� 5-      $
��Auto �6   $	��300��6,   �� 5.      $��* �6    � RowDefinitions 51   
   �� 52      �� 53      $
��Auto �6   $��60��6*   .+�56   
   �� $��0��6   $��0��6"    �� Orientation$ Vertical=�62   $	��300��6I   .+�57      ��    DeviceInformation#��Z 6   $��5q�6R   .��58      �� .+�59      �� ���	#FFD0D1D7$	��5,0q�66   $��0,0,0,1q�6C   $��25��6]    J� SnapsToDevicePixels$
 True��6i   ._�5:      �� $ 
Horizontal=�6&   .+�5;      ��    Name#��Z 6(   $
��Left=�6T   $��Center=�6o   $
��Bold"�6�   $	��120��6�   �� 5<      -   5        $��modelTextBlock��5<   )   $��Center=�6U   $��0,0,20,0q�6p   ��6?   �� *Name+6p   5=      5>      �� 5?      ���	#FFD0D1D7$	��5,0q�65   $��0,0,0,1q�6B   $��25��6\   $
 True��6h   ._�5@      �� $ 
Horizontal=�6%   .+�5A      ��    	IpAddress#��Z 6(   $
��Left=�6Z   $��Center=�6u   $
��Bold"�6�   $	��120��6�   �� 5B      $��Center=�6C   $��0,0,20,0q�6^   ��6(   �� *	IpAddress+6^   5C      5D      �� 5E      ���	#FFD0D1D7$	��5,0q�65   $��0,0,0,1q�6B   $��25��6\   $
 True��6h   ._�5F      �� $ 
Horizontal=�6%   .+�5G      �� 
   Mac#��Z 6(   $
��Left=�6S   $��Center=�6n   $
��Bold"�6�   $	��120��6�   �� 5H      $��Center=�6=   ��6(   �� *Mac+6=   5I      5J      �� 5K      ���	#FFD0D1D7$	��5,0q�65   $��0,0,0,1q�6B   $��25��6\   $
 True��6k   ._�5L      �� $ 
Horizontal=�6%   .+�5M      ��    Location#��Z 6(   $
��Left=�6X   $��Center=�6s   $
��Bold"�6�   $	��120��6�   �� 5N      $	��130��6(   $
��Wrap=�67   $��Center=�6e   $��0,0,20,0q�6�   ��6K   �� *
Location+6�   5O      5P      �� 5Q      ���	#FFD0D1D7$	��5,0q�65   $��0,0,0,1q�6B   $��25��6\   $
 True��6k   ._�5R      �� $ 
Horizontal=�6%   .+�5S      ��    Description#��Z 6(   $
��Left=�6[   $��Center=�6v   $
��Bold"�6�   $	��120��6�   �� 5T      $	��130��6(   $
��Wrap=�67   $��Center=�6i   ��6K   �� *Description+6i   5U      5V      5W      5X      �� 5Z         MonitoringConfiguration#��Z 6   $��5q�6X   .��5[      �� .+�5\      �� $ 
Horizontal=�6!   $��0,5,0,0q�6:   .+�5]      ��    SNMPCommunity#��Z 6$   $
��Left=�6Y   $��Center=�6t   $	��120��6�   #  HandyControl.Controls.TextBox  5^      -   5        $��communityTextBox��5^   %   $��Center=�6]   $��24��6x   $	��150��6�   $��Center=�6�   ��6=   ��  �� Path$ 	Community�6�   5_      �� 5a      $ 
Horizontal=�6!   $��0,5,0,0q�6:   .+�5b      �� $��Center=�6#   $��4,0,0,0q�6E    d� 	IsChecked 6V   �� $ 	SendEmail��� 5c      $   SendEmailIfAvilabilityChanged#��Z 6%   $
��Left=�6j   $
��Wrap=�6�   $	��275��6�   $��5,0,0,0q�6�   5d      5e      5f      5g      &   System.Windows.Controls.DataGrid  5i   
   $��0��6   $��1��6       	RowHeight$ 40Q�5j      $��5q�5k      $
��Auto��5l         AutoGenerateColumns$ False��5n         CanUserResizeColumns$ False��5o         ColumnHeaderStyle# [  5p         SelectionMode$ Single=�5q         CanUserResizeRows$ False��5r         CanUserAddRows$ False��5s      ��5m      �� *OidsViewSource+    Columns  5u      0  *System.Windows.Controls.DataGridTextColumn  5w      ,  &System.Windows.Controls.DataGridColumn!   Header#! Z 6%   "   
IsReadOnly$
" True��5y   %   #   Width5	  /System.Windows.Controls.DataGridLengthConverter$
# Auto	 5z   %   $   MinWidth$	$ 100Q�5{   %   1
  +System.Windows.Controls.DataGridBoundColumn% 
  Binding% 5x   %   �� *Description+& �� 	Converter#& [ 5{   %   4  .System.Windows.Controls.DataGridTemplateColumn  5}      '   Timeout#! Z' 6)   $# *	 5~   )   (   CellTemplate( 5      �� 5�      .R�5�      )  #HandyControl.Controls.NumericUpDown  )   Maximum$
) 2000Q�6n   *   Minimum$* 5Q�6}   $	��5,0q�6�   +   Value+ 6/   �� *	Timeout+, �� UpdateSourceTrigger$, PropertyChanged=�6�   5�      5�        5�      -   Invert#! Z- 6)   $# *	 6Z   ( 5�      �� 5�      .R�5�      �� $ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�   "   �� $��Center=�5�   j   $��Center=�6�   $��Center=�6�   {�5�   '   �� *
Invertible+ 5�   '   �� *Invert+$, PropertyChanged=�6�   5�      5�      5�        5�      $# *	 6)   .   HeaderTemplate. 5�      �� 5�      .R�5�      �  5�   "   �� 5�   &   �� 5�   &   .+�5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $��0��60   /   Check#��Z/ 6=   �� 5�   &   $��1��6/   $��Center=�6<   $��10,0,0,0q�6Y   $
 TrueY�5�   /   ��5�   /   ! 	 Stylet.Xaml.ActionExtension  *CheckOnOffCommand+��5�   /   �� *	IsChecked+0 �� RelativeSource0 �� *Self+5�   #   5�      5�      ( 5�      �� 5�      .R�5�      �� $ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�   "   �� $��Center=�5�   %   $��Center=�6B   $��Center=�6f    5�   %   �� *Enable+1 �� Mode$1 TwoWay=�$, PropertyChanged=�5�      5�      5�        5�      $# *	 6)   . 5�      �� 5�      .R�5�      �  5�   "   �� 5�   &   �� 5�   &   .+�5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $��0��60   2   	SendEmail#��Z2 6=   �� 5�   &   $��1��6/   $��Center=�6<   $��10,0,0,0q�6Y    5�   /   �� *CheckSendEmail+��5�   /     *CheckSendEmailCommand+��5�   /   �� *	IsChecked+0 �� *Self+5�   #   5�      5�      ( 5�      �� 5�      .R�5�      �� $ 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�   "   �� $��Center=�6+   $��Center=�6H   $��Center=�6l    5�   +   �� *	SendEmail+$1 TwoWay=�$, PropertyChanged=�5�      5�      5�      5�      �� 5�   
   3   Confirm#��Z3 6   $��1��5�      $��1��5�      $��Right=�5�      $��Center=�5�      $��0,0,20,0q�5�        ButtonPrimary#��[ 5�      4 � Cursor$
4 Hand��5�      ��5�        *ConfirmConfigurationCommand+*  $Microsoft.Xaml.Behaviors.Interaction5   Triggers5 5�      +  %Microsoft.Xaml.Behaviors.EventTrigger  5�      6   	EventName$6 Click��6!   *  $Microsoft.Xaml.Behaviors.TriggerBase7   Actions.7 5�      4  .Microsoft.Xaml.Behaviors.Core.CallMethodAction  8   
MethodName$8 Close��6g   9   TargetObject9 6*   �� : �� ElementName$: MonitoringConfiguration��6g   5�      5�      5�      5�      